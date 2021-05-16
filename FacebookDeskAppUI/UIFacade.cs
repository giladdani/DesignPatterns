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
            m_LoginForm.ShowDialog();
            m_MainForm = new MainForm();
            m_MainForm.FormClosing += M_MainForm_FormClosing;
            m_MainForm.ShowDialog();
        }

        // Private Methods
        private void M_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(m_MainForm.GetRememberSettingsValue == true)
            {

            }
        }
    }
}
