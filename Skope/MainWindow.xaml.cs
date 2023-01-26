using ServerSkope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Skope
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ClientSkope clientSkope = new ClientSkope("http://localhost:5111");
        DispatcherTimer timer = new DispatcherTimer();
        String modeActual = "";
        public MainWindow()
        {
            InitializeComponent();           
            ChangeMenu("Connect");
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreashFriends();
            if(modeActual == "ChatMessage")
                UpdateChat();
        }

        public void RefreashFriends()
        {
            clientSkope.LocalUser.FriendRequest = clientSkope.LlistarFriendsRequest();
            clientSkope.LocalUser.Friends = clientSkope.LlistarFriends();
            clientSkope.Users = clientSkope.LlistarUsuaris();

            lstFriendRequest.ItemsSource = clientSkope.LocalUser.FriendRequest;
            lstFriends.ItemsSource = clientSkope.LocalUser.Friends;
            lstFriendRequest.Items.Refresh();
            lstFriends.Items.Refresh();
        }

        public void ChangeMenu(String menu)
        {
            if(menu == "Connect")
            {
                modeActual = "Connect";
                gridConnect.Visibility = Visibility.Visible;
                gridChat.Visibility = Visibility.Collapsed;
                gridRegister.Visibility = Visibility.Collapsed;
                gridChatMessage.Visibility = Visibility.Collapsed;
            }
            else if (menu == "Chats")
            {
                modeActual = "Chats";
                gridConnect.Visibility = Visibility.Collapsed;
                gridChat.Visibility = Visibility.Visible;
                gridRegister.Visibility = Visibility.Collapsed;
                gridChatMessage.Visibility = Visibility.Collapsed;
            }
            else if (menu == "Register")
            {
                modeActual = "Register";
                gridConnect.Visibility = Visibility.Collapsed;
                gridChat.Visibility = Visibility.Collapsed;
                gridRegister.Visibility = Visibility.Visible;
                gridChatMessage.Visibility = Visibility.Collapsed;
            }
            else if (menu == "ChatMessage")
            {
                modeActual = "ChatMessage";
                gridConnect.Visibility = Visibility.Collapsed;
                gridChat.Visibility = Visibility.Visible;
                gridRegister.Visibility = Visibility.Collapsed;
                gridChatMessage.Visibility = Visibility.Visible;
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (clientSkope.Connect(txtUsername.Text, txtPassword.Text))
            {
                clientSkope.Users = clientSkope.LlistarUsuaris();
                ChangeMenu("Chats");
                clientSkope.LocalUser = clientSkope.Users[clientSkope.Usernames.IndexOf(txtUsername.Text)];
                txtLocalUserName.Text = "User: " + clientSkope.LocalUser.Name;
                lstFriends.ItemsSource = clientSkope.LocalUser.Friends;
                lstFriendRequest.ItemsSource = clientSkope.LocalUser.FriendRequest;
                RefreashFriends();
                timer.Start();
            }
            else
                MessageBox.Show("Creedentials incorrect!");
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu("Connect");
            gridChatMessage.Children.Clear();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu("Register");
        }

        private void btnRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            User userAdd = new User(txtRegisterUsername.Text, txtRegisterPassword.Text, txtRegisterName.Text, Convert.ToInt32(txtRegisterAge.Text));
            if(clientSkope.Registrar(userAdd))
                MessageBox.Show("User registration complete!");
            else
                MessageBox.Show("Error when trying to register the user!");
            ChangeMenu("Connect");
            txtRegisterUsername.Text = "";
            txtRegisterPassword.Text = "";
            txtRegisterName.Text = "";
            txtRegisterAge.Text = "";
        }

        private void btnRegisterCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu("Connect");
            txtRegisterUsername.Text = "";
            txtRegisterPassword.Text = "";
            txtRegisterName.Text = "";
            txtRegisterAge.Text = "";
        }

        private void lstFriends_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            ChangeMenu("ChatMessage");
            if(item != null)
            {
                String username = item.ToString();
                User userSend = clientSkope.Users[clientSkope.Usernames.IndexOf(username)];
                clientSkope.SendUser = userSend;
                UpdateChat();
            }
        }

        private void UpdateChat()
        {
            String messagesTotal = clientSkope.GetChat(clientSkope.LocalUser, clientSkope.SendUser);
            String[] messages = messagesTotal.Split("\n");
            gridMessages.Children.Clear();
            gridMessages.RowDefinitions.Clear();
            int rowIndex = 0;
            if (messagesTotal != "" && messages[0] != "NEW CHAT")
            {
                for(int i = 0; i < messages.Length; i++)
                    gridMessages.RowDefinitions.Add(new RowDefinition());
                foreach (String message in messages)
                {
                    if(message != "")
                    {
                        String[] camps = message.Split(";");
                        TextBlock tbAdd = new TextBlock();
                        if (camps[1] == clientSkope.LocalUser.Username)
                        {
                            tbAdd.Text = "You: " + camps[0];
                            Grid.SetColumn(tbAdd, 1);
                            Grid.SetRow(tbAdd, rowIndex);
                        }
                        else
                        {
                            tbAdd.Text = clientSkope.SendUser.Name + ": " + camps[0];
                            Grid.SetColumn(tbAdd, 0);
                            Grid.SetRow(tbAdd, rowIndex);
                        }
                        rowIndex++;
                        gridMessages.Children.Add(tbAdd);
                    }           
                }
            }
        }

        private void btnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            clientSkope.AddFriend(clientSkope.LocalUser.Username, txtAddFriend.Text);
        }

        private void lstFriendRequest_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            if (item != null)
            {
                String username = item.ToString();
                User addUser = clientSkope.Users[clientSkope.Usernames.IndexOf(username)];
                clientSkope.LocalUser.Friends = clientSkope.LlistarFriends();
                clientSkope.LocalUser.FriendRequest = clientSkope.LlistarFriendsRequest();
                clientSkope.AcceptFriendRequest(addUser, clientSkope.LocalUser);
                clientSkope.Users = clientSkope.LlistarUsuaris();
                RefreashFriends();
            }
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            clientSkope.SendMessage(tbMessage.Text);
            tbMessage.Text = "";
            UpdateChat();
        }
    }
}
