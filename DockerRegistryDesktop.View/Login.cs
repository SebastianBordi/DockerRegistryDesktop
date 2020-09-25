using DockerRegistryDesktop.Controller;
using DockerRegistryDesktop.View.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DockerRegistryDesktop.View
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.serverLabel.BackColor =
            this.passwordLabel.BackColor =
            this.userLabel.BackColor = Color.FromArgb(164, Color.DarkCyan);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Resources.maxime_horlaville_jJYZLIvReoc_unsplash;
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if(await RepositoryController.TestServer(serverTextBox.Text, userTextBox.Text, passwordTextBox.Text))
                {
                    this.Cursor = Cursors.Default;
                    this.passwordTextBox.Text = "";
                    var mainScreen = new MainScreen(serverTextBox.Text, userTextBox.Text, passwordTextBox.Text);
                    this.Visible = false;
                    mainScreen.ShowDialog();
                    this.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar {ex.Message}");
            }

            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }
    }
}
