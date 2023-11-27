using System.ComponentModel.DataAnnotations;

namespace Hermes.Dtos.Users
{
    partial class UserForLoginConfirmationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserForLoginConfirmationDto()
        {

            if (Email == null)
            {
                Email = "";
            }
            if (Password == null)
            {
                Password = "";
            }
        }
    }
}
