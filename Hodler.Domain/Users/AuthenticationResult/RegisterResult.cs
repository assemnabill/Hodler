namespace Hodler.Domain.Users.AuthenticationResult
{
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public bool IsExistUser { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
