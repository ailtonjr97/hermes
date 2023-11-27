namespace Hermes.Dtos.Users
{
    public partial class UserForRegistrationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public bool IsActive { get; set; }

        public UserForRegistrationDto()
        {
            if (Name == null)
            {
                Name = "";
            }
            if (Email == null)
            {
                Email = "";
            }
            if (Password == null)
            {
                Password = "";
            }
            if (PasswordConfirm == null)
            {
                PasswordConfirm = "";
            }
        }
    }
}
