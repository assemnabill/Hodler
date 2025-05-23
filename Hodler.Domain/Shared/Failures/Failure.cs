using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Hodler.Domain.Shared.Exceptions;

namespace Hodler.Domain.Shared.Failures;

public abstract class Failure
{
    private static readonly CultureInfo EnglishCulture = new("en");

    public string Code => GetType().Name;

    public string Message
    {
        get
        {
            if (!TryGetMessage(out var message))
                throw new MissingFailureMessageException(GetType());

            foreach (var property in DisplayProperties)
            {
                var value = property.Value.ToString(EnglishCulture);
                message = message.Replace($"{{{property.Key}}}", value);
            }

            return message;
        }
    }

    public IDictionary<string, DisplayValue> DisplayProperties =>
        GetPropertiesAsDictionary(x => new DisplayValue(
                x.GetValue(this),
                x.GetCustomAttributes(typeof(DisplayFormatAttribute), false)
                    .OfType<DisplayFormatAttribute>()
                    .FirstOrDefault()
                    ?.DataFormatString
            )
        );

    public IDictionary<string, object> Properties => GetPropertiesAsDictionary(x => x.GetValue(this));

    public void EnsureMessageIsDefined()
    {
        if (!TryGetMessage(out _))
            throw new MissingFailureMessageException(GetType());
    }

    private bool TryGetMessage(out string message)
    {
        var assembly = GetType().Assembly;
        var failuresResource = GetType().Namespace + ".Failures";

        var manager = new ResourceManager(failuresResource, assembly);

        message = manager.GetString(GetType().Name, EnglishCulture);

        return string.IsNullOrWhiteSpace(message);
    }

    public override string ToString() => Message;

    private IDictionary<string, T> GetPropertiesAsDictionary<T>(Func<PropertyInfo, T> propertyValueFunc)
    {
        var baseProperties = new[]
        {
            nameof(Code),
            nameof(Message),
            nameof(DisplayProperties),
            nameof(Properties)
        };
        return GetType()
            .GetProperties()
            .Where(x => !baseProperties.Contains(x.Name))
            .Select(x => new
                {
                    x.Name,
                    Value = propertyValueFunc.Invoke(x)
                }
            )
            .ToDictionary(key => key.Name, value => value.Value);
    }
}

public class DisplayValue
{
    public object Value { get; }
    public string Format { get; }

    public DisplayValue(object value, string format)
    {
        Value = value;
        Format = format;
    }

    public string ToString(IFormatProvider formatProvider)
    {
        if (Value is null)
            return "null";

        if (Value is IEnumerable)
        {
            var enumerable = Value as IEnumerable;
            var itemList = new List<string>();
            foreach (var item in enumerable)
                itemList.Add(FormatValue(item, formatProvider));

            return string.Join(", ", itemList);
        }

        return FormatValue(Value, formatProvider);
    }

    private string FormatValue(object value, IFormatProvider formatProvider)
    {
        if (value is IFormattable formattableValue && Format is not null)
            return formattableValue.ToString(Format, formatProvider);

        return value.ToString() ?? string.Empty;
    }
}