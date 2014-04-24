using System;
using System.Drawing;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Trainer : MonsterObject
    {
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

        // Player attacking trainer.
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = false)
        {
            switch (type)
            {
                case DefenceType.ACAgility:
                    attacker.ReceiveChat(damage + " AC Agility Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.AC:
                    attacker.ReceiveChat(damage + " AC Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.MACAgility:
                    attacker.ReceiveChat(damage + " MAC Agility Damage inflicted on the trainer.", ChatType.Trainer);
                    break;
                case DefenceType.MAC:
                    attacker.ReceiveChat(damage + " MAC Damage inflicted on the trainer.", ChatType.Trainer);
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
            byte _masterLevel = attacker.Master.Level; // max 256
            byte _masterMaxMC = attacker.Master.MaxMC; // max 256
            int _total = (_masterLevel * 10) + _masterMaxMC;

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
    }
}