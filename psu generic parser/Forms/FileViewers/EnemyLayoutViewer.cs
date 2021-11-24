using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using PSULib.FileClasses.Missions;

namespace psu_generic_parser
{
    public partial class EnemyLayoutViewer : UserControl
    {
        EnemyLayoutFile internalFile;
        DataContractJsonSerializer tempJson = new DataContractJsonSerializer(typeof(EnemyLayoutFile));
        public EnemyLayoutViewer(EnemyLayoutFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;

            InitializeDisplay();
        }

        private void InitializeDisplay()
        {
            spawnEntryCB.BeginUpdate();
            spawnEntryCB.Items.Clear();
            for (int i = 0; i < internalFile.spawns.Length; i++)
            {
                spawnEntryCB.Items.Add(i);
            }
            spawnEntryCB.EndUpdate();
            spawnEntryCB.SelectedIndex = 0;

            UpdateSpawnEntryItems();
        }

        //Update all comboboxes with subarrays of the current SpawnEntry
        private void UpdateSpawnEntryItems()
        {
            monsterListCB.BeginUpdate();
            monsterListCB.Items.Clear();
            for(int i = 0; i < internalFile.spawns[spawnEntryCB.SelectedIndex].monsters.Length; i++)
            {
                monsterListCB.Items.Add(i);
            }
            monsterListCB.EndUpdate();
            monsterListCB.SelectedIndex = 0;

            UpdateMonstersItems();

            arrangementCB.BeginUpdate();
            arrangementCB.Items.Clear();
            for (int i = 0; i < internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements.Length; i++)
            {
                arrangementCB.Items.Add(i);
            }
            arrangementCB.EndUpdate();
            arrangementCB.SelectedIndex = 0;
            UpdateArrangementDisplay();
            UpdateSpawnDataDisplay();
        }

        private void UpdateMonstersItems()
        {
            monstersCB.BeginUpdate();
            monstersCB.Items.Clear();
            for (int i = 0; i < internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex].Length; i++)
            {
                monstersCB.Items.Add(i);
            }
            monstersCB.EndUpdate();
            monstersCB.SelectedIndex = 0;

            UpdateMonsterDisplay();
        }

        private void UpdateMonsterDisplay()
        {
            monNumUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].monsterNum;
            monElementUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].element;
            monKingBuffCheck.Checked = byteToBool(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].kingBuff);
            monSwordBuffCheck.Checked = byteToBool(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].swordBuff);
            monShieldBuffCheck.Checked = byteToBool(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].shieldBuff);
            monBootBuffCheck.Checked = byteToBool(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkBuff);
            monStaffBuffCheck.Checked = byteToBool(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].staffBuff);
            monUnkByte1UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkByte1;
            monUnkShort1UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort1;
            monUnkShort2UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort2;
            monSpawnDelayUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].spawnDelay;
            monCountUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].count;
            monUnkShort3UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort3;
            monUnkShort4UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort4;
            unkShort5UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unknownShort5;
            monLevelModifierUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].levelModifier;
            monLevelCapUnusedUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].levelCapUnused;
            monUnkShort7UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort7;
            monUnkShort8UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort8;
            monUnkInt1UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkInt1;
        }

        private void UpdateArrangementDisplay()
        {
            arrIdUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].arrangementId;
            arrSpawnDelayUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].arrangementDelay;
            arrFormationUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].formation;
            arrInitialCountUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].initialCount;
            arrRespawnTriggerUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].respawnTrigger;
            arrUnkShort1UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].unknownShort1;
            arrUnkShort2UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].unknownShort2;
            arrUnkShort3UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].unknownShort3;
        }

        private void UpdateSpawnDataDisplay()
        {
            SDSpawnNumberUD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].spawnNum;
            SDUnkShort1UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unknownFlag1;
            SDUnkShort2UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unknownFlag2;
            SDUnkShort3UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unusedShort1;
            SDUnkShort4UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unusedShort2;
            SDUnkShort5UD.Value = internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unusedShort3;
        }

        private void ExportJSON_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream tempStream = saveFileDialog1.OpenFile();
                tempJson.WriteObject(tempStream, internalFile);
                tempStream.Close();
            }
        }

        private void ImportJSON_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream tempStream = openFileDialog1.OpenFile();
                EnemyLayoutFile tempLayout = (EnemyLayoutFile)tempJson.ReadObject(tempStream);
                tempStream.Close();
                internalFile.spawns = tempLayout.spawns;
                InitializeDisplay();
            }
        }

        private bool byteToBool(byte b)
        {
            return b > 0 ? true : false; 
        }

        private byte boolToByte(bool b)
        {
            return (byte)(b ? 1 : 0);
        }

        private void monstersCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMonsterDisplay();
        }

        private void arrangementCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateArrangementDisplay();
        }

        private void spawnDataCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpawnDataDisplay();
        }

        private void spawnEntryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpawnEntryItems();
        }

        private void monsterListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMonstersItems();
        }

        private void monNumUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].monsterNum = (short)monNumUD.Value;
        }

        private void monElementUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].element = (short)monElementUD.Value;
        }

        private void monKingBuffCheck_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].kingBuff = boolToByte(monKingBuffCheck.Checked);
        }

        private void monSwordBuffCheck_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].swordBuff = boolToByte(monSwordBuffCheck.Checked);
        }

        private void monShieldBuffCheck_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].shieldBuff = boolToByte(monShieldBuffCheck.Checked);
        }

        private void monBootBuffCheck_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkBuff = boolToByte(monBootBuffCheck.Checked);
        }

        private void monStaffCheck_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].staffBuff = boolToByte(monStaffBuffCheck.Checked);
        }

        private void monUnkShort1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort1 = (short)monUnkShort1UD.Value;
        }

        private void monUnkShort2UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort2 = (short)monUnkShort2UD.Value;
        }

        private void monSpawnDelayUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].spawnDelay = (short)monSpawnDelayUD.Value;
        }

        private void monCountUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].count = (short)monCountUD.Value;
        }

        private void monUnkShort3UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort3 = (short)monUnkShort3UD.Value;
        }

        private void monUnkShort4UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort4 = (short)monUnkShort4UD.Value;
        }

        private void monUnkShort5UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unknownShort5 = (short)unkShort5UD.Value;
        }

        private void monLevelModifierUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].levelModifier = (short)monLevelModifierUD.Value;
        }

        private void monLevelCapUnusedUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].levelCapUnused = (short)monLevelCapUnusedUD.Value;
        }

        private void monUnkShort7UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort7 = (short)monUnkShort7UD.Value;
        }

        private void monUnkShort8UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkShort8 = (short)monUnkShort8UD.Value;
        }

        private void monUnkInt1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkInt1 = (int)monUnkInt1UD.Value;
        }

        private void monUnkByte1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[monsterListCB.SelectedIndex][monstersCB.SelectedIndex].unkByte1 = (byte)monUnkByte1UD.Value;
        }

        private void arrIdUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].arrangementId = (short)arrIdUD.Value;
        }

        private void arrSpawnDelayUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].arrangementDelay = (short)arrSpawnDelayUD.Value;
        }

        private void arrFormationUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].formation = (short)arrFormationUD.Value;
        }

        private void arrInitialCountUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].formation = (short)arrFormationUD.Value;
        }

        private void arrRespawnTriggerUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].respawnTrigger = (short)arrRespawnTriggerUD.Value;
        }

        private void arrUnkShort1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].unknownShort1 = (short)arrUnkShort1UD.Value;
        }

        private void arrUnkShort2UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].unknownShort2 = (short)arrUnkShort2UD.Value;
        }

        private void arrUnkShort3UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementCB.SelectedIndex].unknownShort3 = (short)arrUnkShort3UD.Value;
        }

        private void SDSpawnNumberUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].spawnNum = (short)SDSpawnNumberUD.Value;
        }

        private void SDUnkShort1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unknownFlag1 = (short)SDUnkShort1UD.Value;
        }

        private void SDUnkShort2UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unknownFlag2 = (short)SDUnkShort2UD.Value;
        }

        private void SDUnkShort3UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unusedShort1 = (short)SDUnkShort3UD.Value;
        }

        private void SDUnkShort4UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unusedShort2 = (short)SDUnkShort4UD.Value;
        }

        private void SDUnkShort5UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[0].unusedShort3 = (short)SDUnkShort5UD.Value;
        }
    }
}
