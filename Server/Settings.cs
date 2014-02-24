using System;
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
                            NameListPath = EnvirPath + @"\NameLists\";

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
    }
}
