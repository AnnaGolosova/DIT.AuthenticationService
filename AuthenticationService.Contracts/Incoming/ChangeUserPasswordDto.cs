namespace AuthenticationService.Contracts.Incoming
{
    public class ChangeUserPasswordDto
    {
        public string Username { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
