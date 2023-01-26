using ServerSkope;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skope
{
    public class ClientSkope
    {
        String urlServidor = "";
        User localUser;
        User sendUser;
        List<User> users;
        List<String> usernames;
        WebClient wc = new WebClient();

        public User LocalUser { get => localUser; set => localUser = value; }
        public List<User> Users { get => users; set => users = value; }
        public List<string> Usernames { get => usernames; set => usernames = value; }
        public User SendUser { get => sendUser; set => sendUser = value; }

        public ClientSkope(String urlServ)
        {
            urlServidor = urlServ;
            Users = new List<User>();
            WebClient wc = new WebClient();
        }

        public bool Registrar(User registerUser)
        {
            bool resultat = false;
            String dades = "";
            try
            {
                String consulta = urlServidor + "/register?username=" + registerUser.Username + "&password=" + registerUser.Password + "&name=" + registerUser.Name + "&age=" + registerUser.Age;
                dades = this.wc.DownloadString(consulta);
                resultat = true;
            }
            catch (WebException ex)
            {
                using (var render = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dades = render.ReadToEnd();
                }
                resultat = false;
            }
            if (dades == "Error")
                resultat = false;
            return resultat;
        }

        public bool Connect(String username, String password)
        {
            bool resultat = false;
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/login?userName="+username+"&password="+password);
                resultat = true;
            }
            catch (WebException ex)
            {
                using (var render = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dades = render.ReadToEnd();
                }
                resultat = false;
            }
            if (dades == "Error")
                resultat = false;
            return resultat;
        }

        public List<User> LlistarUsuaris()
        {
            List<User> resultat = new List<User>();
            Usernames = new List<string>();
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/llistarUsuaris");
                String[] usuarisSeparats = dades.Split("\n");

                foreach(String s in usuarisSeparats)
                {
                    String[] camps = s.Split(";");
                    resultat.Add(new User(camps[0], camps[1], camps[2], Convert.ToInt32(camps[3])));
                    Usernames.Add(camps[0]);
                }
            }
            catch (WebException ex)
            {

            }
            return resultat;
        }

        public List<User> LlistarFriendsRequest()
        {
            List<User> resultat = new List<User>();
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/llistarAmigosRequest?username=" + this.localUser.Username);
                String[] usuarisSeparats = dades.Split("\n");

                if(dades.Length != 0)
                {
                    foreach (String s in usuarisSeparats)
                    {
                        String[] camps = s.Split(";");
                        User add = new User(camps[0], camps[1], camps[2], Convert.ToInt32(camps[3]));
                        if(!add.Equals(this.LocalUser))
                            resultat.Add(new User(camps[0], camps[1], camps[2], Convert.ToInt32(camps[3])));
                    }
                }
                
            }
            catch (WebException ex)
            {

            }
            return resultat;
        }

        public List<User> LlistarFriends()
        {
            List<User> resultat = new List<User>();
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/llistarFriends?username=" + this.localUser.Username);
                String[] usuarisSeparats = dades.Split("\n");

                if (dades.Length != 0)
                {
                    foreach (String s in usuarisSeparats)
                    {
                        String[] camps = s.Split(";");
                        User add = new User(camps[0], camps[1], camps[2], Convert.ToInt32(camps[3]));
                        if (!add.Equals(this.LocalUser))
                            resultat.Add(new User(camps[0], camps[1], camps[2], Convert.ToInt32(camps[3])));
                    }
                }
            }
            catch (WebException ex)
            {

            }
            return resultat;
        }

        public bool AddFriend(string username1r, string username2n)
        {
            bool resultat = false;
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/enviarSolicitud?userName=" + username1r + "&addUser=" + username2n);
                resultat = true;
            }
            catch (WebException ex)
            {
                using (var render = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dades = render.ReadToEnd();
                }
                resultat = false;
            }
            if (dades == "Error")
                resultat = false;
            return resultat;
        }

        internal String GetChat(User localUser, User userSend)
        {
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/getChat?userName1=" + userSend.Username + "&userName2=" + localUser.Username);
            }
            catch (WebException ex)
            {
                using (var render = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dades = render.ReadToEnd();
                }
            }
            return dades;
        }

        public void AcceptFriendRequest(User addUser, User localUser)
        {
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/aceptarSolicitud?userName1=" + addUser.Username + "&userName2=" + localUser.Username);
            }
            catch (WebException ex)
            {
                using (var render = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dades = render.ReadToEnd();
                }
            }
        }

        public void SendMessage(String msgTxt)
        {
            String dades = "";
            try
            {
                dades = this.wc.DownloadString(urlServidor + "/enviarMsg?localUser=" + LocalUser.Username + "&sendUser=" + SendUser.Username + "&msg=" + msgTxt);
            }
            catch (WebException ex)
            {
                using (var render = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dades = render.ReadToEnd();
                }
            }
        }
    }
}
