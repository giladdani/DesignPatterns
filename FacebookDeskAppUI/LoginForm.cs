using System;
using System.Threading;
using System.Windows.Forms;
using FacebookDeskAppLogic;
using FacebookWrapper;

namespace FacebookDeskAppUI
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Login();
            Close();
        }

        public void Login()
        {
            try
            {
                LoginResult result = FacebookServiceProxy.Login(
                    "2909349805975755",
                    "public_profile",
                    "email",
                    "publish_to_groups",
                    "user_birthday",
                    "user_age_range",
                    "user_gender",
                    "user_link",
                    "user_videos",
                    "publish_to_groups",
                    "groups_access_member_info",
                    "user_friends",
                    "user_events",
                    "user_likes",
                    "user_location",
                    "user_photos",
                    "user_posts",
                    "user_hometown");

                Singleton<LoggedinUserData>.Instance.LoginResult = result;
                Singleton<LoggedinUserData>.Instance.User = result.LoggedInUser;
            }
            catch (Exception)
            {
                MessageBox.Show("Login failed");
            }
        }
    }
}
