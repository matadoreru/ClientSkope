using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSkope
{
    public class User
    {
        private List<User> friends;
        private List<User> friendRequest;
        private List<Chat> chats;
        private List<long> chatsID;
        
        String username;
        String password;
        String name;
        String bio;
        int age;
        bool online;

        public User(String username, String password, String name, int age)
        {
            Friends = new List<User>();
            FriendRequest = new List<User>();
            Chats = new List<Chat>();
            ChatsID = new List<long>();         
            Username = username;
            Password = password;
            Name = name;
            Age = age;
            Bio = "";
            Online = false;
        }

        public String Username { get => username; set => username = value; }
        public String Password { get => password; set => password = value; }
        public String Name { get => name; set => name = value; }
        public String Bio { get => bio; set => bio = value; }
        public int Age { get => age; set => age = value; }
        public List<Chat> Chats { get => chats; set => chats = value; }
        public List<User> Friends { get => friends; set => friends = value; }
        public List<User> FriendRequest { get => friendRequest; set => friendRequest = value; }
        public List<long> ChatsID { get => chatsID; set => chatsID = value; }
        public bool Online { get => online; set => online = value; }

        public override bool Equals(object? obj)
        {
            return obj is User user && Username == user.Username;
        }

        public override string ToString()
        {
            return this.username;
        }
    }
}
