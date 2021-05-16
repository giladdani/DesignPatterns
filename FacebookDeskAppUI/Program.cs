using System;
using System.Windows.Forms;

namespace FacebookDeskAppUI
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UIFacade app = new UIFacade();
            app.Run();
        }
    }
}