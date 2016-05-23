using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;

namespace VKPlayer
{
    public partial class Form1 : Form
    {
        public List<Audio> audioList;

        WMPLib.IWMPPlaylist PlayList;
        WMPLib.IWMPMedia Media;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new VKPlayer_Authorise().Show();
            backgroundWorker1.RunWorkerAsync();
        }

        public class Audio
        {
            public int id { get; set; }
            public int owner_id { get; set; }
            public string artist { get; set; }
            public string title { get; set; }
            public int duration { get; set; }
            public string url { get; set; }
            public string lyrics_id { get; set; }
            public int genre_id { get; set; }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!Settings1.Default.auth)
            {
                Thread.Sleep(500);
            }

            WebRequest request =
                WebRequest.Create("https://api.vk.com/method/audio.get?owner_id=" + Settings1.Default.id + "&need_user=0&access_token=" + Settings1.Default.token);
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();

            responseFromServer = HttpUtility.HtmlDecode(responseFromServer);

            JToken token = JToken.Parse(responseFromServer);
            audioList = token["response"].Children().Skip(1).Select(c => c.ToObject<Audio>()).ToList();

            this.Invoke((MethodInvoker)delegate
            {
                PlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("VKPlayList");

                for (int i = 0; i < audioList.Count; i++)
                {
                    Media = axWindowsMediaPlayer1.newMedia(audioList[i].url);
                    PlayList.appendItem(Media);

                    listBox1.Items.Add(audioList[i].artist + " - " + audioList[i].title);
                }

                axWindowsMediaPlayer1.currentPlaylist = PlayList;
                axWindowsMediaPlayer1.Ctlcontrols.stop();

            });
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
            }
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
            }
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
