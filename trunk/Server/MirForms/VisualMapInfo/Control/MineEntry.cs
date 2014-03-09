using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
using Server.MirForms.VisualMapInfo.Class;
using System.Web.UI.WebControls;

namespace Server.MirForms.VisualMapInfo.Control
{
    public partial class MineEntry : UserControl
    {
        public OvalShape RegionHighlight = new OvalShape();

        public byte MineIndex;

        Point MouseDownLocation;

        public int X, Y;

        public ushort Range
        {
            get
            {
                return (ushort)((RegionHighlight.Width / 2) / VisualizerGlobal.ZoomLevel);
            }
            set
            {
                RegionHighlight.Size = new Size(value * 2, value * 2);
                RegionHighlight.Left = X - value;
                RegionHighlight.Top = Y - value;
            }
        }

        public MineEntry()
        {
            InitializeComponent();
            InitializeRegionHighlight();
        }

        private void InitializeRegionHighlight()
        {
            RegionHighlight.Visible = false;
            RegionHighlight.BorderColor = Color.Green;

            RegionHighlight.BorderWidth = 2;
            RegionHighlight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Region_MouseMove);
            RegionHighlight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Region_MouseDown);
            RegionHighlight.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            RegionHighlight.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
        }

        #region "Start RegionHighlight Methods"
        
        private void Region_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void Region_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.MediumOrchid;
        }
        
        private void Region_MouseDown(object sender, MouseEventArgs e)
        {
            RegionHighlight.BringToFront();

            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void Region_MouseMove(object sender, MouseEventArgs e)
        {
            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Move)
                {
                    RegionHighlight.Left = e.X + RegionHighlight.Left - MouseDownLocation.X;
                    RegionHighlight.Top = e.Y + RegionHighlight.Top - MouseDownLocation.Y;

                    X = RegionHighlight.Left + Range;
                    Y = RegionHighlight.Top + Range;
                }
                else if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Resize)
                {
                    Range += (ushort)(MouseDownLocation.X - e.Location.X);
                }

            Details.Text = string.Format("X: {0} | Y: {1} | Range: {2}", X.ToString(), Y.ToString(), Range.ToString());
        }

        #endregion "End RegionHighlight Methods"

        public void RemoveEntry()
        {
            RegionHighlight.Dispose();
        }

        public void Hide()
        {
            RegionHighlight.Visible = false;
        }

        public void Show()
        {
            RegionHighlight.Visible = true;
        }

        private void MineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MineIndex = Convert.ToByte(MineComboBox.SelectedIndex);
        }

        private void MineEntry_Load(object sender, EventArgs e)
        {
            MineComboBox.Items.Add(new ListItem { Text = "Disabled", Value = "0" });
            for (int i = 0; i < Settings.MineSetList.Count; i++) MineComboBox.Items.Add(new ListItem(Settings.MineSetList[i].Name, (i + 1).ToString()));

            MineComboBox.SelectedIndex = MineIndex;

            Details.Text = string.Format("X: {0} | Y: {1} | Range: {2}", X.ToString(), Y.ToString(), Range.ToString());
        }

    }
}