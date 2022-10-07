using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace test2
{
    public partial class splash : Form
    {
        Timer tmr;

        public splash()
        {
            InitializeComponent();
        }

        private void splash_Load(object sender, EventArgs e)
        {
            player.URL = @"D:\Documents\Intern Project\Agromon Config Tool\test2\vid\Agromon.mp4";  
            player.settings.autoStart = true;
        }

        private void Splash_Shown(object sender, EventArgs e)

        {
            tmr = new Timer();
            //set time interval 3 sec
            tmr.Interval = 15000;
            //starts the timer
            tmr.Start();
            tmr.Tick += tmr_Tick;
        }

        void tmr_Tick(object sender, EventArgs e)

        {
            //after 3 sec stop the timer
            tmr.Stop();
            //display mainform
            Main mf = new Main();
            mf.Show();
            //hide this form
            this.Hide();
        }

        private void splash_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
