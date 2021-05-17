using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FacebookDeskAppLogic;

namespace FacebookDeskAppUI
{
    class UIFacade
    {
        // Data Members
        private LoginForm m_LoginForm = null;
        private MainForm m_MainForm = null;
        private AppSettings m_AppSettings = null;

        // Ctor
        public UIFacade()
        {

        }

        // Public Methods
        public void Run()
        {
            m_LoginForm = new LoginForm();
            m_MainForm = new MainForm();
            m_LoginForm.Shown += M_LoginForm_Shown;
            m_MainForm.FormClosing += M_MainForm_FormClosing;
            m_LoginForm.ShowDialog();
            m_MainForm.InitFormDetails();
            m_MainForm.ShowDialog();
        }

        // Private Methods
        private void M_LoginForm_Shown(object sender, EventArgs e)
        {
            try
            {
                m_AppSettings = AppSettings.LoadFromFile();
                if (m_AppSettings.RememberSettings == true)
                {
                    m_LoginForm.Login();
                    m_MainForm.Location = m_AppSettings.LastWindowLocation;
                    m_MainForm.Size = m_AppSettings.LastWindowSize;
                    m_LoginForm.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void M_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_AppSettings.LastAccessToken = Singleton<LoggedinUserData>.Instance.LoginResult.AccessToken;
            m_AppSettings.RememberSettings = m_MainForm.IsRememberSettingsChecked();
            m_AppSettings.LastWindowLocation = m_MainForm.Location;
            m_AppSettings.LastWindowSize = m_MainForm.Size;
            try
            {
                m_AppSettings.SaveToFile();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}