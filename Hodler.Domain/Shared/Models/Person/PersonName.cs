namespace Hodler.Domain.Shared.Models.Person;

public class PersonName
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? MiddleName { get; }
    public string? NickName { get; }

    public PersonName(
        string firstName,
        string lastName,
        string? middleName = null,
        string? nickName = null
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName);
        ArgumentException.ThrowIfNullOrEmpty(lastName);

        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        NickName = nickName;
    }
}