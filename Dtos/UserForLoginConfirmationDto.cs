using System.ComponentModel.DataAnnotations;

namespace Hermes.Dtos
{
    partial class UserForLoginConfirmationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserForLoginConfirmationDto()
        {

            if(Email == null)
            {
                Email = "";
            }
            if(Password == null)
            {
                Password = "";
            }
        }
    }
}
