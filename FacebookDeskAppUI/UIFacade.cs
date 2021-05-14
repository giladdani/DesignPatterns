using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FacebookDeskAppLogic;

namespace FacebookDeskAppUI
{
    class UIFacade
    {
        // Data Members
        LoginForm m_LoginForm = null;
        MainForm m_MainForm = null;

        // Ctor
        public UIFacade()
        {

        }

        // Public Functions
        public void Run()
        {
            m_LoginForm = new LoginForm();
            m_LoginForm.ShowDialog();
            m_MainForm = new MainForm();
            m_MainForm.ShowDialog();
        }
    }
}
