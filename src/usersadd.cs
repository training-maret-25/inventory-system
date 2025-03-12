class Program
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Role { get; set; }
    }

    public class UserManager
    {
        private const string UserFile = "users.json";
        private User _currentUser = null;

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

    static void AddUsers()
    {
        var a = 1;
    }
}
