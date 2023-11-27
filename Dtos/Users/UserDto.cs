namespace Hermes.Dtos.Users
{
    public partial class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

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
