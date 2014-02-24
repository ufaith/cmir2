using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    public partial class BalanceConfigForm : Form
    {
        public byte SelectedClassID = 0;
        public BalanceConfigForm()
        {
            InitializeComponent();
            ClassComboBox.Items.AddRange(Enum.GetValues(typeof(MirClass)).Cast<object>().ToArray());
            UpdateInterface();
        }

        private void BalanceConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //save configs
            Settings.Save();
            SMain.Envir.RequiresBaseStatUpdate();
        }

        private void ClassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            SelectedClassID = (byte)ClassComboBox.SelectedItem;
            UpdateInterface();
        }

        private void UpdateInterface()
        {
            if (ClassComboBox.SelectedItem == null)
            {
                HpGaintextBox.Text = string.Empty;
                HpGainRatetextBox.Text = string.Empty;
                MpGainBoosttextBox.Text = string.Empty;
                MinAcGainRatetextBox.Text = string.Empty;
                MaxAcRateGaintextBox.Text = string.Empty;
                MinMacGainRatetextBox.Text = string.Empty;
                MaxMacGainRatetextBox.Text = string.Empty;
                MinDcGainRatetextBox.Text = string.Empty;
                MaxDcGainRatetextBox.Text = string.Empty;
                MinMcGainRatetextBox.Text = string.Empty;
                MaxMcGainRatetextBox.Text = string.Empty;
                MinScGainRatetextBox.Text = string.Empty;
                MaxScGainRatetextBox.Text = string.Empty;
                BagWeigthGaintextBox.Text = string.Empty;
                WearWeightGaintextBox.Text = string.Empty;
                HandWeightGaintextBox.Text = string.Empty;
                StartAccuracytextBox.Text = string.Empty;
                StartAgilitytextBox.Text = string.Empty;
                StartCriticalRatetextBox.Text = string.Empty;
                StartCriticalDamagetextBox.Text = string.Empty;
                CriticalRateGaintextBox.Text = string.Empty;
                CriticalDamageGaintextBox.Text = string.Empty;
                MaxHealthRegentextBox.Text = string.Empty;
                HealthRegenWeighttextBox.Text = string.Empty;
                MaxManaRegentextBox.Text = string.Empty;
                ManaRegenWeighttextBox.Text = string.Empty;
                MaxPoisonRecoverytextBox.Text = string.Empty;
            }
            else
            {
                BaseStats Stats = Settings.ClassBaseStats[SelectedClassID];
                HpGaintextBox.Text = Stats.HpGain.ToString();
                HpGainRatetextBox.Text = Stats.HpGainRate.ToString();
                MpGainBoosttextBox.Text = Stats.MpGainRate.ToString();
                MinAcGainRatetextBox.Text = Stats.MinAc.ToString();
                MaxAcRateGaintextBox.Text = Stats.MaxAc.ToString();
                MinMacGainRatetextBox.Text = Stats.MinMac.ToString();
                MaxMacGainRatetextBox.Text = Stats.MaxMac.ToString();
                MinDcGainRatetextBox.Text = Stats.MinDc.ToString();
                MaxDcGainRatetextBox.Text = Stats.MaxDc.ToString();
                MinMcGainRatetextBox.Text = Stats.MinMc.ToString();
                MaxMcGainRatetextBox.Text = Stats.MaxMc.ToString();
                MinScGainRatetextBox.Text = Stats.MinSc.ToString();
                MaxScGainRatetextBox.Text = Stats.MaxSc.ToString();
                BagWeigthGaintextBox.Text = Stats.BagWeightGain.ToString();
                WearWeightGaintextBox.Text = Stats.WearWeightGain.ToString();
                HandWeightGaintextBox.Text = Stats.HandWeightGain.ToString();
                StartAccuracytextBox.Text = Stats.StartAccuracy.ToString();
                StartAgilitytextBox.Text = Stats.StartAgility.ToString();
                StartCriticalRatetextBox.Text = Stats.StartCriticalRate.ToString();
                StartCriticalDamagetextBox.Text = Stats.StartCriticalDamage.ToString();
                CriticalRateGaintextBox.Text = Stats.CritialRateGain.ToString();
                CriticalDamageGaintextBox.Text = Stats.CriticalDamageGain.ToString();
            }
            MaxMagicResisttextbox.Text = Settings.MaxMagicResist.ToString();
            MagicResistWeigttextbox.Text = Settings.MagicResistWeight.ToString();
            MaxPoisonResisttextbox.Text = Settings.MaxPoisonResist.ToString();
            PoisonResistWeighttextbox.Text = Settings.PoisonResistWeight.ToString();
            MaxCriticalRatetextbox.Text = Settings.MaxCriticalRate.ToString();
            CritialRateWeighttextbox.Text = Settings.CriticalRateWeight.ToString();
            MaxCriticalDamagetextbox.Text = Settings.MaxCriticalDamage.ToString();
            CriticalDamagetextbox.Text = Settings.CriticalDamageWeight.ToString();
            MaxPoisonAttacktextbox.Text = Settings.MaxPoisonAttack.ToString();
            PoisonAttackWeighttextbox.Text = Settings.PoisonAttackWeight.ToString();
            MaxFreezingtextbox.Text = Settings.MaxFreezing.ToString();
            FreezingWeighttextbox.Text = Settings.FreezingAttackWeight.ToString();
            MaxHealthRegentextBox.Text = Settings.MaxHealthRegen.ToString();
            HealthRegenWeighttextBox.Text = Settings.HealthRegenWeight.ToString();
            MaxManaRegentextBox.Text = Settings.MaxManaRegen.ToString();
            ManaRegenWeighttextBox.Text = Settings.ManaRegenWeight.ToString();
            MaxPoisonRecoverytextBox.Text = Settings.MaxPoisonRecovery.ToString();


            CanFreezecheckBox.Checked = Settings.PvpCanFreeze;
            CanResistPoisoncheckBox.Checked = Settings.PvpCanResistPoison;
            CanResistMagiccheckBox.Checked = Settings.PvpCanResistMagic;

        }

        private void MaxMagicResisttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxMagicResist = temp;
        }

        private void MagicResistWeigttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MagicResistWeight = temp;
        }

        private void MaxPoisonResisttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxPoisonResist = temp;
        }

        private void PoisonResistWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.PoisonResistWeight = temp;
        }

        private void MaxCriticalRatetextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxCriticalRate = temp;
        }

        private void CritialRateWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.CriticalRateWeight = temp;
        }

        private void MaxCriticalDamagetextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxCriticalDamage = Math.Min((byte)1,temp);
        }

        private void CriticalDamagetextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.CriticalDamageWeight = temp;
        }

        private void MaxFreezingtextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxFreezing = temp;
        }

        private void FreezingWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.FreezingAttackWeight = temp;
        }

        private void MaxPoisonAttacktextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxPoisonAttack = temp;
        }

        private void PoisonAttackWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.PoisonAttackWeight = temp;
        }

        private void CanResistMagiccheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.PvpCanResistMagic = CanResistMagiccheckBox.Checked;
        }

        private void CanResistPoisoncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.PvpCanResistPoison = CanResistPoisoncheckBox.Checked;
        }

        private void CanFreezecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.PvpCanFreeze = CanFreezecheckBox.Checked;
        }

        private void HpGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp;

            if (!float.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].HpGain = temp;
        }

        private void HpGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp;

            if (!float.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].HpGainRate = temp;
        }

        private void MpGainBoosttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp;

            if (!float.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MpGainRate = temp;
        }

        private void BagWeigthGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp;

            if (!float.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].BagWeightGain = temp;
        }

        private void WearWeightGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp;

            if (!float.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].WearWeightGain = temp;
        }

        private void HandWeightGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp;

            if (!float.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].HandWeightGain = temp;
        }

        private void MinAcGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MinAc = temp;
        }

        private void MaxAcRateGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MaxAc = temp;
        }

        private void MinMacGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MinMac = temp;
        }

        private void MaxMacGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MaxMac = temp;
        }

        private void MinDcGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MinDc = temp;
        }

        private void MaxDcGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MaxDc = temp;
        }

        private void MinMcGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MinMc = temp;
        }

        private void MaxMcGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MaxMc = temp;
        }

        private void MinScGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MinSc = temp;
        }

        private void MaxScGainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].MaxSc = temp;
        }

        private void StartAccuracytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].StartAccuracy = temp;
        }

        private void StartAgilitytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].StartAgility = temp;
        }

        private void StartCriticalRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].StartCriticalRate = temp;
        }

        private void StartCriticalDamagetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].StartCriticalDamage = temp;
        }

        private void CriticalRateGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].CritialRateGain = temp;
        }

        private void CriticalDamageGaintextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ClassBaseStats[SelectedClassID].CriticalDamageGain = temp;
        }

        private void MaxHealthRegentextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxHealthRegen = temp;
        }

        private void HealthRegenWeighttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.HealthRegenWeight = temp;
        }

        private void MaxManaRegentextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxManaRegen = temp;
        }

        private void ManaRegenWeighttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.ManaRegenWeight = temp;
        }

        private void MaxPoisonRecoverytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MaxPoisonRecovery = temp;
        }
    }
}
