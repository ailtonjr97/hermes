namespace Hermes.Dtos
{
    public partial class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        public UserDto()
        {
            if (Name == null)
            {
                Name = "";
            }
            if (Email == null)
            {
                Email = "";
            }
        }

    }
}
