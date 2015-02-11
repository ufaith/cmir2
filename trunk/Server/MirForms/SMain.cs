﻿using System;
using System.Collections.Concurrent;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public partial class SMain : Form
    {
        public static readonly Envir Envir = new Envir(), EditEnvir = new Envir();
        private static readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<string> ChatLog = new ConcurrentQueue<string>();

        public SMain()
        {
            InitializeComponent();
            EditEnvir.LoadDB();
            Envir.Start();
        }
        public static void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
            MessageLog.Enqueue(String.Format("[{0}]: {1} - {2}" + Environment.NewLine, DateTime.Now, ex.TargetSite, ex));
        }

        public static void EnqueueDebugging(string msg)
        {
            if (DebugLog.Count < 100)
            DebugLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }
        public static void EnqueueChat(string msg)
        {
            if (ChatLog.Count < 100)
            ChatLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }
        public static void Enqueue(string msg)
        {
            if (MessageLog.Count < 100)
            MessageLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Text = string.Format("Total: {0}, Real: {1}", Envir.LastCount, Envir.LastRealCount);

                PlayersLabel.Text = string.Format("Players: {0}", Envir.Players.Count);
                MonsterLabel.Text = string.Format("Monsters: {0}", Envir.MonsterCount);
                ConnectionsLabel.Text = string.Format("Connections: {0}", Envir.Connections.Count);

                while (!MessageLog.IsEmpty)
                {
                    string message;

                    if (!MessageLog.TryDequeue(out message)) continue;

                    LogTextBox.AppendText(message);
                }

                while (!DebugLog.IsEmpty)
                {
                    string message;

                    if (!DebugLog.TryDequeue(out message)) continue;

                    DebugLogTextBox.AppendText(message);
                }

                while (!ChatLog.IsEmpty)
                {
                    string message;

                    if (!ChatLog.TryDequeue(out message)) continue;

                    ChatLogTextBox.AppendText(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.Start();
        }

        private void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.Stop();
            Envir.MonsterCount = 0;
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void SMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Envir.Stop();
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void closeServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void itemInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemInfoForm form = new ItemInfoForm();

            form.ShowDialog();
        }

        private void monsterInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonsterInfoForm form = new MonsterInfoForm();

            form.ShowDialog();
        }

        private void nPCInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NPCInfoForm form = new NPCInfoForm();

            form.ShowDialog();
        }

        private void dragonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void balanceConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BalanceConfigForm form = new BalanceConfigForm();

            form.ShowDialog();
        }

        private void questInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuestInfoForm form = new QuestInfoForm();

            form.ShowDialog();
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigForm form = new ConfigForm();

            form.ShowDialog();
        }

        private void balanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BalanceConfigForm form = new BalanceConfigForm();

            form.ShowDialog();
        }

        private void systemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountInfoForm form = new AccountInfoForm();

            form.ShowDialog();
        }

        private void mapInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapInfoForm form = new MapInfoForm();

            form.ShowDialog();
        }

        private void itemInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ItemInfoForm form = new ItemInfoForm();

            form.ShowDialog();
        }

        private void monsterInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MonsterInfoForm form = new MonsterInfoForm();

            form.ShowDialog();
        }

        private void nPCInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            NPCInfoForm form = new NPCInfoForm();

            form.ShowDialog();
        }

        private void questInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            QuestInfoForm form = new QuestInfoForm();

            form.ShowDialog();
        }

        private void dragonSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DragonInfoForm form = new DragonInfoForm();

            form.ShowDialog();
        }

        private void miningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MiningInfoForm form = new MiningInfoForm();

            form.ShowDialog();
        }

        private void guildsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuildInfoForm form = new GuildInfoForm();

            form.ShowDialog();
        }

        private void fishingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(0);

            form.ShowDialog();
        }
    }
}