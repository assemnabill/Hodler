namespace Hodler.Domain.User.Models;

public class ApiKey
{
    public string ApiName { get; }
    public string Key { get; }
    public ApiKey(string apiName, string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiName);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        ApiName = apiName;
        Key = key;
    }

}