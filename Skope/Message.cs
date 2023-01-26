using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSkope
{
    public class Message
    {
        String txtMessage;
        User sendUser;
        int date; // Tipus de dade temporal

        public Message(string txtMessage, User sendUser, int date)
        {
            this.TxtMessage = txtMessage;
            this.SendUser = sendUser;
            this.Date = date;
        }

        public string TxtMessage { get => txtMessage; set => txtMessage = value; }
        public User SendUser { get => sendUser; set => sendUser = value; }
        public int Date { get => date; set => date = value; }
    }
}
