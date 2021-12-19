using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace gamelauncher
{
    public partial class Form1 : Form
    {
        const string filename = "glauncher.ini";

        public Form1()
        {
            InitializeComponent();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (sr.LineNumber > 0)
                MessageBox.Show("Error at line: " + sr.LineNumber
                    + "\nError message: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Error message: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            Application.Exit();
        }

        game_button[] button;
        ColorConverter cv = new ColorConverter();
        OVRDStreamReader sr;
        string[] tmp;
        string tmpstr;
        int frm_width = 640, frm_height = 480;
        List<string> glist = new List<string>();
        List<string> elist = new List<string>();
        List<string> ilist = new List<string>();
        List<string> wlist = new List<string>();
        List<string> plist = new List<string>();

        int x = 12, y = 12;
        public int waittime = 500, scaninterval = 200;
        string[] sndlist = new string[2];
        SoundPlayer snd;

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                //Read data
                sr = new OVRDStreamReader(filename, Encoding.Default);
                this.Text = sr.ReadLine();
                tmp = sr.ReadLine().Split(',');
                int.TryParse(tmp[0], out frm_width);
                int.TryParse(tmp[1], out frm_height);
                this.Size = new Size(frm_width, frm_height);
                this.ForeColor = (Color)cv.ConvertFromString(sr.ReadLine());
                this.BackColor = (Color)cv.ConvertFromString(sr.ReadLine());
                this.Icon = new Icon(sr.ReadLine());
                tmp = sr.ReadLine().Split('|');
                sndlist = tmp;
                if (sndlist[0] != "null" || sndlist[1] != "null") snd = new SoundPlayer();
                tmp = sr.ReadLine().Split('|');
                if (tmp[0] != "null") int.TryParse(tmp[0], out waittime);
                if (tmp[1] != "null") int.TryParse(tmp[1], out scaninterval);
                cv = null;
                //Read apps
                while (true)
                {
                    tmpstr = sr.ReadLine();
                    if (tmpstr == null) break;
                    tmp = tmpstr.Split('|');
                    glist.Add(tmp[0]);
                    elist.Add(tmp[1]);
                    ilist.Add(tmp[2]);

                    if(tmp.Length > 4)
                        plist.Add(tmp[4]);
                    else
                        plist.Add(null);

                    if (tmp.Length > 3)
                        wlist.Add(tmp[3]);
                    else
                        wlist.Add(null);
                }
                button = new game_button[glist.ToArray().Length];
                for (int i = 0; i < button.Length; i++)
                {
                    button[i] = new game_button(this);
                    button[i].GameName = glist[i];
                    button[i].GamePath = elist[i];
                    if (wlist[i] != null)
                    {
                        button[i].WorkDirectory = wlist[i];
                        button[i].GameLaunchParam = plist[i];
                    }
                    button[i].Icon = ConvertFromIconToBitmap(Icon.ExtractAssociatedIcon(ilist[i]), new Size(32, 32));
                    button[i].Location = new Point(x, y);
                    button[i].Size = new Size(256, 32);
                    if (y + 128 >= this.Height)
                    {
                        x += 288;
                        y = 12;
                    }
                    else y += 64;
                    Controls.Add(button[i]);
                }
            }
            sr.Close();
            this.CenterToScreen();
        }

        public static Bitmap ConvertFromIconToBitmap(Icon ic, Size sz)
        {
            Bitmap bmp = new Bitmap(sz.Width, sz.Height);

            using (Graphics gp = Graphics.FromImage(bmp))
            {
                gp.Clear(Color.Transparent);
                gp.DrawIcon(ic, new Rectangle(0, 0, sz.Width, sz.Height));
            }
            return bmp;
        }

        public bool PlaySound(bool isselect)
        {
            if(snd != null)
            {
                if (isselect)
                    snd.SoundLocation = sndlist[0];
                else
                    snd.SoundLocation = sndlist[1];

                if (snd.SoundLocation != "null")
                {
                    snd.Play();
                    return true;
                }
            }
            return false;
        }
    }
}
