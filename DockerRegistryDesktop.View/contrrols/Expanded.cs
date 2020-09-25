using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using DockerRegistryDesktop.View.Properties;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using System.Reflection;

namespace DockerRegistryDesktop.View.contrrols
{
    public partial class Expanded : UserControl
    {
        private bool _isExpanded = false;
        private const int COLLAPSED_HEIGHT = 29;

        private int _expandedHeight = 29;
        
        
        
        public int ExpandedHeight
        {
            get { return _expandedHeight; }
            set
            {
                if (value < 29)
                    _expandedHeight = 29 + 29;
                else
                    _expandedHeight = value + 29;
            }
        }
        public ControlCollection ChildControls 
        {
            get { return controlPanel.Controls; }
        }
        public override string Text
        {
            get { return this.textLabel.Text; }
            set 
            {
                this.textLabel.Text = value;
            }
        }

        public Expanded()
        {
            InitializeComponent();
            this.expandButton.Image = Resources.icons8_double_down_25;
            this.controlPanel.AutoSize = true;
            this.ResizeRedraw = true;
        }

        private void expandButton_Click(object sender, EventArgs e)
        {
            if(this._isExpanded)
            {
                this.Height = COLLAPSED_HEIGHT;
                this.expandButton.Image = Resources.icons8_double_down_25;
                this._isExpanded = false;
            }
            else
            {
                int totalHeight  = 29;
                foreach (var item in this.controlPanel.Controls)
                {
                    totalHeight += ((Control)item).Height + ((Control)item).Margin.Top + ((Control)item).Margin.Bottom;
                }
                this.Height = totalHeight;
                this.expandButton.Image = Resources.icons8_double_up_25;
                this._isExpanded = true;
                
            }
        }

        private void controlPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Control control = e.Control;
            var autoSizeProperty = control.GetType().GetProperties().Where(x => x.Name.ToLower() == "autosize").FirstOrDefault();
            if (autoSizeProperty != null)
            {
                Type controlType = control.GetType();
                autoSizeProperty.SetValue(control, false);
            }
                
            control.Width = control.Parent.Width;
        }
    }
}
