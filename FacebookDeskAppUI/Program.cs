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
            //Application.Run(new MainForm());
            UIFacade app = new UIFacade();
            app.Run();
        }
    }
}