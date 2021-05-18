using System;
using System.Windows.Forms;
using FacebookDeskAppLogic;
using FacebookWrapper;

namespace FacebookDeskAppUI
{
    public class UIFacade
    {
        // Data Members
        private LoginForm m_LoginForm = null;
        private MainForm m_MainForm = null;

        // Ctor
        public UIFacade()
        {
        }

        // Public Methods
        public void Run()
        {
            m_LoginForm = new LoginForm();
            m_MainForm = new MainForm();
            m_MainForm.FormClosing += M_MainForm_FormClosing;
            m_LoginForm.ShowDialog();
            m_MainForm.InitFormDetails();
            m_MainForm.ShowDialog();
        }

        // Private Methods
        private void M_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(m_MainForm.IsRememberSettingsChecked() == true)
            {
                FacebookServiceProxy.Logout();
            }
        }
    }
}