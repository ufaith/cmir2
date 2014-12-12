using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class QuestProgressInfo
    {
        public int Index
        {
            get { return Info.Index; }
        }

        public QuestInfo Info;

        public DateTime StartDateTime = DateTime.MinValue;
        public DateTime EndDateTime = DateTime.MaxValue;

        public List<int> KillTaskCount = new List<int>();
        public List<long> ItemTaskCount = new List<long>();

        public List<string> TaskList = new List<string>();

        public bool Taken
        {
            get { return StartDateTime > DateTime.MinValue; }
        }

        public bool Completed
        {
            get { return EndDateTime < DateTime.MaxValue; }
        }


        public QuestProgressInfo(int index)
        {
            Info = SMain.Envir.QuestInfoList.FirstOrDefault(e => e.Index == index);

            if (Info == null) return;

            foreach (var kill in Info.KillTasks)
                KillTaskCount.Add(0);

            foreach (var item in Info.ItemTasks)
                ItemTaskCount.Add(0);

            CheckCompleted();
        }

        public QuestProgressInfo(BinaryReader reader)
        {
            int index = reader.ReadInt32();

            Info = SMain.Envir.QuestInfoList.FirstOrDefault(e => e.Index == index);

            StartDateTime = DateTime.FromBinary(reader.ReadInt64());
            EndDateTime = DateTime.FromBinary(reader.ReadInt64());

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                KillTaskCount.Add(reader.ReadInt32());

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                ItemTaskCount.Add(reader.ReadInt64());
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);

            writer.Write(StartDateTime.ToBinary());
            writer.Write(EndDateTime.ToBinary());

            writer.Write(KillTaskCount.Count);
            for (int i = 0; i < KillTaskCount.Count; i++)
                writer.Write(KillTaskCount[i]);

            writer.Write(ItemTaskCount.Count);
            for (int i = 0; i < ItemTaskCount.Count; i++)
                writer.Write(ItemTaskCount[i]);
        }

        public bool CheckCompleted()
        {
            UpdateTasks();

            bool canComplete = true;

            for (int i = 0; i < Info.KillTasks.Count; i++)
            {
                if (KillTaskCount[i] >= Info.KillTasks[i].Count) continue;

                canComplete = false;
            }

            for (int i = 0; i < Info.ItemTasks.Count; i++)
            {
                if (ItemTaskCount[i] >= Info.ItemTasks[i].Count) continue;

                canComplete = false;
            }

            if (!canComplete) return false;

            if (!Completed)
                EndDateTime = DateTime.Now;

            return true;
        }


        public bool NeedItem(UserItem item)
        {
            return Info.ItemTasks.Where((task, i) => ItemTaskCount[i] < task.Count && task.Item == item.Info).Any();
        }

        public bool NeedKill(MonsterInfo mInfo)
        {
            return Info.KillTasks.Where((task, i) => KillTaskCount[i] < task.Count && task.Monster == mInfo).Any();
        }

        public void ProcessKill(int mobIndex)
        {
            if (Info.KillTasks.Count < 1) return;

            for (int i = 0; i < Info.KillTasks.Count; i++)
            {
                if (Info.KillTasks[i].Monster.Index != mobIndex) continue;

                KillTaskCount[i]++;
                return;
            }
        }

        public void ProcessItem(UserItem[] inventory)
        {
            for (int i = 0; i < Info.ItemTasks.Count; i++)
            {
                long count = inventory.Where(item => item != null).
                    Where(item => item.Info == Info.ItemTasks[i].Item).
                    Aggregate<UserItem, long>(0, (current, item) => current + item.Count);

                ItemTaskCount[i] = count;
            }
        }

        public void UpdateTasks()
        {
            TaskList = new List<string>();

            UpdateKillTasks();
            UpdateItemTasks();
            UpdateGotoTask();
        }

        public void UpdateKillTasks()
        {
            for (int i = 0; i < Info.KillTasks.Count; i++)
            {
                if (Info.KillMessage.Length <= 0)
                    TaskList.Add(string.Format("Kill {0}: {1}/{2} {3}", Info.KillTasks[i].Monster.Name, KillTaskCount[i],
                        Info.KillTasks[i].Count, KillTaskCount[i] >= Info.KillTasks[i].Count ? "(Completed)" : ""));
                else
                    TaskList.Add(Info.KillMessage);
            }
        }

        public void UpdateItemTasks()
        {
            for (int i = 0; i < Info.ItemTasks.Count; i++)
            {
                if (Info.ItemMessage.Length <= 0)
                    TaskList.Add(string.Format("Collect {0}: {1}/{2} {3}", Info.ItemTasks[i].Item.Name, ItemTaskCount[i],
                        Info.ItemTasks[i].Count, ItemTaskCount[i] >= Info.ItemTasks[i].Count ? "(Completed)" : ""));
                else
                    TaskList.Add(Info.ItemMessage);
            }
        }

        public void UpdateGotoTask()
        {
            if (Info.GotoMessage.Length <= 0) return;

            TaskList.Add(Info.GotoMessage);
        }

        public ClientQuestProgress CreateClientQuestProgress()
        {
            return new ClientQuestProgress
            {
                Id = Index,
                TaskList = TaskList,
                Taken = Taken,
                Completed = Completed
            };
        }
    }
}
