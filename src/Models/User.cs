namespace InventorySystem.Models
{
    public class User
    {
        public int Id { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> a3e3245ea24522fa011b93e04547a4357b8b5f77
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
<<<<<<< HEAD
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
=======
>>>>>>> a3e3245ea24522fa011b93e04547a4357b8b5f77
