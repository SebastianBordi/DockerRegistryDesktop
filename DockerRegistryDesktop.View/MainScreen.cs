using DockerRegistryDesktop.Controller;
using DockerRegistryDesktop.View.contrrols;
using DockerRegistryDesktop.View.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockerRegistryDesktop.View
{
    public partial class MainScreen : Form
    {
        private ToolStripLabel serverLabel = new ToolStripLabel();
        private ToolStripLabel userLabel = new ToolStripLabel();
        private ToolStripLabel emconsolLabel = new ToolStripLabel 
        { 
            Text = "emconsol.com", 
            IsLink = true, 
            Alignment = ToolStripItemAlignment.Right
        };

        private string _server;
        private string _user;
        private string _passsword;

        public MainScreen(string server, string user, string password)
        {
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this._server = this.serverLabel.Text = server;
            this._user = this.userLabel.Text = user;
            this._passsword = password;

            InitializeComponent();

            emconsolLabel.Click += new EventHandler(LinkClick);
            statusStrip.Items.Add(userLabel);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(serverLabel);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(emconsolLabel);

        }

        private void LinkClick(object sender, EventArgs e)
        {
            string url = "https://www.emconsol.com";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
        private async void Principal_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            logotypePictureBox.Image = Resources.logotype;
            logotypePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;


            var repositories = await  RepositoryController.GetInstance(_server, _user, _passsword).GetRepositoriesAsync(); ;
            foreach (var item in repositories)
            {
                Expanded expanded = new Expanded();
                expanded.Text = item.Name;
                foreach (var tag in item.Tags)
                {
                    Label tagLabel = new Label { Text = tag.Name };
                    expanded.ChildControls.Add(tagLabel);
                }
                repositoriesPanel.Controls.Add(expanded);
            }
            this.Cursor = Cursors.Default;
        }
    }
}
