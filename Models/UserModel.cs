namespace Hermes.Models
{
    public partial class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Salt { get; set; }
        public int Active { get; set; }
        public int Admin { get; set; }
        public string Jwt { get; set; }
        public int Intranet_Id { get; set; }
        public int Dpo { get; set; }
        public string Setor { get; set; }

        public UserModel()
        {
            if(Name == null)
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
            if (Jwt == null)
            {
                Jwt = "";
            }
            if (Setor == null)
            {
                Setor = "";
            }
        }

    }
}
