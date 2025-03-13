namespace InventorySystem.Models
{
    public class User
    {
        public int Id { get; set; }
<<<<<<< HEAD
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
=======
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User(int id, string username, string password, string role)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
>>>>>>> main
