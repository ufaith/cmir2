﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Server.MirDatabase;

namespace Server
{
    internal static class Settings
    {
        public const int Day = 24 * Hour, Hour = 60 * Minute, Minute = 60 * Second, Second = 1000;

        public const string EnvirPath = @".\Envir\",
                            MapPath = @".\Maps\",
                            ExportPath = @".\Exports\",
                            NPCPath = EnvirPath + @".\NPCs\",
                            QuestPath = EnvirPath + @".\Quests\",
                            DropPath = EnvirPath + @".\Drops\",
                            NameListPath = EnvirPath + @".\NameLists\";

        private static readonly InIReader Reader = new InIReader(@".\Setup.ini");


        //General
        public static string VersionPath = @".\Mir2.Exe";
        public static bool CheckVersion = true;
        public static byte[] VersionHash;
        public static string GMPassword = "C#Mir 4.0";

        //Network
        public static string IPAddress = "127.0.0.1";

        public static ushort Port = 7000,
                             TimeOut = 10000,
                             MaxUser = 50,
                             RelogDelay = 50;


        //Permission
        public static bool AllowNewAccount = true,
                           AllowChangePassword = true,
                           AllowLogin = true,
                           AllowNewCharacter = true,
                           AllowDeleteCharacter = true,
                           AllowStartGame;

        //Database
        public static int SaveDelay = 5;

        //Game
        public static List<long> ExperienceList = new List<long>();

        public static float DropRate = 2F, ExpRate = 3F;

        public static int ItemTimeOut = 30,
                          DropRange = 4,
                          DropStackSize = 5,
                          PKDelay = 12;

        public static string SkeletonName = "Bone Familiar",
                             ShinsuName = "Shinsu",
                             BugBatName = "Bug Bat",
                             Zuma1 = "Zuma Statue",
                             Zuma2 = "Zuma Guardian",
                             Zuma3 = "Zuma Archer",
                             Zuma4 = "Wedge Moth",
                             Zuma5 = "Zuma Archer3",
                             Zuma6 = "Zuma Statue3",
                             Zuma7 = "Zuma Guardian3",
                             BoneMonster1 = "Bone Spearman",
                             BoneMonster2 = "Bone Blademan",
                             BoneMonster3 = "Bone Archer",
                             BoneMonster4 = "Bone Captain",
                             WhiteSnake = "White Serpent",
                             AngelName = "Holy Deva",
                             BombSpiderName = "Bomb Spider",
                             CloneName = "Clone";

        public static string HealRing = "Healing",
                             FireRing = "FireBall",
                             ParalysisRing = "Paralysis";
                            

        //character settings
        private static String[] BaseStatClassNames = { "Warrior", "Wizard", "Taoist", "Assasin" };
        public static BaseStats[] ClassBaseStats = new BaseStats[4]{new BaseStats(MirClass.Warrior),new BaseStats(MirClass.Wizard), new BaseStats(MirClass.Taoist), new BaseStats(MirClass.Assassin)};
        public static List<RandomItemStat> RandomItemStatsList = new List<RandomItemStat>();
        public static List<MineSet> MineSetList = new List<MineSet>();
        
        //item related settings
        public static byte MaxMagicResist = 6,
                    MagicResistWeight = 10,
                    MaxPoisonResist = 6,
                    PoisonResistWeight = 10,
                    MaxCriticalRate = 18,
                    CriticalRateWeight = 5,
                    MaxCriticalDamage = 10,
                    CriticalDamageWeight = 50,
                    MaxFreezing = 6,
                    FreezingAttackWeight = 10,
                    MaxPoisonAttack = 6,
                    PoisonAttackWeight = 10,
                    MaxHealthRegen = 8,
                    HealthRegenWeight = 10,
                    MaxManaRegen = 8,
                    ManaRegenWeight = 10,
                    MaxPoisonRecovery = 6;
        public static Boolean PvpCanResistMagic = false,
                              PvpCanResistPoison = false,
                              PvpCanFreeze = false;

        //guild related settings
        public static byte Guild_RequiredLevel = 22, Guild_PointPerLevel = 0;
        public static float Guild_ExpRate = 0.01f;
        public static List<ItemVolume> Guild_CreationCostList = new List<ItemVolume>();
        public static List<long> Guild_ExperienceList = new List<long>();
        public static List<int> Guild_MembercapList = new List<int>();
        public static List<GuildBuff> Guild_BuffList = new List<GuildBuff>();

        public static void Load()
        {
            //General
            VersionPath = Reader.ReadString("General", "VersionPath", VersionPath);
            CheckVersion = Reader.ReadBoolean("General", "CheckVersion", CheckVersion);
            RelogDelay = Reader.ReadUInt16("General", "RelogDelay", RelogDelay);
            GMPassword = Reader.ReadString("General", "GMPassword", GMPassword);

            //Paths
            IPAddress = Reader.ReadString("Network", "IPAddress", IPAddress);
            Port = Reader.ReadUInt16("Network", "Port", Port);
            TimeOut = Reader.ReadUInt16("Network", "TimeOut", TimeOut);
            MaxUser = Reader.ReadUInt16("Network", "MaxUser", MaxUser);

            //Permission
            AllowNewAccount = Reader.ReadBoolean("Permission", "AllowNewAccount", AllowNewAccount);
            AllowChangePassword = Reader.ReadBoolean("Permission", "AllowChangePassword", AllowChangePassword);
            AllowLogin = Reader.ReadBoolean("Permission", "AllowLogin", AllowLogin);
            AllowNewCharacter = Reader.ReadBoolean("Permission", "AllowNewCharacter", AllowNewCharacter);
            AllowDeleteCharacter = Reader.ReadBoolean("Permission", "AllowDeleteCharacter", AllowDeleteCharacter);
            AllowStartGame = Reader.ReadBoolean("Permission", "AllowStartGame", AllowStartGame);

            //Database
            SaveDelay = Reader.ReadInt32("Database", "SaveDelay", SaveDelay);

            //Game
            DropRate = Reader.ReadSingle("Game", "DropRate", DropRate);
            ExpRate = Reader.ReadSingle("Game", "ExpRate", ExpRate);
            ItemTimeOut = Reader.ReadInt32("Game", "ItemTimeOut", ItemTimeOut);
            ItemTimeOut = Reader.ReadInt32("Game", "PKDelay", PKDelay);
            SkeletonName = Reader.ReadString("Game", "SkeletonName", SkeletonName);
            BugBatName = Reader.ReadString("Game", "BugBatName", BugBatName);
            ShinsuName = Reader.ReadString("Game", "ShinsuName", ShinsuName);
            Zuma1 = Reader.ReadString("Game", "Zuma1", Zuma1);
            Zuma2 = Reader.ReadString("Game", "Zuma2", Zuma2);
            Zuma3 = Reader.ReadString("Game", "Zuma3", Zuma3);
            Zuma4 = Reader.ReadString("Game", "Zuma4", Zuma4);
            Zuma5 = Reader.ReadString("Game", "Zuma5", Zuma5);
            Zuma6 = Reader.ReadString("Game", "Zuma6", Zuma6);
            Zuma7 = Reader.ReadString("Game", "Zuma7", Zuma7);
            BoneMonster1 = Reader.ReadString("Game", "BoneMonster1", BoneMonster1);
            BoneMonster2 = Reader.ReadString("Game", "BoneMonster2", BoneMonster2);
            BoneMonster3 = Reader.ReadString("Game", "BoneMonster3", BoneMonster3);
            BoneMonster4 = Reader.ReadString("Game", "BoneMonster4", BoneMonster4);
            WhiteSnake = Reader.ReadString("Game", "WhiteSnake", WhiteSnake);
            AngelName = Reader.ReadString("Game", "AngelName", AngelName);
            BombSpiderName = Reader.ReadString("Game", "BombSpiderName", BombSpiderName);
            CloneName = Reader.ReadString("Game", "CloneName", CloneName);

            //Items
            HealRing = Reader.ReadString("Items", "HealRing", HealRing);
            FireRing = Reader.ReadString("Items", "FireRing", FireRing);

            for (int i = 0; i < ClassBaseStats.Length; i++)
            {
                ClassBaseStats[i].HpGain = Reader.ReadFloat(BaseStatClassNames[i], "HpGain", ClassBaseStats[i].HpGain);
                ClassBaseStats[i].HpGainRate = Reader.ReadFloat(BaseStatClassNames[i], "HpGainRate", ClassBaseStats[i].HpGainRate);
                ClassBaseStats[i].MpGainRate = Reader.ReadFloat(BaseStatClassNames[i], "MpGainRate", ClassBaseStats[i].MpGainRate);
                ClassBaseStats[i].BagWeightGain = Reader.ReadFloat(BaseStatClassNames[i], "BagWeightGain", ClassBaseStats[i].BagWeightGain);
                ClassBaseStats[i].WearWeightGain = Reader.ReadFloat(BaseStatClassNames[i], "WearWeightGain", ClassBaseStats[i].WearWeightGain);
                ClassBaseStats[i].HandWeightGain = Reader.ReadFloat(BaseStatClassNames[i], "HandWeightGain", ClassBaseStats[i].HandWeightGain);
                ClassBaseStats[i].MinAc = Reader.ReadByte(BaseStatClassNames[i], "MinAc", ClassBaseStats[i].MinAc);
                ClassBaseStats[i].MaxAc = Reader.ReadByte(BaseStatClassNames[i], "MaxAc", ClassBaseStats[i].MaxAc);
                ClassBaseStats[i].MinMac = Reader.ReadByte(BaseStatClassNames[i], "MinMac", ClassBaseStats[i].MinMac);
                ClassBaseStats[i].MaxMac = Reader.ReadByte(BaseStatClassNames[i], "MaxMac", ClassBaseStats[i].MaxMac);
                ClassBaseStats[i].MinDc = Reader.ReadByte(BaseStatClassNames[i], "MinDc", ClassBaseStats[i].MinDc);
                ClassBaseStats[i].MaxDc = Reader.ReadByte(BaseStatClassNames[i], "MaxDc", ClassBaseStats[i].MaxDc);
                ClassBaseStats[i].MinMc = Reader.ReadByte(BaseStatClassNames[i], "MinMc", ClassBaseStats[i].MinMc);
                ClassBaseStats[i].MaxMc = Reader.ReadByte(BaseStatClassNames[i], "MaxMc", ClassBaseStats[i].MaxMc);
                ClassBaseStats[i].MinSc = Reader.ReadByte(BaseStatClassNames[i], "MinSc", ClassBaseStats[i].MinSc);
                ClassBaseStats[i].MaxSc = Reader.ReadByte(BaseStatClassNames[i], "MaxSc", ClassBaseStats[i].MaxSc);
                ClassBaseStats[i].StartAgility = Reader.ReadByte(BaseStatClassNames[i], "StartAgility", ClassBaseStats[i].StartAgility);
                ClassBaseStats[i].StartAccuracy = Reader.ReadByte(BaseStatClassNames[i], "StartAccuracy", ClassBaseStats[i].StartAccuracy);
                ClassBaseStats[i].StartCriticalRate = Reader.ReadByte(BaseStatClassNames[i], "StartCriticalRate", ClassBaseStats[i].StartCriticalRate);
                ClassBaseStats[i].StartCriticalDamage = Reader.ReadByte(BaseStatClassNames[i], "StartCriticalDamage", ClassBaseStats[i].StartCriticalDamage);
                ClassBaseStats[i].CritialRateGain = Reader.ReadByte(BaseStatClassNames[i], "CritialRateGain", ClassBaseStats[i].CritialRateGain);
                ClassBaseStats[i].CriticalDamageGain = Reader.ReadByte(BaseStatClassNames[i], "CriticalDamageGain", ClassBaseStats[i].CriticalDamageGain);
            }

            MaxMagicResist = Reader.ReadByte("Items","MaxMagicResist",MaxMagicResist);
            MagicResistWeight = Reader.ReadByte("Items","MagicResistWeight",MagicResistWeight);
            MaxPoisonResist = Reader.ReadByte("Items","MaxPoisonResist",MaxPoisonResist);
            PoisonResistWeight = Reader.ReadByte("Items","PoisonResistWeight",PoisonResistWeight);
            MaxCriticalRate = Reader.ReadByte("Items","MaxCriticalRate",MaxCriticalRate);
            CriticalRateWeight = Reader.ReadByte("Items","CriticalRateWeight",CriticalRateWeight);
            MaxCriticalDamage = Reader.ReadByte("Items","MaxCriticalDamage",MaxCriticalDamage);
            CriticalDamageWeight = Math.Max((byte)1,Reader.ReadByte("Items","CriticalDamageWeight",CriticalDamageWeight));
            MaxFreezing = Reader.ReadByte("Items","MaxFreezing",MaxFreezing);
            FreezingAttackWeight = Reader.ReadByte("Items","FreezingAttackWeight",FreezingAttackWeight);
            MaxPoisonAttack = Reader.ReadByte("Items","MaxPoisonAttack",MaxPoisonAttack);
            PoisonAttackWeight = Reader.ReadByte("Items","PoisonAttackWeight",PoisonAttackWeight);
            MaxHealthRegen = Reader.ReadByte("Items", "MaxHealthRegen", MaxHealthRegen);
            HealthRegenWeight = Reader.ReadByte("Items", "HealthRegenWeight", HealthRegenWeight);
            MaxManaRegen = Reader.ReadByte("Items", "MaxManaRegen", MaxManaRegen);
            ManaRegenWeight = Reader.ReadByte("Items", "ManaRegenWeight", ManaRegenWeight);
            MaxPoisonRecovery = Reader.ReadByte("Items", "MaxPoisonRecovery", MaxPoisonRecovery);

            PvpCanResistMagic = Reader.ReadBoolean("Items","PvpCanResistMagic",PvpCanResistMagic);
            PvpCanResistPoison = Reader.ReadBoolean("Items", "PvpCanResistPoison", PvpCanResistPoison);
            PvpCanFreeze = Reader.ReadBoolean("Items", "PvpCanFreeze", PvpCanFreeze);

            if (!Directory.Exists(EnvirPath))
                Directory.CreateDirectory(EnvirPath);

            //Temp code to move old revision folders
            if (Directory.Exists(@".\NPCs\") && !Directory.Exists(NPCPath))
                Directory.Move(@".\NPCs\", NPCPath);
            if (Directory.Exists(@".\Drops\") && !Directory.Exists(DropPath))
                Directory.Move(@".\Drops\", DropPath);
            if (Directory.Exists(@".\Quests\") && !Directory.Exists(QuestPath))
                Directory.Move(@".\Quests\", QuestPath);

            if (!Directory.Exists(MapPath))
                Directory.CreateDirectory(MapPath);
            if (!Directory.Exists(NPCPath))
                Directory.CreateDirectory(NPCPath);
            if (!Directory.Exists(QuestPath))
                Directory.CreateDirectory(QuestPath);
            if (!Directory.Exists(DropPath))
                Directory.CreateDirectory(DropPath);
            if (!Directory.Exists(ExportPath))
                Directory.CreateDirectory(ExportPath);
            
            if (!Directory.Exists(NameListPath))
                Directory.CreateDirectory(NameListPath);

            LoadVersion();
            LoadEXP();
            LoadRandomItemStats();
            LoadMines();
            LoadGuildSettings();
        }

        public static void LoadVersion()
        {
            try
            {
                if (File.Exists(VersionPath))
                    using (FileStream stream = new FileStream(VersionPath, FileMode.Open, FileAccess.Read))
                    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                        VersionHash = md5.ComputeHash(stream);
            }
            catch (Exception ex)
            {
                SMain.Enqueue(ex);
            }
        }
        public static void Save()
        {
            //General
            Reader.Write("General", "VersionPath", VersionPath);
            Reader.Write("General", "CheckVersion", CheckVersion);
            Reader.Write("General", "RelogDelay", RelogDelay);

            //Paths
            Reader.Write("Network", "IPAddress", IPAddress);
            Reader.Write("Network", "Port", Port);
            Reader.Write("Network", "TimeOut", TimeOut);
            Reader.Write("Network", "MaxUser", MaxUser);

            //Permission
            Reader.Write("Permission", "AllowNewAccount", AllowNewAccount);
            Reader.Write("Permission", "AllowChangePassword", AllowChangePassword);
            Reader.Write("Permission", "AllowLogin", AllowLogin);
            Reader.Write("Permission", "AllowNewCharacter", AllowNewCharacter);
            Reader.Write("Permission", "AllowDeleteCharacter", AllowDeleteCharacter);
            Reader.Write("Permission", "AllowStartGame", AllowStartGame);
            
            //Database
            Reader.Write("Database", "SaveDelay", SaveDelay);

            //Game
            Reader.Write("Game", "DropRate", DropRate);
            Reader.Write("Game", "ExpRate", ExpRate);
            Reader.Write("Game", "ItemTimeOut", ItemTimeOut);
            Reader.Write("Game", "PKDelay", PKDelay);
            Reader.Write("Game", "SkeletonName", SkeletonName);
            Reader.Write("Game", "BugBatName", BugBatName);
            Reader.Write("Game", "ShinsuName", ShinsuName);

            Reader.Write("Game", "Zuma1", Zuma1);
            Reader.Write("Game", "Zuma2", Zuma2);
            Reader.Write("Game", "Zuma3", Zuma3);
            Reader.Write("Game", "Zuma4", Zuma4);
            Reader.Write("Game", "Zuma5", Zuma5);
            Reader.Write("Game", "Zuma6", Zuma6);
            Reader.Write("Game", "Zuma7", Zuma7);

            Reader.Write("Game", "BoneMonster1", BoneMonster1);
            Reader.Write("Game", "BoneMonster2", BoneMonster2);
            Reader.Write("Game", "BoneMonster3", BoneMonster3);
            Reader.Write("Game", "BoneMonster4", BoneMonster4);

            Reader.Write("Game", "WhiteSnake", WhiteSnake);
            Reader.Write("Game", "AngelName", AngelName);
            Reader.Write("Game", "BombSpiderName", BombSpiderName);
            Reader.Write("Game", "CloneName", CloneName);

            Reader.Write("Items", "HealRing", HealRing);
            Reader.Write("Items", "FireRing", FireRing);

            for (int i = 0; i < ClassBaseStats.Length; i++)
            {
                Reader.Write(BaseStatClassNames[i], "HpGain", ClassBaseStats[i].HpGain);
                Reader.Write(BaseStatClassNames[i], "HpGainRate", ClassBaseStats[i].HpGainRate);
                Reader.Write(BaseStatClassNames[i], "MpGainRate", ClassBaseStats[i].MpGainRate);
                Reader.Write(BaseStatClassNames[i], "BagWeightGain", ClassBaseStats[i].BagWeightGain);
                Reader.Write(BaseStatClassNames[i], "WearWeightGain", ClassBaseStats[i].WearWeightGain);
                Reader.Write(BaseStatClassNames[i], "HandWeightGain", ClassBaseStats[i].HandWeightGain);
                Reader.Write(BaseStatClassNames[i], "MinAc", ClassBaseStats[i].MinAc);
                Reader.Write(BaseStatClassNames[i], "MaxAc", ClassBaseStats[i].MaxAc);
                Reader.Write(BaseStatClassNames[i], "MinMac", ClassBaseStats[i].MinMac);
                Reader.Write(BaseStatClassNames[i], "MaxMac", ClassBaseStats[i].MaxMac);
                Reader.Write(BaseStatClassNames[i], "MinDc", ClassBaseStats[i].MinDc);
                Reader.Write(BaseStatClassNames[i], "MaxDc", ClassBaseStats[i].MaxDc);
                Reader.Write(BaseStatClassNames[i], "MinMc", ClassBaseStats[i].MinMc);
                Reader.Write(BaseStatClassNames[i], "MaxMc", ClassBaseStats[i].MaxMc);
                Reader.Write(BaseStatClassNames[i], "MinSc", ClassBaseStats[i].MinSc);
                Reader.Write(BaseStatClassNames[i], "MaxSc", ClassBaseStats[i].MaxSc);
                Reader.Write(BaseStatClassNames[i], "StartAgility", ClassBaseStats[i].StartAgility);
                Reader.Write(BaseStatClassNames[i], "StartAccuracy", ClassBaseStats[i].StartAccuracy);
                Reader.Write(BaseStatClassNames[i], "StartCriticalRate", ClassBaseStats[i].StartCriticalRate);
                Reader.Write(BaseStatClassNames[i], "StartCriticalDamage", ClassBaseStats[i].StartCriticalDamage);
                Reader.Write(BaseStatClassNames[i], "CritialRateGain", ClassBaseStats[i].CritialRateGain);
                Reader.Write(BaseStatClassNames[i], "CriticalDamageGain", ClassBaseStats[i].CriticalDamageGain);
            }

            Reader.Write("Items", "MaxMagicResist", MaxMagicResist);
            Reader.Write("Items", "MagicResistWeight", MagicResistWeight);
            Reader.Write("Items", "MaxPoisonResist", MaxPoisonResist);
            Reader.Write("Items", "PoisonResistWeight", PoisonResistWeight);
            Reader.Write("Items", "MaxCriticalRate", MaxCriticalRate);
            Reader.Write("Items", "CriticalRateWeight", CriticalRateWeight);
            Reader.Write("Items", "MaxCriticalDamage", MaxCriticalDamage);
            Reader.Write("Items", "CriticalDamageWeight", CriticalDamageWeight);
            Reader.Write("Items", "MaxFreezing", MaxFreezing);
            Reader.Write("Items", "FreezingAttackWeight", FreezingAttackWeight);
            Reader.Write("Items", "MaxPoisonAttack", MaxPoisonAttack);
            Reader.Write("Items", "PoisonAttackWeight", PoisonAttackWeight);
            Reader.Write("Items", "MaxHealthRegen", MaxHealthRegen);
            Reader.Write("Items", "HealthRegenWeight", HealthRegenWeight);
            Reader.Write("Items", "MaxManaRegen", MaxManaRegen);
            Reader.Write("Items", "ManaRegenWeight", ManaRegenWeight);
            Reader.Write("ITems", "MaxPoisonRecovery", MaxPoisonRecovery);
            Reader.Write("Items", "PvpCanResistMagic", PvpCanResistMagic);
            Reader.Write("Items", "PvpCanResistPoison", PvpCanResistPoison);
            Reader.Write("Items", "PvpCanFreeze", PvpCanFreeze);
        }

        public static void LoadEXP()
        {
            long exp = 100;
            InIReader reader = new InIReader(@".\ExpList.ini");

            for (int i = 1; i <= byte.MaxValue - 1; i++)
            {
                exp = reader.ReadInt64("Exp", "Level" + i, exp);
                ExperienceList.Add(exp);
            }
        }
        public static void LoadRandomItemStats()
        {
            //note: i could have used a flat file system for this which would be faster, 
            //BUT: it's only loaded @ server startup so speed isnt vital.
            //and i think settings should be available outside the exe for ppl to edit it easyer + lets ppl share config without forcing ppl to run it in an exe
            if (!File.Exists(@".\RandomItemStats.ini"))
            {
                RandomItemStatsList.Add(new RandomItemStat());
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Weapon));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Armour));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Helmet));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Necklace));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Bracelet));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Ring));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Belt));
                SaveRandomItemStats();
                return;
            }
            InIReader reader = new InIReader(@".\RandomItemStats.ini");
            int i = 0;
            RandomItemStat stat;
            while (reader.ReadByte("Item" + i.ToString(),"MaxDuraChance",255) != 255)
            {
                stat = new RandomItemStat();
                stat.MaxDuraChance = reader.ReadByte("Item" + i.ToString(), "MaxDuraChance", 0);
                stat.MaxDuraStatChance = reader.ReadByte("Item" + i.ToString(), "MaxDuraStatChance", 1);
                stat.MaxDuraMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxDuraMaxStat", 1);
                stat.MaxAcChance = reader.ReadByte("Item" + i.ToString(), "MaxAcChance", 0);
                stat.MaxAcStatChance = reader.ReadByte("Item" + i.ToString(), "MaxAcStatChance", 1);
                stat.MaxAcMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxAcMaxStat", 1);
                stat.MaxMacChance = reader.ReadByte("Item" + i.ToString(), "MaxMacChance", 0);
                stat.MaxMacStatChance = reader.ReadByte("Item" + i.ToString(), "MaxMacStatChance", 1);
                stat.MaxMacMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxMACMaxStat", 1);
                stat.MaxDcChance = reader.ReadByte("Item" + i.ToString(), "MaxDcChance", 0);
                stat.MaxDcStatChance = reader.ReadByte("Item" + i.ToString(), "MaxDcStatChance", 1);
                stat.MaxDcMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxDcMaxStat", 1);
                stat.MaxMcChance = reader.ReadByte("Item" + i.ToString(), "MaxMcChance", 0);
                stat.MaxMcStatChance = reader.ReadByte("Item" + i.ToString(), "MaxMcStatChance", 1);
                stat.MaxMcMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxMcMaxStat", 1);
                stat.MaxScChance = reader.ReadByte("Item" + i.ToString(), "MaxScChance", 0);
                stat.MaxScStatChance = reader.ReadByte("Item" + i.ToString(), "MaxScStatChance", 1);
                stat.MaxScMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxScMaxStat", 1);
                stat.AccuracyChance = reader.ReadByte("Item" + i.ToString(), "AccuracyChance", 0);
                stat.AccuracyStatChance = reader.ReadByte("Item" + i.ToString(), "AccuracyStatChance", 1);
                stat.AccuracyMaxStat = reader.ReadByte("Item" + i.ToString(), "AccuracyMaxStat", 1);
                stat.AgilityChance = reader.ReadByte("Item" + i.ToString(), "AgilityChance", 0);
                stat.AgilityStatChance = reader.ReadByte("Item" + i.ToString(), "AgilityStatChance", 1);
                stat.AgilityMaxStat = reader.ReadByte("Item" + i.ToString(), "AgilityMaxStat", 1);
                stat.HpChance = reader.ReadByte("Item" + i.ToString(), "HpChance", 0);
                stat.HpStatChance = reader.ReadByte("Item" + i.ToString(), "HpStatChance", 1);
                stat.HpMaxStat = reader.ReadByte("Item" + i.ToString(), "HpMaxStat", 1);
                stat.MpChance = reader.ReadByte("Item" + i.ToString(), "MpChance", 0);
                stat.MpStatChance = reader.ReadByte("Item" + i.ToString(), "MpStatChance", 1);
                stat.MpMaxStat = reader.ReadByte("Item" + i.ToString(), "MpMaxStat", 1);
                stat.StrongChance = reader.ReadByte("Item" + i.ToString(), "StrongChance", 0);
                stat.StrongStatChance = reader.ReadByte("Item" + i.ToString(), "StrongStatChance", 1);
                stat.StrongMaxStat = reader.ReadByte("Item" + i.ToString(), "StrongMaxStat", 1);
                stat.MagicResistChance = reader.ReadByte("Item" + i.ToString(), "MagicResistChance", 0);
                stat.MagicResistStatChance = reader.ReadByte("Item" + i.ToString(), "MagicResistStatChance", 1);
                stat.MagicResistMaxStat = reader.ReadByte("Item" + i.ToString(), "MagicResistMaxStat", 1);
                stat.PoisonResistChance = reader.ReadByte("Item" + i.ToString(), "PoisonResistChance", 0);
                stat.PoisonResistStatChance = reader.ReadByte("Item" + i.ToString(), "PoisonResistStatChance", 1);
                stat.PoisonResistMaxStat = reader.ReadByte("Item" + i.ToString(), "PoisonResistMaxStat", 1);
                stat.HpRecovChance = reader.ReadByte("Item" + i.ToString(), "HpRecovChance", 0);
                stat.HpRecovStatChance = reader.ReadByte("Item" + i.ToString(), "HpRecovStatChance", 1);
                stat.HpRecovMaxStat = reader.ReadByte("Item" + i.ToString(), "HpRecovMaxStat", 1);
                stat.MpRecovChance = reader.ReadByte("Item" + i.ToString(), "MpRecovChance", 0);
                stat.MpRecovStatChance = reader.ReadByte("Item" + i.ToString(), "MpRecovStatChance", 1);
                stat.MpRecovMaxStat = reader.ReadByte("Item" + i.ToString(), "MpRecovMaxStat", 1);
                stat.PoisonRecovChance = reader.ReadByte("Item" + i.ToString(), "PoisonRecovChance", 0);
                stat.PoisonRecovStatChance = reader.ReadByte("Item" + i.ToString(), "PoisonRecovStatChance", 1);
                stat.PoisonRecovMaxStat = reader.ReadByte("Item" + i.ToString(), "PoisonRecovMaxStat", 1);
                stat.CriticalRateChance = reader.ReadByte("Item" + i.ToString(), "CriticalRateChance", 0);
                stat.CriticalRateStatChance = reader.ReadByte("Item" + i.ToString(), "CriticalRateStatChance", 1);
                stat.CriticalRateMaxStat = reader.ReadByte("Item" + i.ToString(), "CriticalRateMaxStat", 1);
                stat.CriticalDamageChance = reader.ReadByte("Item" + i.ToString(), "CriticalDamageChance", 0);
                stat.CriticalDamageStatChance = reader.ReadByte("Item" + i.ToString(), "CriticalDamageStatChance", 1);
                stat.CriticalDamageMaxStat = reader.ReadByte("Item" + i.ToString(), "CriticalDamageMaxStat", 1);
                stat.FreezeChance = reader.ReadByte("Item" + i.ToString(), "FreezeChance", 0);
                stat.FreezeStatChance = reader.ReadByte("Item" + i.ToString(), "FreezeStatChance", 1);
                stat.FreezeMaxStat = reader.ReadByte("Item" + i.ToString(), "FreezeMaxStat", 1);
                stat.PoisonAttackChance = reader.ReadByte("Item" + i.ToString(), "PoisonAttackChance", 0);
                stat.PoisonAttackStatChance = reader.ReadByte("Item" + i.ToString(), "PoisonAttackStatChance", 1);
                stat.PoisonAttackMaxStat = reader.ReadByte("Item" + i.ToString(), "PoisonAttackMaxStat", 1);
                stat.AttackSpeedChance = reader.ReadByte("Item" + i.ToString(), "AttackSpeedChance", 0);
                stat.AttackSpeedStatChance = reader.ReadByte("Item" + i.ToString(), "AttackSpeedStatChance", 1);
                stat.AttackSpeedMaxStat = reader.ReadByte("Item" + i.ToString(), "AttackSpeedMaxStat", 1);
                stat.LuckChance = reader.ReadByte("Item" + i.ToString(), "LuckChance", 0);
                stat.LuckStatChance = reader.ReadByte("Item" + i.ToString(), "LuckStatChance", 1);
                stat.LuckMaxStat = reader.ReadByte("Item" + i.ToString(), "LuckMaxStat", 1);
                stat.CurseChance = reader.ReadByte("Item" + i.ToString(), "CurseChance", 0);
                RandomItemStatsList.Add(stat);
                i++;
            }
        }
        public static void SaveRandomItemStats()
        {
            File.Delete(@".\RandomItemStats.ini");
            InIReader reader = new InIReader(@".\RandomItemStats.ini");
            RandomItemStat stat;
            for (int i = 0; i < RandomItemStatsList.Count; i++)
            {
                stat = RandomItemStatsList[i];
                reader.Write("Item" + i.ToString(), "MaxDuraChance", stat.MaxDuraChance);
                reader.Write("Item" + i.ToString(), "MaxDuraStatChance", stat.MaxDuraStatChance);
                reader.Write("Item" + i.ToString(), "MaxDuraMaxStat", stat.MaxDuraMaxStat);
                reader.Write("Item" + i.ToString(), "MaxAcChance", stat.MaxAcChance);
                reader.Write("Item" + i.ToString(), "MaxAcStatChance", stat.MaxAcStatChance);
                reader.Write("Item" + i.ToString(), "MaxAcMaxStat", stat.MaxAcMaxStat);
                reader.Write("Item" + i.ToString(), "MaxMacChance", stat.MaxMacChance);
                reader.Write("Item" + i.ToString(), "MaxMacStatChance", stat.MaxMacStatChance);
                reader.Write("Item" + i.ToString(), "MaxMACMaxStat", stat.MaxMacMaxStat);
                reader.Write("Item" + i.ToString(), "MaxDcChance", stat.MaxDcChance);
                reader.Write("Item" + i.ToString(), "MaxDcStatChance", stat.MaxDcStatChance);
                reader.Write("Item" + i.ToString(), "MaxDcMaxStat", stat.MaxDcMaxStat);
                reader.Write("Item" + i.ToString(), "MaxMcChance", stat.MaxMcChance);
                reader.Write("Item" + i.ToString(), "MaxMcStatChance",  stat.MaxMcStatChance);
                reader.Write("Item" + i.ToString(), "MaxMcMaxStat", stat.MaxMcMaxStat);
                reader.Write("Item" + i.ToString(), "MaxScChance", stat.MaxScChance);
                reader.Write("Item" + i.ToString(), "MaxScStatChance", stat.MaxScStatChance);
                reader.Write("Item" + i.ToString(), "MaxScMaxStat", stat.MaxScMaxStat);
                reader.Write("Item" + i.ToString(), "AccuracyChance", stat.AccuracyChance);
                reader.Write("Item" + i.ToString(), "AccuracyStatChance", stat.AccuracyStatChance);
                reader.Write("Item" + i.ToString(), "AccuracyMaxStat", stat.AccuracyMaxStat);
                reader.Write("Item" + i.ToString(), "AgilityChance", stat.AgilityChance);
                reader.Write("Item" + i.ToString(), "AgilityStatChance", stat.AgilityStatChance);
                reader.Write("Item" + i.ToString(), "AgilityMaxStat", stat.AgilityMaxStat);
                reader.Write("Item" + i.ToString(), "HpChance", stat.HpChance);
                reader.Write("Item" + i.ToString(), "HpStatChance", stat.HpStatChance);
                reader.Write("Item" + i.ToString(), "HpMaxStat", stat.HpMaxStat);
                reader.Write("Item" + i.ToString(), "MpChance", stat.MpChance);
                reader.Write("Item" + i.ToString(), "MpStatChance", stat.MpStatChance);
                reader.Write("Item" + i.ToString(), "MpMaxStat", stat.MpMaxStat);
                reader.Write("Item" + i.ToString(), "StrongChance", stat.StrongChance);
                reader.Write("Item" + i.ToString(), "StrongStatChance", stat.StrongStatChance);
                reader.Write("Item" + i.ToString(), "StrongMaxStat", stat.StrongMaxStat);
                reader.Write("Item" + i.ToString(), "MagicResistChance", stat.MagicResistChance);
                reader.Write("Item" + i.ToString(), "MagicResistStatChance", stat.MagicResistStatChance);
                reader.Write("Item" + i.ToString(), "MagicResistMaxStat", stat.MagicResistMaxStat);
                reader.Write("Item" + i.ToString(), "PoisonResistChance", stat.PoisonResistChance);
                reader.Write("Item" + i.ToString(), "PoisonResistStatChance", stat.PoisonResistStatChance);
                reader.Write("Item" + i.ToString(), "PoisonResistMaxStat", stat.PoisonResistMaxStat);
                reader.Write("Item" + i.ToString(), "HpRecovChance", stat.HpRecovChance);
                reader.Write("Item" + i.ToString(), "HpRecovStatChance", stat.HpRecovStatChance);
                reader.Write("Item" + i.ToString(), "HpRecovMaxStat", stat.HpRecovMaxStat);
                reader.Write("Item" + i.ToString(), "MpRecovChance", stat.MpRecovChance);
                reader.Write("Item" + i.ToString(), "MpRecovStatChance", stat.MpRecovStatChance);
                reader.Write("Item" + i.ToString(), "MpRecovMaxStat", stat.MpRecovMaxStat);
                reader.Write("Item" + i.ToString(), "PoisonRecovChance", stat.PoisonRecovChance);
                reader.Write("Item" + i.ToString(), "PoisonRecovStatChance", stat.PoisonRecovStatChance);
                reader.Write("Item" + i.ToString(), "PoisonRecovMaxStat", stat.PoisonRecovMaxStat);
                reader.Write("Item" + i.ToString(), "CriticalRateChance", stat.CriticalRateChance);
                reader.Write("Item" + i.ToString(), "CriticalRateStatChance", stat.CriticalRateStatChance);
                reader.Write("Item" + i.ToString(), "CriticalRateMaxStat", stat.CriticalRateMaxStat);
                reader.Write("Item" + i.ToString(), "CriticalDamageChance", stat.CriticalDamageChance);
                reader.Write("Item" + i.ToString(), "CriticalDamageStatChance", stat.CriticalDamageStatChance);
                reader.Write("Item" + i.ToString(), "CriticalDamageMaxStat", stat.CriticalDamageMaxStat);
                reader.Write("Item" + i.ToString(), "FreezeChance", stat.FreezeChance);
                reader.Write("Item" + i.ToString(), "FreezeStatChance", stat.FreezeStatChance);
                reader.Write("Item" + i.ToString(), "FreezeMaxStat", stat.FreezeMaxStat);
                reader.Write("Item" + i.ToString(), "PoisonAttackChance", stat.PoisonAttackChance);
                reader.Write("Item" + i.ToString(), "PoisonAttackStatChance", stat.PoisonAttackStatChance);
                reader.Write("Item" + i.ToString(), "PoisonAttackMaxStat", stat.PoisonAttackMaxStat);
                reader.Write("Item" + i.ToString(), "AttackSpeedChance", stat.AttackSpeedChance);
                reader.Write("Item" + i.ToString(), "AttackSpeedStatChance", stat.AttackSpeedStatChance);
                reader.Write("Item" + i.ToString(), "AttackSpeedMaxStat", stat.AttackSpeedMaxStat);
                reader.Write("Item" + i.ToString(), "LuckChance", stat.LuckChance);
                reader.Write("Item" + i.ToString(), "LuckStatChance", stat.LuckStatChance);
                reader.Write("Item" + i.ToString(), "LuckMaxStat", stat.LuckMaxStat);
                reader.Write("Item" + i.ToString(), "CurseChance", stat.CurseChance);
            }
        }

        public static void LoadMines()
        {
            if (!File.Exists(@".\Mines.ini"))
            {
                MineSetList.Add(new MineSet(1));
                MineSetList.Add(new MineSet(2));
                SaveMines();
                return;
            }
            InIReader reader = new InIReader(@".\Mines.ini");
            int i = 0;
            MineSet Mine;
            while (reader.ReadByte("Mine" + i.ToString(), "SpotRegenRate", 255) != 255)
            {
                Mine = new MineSet();
                Mine.Name = reader.ReadString("Mine" + i.ToString(), "Name", Mine.Name);
                Mine.SpotRegenRate = reader.ReadByte("Mine" + i.ToString(), "SpotRegenRate", Mine.SpotRegenRate);
                Mine.MaxStones = reader.ReadByte("Mine" + i.ToString(), "MaxStones", Mine.MaxStones);
                Mine.HitRate = reader.ReadByte("Mine" + i.ToString(), "HitRate", Mine.HitRate);
                Mine.DropRate = reader.ReadByte("Mine" + i.ToString(), "DropRate", Mine.DropRate);
                Mine.TotalSlots = reader.ReadByte("Mine" + i.ToString(), "TotalSlots", Mine.TotalSlots);
                int j = 0;
                while (reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MinSlot", 255) != 255)
                {
                    Mine.Drops.Add(new MineDrop()
                        {
                            ItemName = reader.ReadString("Mine" + i.ToString(), "D" + j.ToString() + "-ItemName", ""),
                            MinSlot = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MinSlot", 255),
                            MaxSlot = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MaxSlot", 255),
                            MinDura = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MinDura", 255),
                            MaxDura = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MaxDura", 255),
                            BonusChance = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-BonusChance", 255),
                            MaxBonusDura = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MaxBonusDura", 255)
                        });
                    j++;
                }
                MineSetList.Add(Mine);
                i++;
            }

        }
        public static void SaveMines()
        {
            File.Delete(@".\Mines.ini");
            InIReader reader = new InIReader(@".\Mines.ini");
            MineSet Mine;
            for (int i = 0; i < MineSetList.Count; i++)
            {
                Mine = MineSetList[i];
                reader.Write("Mine" + i.ToString(), "Name", Mine.Name);
                reader.Write("Mine" + i.ToString(), "SpotRegenRate", Mine.SpotRegenRate);
                reader.Write("Mine" + i.ToString(), "MaxStones", Mine.MaxStones);
                reader.Write("Mine" + i.ToString(), "HitRate", Mine.HitRate);
                reader.Write("Mine" + i.ToString(), "DropRate", Mine.DropRate);
                reader.Write("Mine" + i.ToString(), "TotalSlots", Mine.TotalSlots);
                
                for (int j = 0; j < Mine.Drops.Count; j++)
                {
                    MineDrop Drop = Mine.Drops[j];
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-ItemName", Drop.ItemName);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MinSlot", Drop.MinSlot);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MaxSlot", Drop.MaxSlot);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MinDura", Drop.MinDura);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MaxDura", Drop.MaxDura);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-BonusChance", Drop.BonusChance);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MaxBonusDura", Drop.MaxBonusDura);
                }
            }
        }
        public static void LoadGuildSettings()
        {
            if (!File.Exists(@".\GuildSettings.ini"))
            {
                Guild_CreationCostList.Add(new ItemVolume(){Amount = 1000000});
                Guild_CreationCostList.Add(new ItemVolume(){ItemName = "WoomaHorn",Amount = 1});
                return;
            }
            InIReader reader = new InIReader(@".\GuildSettings.ini");
            Guild_RequiredLevel = reader.ReadByte("Guilds", "MinimuLevel", Guild_RequiredLevel);
            Guild_ExpRate = reader.ReadFloat("Guilds", "ExpRate", Guild_ExpRate);
            Guild_PointPerLevel = reader.ReadByte("Guilds", "PointPerLevel", Guild_PointPerLevel);
            int i = 0;
            while (reader.ReadUInt32("Required-" + i.ToString(),"Amount",0) != 0)
            {
                Guild_CreationCostList.Add(new ItemVolume()
                {
                    ItemName = reader.ReadString("Required-" + i.ToString(), "ItemName", ""),
                    Amount = reader.ReadUInt32("Required-" + i.ToString(), "Amount", 0)
                }
                );
                i++;
            }
            i = 0;
            while (reader.ReadInt64("Exp", "Level-" + i.ToString(), -1) != -1)
            {
                Guild_ExperienceList.Add(reader.ReadInt64("Exp", "Level-" + i.ToString(), 0));
                i++;
            }
            i = 0;
            while (reader.ReadInt32("Cap", "Level-" + i.ToString(), -1) != -1)
            {
                Guild_MembercapList.Add(reader.ReadInt32("Cap", "Level-" + i.ToString(), 0));
                i++;
            }
            byte TotalBuffs = reader.ReadByte("Guilds", "TotalBuffs", 0);
            for (i = 0; i < TotalBuffs; i++)
            {
                Guild_BuffList.Add(new GuildBuff()
                {
                    Cost = reader.ReadInt32("Buff-" + i.ToString(), "Cost",0),
                    PointsNeeded = reader.ReadByte("Buff-" + i.ToString(), "PointsNeeded", 0),
                    RunTime = reader.ReadInt64("Buff-" + i.ToString(), "RunTime", 0),
                    MinimumLevel = reader.ReadByte("Buff-" + i.ToString(), "MinimumLevel",0)
                });
            }

        }
        public static void SaveGuildSettings()
        {
            File.Delete(@".\GuildSettings.ini");
            InIReader reader = new InIReader(@".\GuildSettings.ini");
            reader.Write("Guilds", "MinimumLevel", Guild_RequiredLevel);
            reader.Write("Guilds", "ExpRate", Guild_ExpRate);
            reader.Write("Guilds", "PointPerLevel", Guild_PointPerLevel);
            reader.Write("Guilds", "TotalBuffs", Guild_BuffList.Count);
            int i = 0;
            for (i = 0; i < Guild_ExperienceList.Count; i++)
            {
                reader.Write("Exp", "Level-" + i.ToString(), Guild_ExperienceList[i]);
            }
            for (i = 0; i < Guild_MembercapList.Count; i++)
            {
                reader.Write("Cap", "Level-" + i.ToString(), Guild_MembercapList[i]);
            }
            for (i = 0; i < Guild_CreationCostList.Count; i++)
            {
                reader.Write("Required-" + i.ToString(), "ItemName", Guild_CreationCostList[i].ItemName);
                reader.Write("Required-" + i.ToString(), "Amount", Guild_CreationCostList[i].Amount);
            }
            for (i = 0; i < Guild_BuffList.Count; i++)
            {
                reader.Write("Buff-" + i.ToString(), "Cost", Guild_BuffList[i].Cost);
                reader.Write("Buff-" + i.ToString(), "PointsNeeded", Guild_BuffList[i].PointsNeeded);
                reader.Write("Buff-" + i.ToString(), "RunTime", Guild_BuffList[i].RunTime);
                reader.Write("Buff-" + i.ToString(), "MinimumLevel", Guild_BuffList[i].MinimumLevel);
            }
        }
        public static void LinkGuildCreationItems(List<ItemInfo> ItemList)
        {
            for (int i = 0; i < Guild_CreationCostList.Count; i++)
            {
                if (Guild_CreationCostList[i].ItemName != "")
                    for (int j = 0; j < ItemList.Count; j++)
                    {
                        if (String.Compare(ItemList[j].Name.Replace(" ", ""), Guild_CreationCostList[i].ItemName, StringComparison.OrdinalIgnoreCase) != 0) continue;
                        Guild_CreationCostList[i].Item = ItemList[j];
                        break;
                    }
                  
            }
        }
    }
}
