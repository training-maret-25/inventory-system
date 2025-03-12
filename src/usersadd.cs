class Program
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Password dalam format hash
        public string Role { get; set; } // "Admin" atau "Employee"
    }

    public class UserManager
    {
        private const string UserFile = "users.json";
        private User _currentUser = null; // Menyimpan user yang sedang login

        // Membaca data user dari JSON
        private List<User> LoadUsers()
        {
            if (!File.Exists(UserFile)) return new List<User>();
            return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(UserFile)) ?? new List<User>();
        }
    }

    static string InputEmployee()
    {
        string input = Console.ReadLine();
    }
}
