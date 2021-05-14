using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FacebookDeskAppUI
{
    class PictureBoxProxy : PictureBox
    {
        protected override void OnClick(EventArgs e)
        {
            // photo go BIG
            Width = Width * 2;
            Height = Height * 2;
            base.OnClick(e);
        }
    }
}
