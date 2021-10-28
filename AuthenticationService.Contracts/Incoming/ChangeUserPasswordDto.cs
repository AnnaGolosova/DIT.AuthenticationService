namespace AuthenticationService.Contracts.Incoming
{
    public class ChangeUserPasswordDto
    {
        public string UserName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
