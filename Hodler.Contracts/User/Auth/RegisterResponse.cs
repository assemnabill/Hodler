namespace Hodler.Contracts.User.Auth
{
    public class RegisterResponse
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
