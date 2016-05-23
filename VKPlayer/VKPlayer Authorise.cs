using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VKPlayer
{
    public partial class VKPlayer_Authorise : Form
    {
        int id = 5413320;



        public VKPlayer_Authorise()
        {
            InitializeComponent();
        }

        private void VKPlayer_Authorise_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=" + id + "&display=popup&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.52");
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            toolStripStatusLabel1.Text = "Loading please wait";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Loading completed";

            try
            {
                string url = webBrowser1.Url.ToString();
                string l = url.Split('#')[1];
                if (l[0] == 'a')
                {
                    Settings1.Default.token = l.Split('&')[0].Split('=')[1];
                    Settings1.Default.id = l.Split('=')[3];
                    Settings1.Default.auth = true;

                    MessageBox.Show("Authorization completed successfully");
                    //MessageBox.Show(Settings1.Default.token + " " + Settings1.Default.id);
                    this.Close();
                }
            }
            catch { }
        }

    }
}
