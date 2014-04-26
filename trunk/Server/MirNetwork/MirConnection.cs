﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using C = ClientPackets;
using S = ServerPackets;


namespace Server.MirNetwork
{
    public enum GameStage { None, Login, Select, Game, Disconnected }

    public class MirConnection
    {
        public readonly int SessionID;
        public readonly string IPAddress;

        public GameStage Stage;

        private TcpClient _client;
        private ConcurrentQueue<Packet> _receiveList;
        private Queue<Packet> _sendList, _retryList;

        private bool _disconnecting;
        public bool Connected;
        public bool Disconnecting
        {
            get { return _disconnecting; }
            set
            {
                if (_disconnecting == value) return;
                _disconnecting = value;
                TimeOutTime = SMain.Envir.Time + 500;
            }
        }
        public readonly long TimeConnected;
        public long TimeDisconnected, TimeOutTime;

        byte[] _rawData = new byte[0];

        public AccountInfo Account;
        public PlayerObject Player;
        public List<ItemInfo> SentItemInfo = new List<ItemInfo>();
        public bool StorageSent;


        public MirConnection(int sessionID, TcpClient client)
        {
            SessionID = sessionID;
            IPAddress = client.Client.RemoteEndPoint.ToString().Split(':')[0];


            SMain.Enqueue(IPAddress + ", Connected.");

            _client = client;
            _client.NoDelay = true;

            TimeConnected = SMain.Envir.Time;
            TimeOutTime = TimeConnected + Settings.TimeOut;


            _receiveList = new ConcurrentQueue<Packet>();
            _sendList = new Queue<Packet>(new[] { new S.Connected() });
            _retryList = new Queue<Packet>();

            Connected = true;
            BeginReceive();
        }

        private void BeginReceive()
        {
            if (!Connected) return;

            byte[] rawBytes = new byte[8 * 1024];

            try
            {
                _client.Client.BeginReceive(rawBytes, 0, rawBytes.Length, SocketFlags.None, ReceiveData, rawBytes);
            }
            catch
            {
                Disconnecting = true;
            }
        }
        private void ReceiveData(IAsyncResult result)
        {
            if (!Connected) return;

            int dataRead;

            try
            {
                dataRead = _client.Client.EndReceive(result);
            }
            catch
            {
                Disconnecting = true;
                return;
            }

            if (dataRead == 0)
            {
                Disconnecting = true;
                return;
            }

            byte[] rawBytes = result.AsyncState as byte[];

            byte[] temp = _rawData;
            _rawData = new byte[dataRead + temp.Length];
            Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
            Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

            Packet p;
            while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null)
                _receiveList.Enqueue(p);

            BeginReceive();
        }
        private void BeginSend(List<byte> data)
        {
            if (!Connected || data.Count == 0) return;

            //Interlocked.Add(ref Network.Sent, data.Count);

            try
            {
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, Disconnecting);
            }
            catch
            {
                Disconnecting = true;
            }
        }
        private void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result);
            }
            catch
            { }
        }
        
        public void Enqueue(Packet p)
        {
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);
        }
        
        public void Process()
        {
            if (_client == null || !_client.Connected)
            {
                Disconnect();
                return;
            }

            while (!_receiveList.IsEmpty && !Disconnecting)
            {
                Packet p;
                if (!_receiveList.TryDequeue(out p)) continue;
                TimeOutTime = SMain.Envir.Time + Settings.TimeOut;
                ProcessPacket(p);
            }

            while (_retryList.Count > 0)
                _receiveList.Enqueue(_retryList.Dequeue());

            if (SMain.Envir.Time > TimeOutTime)
            {
                Disconnect();
                return;
            }

            if (_sendList == null || _sendList.Count <= 0) return;

            List<byte> data = new List<byte>();
            while (_sendList.Count > 0)
            {
                Packet p = _sendList.Dequeue();
                data.AddRange(p.GetPacketBytes());
            }

            BeginSend(data);
        }
        private void ProcessPacket(Packet p)
        {
            if (p == null || Disconnecting) return;

            switch (p.Index)
            {
                case (short)ClientPacketIds.ClientVersion:
                    ClientVersion((C.ClientVersion) p);
                    break;
                case (short)ClientPacketIds.Disconnect:
                    Disconnect();
                    break;
                case (short)ClientPacketIds.KeepAlive: // Keep Alive
                    return;
                case (short)ClientPacketIds.NewAccount:
                    NewAccount((C.NewAccount) p);
                    break;
                case (short)ClientPacketIds.ChangePassword:
                    ChangePassword((C.ChangePassword) p);
                    break;
                case (short)ClientPacketIds.Login:
                    Login((C.Login) p);
                    break;
                case (short)ClientPacketIds.NewCharacter:
                    NewCharacter((C.NewCharacter) p);
                    break;
                case (short)ClientPacketIds.DeleteCharacter:
                    DeleteCharacter((C.DeleteCharacter) p);
                    break;
                case (short)ClientPacketIds.StartGame:
                    StartGame((C.StartGame) p);
                    break;
                case (short)ClientPacketIds.LogOut:
                    LogOut();
                    break;
                case (short)ClientPacketIds.Turn:
                    Turn((C.Turn) p);
                    break;
                case (short)ClientPacketIds.Walk:
                    Walk((C.Walk) p);
                    break;
                case (short)ClientPacketIds.Run:
                    Run((C.Run) p);
                    break;
                case (short)ClientPacketIds.Chat:
                    Chat((C.Chat) p);
                    break;
                case (short)ClientPacketIds.MoveItem:
                    MoveItem((C.MoveItem) p);
                    break;
                case (short)ClientPacketIds.StoreItem:
                    StoreItem((C.StoreItem) p);
                    break;
                case (short)ClientPacketIds.TakeBackItem:
                    TakeBackItem((C.TakeBackItem) p);
                    break;
                case (short)ClientPacketIds.MergeItem:
                    MergeItem((C.MergeItem) p);
                    break;
                case (short)ClientPacketIds.EquipItem:
                    EquipItem((C.EquipItem) p);
                    break;
                case (short)ClientPacketIds.RemoveItem:
                    RemoveItem((C.RemoveItem) p);
                    break;
                case (short)ClientPacketIds.SplitItem:
                    SplitItem((C.SplitItem) p);
                    break;
                case (short)ClientPacketIds.UseItem:
                    UseItem((C.UseItem) p);
                    break;
                case (short)ClientPacketIds.DropItem:
                    DropItem((C.DropItem) p);
                    break;
                case (short)ClientPacketIds.DropGold:
                    DropGold((C.DropGold) p);
                    break;
                case (short)ClientPacketIds.PickUp:
                    PickUp();
                    break;
                case (short)ClientPacketIds.Inspect:
                    Inspect((C.Inspect)p);
                    break;
                case (short)ClientPacketIds.ChangeAMode:
                    ChangeAMode((C.ChangeAMode)p);
                    break;
                case (short)ClientPacketIds.ChangePMode:
                    ChangePMode((C.ChangePMode)p);
                    break;
                case (short)ClientPacketIds.Attack:
                    Attack((C.Attack)p);
                    break;
                case (short)ClientPacketIds.Harvest:
                    Harvest((C.Harvest)p);
                    break;
                case (short)ClientPacketIds.CallNPC:
                    CallNPC((C.CallNPC)p);
                    break;
                case (short)ClientPacketIds.BuyItem:
                    BuyItem((C.BuyItem)p);
                    break;
                case (short)ClientPacketIds.SellItem:
                    SellItem((C.SellItem)p);
                    break;
                case (short)ClientPacketIds.RepairItem:
                    RepairItem((C.RepairItem)p);
                    break;
                case (short)ClientPacketIds.BuyItemBack:
                    BuyItemBack((C.BuyItemBack)p);
                    break;
                case (short)ClientPacketIds.SRepairItem:
                    SRepairItem((C.SRepairItem)p);
                    break;
                case (short)ClientPacketIds.MagicKey:
                    MagicKey((C.MagicKey)p);
                    break;
                case (short)ClientPacketIds.Magic:
                    Magic((C.Magic)p);
                    break;
                case (short)ClientPacketIds.SwitchGroup:
                    SwitchGroup((C.SwitchGroup)p);
                    return;
                case (short)ClientPacketIds.AddMember:
                    AddMember((C.AddMember)p);
                    return;
                case (short)ClientPacketIds.DellMember:
                    DelMember((C.DelMember)p);
                    return;
                case (short)ClientPacketIds.GroupInvite:
                    GroupInvite((C.GroupInvite)p);
                    return;
                case (short)ClientPacketIds.TownRevive:
                    TownRevive();
                    return;
                case (short)ClientPacketIds.SpellToggle:
                    SpellToggle((C.SpellToggle)p);
                    return;
                case (short)ClientPacketIds.ConsignItem:
                    ConsignItem((C.ConsignItem)p);
                    return;
                case (short)ClientPacketIds.MarketSearch:
                    MarketSearch((C.MarketSearch)p);
                    return;
                case (short)ClientPacketIds.MarketRefresh:
                    MarketRefresh();
                    return;
                case (short)ClientPacketIds.MarketPage:
                    MarketPage((C.MarketPage) p);
                    return;
                case (short)ClientPacketIds.MarketBuy:
                    MarketBuy((C.MarketBuy)p);
                    return;
                case (short)ClientPacketIds.MarketGetBack:
                    MarketGetBack((C.MarketGetBack)p);
                    return;
                case (short)ClientPacketIds.RequestUserName:
                    RequestUserName((C.RequestUserName)p);
                    return;
                case (short)ClientPacketIds.RequestChatItem:
                    RequestChatItem((C.RequestChatItem)p);
                    return;
                case (short)ClientPacketIds.EditGuildMember:
                    EditGuildMember((C.EditGuildMember)p);
                    return;
                case (short)ClientPacketIds.EditGuildNotice:
                    EditGuildNotice((C.EditGuildNotice)p);
                    return;
                case (short)ClientPacketIds.GuildInvite:
                    GuildInvite((C.GuildInvite)p);
                    return;
                case (short)ClientPacketIds.RequestGuildInfo:
                    RequestGuildInfo((C.RequestGuildInfo)p);
                    return;
                case (short)ClientPacketIds.GuildNameReturn:
                    GuildNameReturn((C.GuildNameReturn)p);
                    return;
                case (short)ClientPacketIds.GuildStorageGoldChange:
                    GuildStorageGoldChange((C.GuildStorageGoldChange)p);
                    return;
                case (short)ClientPacketIds.GuildStorageItemChange:
                    GuildStorageItemChange((C.GuildStorageItemChange)p);
                    return;
                default:
                    throw new NotImplementedException();
            }
        }

        public void SoftDisconnect()
        {
            Stage = GameStage.Disconnected;
            TimeDisconnected = SMain.Envir.Time;
            
            lock (Envir.AccountLock)
            {
                if (Player != null)
                    Player.StopGame();

                if (Account != null && Account.Connection == this)
                    Account.Connection = null;
            }

            Account = null;
        }
        public void Disconnect()
        {
            if (!Connected) return;

            Connected = false;
            Stage = GameStage.Disconnected;
            TimeDisconnected = SMain.Envir.Time;

            lock (SMain.Envir.Connections)
                SMain.Envir.Connections.Remove(this);

            lock (Envir.AccountLock)
            {
                if (Player != null)
                    Player.StopGame();

                if (Account != null && Account.Connection == this)
                    Account.Connection = null;

            }

            Account = null;

            _sendList = null;
            _receiveList = null;
            _retryList = null;
            _rawData = null;

            if (_client != null) _client.Client.Dispose();
            _client = null;
        }
        public void SendDisconnect(byte reason)
        {
            if (!Connected)
            {
                Disconnecting = true;
                SoftDisconnect();
                return;
            }
            
            Disconnecting = true;

            List<byte> data = new List<byte>();

            data.AddRange(new S.Disconnect { Reason = reason }.GetPacketBytes());

            BeginSend(data);
            SoftDisconnect();
        }

        private void ClientVersion(C.ClientVersion p)
        {
            if (Stage != GameStage.None) return;

            if (Settings.CheckVersion)
                if (!Functions.CompareBytes(Settings.VersionHash, p.VersionHash))
                {
                    Disconnecting = true;

                    List<byte> data = new List<byte>();

                    data.AddRange(new S.ClientVersion {Result = 0}.GetPacketBytes());

                    BeginSend(data);
                    SoftDisconnect();
                    SMain.Enqueue(SessionID + ", Disconnnected - Wrong Client Version.");
                    return;
                }
            Enqueue(new S.ClientVersion { Result = 1 });
            Stage = GameStage.Login;
        }
        private void NewAccount(C.NewAccount p)
        {
            if (Stage != GameStage.Login) return;

            SMain.Envir.NewAccount(p, this);
        }
        private void ChangePassword(C.ChangePassword p)
        {
            if (Stage != GameStage.Login) return;

            SMain.Envir.ChangePassword(p, this);
        }
        private void Login(C.Login p)
        {
            if (Stage != GameStage.Login) return;

            SMain.Envir.Login(p, this);
        }
        private void NewCharacter(C.NewCharacter p)
        {
            if (Stage != GameStage.Select) return;

            SMain.Envir.NewCharacter(p, this);
        }
        private void DeleteCharacter(C.DeleteCharacter p)
        {
            if (Stage != GameStage.Select) return;
            
            if (!Settings.AllowDeleteCharacter)
            {
                Enqueue(new S.DeleteCharacter { Result = 0 });
                return;
            }

            CharacterInfo temp = null;

            for (int i = 0; i < Account.Characters.Count; i++)
			{
			    if (Account.Characters[i].Index != p.CharacterIndex) continue;

			    temp = Account.Characters[i];
			    break;
			}

            if (temp == null)
            {
                Enqueue(new S.DeleteCharacter { Result = 1 });
                return;
            }

            temp.Deleted = true;
            temp.DeleteDate = SMain.Envir.Now;


            Enqueue(new S.DeleteCharacterSuccess { CharacterIndex = temp.Index });
        }
        private void StartGame(C.StartGame p)
        {
            if (Stage != GameStage.Select) return;

            if (!Settings.AllowStartGame)
            {
                Enqueue(new S.StartGame { Result = 0 });
                return;
            }

            if (Account == null)
            {
                Enqueue(new S.StartGame { Result = 1 });
                return;
            }


            CharacterInfo info = null;

            for (int i = 0; i < Account.Characters.Count; i++)
            {
                if (Account.Characters[i].Index != p.CharacterIndex) continue;

                info = Account.Characters[i];
                break;
            }
            if (info == null)
            {
                Enqueue(new S.StartGame { Result = 2 });
                return;
            }

            if (info.Banned)
            {
                if (info.ExpiryDate > DateTime.Now)
                {
                    Enqueue(new S.StartGameBanned { Reason = info.BanReason, ExpiryDate = info.ExpiryDate });
                    return;
                }
                info.Banned = false;
            }
            info.BanReason = string.Empty;
            info.ExpiryDate = DateTime.MinValue;

            long delay = (long) (SMain.Envir.Now - info.LastDate).TotalMilliseconds;


            if (delay < Settings.RelogDelay)
            {
                Enqueue(new S.StartGameDelay { Milliseconds = Settings.RelogDelay - delay });
                return;
            }

            Player = new PlayerObject(info, this);
            Player.StartGame();
        }

        public void LogOut()
        {
            if (Stage != GameStage.Game) return;

            Player.StopGame();

            Stage = GameStage.Select;
            Player = null;

            Enqueue(new S.LogOutSuccess { Characters = Account.GetSelectInfo() });
        }

        private void Turn(C.Turn p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > SMain.Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Turn(p.Direction);
        }
        private void Walk(C.Walk p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > SMain.Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Walk(p.Direction);
        }
        private void Run(C.Run p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > SMain.Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Run(p.Direction);
        }
        
        private void Chat(C.Chat p)
        {
            if (p.Message.Length > Globals.MaxChatLength)
            {
                SendDisconnect(2);
                return;
            }

            if (Stage != GameStage.Game) return;

            Player.Chat(p.Message);
        }

        private void MoveItem(C.MoveItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.MoveItem(p.Grid, p.From, p.To);
        }
        private void StoreItem(C.StoreItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.StoreItem(p.From, p.To);
        }
        private void TakeBackItem(C.TakeBackItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TakeBackItem(p.From, p.To);
        }
        private void MergeItem(C.MergeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.MergeItem(p.GridFrom, p.GridTo, p.IDFrom, p.IDTo);
        }
        private void EquipItem(C.EquipItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.EquipItem(p.Grid, p.UniqueID, p.To);
        }
        private void RemoveItem(C.RemoveItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveItem(p.Grid, p.UniqueID, p.To);
        }
        private void SplitItem(C.SplitItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SplitItem(p.Grid, p.UniqueID, p.Count);
        }
        private void UseItem(C.UseItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.UseItem(p.UniqueID);
        }
        private void DropItem(C.DropItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DropItem(p.UniqueID, p.Count);
        }
        private void DropGold(C.DropGold p)
        {
            if (Stage != GameStage.Game) return;

            Player.DropGold(p.Amount);
        }
        private void PickUp()
        {
            if (Stage != GameStage.Game) return;

            Player.PickUp();
        }
        private void Inspect(C.Inspect p)
        {
            if (Stage != GameStage.Game) return;


            Player.Inspect(p.ObjectID);
        }
        private void ChangeAMode(C.ChangeAMode p)
        {
            if (Stage != GameStage.Game) return;

            Player.AMode = p.Mode;

            Enqueue(new S.ChangeAMode {Mode = Player.AMode});
        }
        private void ChangePMode(C.ChangePMode p)
        {
            if (Stage != GameStage.Game) return;
            if (Player.Class != MirClass.Wizard && Player.Class != MirClass.Taoist && Player.Pets.Count == 0)
                return;

            Player.PMode = p.Mode;

            Enqueue(new S.ChangePMode { Mode = Player.PMode });
        }
        private void Attack(C.Attack p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && (Player.ActionTime > SMain.Envir.Time || Player.AttackTime > SMain.Envir.Time))
                _retryList.Enqueue(p);
            else
                Player.Attack(p.Direction, p.Spell);
        }
        private void Harvest(C.Harvest p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && Player.ActionTime > SMain.Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Harvest(p.Direction);
        }

        private void CallNPC(C.CallNPC p)
        {
            if (Stage != GameStage.Game) return;

            Player.CallNPC(p.ObjectID, p.Key);
        }
        private void BuyItem(C.BuyItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.BuyItem(p.ItemIndex, p.Count);
        }
        private void SellItem(C.SellItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SellItem(p.UniqueID, p.Count);
        }
        private void RepairItem(C.RepairItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RepairItem(p.UniqueID);
        }
        private void BuyItemBack(C.BuyItemBack p)
        {
            if (Stage != GameStage.Game) return;

           // Player.BuyItemBack(p.UniqueID, p.Count);
        }
        private void SRepairItem(C.SRepairItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RepairItem(p.UniqueID, true);
        }
        private void MagicKey(C.MagicKey p)
        {
            if (Stage != GameStage.Game) return;

            for (int i = 0; i < Player.Info.Magics.Count; i++)
            {
                UserMagic magic = Player.Info.Magics[i];
                if (magic.Spell != p.Spell)
                {
                    if (magic.Key == p.Key)
                        magic.Key = 0;
                    continue;
                }

                magic.Key = p.Key;
            }
        }
        private void Magic(C.Magic p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && (Player.ActionTime > SMain.Envir.Time || Player.SpellTime > SMain.Envir.Time))
                _retryList.Enqueue(p);
            else
                Player.Magic(p.Spell, p.Direction, p.TargetID, p.Location);
        }

        private void SwitchGroup(C.SwitchGroup p)
        {
            if (Stage != GameStage.Game) return;

            Player.SwitchGroup(p.AllowGroup);
        }
        private void AddMember(C.AddMember p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMember(p.Name);
        }
        private void DelMember(C.DelMember p)
        {
            if (Stage != GameStage.Game) return;

            Player.DelMember(p.Name);
        }
        private void GroupInvite(C.GroupInvite p)
        {
            if (Stage != GameStage.Game) return;

            Player.GroupInvite(p.AcceptInvite);
        }

        private void TownRevive()
        {
            if (Stage != GameStage.Game) return;

            Player.TownRevive();
        }

        private void SpellToggle(C.SpellToggle p)
        {
            if (Stage != GameStage.Game) return;

            Player.SpellToggle(p.Spell, p.CanUse);
        }
        private void ConsignItem(C.ConsignItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.ConsignItem(p.UniqueID, p.Price);
        }
        private void MarketSearch(C.MarketSearch p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketSearch(p.Match);
        }
        private void MarketRefresh()
        {
            if (Stage != GameStage.Game) return;

            Player.MarketRefresh();
        }

        private void MarketPage(C.MarketPage p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketPage(p.Page);
        }
        private void MarketBuy(C.MarketBuy p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketBuy(p.AuctionID);
        }
        private void MarketGetBack(C.MarketGetBack p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketGetBack(p.AuctionID);
        }
        private void RequestUserName(C.RequestUserName p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestUserName(p.UserID);
        }
        private void RequestChatItem(C.RequestChatItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestChatItem(p.ChatItemID);
        }
        private void EditGuildMember(C.EditGuildMember p)
        {
            if (Stage != GameStage.Game) return;
            Player.EditGuildMember(p.Name,p.RankName,p.RankIndex,p.ChangeType);
        }
        private void EditGuildNotice(C.EditGuildNotice p)
        {
            if (Stage != GameStage.Game) return;
            Player.EditGuildNotice(p.notice);
        }
        private void GuildInvite(C.GuildInvite p)
        {
            if (Stage != GameStage.Game) return;

            Player.GuildInvite(p.AcceptInvite);
        }
        private void RequestGuildInfo(C.RequestGuildInfo p)
        {
            if (Stage != GameStage.Game) return;
            Player.RequestGuildInfo(p.Type);
        }
        private void GuildNameReturn(C.GuildNameReturn p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildNameReturn(p.Name);
        }
        private void GuildStorageGoldChange(C.GuildStorageGoldChange p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildStorageGoldChange(p.Type, p.Amount);
        }
        private void GuildStorageItemChange(C.GuildStorageItemChange p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildStorageItemChange(p.Type, p.From, p.To);
        }
    }
}
