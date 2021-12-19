using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gamelauncher
{
    public partial class game_button : UserControl
    {
        string gpath = null;
        string wdir = null;
        string param = null;
        bool isin = false;
        Form1 frm;
        Rectangle rect;

        public game_button(Form1 input)
        {
            InitializeComponent();
            frm = input;
            timer1.Interval = frm.scaninterval;
            rect = new Rectangle(this.Location.X, this.Location.Y, 256, 32);
        }

        public Bitmap Icon
        {
            get
            {
                return (Bitmap)pictureBox1.Image;
            }
            set
            {
                pictureBox1.Image = value;
            }
        }

        public string GameName
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public string GamePath
        {
            get
            {
                return gpath;
            }
            set
            {
                gpath = value;
            }
        }

        public string WorkDirectory
        {
            get
            {
                return wdir;
            }
            set
            {
                wdir = value;
            }
        }

        public string GameLaunchParam
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
            }
        }

        private void game_button_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
            proc.FileName = gpath;
            if (wdir == null)
                proc.WorkingDirectory = gpath.Substring(0, gpath.LastIndexOf('\\'));
            else
                proc.WorkingDirectory = wdir;

            if (param != null)
                proc.Arguments = param;

            if (frm.PlaySound(false))
                System.Threading.Thread.Sleep(frm.waittime);

            System.Diagnostics.Process.Start(proc);
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (frm.ActiveControl.Focused)
            {
                if (rect.Contains(this.PointToClient(Cursor.Position)) && !isin)
                {
                    isin = true;
                    this.BorderStyle = BorderStyle.Fixed3D;
                    frm.PlaySound(true);
                }
                else if (!rect.Contains(this.PointToClient(Cursor.Position)) && isin)
                {
                    isin = false;
                    this.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }
    }
}
