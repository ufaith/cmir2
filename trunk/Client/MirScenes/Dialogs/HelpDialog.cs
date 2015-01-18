using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using S = ServerPackets;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class HelpDialog : MirImageControl
    {
        public List<HelpPage> Pages = new List<HelpPage>();

        public MirButton CloseButton, NextButton, PreviousButton;
        public MirLabel PageLabel;
        public HelpPage CurrentPage;

        public int CurrentPageNumber = 0;

        public HelpDialog()
        {
            Index = 920;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            
            Location = new Point((800 - Size.Width) / 2, (600 - Size.Height) / 2);

            MirImageControl TitleLabel = new MirImageControl
            {
                Index = 57,
                Library = Libraries.Title,
                Location = new Point(18, 5),
                Parent = this
            };

            PreviousButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(210, 485),
                Sound = SoundList.ButtonA,
            };
            PreviousButton.Click += (o, e) =>
            {
                CurrentPageNumber--;

                DisplayPage(CurrentPageNumber);
            };

            NextButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(310, 485),
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) =>
            {
                CurrentPageNumber++;

                DisplayPage(CurrentPageNumber);
            };

            PageLabel = new MirLabel
            {
                Text = "",
                Font = new Font(Settings.FontName, 9F),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Location = new Point(230, 480),
                Size = new Size(80, 20)
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(509, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            LoadPages();

            DisplayPage();
        }

        private void LoadPages()
        {
            Point location = new Point(12, 35);

            List<HelpPage> pages = new List<HelpPage> { 
                new HelpPage("Movements", 0, null) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("Attacking", 1, null) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("Collecting Items", 2, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Health", 3, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Skills", 4, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Skills", 5, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Mana", 6, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Chatting", 7, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Groups", 8, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Durability", 9, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Purchasing", 10, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Selling", 11, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Repairing", 12, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Trading", 13, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Inspecting", 14, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Statistics", 15, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Statistics", 16, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Statistics", 17, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Statistics", 18, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Statistics", 19, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Statistics", 20, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Quests", 21, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Quests", 22, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Quests", 23, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Quests", 24, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Mounts", 25, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Mounts", 26, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Fishing", 27, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("Gems and Orbs", 28, null) { Parent = this, Location = location, Visible = false },
            };

            Pages.AddRange(pages);
        }

        public void DisplayPage(string pageName)
        {
            if (Pages.Count < 1) return;

            for (int i = 0; i < Pages.Count; i++)
            {
                if (Pages[i].Title.ToLower() != pageName.ToLower()) continue;

                DisplayPage(i);
                break;
            }
        }

        public void DisplayPage(int id = 0)
        {
            if (Pages.Count < 1) return;

            if (id > Pages.Count - 1) id = Pages.Count - 1;
            if (id < 0) id = 0;

            if (CurrentPage != null)
                CurrentPage.Visible = false;

            CurrentPage = Pages[id];

            if (CurrentPage == null) return;

            CurrentPage.Visible = true;
            CurrentPageNumber = id;

            PageLabel.Text = string.Format("{0} / {1}", id + 1, Pages.Count);

            Show();
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }
    }

    public class HelpPage : MirControl
    {
        public string Title;
        public int ImageID;
        public MirControl Page;

        public MirLabel PageTitleLabel;

        public HelpPage(string title, int imageID, MirControl page)
        {
            Title = title;
            ImageID = imageID;
            Page = page;

            NotControl = true;
            Size = new System.Drawing.Size(508, 396 + 40);

            BeforeDraw += HelpPage_BeforeDraw;

            PageTitleLabel = new MirLabel
            {
                Text = Title,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new System.Drawing.Size(242, 30),
                Location = new Point(135, 4)
            };
        }

        void HelpPage_BeforeDraw(object sender, EventArgs e)
        {
            if (ImageID < 0) return;

            Libraries.Help.Draw(ImageID, new Point(DisplayLocation.X, DisplayLocation.Y + 40), Color.White, false);
        }
    }
}
