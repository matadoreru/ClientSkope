using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSkope
{
    public class Chat
    {
        long idChat;
        User user1r;
        User user2n;
        List<Message> chatMsgs;

        public Chat(long idChat, User user1r, User user2n, List<Message> chatMsgs)
        {
            this.idChat = idChat;
            this.user1r = user1r;
            this.user2n = user2n;
            this.chatMsgs = chatMsgs;
        }

        public void AfegirMissatge(String txtMsg, User sender)
        {
            Message msgAdd = new Message(txtMsg, sender, 1);
            chatMsgs.Add(msgAdd);
        }

        public override bool Equals(object? obj)
        {
            return obj is Chat chat &&
                   idChat == chat.idChat;
        }
    }
}
