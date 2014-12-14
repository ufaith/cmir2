﻿using System;
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

    public sealed class QuestListDialog : MirImageControl
    {
        private readonly MirButton _acceptButton, _finishButton;

        public List<ClientQuestProgress> Quests = new List<ClientQuestProgress>();

        public int SelectedIndex = 0;

        public ClientQuestProgress SelectedQuest;
        public QuestMessage Message;
        public QuestReward Reward;

        public QuestRow[] Rows = new QuestRow[5];

        public int StartIndex = 0;

        public uint CurrentNPCID = 0;

        public QuestListDialog()
        {
            Index = 950;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = new Point(GameScene.Scene.NPCDialog.Size.Width + 47, 0);

            #region QuestSelection

            MirButton upQuestButton = new MirButton
            {
                Index = 951,
                HoverIndex = 952,
                PressedIndex = 953,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(291, 35),
                Sound = SoundList.ButtonA,
            };
            upQuestButton.Click += (o, e) =>
            {
                if (SelectedQuest != null)
                {
                    SelectedIndex = FindSelectedIndex();

                    if (SelectedIndex > 0)
                        SelectedIndex--;
                    else
                        StartIndex--;
                }

                RefreshInterface();
            };

            MirButton downQuestButton = new MirButton
            {
                Index = 957,
                HoverIndex = 958,
                PressedIndex = 959,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(291, 83),
                Sound = SoundList.ButtonA,
            };
            downQuestButton.Click += (o, e) =>
            {
                if (SelectedQuest != null)
                {
                    SelectedIndex = FindSelectedIndex();

                    if (SelectedIndex < Rows.Length - 1)
                        SelectedIndex++;
                    else
                        StartIndex++;
                }

                RefreshInterface();
            };
            #endregion

            #region Buttons

            _acceptButton = new MirButton
            {
                Index = 270,
                HoverIndex = 271,
                PressedIndex = 272,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(40, 437),
                Sound = SoundList.ButtonA,
            };
            _acceptButton.Click += (o, e) =>
            {
                if (Reward == null || SelectedQuest.Taken) return;

                Network.Enqueue(new C.AcceptQuest { NPCIndex = SelectedQuest.QuestInfo.NPCIndex, QuestIndex = SelectedQuest.QuestInfo.Index });
                //Hide();
            };

            _finishButton = new MirButton
            {
                Index = 273,
                HoverIndex = 274,
                PressedIndex = 275,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(40, 437),
                Sound = SoundList.ButtonA,
                Visible = false
            };
            _finishButton.Click += (o, e) =>
            {
                if (Reward == null || !SelectedQuest.Completed) return;

                if (Reward.SelectedItemIndex < 0 && SelectedQuest.QuestInfo.RewardsSelectItem.Count > 0)
                {
                    MirMessageBox messageBox = new MirMessageBox("You must select a reward item.");
                    messageBox.Show();
                    return;
                }

                Network.Enqueue(new C.FinishQuest { QuestIndex = SelectedQuest.QuestInfo.Index, SelectedItemIndex = Reward.SelectedItemIndex });
                //Hide();
            };

            MirButton leaveButton = new MirButton
            {
                Index = 276,
                HoverIndex = 277,
                PressedIndex = 278,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(205, 437),
                Sound = SoundList.ButtonA,
            };
            leaveButton.Click += (o, e) => Hide();

            #endregion

            #region Message Area

            MirButton upButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(292, 136),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            MirButton downButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                PressedIndex = 209,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(292, 282),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            MirButton positionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(292, 149),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = false
            };

            Message = new QuestMessage(upButton, downButton, positionBar, 10)
            {
                Font = new Font(Settings.FontName, 9F),
                Parent = this,
                Size = new Size(280, 160),
                Location = new Point(10, 135),
                PosMinY = 149,
                PosMaxY = 263
            };

            #endregion

            #region Rewards

            Reward = new QuestReward
            {
                Parent = this,
                Visible = false,
                Size = new Size(313, 130),
                Location = new Point(5, 307)
            };

            #endregion

            MirButton closeButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(289, 3),
                Sound = SoundList.ButtonA,
            };
            closeButton.Click += (o, e) => Hide();

        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;

            CurrentNPCID = GameScene.NPCID;

            Reset();

            DisplayInfo();
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;

            GameScene.Scene.NPCDialog.Hide();
        }

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }

        public void DisplayInfo()
        {
            if (!GetAvailableQuests())
            {
                Hide();
                return;
            }
            Reset();
            RefreshInterface();
        }

        public void Reset()
        {
            StartIndex = 0;
            SelectedIndex = 0;
            SelectedQuest = null;

            Message.Visible = false;
            Reward.Visible = false;

            for (int i = 0; i < Rows.Length; i++)
            {
                if(Rows[i] != null) Rows[i].Dispose();

                Rows[i] = null;
            }
        }

        public bool GetAvailableQuests()
        {
            NPCObject npc = (NPCObject)MapControl.GetObject(CurrentNPCID);
            if (npc != null)
            {
                Quests = npc.GetAvailableQuests();
            }

            return Quests.Count >= 1 && npc != null;
        }

        public void RefreshInterface()
        {
            int maxIndex = Quests.Count - Rows.Length;

            if (StartIndex > maxIndex) StartIndex = maxIndex;
            if (StartIndex < 0) StartIndex = 0;

            for (int i = 0; i < Rows.Length; i++)
            {
                if (i >= Quests.Count) break;

                if (Rows[i] != null)
                    Rows[i].Dispose();

                Rows[i] = new QuestRow
                {
                    Quest = Quests[i + StartIndex],
                    Location = new Point(9, 36 + i * 19),
                    Parent = this,
                };
                Rows[i].Click += (o, e) =>
                {
                    QuestRow row = (QuestRow) o;

                    if (row.Quest != SelectedQuest)
                    {
                        SelectedQuest = row.Quest;
                        Reward.UpdateRewards(SelectedQuest);
                        Message.UpdateQuest(SelectedQuest);
                        SelectedIndex = FindSelectedIndex();
                        UpdateRows();

                        ReDisplayButtons();
                    }
                };

                if (SelectedQuest != null)
                {
                    if (SelectedIndex == i)
                    {
                        SelectedQuest = Rows[i].Quest;
                        Reward.UpdateRewards(SelectedQuest);
                        Message.UpdateQuest(SelectedQuest);
                    }
                }
            }

            UpdateRows();

            ReDisplayButtons();
        }

        public void UpdateRows()
        {
            if (SelectedQuest == null)
            {
                if (Rows[0] == null) return;

                SelectedQuest = Rows[0].Quest;
                Reward.UpdateRewards(SelectedQuest);
                Message.UpdateQuest(SelectedQuest);
            }

            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i] == null) continue;

                Rows[i].Selected = false;

                if (Rows[i].Quest == SelectedQuest)
                {
                    Rows[i].Selected = true;
                }

                Rows[i].UpdateInterface();
            }
        }

        public void ReDisplayButtons()
        {
            _acceptButton.Visible = false;
            _finishButton.Visible = false;

            if (Reward != null)
            {
                Reward.Visible = true;
                Message.Visible = true;
            }

            if (SelectedQuest != null)
            {
                if (!SelectedQuest.Taken && MapControl.User.CurrentQuests.Count < Globals.MaxConcurrentQuests)
                    _acceptButton.Visible = true;

                if (SelectedQuest.Completed && Reward != null)
                    _finishButton.Visible = true;
            }
        }

        public int FindSelectedIndex()
        {
            int selectedIndex = 0;
            if (SelectedQuest != null)
            {
                for (int i = 0; i < Rows.Length; i++)
                {
                    if (Rows[i] == null || SelectedQuest != Rows[i].Quest) continue;

                    selectedIndex = i;
                }
            }

            return selectedIndex;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            Quests.Clear();

            SelectedQuest = null;
            Message = null;
            Reward = null;

            foreach (QuestRow row in Rows.Where(row => row != null && !row.IsDisposed))
            {
                row.Dispose();
            }
        }

    }
    public sealed class QuestDetailDialog : MirImageControl
    {
        private readonly MirButton _shareButton, _pauseButton, _cancelButton;

        public ClientQuestProgress Quest;
        public QuestMessage Message;
        public QuestReward Reward;

        public QuestDetailDialog()
        {
            Index = 960;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = new Point(Settings.ScreenWidth / 2 + 20, 60);

            #region Message Area

            MirButton upButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(293, 33),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            MirButton downButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                PressedIndex = 209,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(293, 280),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            MirButton positionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(293, 48),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = false
            };

            Message = new QuestMessage(upButton, downButton, positionBar, 16, true)
            {
                Font = new Font(Settings.FontName, 9F),
                Parent = this,
                Size = new Size(280, 320),
                Location = new Point(10, 35),
                PosMinY = 46,
                PosMaxY = 261
            };

            #endregion

            #region Rewards

            Reward = new QuestReward
            {
                Parent = this,
                Size = new Size(315, 130),
                Location = new Point(5, 307)
            };

            #endregion

            #region Buttons

            _shareButton = new MirButton
            {
                Index = 270,
                HoverIndex = 271,
                PressedIndex = 272,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(40, 437),
                Sound = SoundList.ButtonA,
                Visible = false
            };

            _pauseButton = new MirButton
            {
                Index = 270,
                HoverIndex = 271,
                PressedIndex = 272,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(120, 437),
                Sound = SoundList.ButtonA,
                Visible = false
            };

            _cancelButton = new MirButton
            {
                Index = 270,
                HoverIndex = 271,
                PressedIndex = 272,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(200, 437),
                Sound = SoundList.ButtonA,
                Visible = false
            };

            #endregion

            MirButton closeButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(289, 3),
                Sound = SoundList.ButtonA,
            };
            closeButton.Click += (o, e) => Hide();
        }

        public void DisplayQuestDetails(ClientQuestProgress quest)
        {
            if (quest == null) return;

            Quest = quest;
            Reward.UpdateRewards(Quest);
            Message.UpdateQuest(Quest);

            Show();
        }

        private void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
    }
    public sealed class QuestRow : MirControl
    {
        public ClientQuestProgress Quest;
        public MirLabel NameLabel, RequirementLabel;
        public MirImageControl SelectedImage, IconImage;

        public bool Selected = false;

        public QuestRow()
        {
            Sound = SoundList.ButtonA;
            Size = new Size(200, 17);

            BeforeDraw += QuestRow_BeforeDraw;

            SelectedImage = new MirImageControl
            {
                Index = 956,
                Library = Libraries.Prguse,
                Location = new Point(25, 0),
                Parent = this,
                Visible = false
            };

            IconImage = new MirImageControl
            {
                Index = 0,
                Library = Libraries.Prguse,
                Location = new Point(3, 0),
                Parent = this,
                Visible = false
            };

            RequirementLabel = new MirLabel
            {
                Location = new Point(20, 0),
                Size = new Size(178, 17),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
            };

            NameLabel = new MirLabel
            {
                Location = new Point(60, 0),
                Size = new Size(140, 17),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
            };

            UpdateInterface();
        }

        void QuestRow_BeforeDraw(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        public void UpdateInterface()
        {
            if (Quest == null) return;

            int iconTypeOffset = (int)Quest.Icon > 3 ? 15 : 0;

            IconImage.Index = 961 + (int)Quest.Icon + iconTypeOffset;
            IconImage.Visible = true;

            NameLabel.Text = Quest.QuestInfo.Name;
            RequirementLabel.Text = Quest.QuestInfo.LevelNeeded > 0 ? "Lv " + Quest.QuestInfo.LevelNeeded : "";

            SelectedImage.Visible = Selected;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Quest = null;
            NameLabel = null;
            RequirementLabel = null;
            SelectedImage = null;
            IconImage = null;

            Selected = false;
        }
    }
    public sealed class QuestMessage : MirControl
    {
        public ClientQuestProgress Quest;
        public MirButton ScrollUpButton, ScrollDownButton, PositionBar;

        private static readonly Regex C = new Regex(@"{(.*?/.*?)}");

        private readonly MirLabel[] _textLabel;
        private readonly List<MirLabel> _textButtons = new List<MirLabel>();

        public int TopLine;
        public int BottomLine;
        public int LineCount = 10;
        public bool DisplayProgress;

        public int PosX, PosMinY, PosMaxY;

        public Font Font = new Font(Settings.FontName, 8F);
        public List<string> CurrentLines = new List<string>();

        private const string TaskTitle = "Tasks", ProgressTitle = "Progress";

        public QuestMessage(MirButton scrollUpButton, MirButton scrollDownButton, MirButton positionBar, int lineCount, bool displayProgress = false)
        {
            ScrollUpButton = scrollUpButton;
            ScrollDownButton = scrollDownButton;
            PositionBar = positionBar;
            DisplayProgress = displayProgress;

            MouseWheel += QuestMessage_MouseWheel;
            PositionBar.OnMoving += PositionBar_OnMoving;

            LineCount = lineCount;
            _textLabel = new MirLabel[LineCount];

            PosX = PositionBar.Location.X;
            PosMinY = PositionBar.Location.Y;
            PosMaxY = PositionBar.Location.Y;

            ScrollUpButton.Click += (o, e) =>
            {
                if (TopLine <= 0) return;

                TopLine--;

                NewText(CurrentLines, false);
                UpdatePositionBar();
            };

            ScrollDownButton.Click += (o, e) =>
            {
                if (TopLine + LineCount >= CurrentLines.Count) return;

                TopLine++;

                NewText(CurrentLines, false);
                UpdatePositionBar();
            };

            AfterDraw += QuestMessage_AfterDraw;
        }

        private void QuestMessage_AfterDraw(object sender, EventArgs e)
        {
            if (Quest == null || CurrentLines.Count < 1) return;

            int adjust = 0;

            for (int i = TopLine; i < BottomLine; i++)
            {
                if (i != 0 && CurrentLines[i] != TaskTitle && CurrentLines[i] != ProgressTitle) continue;

                Libraries.Prguse.Draw(919, new Point(DisplayLocation.X + 5, DisplayLocation.Y + 5 + (i - TopLine) * 15 + adjust), Color.White);

                adjust += 5;
            }
        }

        private void QuestMessage_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (TopLine == 0 && count >= 0) return;
            if (TopLine == CurrentLines.Count - 1 && count <= 0) return;
            if (CurrentLines.Count <= LineCount) return;

            TopLine -= count;

            if (TopLine < 0) TopLine = 0;
            if (TopLine + LineCount > CurrentLines.Count - 1) TopLine = CurrentLines.Count - LineCount;

            NewText(CurrentLines, false);

            UpdatePositionBar();
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = PosX;
            int y = PositionBar.Location.Y;

            if (y >= PosMaxY) y = PosMaxY;
            if (y <= PosMinY) y = PosMinY;

            int location = y - PosMinY;
            int interval = (PosMaxY - PosMinY) / (CurrentLines.Count - LineCount);

            double yPoint = (double)location / interval;

            TopLine = Convert.ToInt16(Math.Floor(yPoint));

            PositionBar.Location = new Point(x, y);

            NewText(CurrentLines, false);
        }

        private void UpdatePositionBar()
        {
            if (CurrentLines.Count <= LineCount)
            {
                PositionBar.Visible = false;
                return;
            }

            PositionBar.Visible = true;

            int interval = (PosMaxY - PosMinY) / (CurrentLines.Count - LineCount);

            int x = PosX;
            int y = PosMinY + (TopLine * interval);

            if (y >= PosMaxY) y = PosMaxY;
            if (y <= PosMinY) y = PosMinY;


            PositionBar.Location = new Point(x, y);
        }

        public void UpdateQuest(ClientQuestProgress quest)
        {
            Quest = quest;

            CurrentLines.Clear();

            //add quest title at the beginning
            CurrentLines.Insert(0, Quest.QuestInfo.Name);

            if (Quest.Taken && !Quest.QuestInfo.SameFinishNPC && Quest.QuestInfo.CompletionDescription.Count > 0 && GameScene.Scene.QuestListDialog.CurrentNPCID == Quest.QuestInfo.FinishNPCIndex)
            {
                foreach (var line in Quest.QuestInfo.CompletionDescription)
                {
                    CurrentLines.Add(line);
                }
            }
            else
            {
                foreach (var line in Quest.QuestInfo.Description)
                {
                    CurrentLines.Add(line);
                }

                AdjustDescription();
            }

            NewText(CurrentLines);
        }

        private void AdjustDescription()
        {
            if (Quest.QuestInfo.TaskDescription.Count > 0)
            {
                CurrentLines.Add(" ");

                CurrentLines.Add(TaskTitle);

                foreach (string task in Quest.QuestInfo.TaskDescription)
                {
                    CurrentLines.Add(task);
                }
            }

            if (Quest.Taken && Quest.TaskList.Count > 0 && DisplayProgress)
            {
                CurrentLines.Add(" ");

                CurrentLines.Add(ProgressTitle);

                foreach (string task in Quest.TaskList)
                {
                    CurrentLines.Add(task);
                }
            }
        }

        private void NewText(List<string> lines, bool resetIndex = true)
        {
            if (resetIndex)
            {
                TopLine = 0;
                CurrentLines = lines;
                UpdatePositionBar();
            }

            foreach (MirLabel t in _textButtons.Where(t => t != null))
                t.Dispose();

            foreach (MirLabel t in _textLabel.Where(t => t != null))
                t.Text = "";

            _textButtons.Clear();

            int adjust = 0;

            BottomLine = lines.Count > LineCount ? ((LineCount + TopLine) > lines.Count ? lines.Count : (LineCount + TopLine)) : lines.Count;

            for (int i = TopLine; i < BottomLine; i++)
            {
                Font font = Font;
                Color fontColor = Color.White;
                bool title = false;

                if (i == 0 || lines[i] == TaskTitle || lines[i] == ProgressTitle)
                {
                    font = new Font(Settings.FontName, 10F, FontStyle.Bold);
                    fontColor = Color.Yellow;
                    title = true;
                }

                _textLabel[i - TopLine] = new MirLabel
                {
                    Font = font,
                    ForeColour = fontColor,
                    DrawFormat = TextFormatFlags.WordBreak,
                    Visible = true,
                    Parent = this,
                    Size = new Size(Size.Width, 20),
                    Location = new Point(title ? 15 : 0, 0 + (i - TopLine) * 15 + adjust),
                    NotControl = true
                };

                if (title)
                {
                    adjust += 5;
                }

                if (i >= lines.Count)
                {
                    _textLabel[i - TopLine].Text = string.Empty;
                    continue;
                }

                string currentLine = lines[i];

                List<Match> matchList = C.Matches(currentLine).Cast<Match>().ToList();

                int oldLength = currentLine.Length;

                foreach (Match match in matchList.OrderBy(o => o.Index).ToList())
                {
                    int offSet = oldLength - currentLine.Length;

                    Capture capture = match.Groups[1].Captures[0];
                    string[] values = capture.Value.Split('/');
                    currentLine = currentLine.Remove(capture.Index - 1 - offSet, capture.Length + 2).Insert(capture.Index - 1 - offSet, values[0]);
                    string text = currentLine.Substring(0, capture.Index - 1 - offSet) + " ";
                    Size size = TextRenderer.MeasureText(CMain.Graphics, text, _textLabel[i - TopLine].Font, _textLabel[i - TopLine].Size, TextFormatFlags.TextBoxControl);

                    if (C.Match(match.Value).Success)
                        NewColour(values[0], values[1], _textLabel[i - TopLine].Location.Add(new Point(size.Width - 10, 0)));
                }

                _textLabel[i - TopLine].Text = currentLine;
                _textLabel[i - TopLine].MouseWheel += QuestMessage_MouseWheel;
            }
        }

        private void NewColour(string text, string colour, Point p)
        {
            Color textColour = Color.FromName(colour);

            MirLabel temp = new MirLabel
            {
                AutoSize = true,
                Visible = true,
                Parent = this,
                Location = p,
                Text = text,
                ForeColour = textColour,
                Font = Font
            };
            temp.MouseWheel += QuestMessage_MouseWheel;

            _textButtons.Add(temp);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Quest = null;
            ScrollUpButton = null;
            ScrollDownButton = null;
            PositionBar = null;

            CurrentLines.Clear();
        }
    }
    public sealed class QuestReward : MirControl
    {
        private readonly MirLabel _goldLabel,  _expLabel;

        public ClientQuestProgress Quest;

        public int SelectedItemIndex = -1;
        public ItemInfo SelectedItem = null;

        public static QuestCell[] FixedItems = new QuestCell[5];
        public static QuestCell[] SelectItems = new QuestCell[5];

        public QuestReward()
        {
            _expLabel = new MirLabel
            {
                Size = new Size(75, 20),
                Location = new Point(40, 0),
                Parent = this
            };

            _goldLabel = new MirLabel
            {
                Size = new Size(75, 20),
                Parent = this
            };

            BeforeDraw += QuestReward_BeforeDraw;
        }

        public void UpdateRewards(ClientQuestProgress quest)
        {
            Quest = quest;

            CleanRewards();
            UpdateInterface();
            Redraw();
        }

        void QuestReward_BeforeDraw(object sender, EventArgs e)
        {
            if (Quest == null) return;

            ClientQuestInfo quest = Quest.QuestInfo;

            int goldXOffset = 0;

            if (quest.RewardExp > 0)
                Libraries.Prguse.Draw(966, DisplayLocation.X + 10, DisplayLocation.Y);
            else
                goldXOffset = -90;

            if (quest.RewardGold > 0)
                Libraries.Prguse.Draw(965, DisplayLocation.X + 100 + goldXOffset, DisplayLocation.Y);

            Libraries.Title.Draw(17, DisplayLocation.X + 20, DisplayLocation.Y + 67);
        }


        public void CleanRewards()
        {
            foreach (QuestCell item in SelectItems.Where(item => item != null))
            {
                item.Dispose();
            }

            foreach (QuestCell item in FixedItems.Where(item => item != null))
            {
                item.Dispose();
            }

            SelectedItemIndex = -1;
            SelectedItem = null;
        }

        public void UpdateInterface()
        {
            ClientQuestInfo quest = Quest.QuestInfo;

            _goldLabel.Visible = false;
            _expLabel.Visible = false;

            int goldXOffset = 0;

            if (quest.RewardExp > 0)
            {
                _expLabel.Text = quest.RewardExp.ToString();
                _expLabel.Visible = true;
            }
            else
                goldXOffset = -90;

            if (quest.RewardGold > 0)
            {
                _goldLabel.Text = quest.RewardGold.ToString();
                _goldLabel.Location = new Point(120 + goldXOffset, 0);
                _goldLabel.Visible = true;
            }

            if (quest.RewardsFixedItem.Count > 0)
            {
                for (int i = 0; i < FixedItems.Length; i++)
                {
                    if (i >= quest.RewardsFixedItem.Count) break;

                    FixedItems[i] = new QuestCell
                    {
                        Item = quest.RewardsFixedItem[i],
                        Parent = this,
                        Location = new Point(i * 45 + 15, 24),
                        Fixed = true
                    };
                }
            }

            if (quest.RewardsSelectItem.Count > 0)
            {
                for (int i = 0; i < SelectItems.Length; i++)
                {
                    if (i >= quest.RewardsSelectItem.Count) break;

                    SelectItems[i] = new QuestCell
                    {
                        Item = quest.RewardsSelectItem[i],
                        Parent = this,
                        Location = new Point(i * 45 + 15, 89),
                    };
                    SelectItems[i].Click += (o, e) =>
                    {
                        foreach (var itm in SelectItems)
                        {
                            if (itm == null) continue;

                            if (itm == o)
                            {
                                itm.Selected = true;
                                SelectedItem = itm.Item;
                                SelectedItemIndex = FindSelectedItemIndex();
                            }
                            else
                                itm.Selected = false;
                        }

                        Redraw();
                    };
                }
            }
        }

        public int FindSelectedItemIndex()
        {
            int selectedItemIndex = 0;
            if (SelectedItem == null) return selectedItemIndex;

            for (int i = 0; i < Quest.QuestInfo.RewardsSelectItem.Count; i++)
            {
                ItemInfo item = Quest.QuestInfo.RewardsSelectItem[i];

                if (item == null || SelectedItem != item) continue;

                selectedItemIndex = i;
            }

            return selectedItemIndex;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Quest = null;

            foreach (QuestCell cell in FixedItems.Where(cell => cell != null && !cell.IsDisposed))
            {
                cell.Dispose();
            }

            foreach (QuestCell cell in SelectItems.Where(cell => cell != null && !cell.IsDisposed))
            {
                cell.Dispose();
            }
        }

    }
    public sealed class QuestCell : MirControl
    {
        public ItemInfo Item;
        public UserItem ShowItem;

        public bool Selected;
        public bool Fixed;

        public QuestCell()
        {
            Size = new Size(32, 32);

            AfterDraw += (o, e) => DrawItem();
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();

            if (ShowItem == null) ShowItem = new UserItem(Item) { MaxDura = Item.Durability, CurrentDura = Item.Durability };

            GameScene.Scene.CreateItemLabel(ShowItem);
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            GameScene.Scene.DisposeItemLabel();
            GameScene.HoverItem = null;
            ShowItem = null;
        }

        private void DrawItem()
        {
            if (Item == null) return;

            Size size = Libraries.Items.GetTrueSize(Item.Image);
            Point offSet = new Point((40 - size.Width) / 2, (32 - size.Height) / 2);

            if (Fixed)
            {
                Libraries.Prguse.Draw(989, DisplayLocation.X, DisplayLocation.Y - 1);
            }
            else if (Selected)
            {
                Libraries.Prguse.Draw(979, DisplayLocation.X, DisplayLocation.Y - 5);
            }

            Libraries.Items.Draw(Item.Image, offSet.X + DisplayLocation.X, offSet.Y + DisplayLocation.Y);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Item = null;
            ShowItem = null;

        }
    }


    public sealed class QuestLogDialog : MirImageControl
    {
        public List<ClientQuestProgress> Quests = new List<ClientQuestProgress>();
        public List<QuestGroupQuestItem> TaskGroups = new List<QuestGroupQuestItem>();

        public List<string> ExpandedGroups = new List<string>(); 

        public QuestLogDialog()
        {
            Index = 961;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = new Point(Settings.ScreenWidth / 2 - 300 - 20, 60);

            MirButton closeButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(289, 3),
                Sound = SoundList.ButtonA,
            };
            closeButton.Click += (o, e) => Hide();
        }

        public void DisplayQuests()
        {
            ClearLog();

            Quests = GameScene.User.CurrentQuests;

            var groupedQuests = Quests.GroupBy(d => d.QuestInfo.Group).ToList();

            int nextY = 40;

            foreach (var group in groupedQuests)
            {
                List<ClientQuestProgress> singleGroup = @group.ToList();

                bool expanded = ExpandedGroups.Count <= 0 || ExpandedGroups.Contains(@group.Key);

                QuestGroupQuestItem groupQuest = new QuestGroupQuestItem(group.Key, singleGroup, expanded)
                {
                    Parent = this,
                    Visible = true,
                    Location = new Point(15, nextY), 
                };
                groupQuest.ExpandedChanged += (o, e) =>
                {
                    nextY = 40;

                    foreach (QuestGroupQuestItem task in TaskGroups)
                    {
                        task.Location = new Point(15, nextY);
                        nextY += task.SizeY;

                        if (task.Expanded)
                        {
                            if (!ExpandedGroups.Contains(task.Group)) 
                                ExpandedGroups.Add(task.Group);     
                        }                          
                        else
                            ExpandedGroups.Remove(task.Group);
                    }
                };
                groupQuest.SelectedQuestChanged += (o, e) =>
                {
                    QuestSingleQuestItem singleQuestItem = (QuestSingleQuestItem)o;

                    if (singleQuestItem == null) return;

                    foreach (QuestGroupQuestItem item in TaskGroups)
                    {
                        item.DeselectQuests();
                    }

                    singleQuestItem.Selected = true;
                };

                nextY += groupQuest.SizeY;

                TaskGroups.Add(groupQuest);
            }
        }

        public void ClearLog()
        {
            foreach (QuestGroupQuestItem taskGroupItem in TaskGroups)
            {
                taskGroupItem.Dispose();
            }
            TaskGroups.Clear();
        }
        public void Show()
        {
            if (Visible) return;
            Visible = true;

            DisplayQuests();
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Quests.Clear();
            ClearLog();
        }
    }
    public sealed class QuestGroupQuestItem : MirControl
    {
        public string Group = string.Empty;
        public List<ClientQuestProgress> Quests = new List<ClientQuestProgress>();

        public bool Expanded = true;
        public int SizeY = 15;

        private readonly MirButton _expandButton;
        private readonly MirLabel _groupLabel;
        private readonly List<QuestSingleQuestItem> _tasks = new List<QuestSingleQuestItem>();

        public event EventHandler ExpandedChanged;
        private void OnExpandedChanged()
        {
            if (ExpandedChanged != null)
                ExpandedChanged.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler SelectedQuestChanged;
        private void OnSelectedQuestChanged(object ob)
        {
            if (SelectedQuestChanged != null)
                SelectedQuestChanged.Invoke(ob, EventArgs.Empty);
        }

        public QuestGroupQuestItem(string group, List<ClientQuestProgress> quests, bool expanded)
        {
            Group = group;
            Quests = quests;
            Expanded = expanded;

            _expandButton = new MirButton
            {
                Index = Expanded ? 917 : 918,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(0, 0),
                Sound = SoundList.ButtonA
            };
            _expandButton.Click += (o, e) => ChangeExpand();

            _groupLabel = new MirLabel
            {
                Text = Group,
                AutoSize = true,
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.DodgerBlue,
                Location = new Point(18, 0),
                Visible = true,
            };

            for (int i = 0; i < Quests.Count; i++)
            {
                QuestSingleQuestItem singleQuest = new QuestSingleQuestItem(Quests[i])
                {
                    Parent = this,
                    Location = new Point(18, (15 * (i + 1))),
                    Size = new Size(280, 15),
                    Visible = Expanded
                };
                singleQuest.SelectedQuestChanged += (o, e) => OnSelectedQuestChanged(o);

                _tasks.Add(singleQuest);

                if (Expanded)
                    SizeY += 15;
            }

            Size = new Size(280, SizeY);
        }

        public void UpdatePositions()
        {
            SizeY = 15;

            foreach (var singleTask in _tasks)
            {
                singleTask.Visible = Expanded;

                if (Expanded) SizeY += 15;
            }

            Size = new Size(280, SizeY);
        }

        public void ClearTasks()
        {
            foreach (QuestSingleQuestItem task in _tasks)
            {
                task.Dispose();
            }
            _tasks.Clear();
        }

        public void DeselectQuests()
        {
            foreach (QuestSingleQuestItem task in _tasks)
            {
                task.Selected = false;
            }
        }

        private void ChangeExpand()
        {
            Expanded = !Expanded;

            _expandButton.Index = Expanded ? 917 : 918;

            UpdatePositions();

            OnExpandedChanged();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Group = string.Empty;
            Quests.Clear();

            _expandButton.Dispose();
            _groupLabel.Dispose();

            ClearTasks();
        }
    }
    public sealed class QuestSingleQuestItem : MirControl
    {
        private MirLabel _questLabel;
        private readonly MirImageControl _selectedImage;

        public ClientQuestProgress Quest;

        public bool Selected;
        public bool TrackQuest;

        public event EventHandler SelectedQuestChanged;
        private void OnSelectedQuestChanged()
        {
            if (SelectedQuestChanged != null)
                SelectedQuestChanged.Invoke(this, EventArgs.Empty);
        }

        public QuestSingleQuestItem(ClientQuestProgress quest)
        {
            Quest = quest;
            Size = new Size(250, 15);
            TrackQuest = GameScene.Scene.QuestTrackingDialog.TrackedQuestsIds.Contains(quest.Id);

            string name = Quest.QuestInfo.Name;
            string level = string.Format("Lv {0}", Quest.QuestInfo.LevelNeeded);
            string state = quest.Completed ? "(Complete)" : "(Waiting)"; //not in use

            BeforeDraw += QuestTaskSingleItem_BeforeDraw;
            AfterDraw += QuestTaskSingleItem_AfterDraw;

            _selectedImage = new MirImageControl
            {
                Index = 956,
                Library = Libraries.Prguse,
                Location = new Point(-10, 0),
                Parent = this,
                Visible = false
            };

            _questLabel = new MirLabel
            {
                Text = string.Format("{0,-4} {1}", level, name),
                AutoSize = true,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Parent = this,
                Location = new Point(0, 0),
                Sound = SoundList.ButtonA
            };

            _questLabel.Click += (o, e) =>
            {
                MouseEventArgs me = e as MouseEventArgs;

                if (me == null) return;

                switch (me.Button)
                {
                    case MouseButtons.Left:
                        GameScene.Scene.QuestDetailDialog.DisplayQuestDetails(Quest);
                        break;
                    case MouseButtons.Right:
                        TrackQuest = !TrackQuest;
                        break;
                }

                OnSelectedQuestChanged();

                if (TrackQuest)
                {
                    GameScene.Scene.QuestTrackingDialog.AddQuest(Quest);

                    if (GameScene.Scene.QuestTrackingDialog.TrackedQuestsIds.Count <= 5) return;

                    TrackQuest = false;
                    GameScene.Scene.QuestTrackingDialog.RemoveQuest(Quest);
                }
                else
                {
                    GameScene.Scene.QuestTrackingDialog.RemoveQuest(Quest);
                }
            };
        }

        void QuestTaskSingleItem_BeforeDraw(object sender, EventArgs e)
        {
            _selectedImage.Visible = Selected;
        }

        void QuestTaskSingleItem_AfterDraw(object sender, EventArgs e)
        {
            if (TrackQuest)
            {
                Libraries.Prguse.Draw(997, new Point(DisplayLocation.X - 15, DisplayLocation.Y), Color.White);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Quest = null;
            _questLabel.Dispose();
            _questLabel = null;
        }
    }


    public sealed class QuestTrackingDialog : MirImageControl
    {
        public List<int> TrackedQuestsIds = new List<int>(); 
        public List<MirLabel> TaskLines = new List<MirLabel>();

        public Font QuestFont = new Font(Settings.FontName, 8F);

        private MirLabel _questNameLabel, _questTaskLabel;

        public QuestTrackingDialog()
        {
            Movable = true;
            Location = new Point(0, 100);
            Sort = false;
            Size = new Size(150, 150);
        }

        public void DisplayQuests()
        {
            foreach (MirLabel label in TaskLines)
                label.Dispose();

            List<ClientQuestProgress> questsToTrack = new List<ClientQuestProgress>();

            questsToTrack.Clear();

            questsToTrack.AddRange(from quest in MapObject.User.CurrentQuests from id in TrackedQuestsIds.Where(id => quest.Id == id) select quest);

            if (questsToTrack.Count < 1)
            {
                Hide();
                return;
            }

            int y = 0;

            for (int i = 0; i < questsToTrack.Count; i++)
            {
                _questNameLabel = new MirLabel
                {
                    Text = questsToTrack[i].QuestInfo.Name,
                    AutoSize = true,
                    BackColour = Color.Transparent,
                    Font = QuestFont,
                    ForeColour = Color.DodgerBlue,
                    Location = new Point(20, 20 + y),
                    OutLine = true,
                    Parent = this,
                    Visible = true,
                };

                TaskLines.Add(_questNameLabel);

                foreach (string questToTrack in questsToTrack[i].TaskList)
                {
                    y += 15;

                    string trackedQuest = questToTrack;

                    _questTaskLabel = new MirLabel
                    {
                        Text = trackedQuest,
                        AutoSize = true,
                        BackColour = Color.Transparent,
                        Font = QuestFont,
                        ForeColour = Color.White, //trackedQuest.Contains("(Completed)") ? Color.LimeGreen : 
                        Location = new Point(40, 20 + y),
                        OutLine = true,
                        Parent = this,
                        Visible = true,
                    };

                    TaskLines.Add(_questTaskLabel);
                }

                if (i >= 5) break;

                y += 30;
            }

            Show();
        }

        public void AddQuest(ClientQuestProgress quest)
        {
            if (TrackedQuestsIds.Any(d => d == quest.Id)) return;

            TrackedQuestsIds.Add(quest.Id);

            DisplayQuests();
        }

        public void RemoveQuest(ClientQuestProgress quest)
        {
            TrackedQuestsIds.Remove(quest.Id);

            DisplayQuests();
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }
    }


}
