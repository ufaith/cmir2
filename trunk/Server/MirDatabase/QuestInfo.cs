using System.Drawing;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Server.MirDatabase
{
    #region Notes
    //kill, item shown as yellow notification top left if quest not on screen
    #endregion

    public class QuestInfo
    {
        public int Index;

        public uint NpcIndex;
        public NPCInfo NpcInfo;

        private uint _finishNpcIndex;

        public uint FinishNpcIndex
        {
            get { return _finishNpcIndex == 0 ? NpcIndex : _finishNpcIndex; }
            set { _finishNpcIndex = value; }
        }

        public string 
            Name = string.Empty, 
            Group = string.Empty, 
            FileName = string.Empty, 
            GotoMessage = string.Empty, 
            KillMessage = string.Empty, 
            ItemMessage = string.Empty;

        public List<string> Description = new List<string>();
        public List<string> TaskDescription = new List<string>(); 
        public List<string> CompletionDescription = new List<string>(); 

        public int RequiredLevel, RequiredQuest;
        public RequiredClass RequiredClass = RequiredClass.None;

        public QuestType Type;

        public List<QuestItemTask> CarryItems = new List<QuestItemTask>(); 

        public List<QuestKillTask> KillTasks = new List<QuestKillTask>();
        public List<QuestItemTask> ItemTasks = new List<QuestItemTask>();

        public uint GoldReward;
        public uint ExpReward;
        public List<ItemInfo> FixedRewards = new List<ItemInfo>();
        public List<ItemInfo> SelectRewards = new List<ItemInfo>();

        public QuestInfo() { }

        public QuestInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadString();
            Group = reader.ReadString();
            FileName = reader.ReadString();
            RequiredLevel = reader.ReadInt32();
            RequiredQuest = reader.ReadInt32();
            RequiredClass = (RequiredClass)reader.ReadByte();
            Type = (QuestType)reader.ReadByte();
            GotoMessage = reader.ReadString();
            KillMessage = reader.ReadString();
            ItemMessage = reader.ReadString();

            LoadInfo();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Group);
            writer.Write(FileName);
            writer.Write(RequiredLevel);
            writer.Write(RequiredQuest);
            writer.Write((byte)RequiredClass);
            writer.Write((byte)Type);
            writer.Write(GotoMessage);
            writer.Write(KillMessage);
            writer.Write(ItemMessage);
        }

        public void LoadInfo(bool clear = false)
        {
            if (clear) ClearInfo();

            if (!Directory.Exists(Settings.QuestPath)) return;

            string fileName = Path.Combine(Settings.QuestPath, FileName + ".txt");

            if (File.Exists(fileName))
            {
                List<string> lines = File.ReadAllLines(fileName).ToList();

                ParseFile(lines);
            }
            else
                SMain.Enqueue(string.Format("File Not Found: {0}, Quest: {1}", fileName, Name));
        }

        public void ClearInfo()
        {
            Description.Clear();
            KillTasks = new List<QuestKillTask>();
            ItemTasks = new List<QuestItemTask>();
            FixedRewards = new List<ItemInfo>();
            SelectRewards = new List<ItemInfo>();
            ExpReward = 0;
            GoldReward = 0;
        }

        public void ParseFile(List<string> lines)
        {
            const string
                descriptionCollectKey = "[@DESCRIPTION]",
                descriptionTaskKey = "[@TASKDESCRIPTION]",
                descriptionCompletionKey = "[@COMPLETION]",
                carryItemsKey = "[@CARRYITEMS]",
                killTasksKey = "[@KILLTASKS]",
                itemTasksKey = "[@ITEMTASKS]",
                fixedRewardsKey = "[@FIXEDREWARDS]",
                selectRewardsKey = "[@SELECTREWARDS]",
                expRewardKey = "[@EXPREWARD]",
                goldRewardKey = "[@GOLDREWARD]";

            List<string> headers = new List<string> 
            { 
                descriptionCollectKey, descriptionTaskKey, descriptionCompletionKey,
                carryItemsKey, killTasksKey, itemTasksKey,
                fixedRewardsKey, selectRewardsKey, expRewardKey, goldRewardKey
            };

            int currentHeader = 0;

            while (currentHeader < headers.Count)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i].ToUpper();

                    if (line != headers[currentHeader].ToUpper()) continue;

                    for (int j = i + 1; j < lines.Count; j++)
                    {
                        string innerLine = lines[j];

                        if (innerLine.StartsWith("[")) break;
                        if (string.IsNullOrEmpty(lines[j])) continue;

                        switch (line)
                        {
                            case descriptionCollectKey:
                                Description.Add(innerLine);
                                break;
                            case descriptionTaskKey:
                                TaskDescription.Add(innerLine);
                                break;
                            case descriptionCompletionKey:
                                CompletionDescription.Add(innerLine);
                                break;
                            case carryItemsKey:
                                QuestItemTask t = ParseItem(innerLine);
                                if (t != null) CarryItems.Add(t);
                                break;
                            case killTasksKey:
                                QuestKillTask t1 = ParseKill(innerLine);
                                if(t1 != null) KillTasks.Add(t1);
                                break;
                            case itemTasksKey:
                                QuestItemTask t2 = ParseItem(innerLine);
                                if (t2 != null) ItemTasks.Add(t2);
                                break;
                            case fixedRewardsKey:
                                ItemInfo mInfo = SMain.Envir.GetItemInfo(innerLine);
                                if (mInfo != null) FixedRewards.Add(mInfo);
                                break;
                            case selectRewardsKey:
                                mInfo = SMain.Envir.GetItemInfo(innerLine);
                                if (mInfo != null) SelectRewards.Add(mInfo);
                                break;
                            case expRewardKey:
                                uint.TryParse(innerLine, out ExpReward);
                                break;
                            case goldRewardKey:
                                uint.TryParse(innerLine, out GoldReward);
                                break;
                        }
                    }
                }

                currentHeader++;
            }
        }

        public QuestKillTask ParseKill(string line)
        {
            if (line.Length < 1) return null;

            string[] split = line.Split(' ');
            int count = 1;

            MonsterInfo mInfo = SMain.Envir.GetMonsterInfo(split[0]);
            if (split.Length > 1)
                int.TryParse(split[1], out count);

            return mInfo == null ? null : new QuestKillTask() { Monster = mInfo, Count = count };
        }

        public QuestItemTask ParseItem(string line)
        {
            if (line.Length < 1) return null;

            string[] split = line.Split(' ');
            uint count = 1;

            ItemInfo mInfo = SMain.Envir.GetItemInfo(split[0]);
            if (split.Length > 1)
                uint.TryParse(split[1], out count);

            //if (mInfo.StackSize <= 1)
            //{
            //    //recursively add item if cant stack???
            //}

            return mInfo == null ? null : new QuestItemTask { Item = mInfo, Count = count };
        }

        public bool CanAccept(PlayerObject player)
        {
            if (RequiredLevel > player.Level)
                return false;

            if (RequiredQuest > 0 && !player.CompletedQuests.Contains(RequiredQuest))
                return false;

            switch (player.Class)
            {
                case MirClass.Warrior:
                    if (!RequiredClass.HasFlag(RequiredClass.Warrior))
                        return false;
                    break;
                case MirClass.Wizard:
                    if (!RequiredClass.HasFlag(RequiredClass.Wizard))
                        return false;
                    break;
                case MirClass.Taoist:
                    if (!RequiredClass.HasFlag(RequiredClass.Taoist))
                        return false;
                    break;
                case MirClass.Assassin:
                    if (!RequiredClass.HasFlag(RequiredClass.Assassin))
                        return false;
                    break;
                case MirClass.Archer:
                    if (!RequiredClass.HasFlag(RequiredClass.Archer))
                        return false;
                    break;
            }

            return true;
        }

        public ClientQuestInfo CreateClientQuestInfo()
        {
            return new ClientQuestInfo
            {
                Index = Index,
                NPCIndex = NpcIndex,
                FinishNPCIndex = FinishNpcIndex,
                Name = Name,
                Group = Group,
                Description = Description,
                TaskDescription = TaskDescription,
                CompletionDescription = CompletionDescription,
                LevelNeeded = RequiredLevel,
                ClassNeeded = RequiredClass,
                QuestNeeded = RequiredQuest,
                Type = Type,
                RewardGold = GoldReward,
                RewardExp = ExpReward,
                RewardsFixedItem = FixedRewards,
                RewardsSelectItem = SelectRewards
            };
        }

        public static void FromText(string text)
        {
            string[] data = text.Split(new[] { ',' });

            if (data.Length < 9) return;

            QuestInfo info = new QuestInfo();

            info.Name = data[0];
            info.Group = data[1];

            byte temp;

            byte.TryParse(data[2], out temp);

            info.Type = (QuestType)temp;

            info.FileName = data[3];
            info.GotoMessage = data[4];
            info.KillMessage = data[5];
            info.ItemMessage = data[6];

            int.TryParse(data[7], out info.RequiredLevel);
            int.TryParse(data[8], out info.RequiredQuest);

            byte.TryParse(data[9], out temp);

            info.RequiredClass = (RequiredClass)temp;

            info.Index = ++SMain.EditEnvir.QuestIndex;
            SMain.EditEnvir.QuestInfoList.Add(info);
        }

        public string ToText()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                Name, Group, (byte)Type, FileName, GotoMessage, KillMessage, ItemMessage, RequiredLevel, RequiredQuest, (byte)RequiredClass);
        }

        public override string ToString()
        {
            return string.Format("{0}:   {1}", Index, Name);
        }
    }

    public class QuestKillTask
    {
        public int Count;
        public MonsterInfo Monster;
    }

    public class QuestItemTask
    {
        public uint Count;
        public ItemInfo Item;
    }

}
