namespace Hodler.Domain.Users.AuthenticationResult
{
    public class RegisterResult
    {
        public RegisterResult(bool succeeded, bool isExistUser, List<string>? errors)
        {
            Succeeded = succeeded;
            IsExistUser = isExistUser;
            Errors = errors ?? new();
        }

        public bool Succeeded { get; init; }
        public bool IsExistUser { get; init; }
        public List<string> Errors { get; init; }
    }
}
