﻿namespace Hermes.Models
{
    public partial class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int Is_Active { get; set; }

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
        }

    }
}
