using System;
using System.Drawing;
using System.Windows.Forms;
using Server.MirForms.VisualMapInfo.Class;
using Server.MirForms.VisualMapInfo.Control;
using Microsoft.VisualBasic.PowerPacks;

namespace Server.MirForms.VisualMapInfo
{
    public partial class VForm : Form
    {
        ShapeContainer Canvas = new ShapeContainer();
        public Point MouseDownLocation;

        public VForm()
        {
            InitializeComponent();
        }

        private void VForm_Load(object sender, EventArgs e)
        {
            InitializeMap();
            InitializeMineInfo();
            InitializeRespawnInfo();
        }

        private void VForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VisualizerGlobal.MapInfo.Respawns.Clear();
            VisualizerGlobal.MapInfo.MineZones.Clear();

            for (int i = 0; i < RespawnPanel.Controls.Count; i++)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    MirDatabase.RespawnInfo NewRespawnZone = new MirDatabase.RespawnInfo();

                    NewRespawnZone.Location = new Point(RespawnControl.X, RespawnControl.Y);
                    NewRespawnZone.MonsterIndex = RespawnControl.MonsterIndex;
                    NewRespawnZone.Spread = RespawnControl.Range;
                    NewRespawnZone.Count = Convert.ToUInt16(RespawnControl.Count.Text);
                    NewRespawnZone.Delay = Convert.ToUInt16(RespawnControl.Delay.Text);

                    VisualizerGlobal.MapInfo.Respawns.Add(NewRespawnZone);
                }
                catch (Exception) { continue; }
            }

            for (int i = 0; i < MiningPanel.Controls.Count; i++)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    MineZone NewMineZone = new MineZone();

                    NewMineZone.Location = new Point(MineControl.X, MineControl.Y);
                    NewMineZone.Mine = MineControl.MineIndex;
                    NewMineZone.Size = MineControl.Range;

                    VisualizerGlobal.MapInfo.MineZones.Add(NewMineZone);
                }
                catch (Exception) { continue; }
            }
        }

        private void InitializeMap()
        {
            ReadMap readMap = new ReadMap();

            readMap.mapFile = VisualizerGlobal.MapInfo.FileName;
            readMap.Load();

            MapImage.Image = VisualizerGlobal.ClippingMap;

            Canvas.Parent = MapImage;
            Canvas.BringToFront();
        }

        private void InitializeMineInfo()
        {
            for (int i = 0; i < VisualizerGlobal.MapInfo.MineZones.Count; i++)
            {
                MineEntry MineRegion = new MineEntry();
                MineRegion.Dock = DockStyle.Top;
                MineRegion.MineIndex = VisualizerGlobal.MapInfo.MineZones[i].Mine;
                MineRegion.X = VisualizerGlobal.MapInfo.MineZones[i].Location.X;
                MineRegion.Y = VisualizerGlobal.MapInfo.MineZones[i].Location.Y;
                MineRegion.Range = VisualizerGlobal.MapInfo.MineZones[i].Size;
                MineRegion.Show();

                MiningPanel.Controls.Add(MineRegion);

                MineRegion.RegionHighlight.Parent = Canvas;
            }
        }

        private void InitializeRespawnInfo()
        {
            for (int i = 0; i < VisualizerGlobal.MapInfo.Respawns.Count; i++)
            {
                RespawnEntry RespawnRegion = new RespawnEntry();
                RespawnRegion.Dock = DockStyle.Top;
                RespawnRegion.MonsterIndex = VisualizerGlobal.MapInfo.Respawns[i].MonsterIndex;
                RespawnRegion.X = VisualizerGlobal.MapInfo.Respawns[i].Location.X;
                RespawnRegion.Y = VisualizerGlobal.MapInfo.Respawns[i].Location.Y;
                RespawnRegion.Range = VisualizerGlobal.MapInfo.Respawns[i].Spread;
                RespawnRegion.Count.Text = VisualizerGlobal.MapInfo.Respawns[i].Count.ToString();
                RespawnRegion.Delay.Text = VisualizerGlobal.MapInfo.Respawns[i].Delay.ToString();

                RespawnPanel.Controls.Add(RespawnRegion);

                RespawnRegion.RegionHighlight.Parent = Canvas;
            }
        }

        private void ToolSelectedChanged(object sender, EventArgs e)
        {
            ToolStripButton[] ToolButtons = new ToolStripButton[] { SelectButton, AddButton, MoveButton, ResizeButton };
            foreach (var Tool in ToolButtons)
            {
                Tool.Checked = false;
            }

            ToolStripButton ToolSender = (ToolStripButton)sender;
            ToolSender.Checked = true;

            switch (ToolSender.Text)
            {
                case "Select Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Select;
                    break;
                case "Add Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Add;
                    break;
                case "Move Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Move;
                    break;
                case "Resize Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Resize;
                    break;
                default:
                    break;
            }
        }

        #region "START Mining Tool Bar"

        private void MiningSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                MineControl.Selected.Checked = true;
            }
        }

        private void MiningSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                MineControl.Selected.Checked = false;
            }
        }

        private void MiningRemoveSelected_Click(object sender, EventArgs e)
        {
            if (MiningPanel.Controls.Count == 0) return;

            DialogResult result = MessageBox.Show("Remove selected records?", "", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes) return;

            for (int i = MiningPanel.Controls.Count; i > -1; --i)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    if (MineControl.Selected.Checked == true)
                    {
                        MineControl.RemoveEntry();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        #endregion "END Mining Tool Bar"

        #region "START Respawn Tool Bar

        private void RespawnsSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                RespawnControl.Selected.Checked = true;
            }
        }

        private void RespawnsSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                RespawnControl.Selected.Checked = false;
            }
        }

        private void RespawnsRemoveSelected_Click(object sender, EventArgs e)
        {
            if (RespawnPanel.Controls.Count == 0) return;

            DialogResult result = MessageBox.Show("Remove selected records?", "", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes) return;

            for (int i = RespawnPanel.Controls.Count; i > -1; --i)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    if (RespawnControl.Selected.Checked == true)
                    {
                        RespawnControl.RemoveEntry();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        #endregion "END Respawn Tool Bar

        private void MapImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void MapImage_Click(object sender, EventArgs e)
        {
            if (RegionTabs.SelectedTab.Text == "Mining")
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Add)
                {
                    MineEntry MineControl = new MineEntry();
                    MineControl.Dock = DockStyle.Top;
                    MineControl.X = MouseDownLocation.X;
                    MineControl.Y = MouseDownLocation.Y;
                    MineControl.Range = 50;
                    MineControl.Show();
                    MiningPanel.Controls.Add(MineControl);

                    MineControl.RegionHighlight.Parent = Canvas;


                    ToolSelectedChanged(SelectButton, e);
                }

            if (RegionTabs.SelectedTab.Text == "Respawns")
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Add)
                {
                    RespawnEntry RespawnControl = new RespawnEntry();
                    RespawnControl.Dock = DockStyle.Top;
                    RespawnControl.X = MouseDownLocation.X;
                    RespawnControl.Y = MouseDownLocation.Y;
                    RespawnControl.Range = 50;
                    RespawnControl.Show();
                    RespawnPanel.Controls.Add(RespawnControl);

                    RespawnControl.RegionHighlight.Parent = Canvas;

                    ToolSelectedChanged(SelectButton, e);
                }
        }

        private void RegionTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count; i > -1; --i)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    MineControl.Hide();
                }
                catch (Exception)
                {
                    continue;
                }
            }

            for (int i = RespawnPanel.Controls.Count; i > -1; --i)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    RespawnControl.Hide();
                }
                catch (Exception)
                {
                    continue;
                }
            }

            if (RegionTabs.SelectedTab.Text == "Mining")
                for (int i = MiningPanel.Controls.Count; i > -1; --i)
                {
                    try
                    {
                        MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                        MineControl.Show();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

            if (RegionTabs.SelectedTab.Text == "Respawns")
                for (int i = RespawnPanel.Controls.Count; i > -1; --i)
                {
                    try
                    {
                        RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                        RespawnControl.Show();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
        }             
    }
}