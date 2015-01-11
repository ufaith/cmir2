using System;
using System.Drawing;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Trainer : MonsterObject
    {
        private PlayerObject _CurrentAttacker = null;
        private int _hitCount = 0, _totalDamage = 0;
        private long _lastAttackTime = 0;

        protected override bool CanAttack { get { return false; } }
        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }
        protected override bool DropItem(UserItem item) { throw new NotSupportedException(); }
        protected override bool DropGold(uint gold) { throw new NotSupportedException(); }

        protected override void Attack() { }
        protected override void ProcessRegen() { }
        protected override void ProcessRoam() { }

        public override bool Blocking { get { return true; } }
        public override bool IsAttackTarget(PlayerObject attacker) { return true; }
        public override bool IsAttackTarget(MonsterObject attacker) { return true; }

        public override void Die() { }
        public override void ReceiveChat(string text, ChatType type) { }

        protected internal Trainer(MonsterInfo info)
            : base(info) { }

        public override void Spawned()
        {
            if (Respawn != null && Respawn.Info.Direction < 8)
                Direction = (MirDirection)Respawn.Info.Direction;

            base.Spawned();
        }

        public override void Process()
        {
            base.Process();

            if (_CurrentAttacker != null && _lastAttackTime + 5000 < Envir.Time)
            {
                OutputAverage();
                ResetStats();
            }
        }

        // Player attacking trainer.
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = false)
        {
            if (attacker == null) return 0;

            if (_CurrentAttacker != null && _CurrentAttacker != attacker)
            {
                OutputAverage();
                ResetStats();
            }

            _CurrentAttacker = attacker;
            _hitCount++;
            _totalDamage += damage;
            _lastAttackTime = Envir.Time;

            switch (type)
            {
                case DefenceType.ACAgility:
                    attacker.ReceiveChat(damage + " Physical Agility Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.AC:
                    attacker.ReceiveChat(damage + " Physical Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.MACAgility:
                    attacker.ReceiveChat(damage + " Magic Agility Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.MAC:
                    attacker.ReceiveChat(damage + " Magic Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.Agility:
                    attacker.ReceiveChat(damage + " Agility Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
            }
            return 1;
        }

        // Pet attacking trainer.
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            if (attacker == null || attacker.Master == null) return 0;
            
            byte _masterLevel = attacker.Master.Level; // max 256
            byte _masterMaxMC = attacker.Master.MaxMC; // max 256
            int _total = (_masterLevel * 10) + _masterMaxMC;

            if (_CurrentAttacker != null && _CurrentAttacker != attacker.Master)
            {
                OutputAverage();
                ResetStats();
            }

            _CurrentAttacker = (PlayerObject)attacker.Master;
            _hitCount++;
            _totalDamage += damage;
            _lastAttackTime = Envir.Time;


            switch (type)
            {
                case DefenceType.ACAgility:
                    attacker.Master.ReceiveChat(damage + " AC Agility Damage inflicted on the trainer by your pet.", ChatType.Trainer);
                    break;
                case DefenceType.AC:
                    attacker.Master.ReceiveChat(damage + " AC Damage inflicted on the trainer by your pet.", ChatType.Trainer);
                    break;
                case DefenceType.MACAgility:
                    attacker.Master.ReceiveChat(damage + " MAC Agility Damage inflicted on the trainer by your pet.", ChatType.Trainer);
                    break;
                case DefenceType.MAC:
                    attacker.Master.ReceiveChat(damage + " MAC Damage inflicted on the trainer by your pet.", ChatType.Trainer);
                    break;
                case DefenceType.Agility:
                    attacker.Master.ReceiveChat(damage + " Agility Damage inflicted on the trainer by your pet.", ChatType.Trainer);
                    break;
            }

            attacker.PetExp((uint)_total);
            return 1;
        }

        private void ResetStats()
        {
            _CurrentAttacker = null;
            _hitCount = 0;
            _totalDamage = 0;
            _lastAttackTime = 0;
        }

        private void OutputAverage()
        {
            if (_CurrentAttacker == null) return;

            _CurrentAttacker.ReceiveChat((_totalDamage / _hitCount) + " Average Damage inflicted on the trainer.", ChatType.Trainer);
        }
    }
}