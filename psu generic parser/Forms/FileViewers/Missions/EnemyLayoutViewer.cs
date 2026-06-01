using PSULib.FileClasses.Missions;
using PSULib.FileClasses.Missions.Sets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using static PSULib.FileClasses.Missions.EnemyLayoutFile;

namespace psu_generic_parser
{
    public partial class EnemyLayoutViewer : UserControl
    {
        EnemyLayoutFile internalFile;
        DataContractJsonSerializer tempJson = new DataContractJsonSerializer(typeof(EnemyLayoutFile));
        bool initializing = true;
        public EnemyLayoutViewer(EnemyLayoutFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            //Validate that files actually ARE how we expect here
            //We don't want to mangle someone's enemy file...
            for (int i = 0; i < internalFile.spawns.Count; i++)
            {
                if (internalFile.spawns[i].monsters.Count != internalFile.spawns[i].arrangements.Count)
                {
                    MessageBox.Show("This file appears to be malformed; spawn " + i + " has " + internalFile.spawns[i].monsters.Count + " monster entries but " + internalFile.spawns[i].arrangements.Count + " arrangements. This will not work in the game.");
                }
            }

            UpdateSpawnEntryList(0);
            UpdateArrangementList(0);
            UpdateSpawnDataList(0);
            UpdateMonstersItems(0);
            initializing = false;
            UpdateMonsterDisplay();
            UpdateArrangementDisplay();
            UpdateSpawnDataDisplay();
        }

        private void UpdateSpawnEntryList(int newIndex)
        {
            spawnEntryCB.BeginUpdate();
            spawnEntryCB.Items.Clear();
            for (int i = 0; i < internalFile.spawns.Count; i++)
            {
                var spawnEntry = internalFile.spawns[i];
                HashSet<string> foundMonsters = getMonsters(spawnEntry);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(i);
                stringBuilder.Append(": ");
                stringBuilder.Append(string.Join(", ", foundMonsters));

                spawnEntryCB.Items.Add(string.Format("{0}: {1}", i, string.Join(", ", foundMonsters)));
            }
            spawnEntryCB.EndUpdate();
            spawnEntryCB.Enabled = spawnEntryCB.Items.Count > 1;
            removeEntryButton.Enabled = spawnEntryCB.Items.Count > 1;
            spawnEntryCB.SelectedIndex = newIndex;
        }

        //Update all comboboxes with subarrays of the current SpawnEntry
        private void UpdateArrangementList(int newIndex)
        {
            arrangementComboBox.BeginUpdate();
            arrangementComboBox.Items.Clear();
            for (int i = 0; i < internalFile.spawns[spawnEntryCB.SelectedIndex].monsters.Count; i++)
            {
                HashSet<string> foundMonsters = getMonsters(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[i]);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(i);
                stringBuilder.Append(": ");
                stringBuilder.Append(string.Join(", ", foundMonsters));
                arrangementComboBox.Items.Add(string.Format("{0}: {1}", i, string.Join(", ", foundMonsters)));
                //arrangementComboBox.Items.Add(stringBuilder);
            }
            arrangementComboBox.EndUpdate();
            arrangementComboBox.Enabled = arrangementComboBox.Items.Count > 1;
            removeArrangementButton.Enabled = arrangementComboBox.Items.Count > 1;
            arrangementComboBox.SelectedIndex = newIndex;
        }

        private void UpdateSpawnDataList(int newIndex) { 

            spawnDataSelectionComboBox.BeginUpdate();
            spawnDataSelectionComboBox.Items.Clear();
            for (int i = 0; i < internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData.Count; i++)
            {
                spawnDataSelectionComboBox.Items.Add(string.Format("{0} / {1}", i + 1, internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData.Count));
            }
            spawnDataSelectionComboBox.EndUpdate();
            spawnDataSelectionComboBox.Enabled = spawnDataSelectionComboBox.Items.Count > 1;
            removeSpawnDataButton.Enabled = spawnDataSelectionComboBox.Items.Count > 1;
            spawnDataSelectionComboBox.SelectedIndex = newIndex;
        }

        private void UpdateMonstersItems(int newIndex)
        {
            monstersCB.BeginUpdate();
            monstersCB.Items.Clear();
            for (int i = 0; i < internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex].Count; i++)
            {
                monstersCB.Items.Add(string.Format("{0}: {1}", i, string.Join(", ", DefaultEnemyText.getEnemyName(internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][i].monsterNum))));
            }
            monstersCB.EndUpdate();
            monstersCB.Enabled = monstersCB.Items.Count > 1;
            removeMonsterButton.Enabled = monstersCB.Items.Count > 1;
            monstersCB.SelectedIndex = newIndex;
        }

        private void UpdateMonsterDisplay()
        {
            int selectedEntry = Math.Max(0, spawnEntryCB.SelectedIndex);
            int selectedArrangement = Math.Max(0, arrangementComboBox.SelectedIndex);
            int selectedMonster = Math.Max(0, monstersCB.SelectedIndex);
            var activeMonster = internalFile.spawns[selectedEntry].monsters[selectedArrangement][selectedMonster];
            monNumUD.Value = activeMonster.monsterNum;
            monElementUD.Value = activeMonster.element;
            monKingBuffCheck.Checked = byteToBool(activeMonster.kingBuff);
            monBuff1Check.Checked = byteToBool(activeMonster.buff1);
            monBuff2Check.Checked = byteToBool(activeMonster.buff2);
            monBuff3Check.Checked = byteToBool(activeMonster.buff3);
            monBuff4Check.Checked = byteToBool(activeMonster.buff4);
            monUnkByte1UD.Value = activeMonster.unkByte1;
            monUnkShort1UD.Value = activeMonster.spawnAnimation;
            monUnkShort2UD.Value = activeMonster.unkShort2;
            monSpawnDelayUD.Value = activeMonster.spawnDelay;
            monCountUD.Value = activeMonster.count;
            monUnkShort3UD.Value = activeMonster.unkShort3;
            monUnkShort4UD.Value = activeMonster.unkShort4;
            unkShort5UD.Value = activeMonster.unknownShort5;
            monLevelModifierUD.Value = activeMonster.levelModifier;
            monLevelCapUnusedUD.Value = activeMonster.levelCapUnused;
            monUnkShort7UD.Value = activeMonster.unkShort7;
            monUnkShort8UD.Value = activeMonster.unkShort8;
            monUnkInt1UD.Value = activeMonster.unkInt1;
        }

        private void UpdateArrangementDisplay()
        {
            int selectedEntry = Math.Max(0, spawnEntryCB.SelectedIndex);
            int selectedArrangement = Math.Max(0, arrangementComboBox.SelectedIndex);
            var activeArrangement = internalFile.spawns[selectedEntry].arrangements[selectedArrangement];
            arrIdUD.Value = activeArrangement.arrangementId;
            arrSpawnDelayUD.Value = activeArrangement.arrangementDelay;
            arrFormationUD.Value = activeArrangement.formation;
            arrInitialCountUD.Value = activeArrangement.initialCount;
            arrRespawnTriggerUD.Value = activeArrangement.respawnTrigger;
            arrUnkShort1UD.Value = activeArrangement.unknownShort1;
            arrUnkShort2UD.Value = activeArrangement.unknownShort2;
            arrUnkShort3UD.Value = activeArrangement.unknownShort3;
        }

        private void UpdateSpawnDataDisplay()
        {
            int selectedEntry = Math.Max(0, spawnEntryCB.SelectedIndex);
            int selectedSpawnData = Math.Max(0, spawnDataSelectionComboBox.SelectedIndex);
            var activeSpawnData = internalFile.spawns[selectedEntry].spawnData[selectedSpawnData];
            SDSpawnNumberUD.Value = activeSpawnData.spawnNum;
            SDUnkShort1UD.Value = activeSpawnData.unknownFlag1;
            SDUnkShort2UD.Value = activeSpawnData.unknownFlag2;
            SDUnkShort3UD.Value = activeSpawnData.unusedShort1;
            SDUnkShort4UD.Value = activeSpawnData.unusedShort2;
            SDUnkShort5UD.Value = activeSpawnData.unusedShort3;
        }

        private HashSet<string> getMonsters(EnemyLayoutFile.SpawnEntry spawnEntry, HashSet<string> baseSet = null)
        {
            HashSet<string> foundMonsters = baseSet != null ? baseSet : new HashSet<string>();
            for (int j = 0; j < spawnEntry.monsters.Count; j++)
            {
                getMonsters(spawnEntry.monsters[j], foundMonsters);
            }
            return foundMonsters;
        }

        private HashSet<string> getMonsters(List<EnemyLayoutFile.MonsterEntry> monsterList, HashSet<string> baseSet = null)
        {
            HashSet<string> foundMonsters = baseSet != null ? baseSet : new HashSet<string>();
            for (int k = 0; k < monsterList.Count; k++)
            {
                foundMonsters.Add(DefaultEnemyText.getEnemyName(monsterList[k].monsterNum));
            }
            return foundMonsters;
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
                UpdateSpawnEntryList(0);
            }
        }

        private bool byteToBool(byte b)
        {
            return b > 0; 
        }

        private byte boolToByte(bool b)
        {
            return (byte)(b ? 1 : 0);
        }

        private void monstersCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                UpdateMonsterDisplay();
            }
        }

        private void spawnDataSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                UpdateSpawnDataDisplay();
            }
        }

        private void spawnEntryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                UpdateArrangementList(0);
                UpdateSpawnDataList(0);
                UpdateMonstersItems(0);
                monstersCB.SelectedIndex = 0;
            }
        }

        private void monsterListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                UpdateMonstersItems(0);
                UpdateArrangementDisplay();
            }
        }

        private void monNumUD_ValueChanged(object sender, EventArgs e)
        {
            short enemyIndex = (short)monNumUD.Value;
            enemyNameLabel.Text = DefaultEnemyText.getEnemyName(enemyIndex);
            int selectedEntryIndex = spawnEntryCB.SelectedIndex;
            int selectedArrangementIndex = arrangementComboBox.SelectedIndex;
            int selectedMonsterIndex = monstersCB.SelectedIndex;
            if (enemyIndex != internalFile.spawns[selectedEntryIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].monsterNum)
            {
                internalFile.spawns[selectedEntryIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].monsterNum = enemyIndex;
                //Update the combo box text for this. Because we're not adding/removing anything, we don't need to update the selected indexes.
                UpdateSpawnEntryList(selectedEntryIndex);
                arrangementComboBox.SelectedIndex = selectedArrangementIndex;
                monstersCB.SelectedIndex = selectedMonsterIndex;
            }
        }

        private void monElementUD_ValueChanged(object sender, EventArgs e)
        {
            int elementIndex = (int)monElementUD.Value;
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].element = (short)monElementUD.Value;
            elementNameLabel.Text = DefaultEnemyText.getElementName(elementIndex);
        }

        private void monKingBuffCheck_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].kingBuff = boolToByte(monKingBuffCheck.Checked);
        }

        private void monBuff1Check_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].buff1 = boolToByte(monBuff1Check.Checked);
        }

        private void monBuff2Check_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].buff2 = boolToByte(monBuff2Check.Checked);
        }

        private void monBuff3Check_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].buff3 = boolToByte(monBuff3Check.Checked);
        }

        private void monBuff4Check_CheckedChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].buff4 = boolToByte(monBuff4Check.Checked);
        }

        private void monUnkShort1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].spawnAnimation = (short)monUnkShort1UD.Value;
        }

        private void monUnkShort2UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkShort2 = (short)monUnkShort2UD.Value;
        }

        private void monSpawnDelayUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].spawnDelay = (short)monSpawnDelayUD.Value;
        }

        private void monCountUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].count = (short)monCountUD.Value;
        }

        private void monUnkShort3UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkShort3 = (short)monUnkShort3UD.Value;
        }

        private void monUnkShort4UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkShort4 = (short)monUnkShort4UD.Value;
        }

        private void monUnkShort5UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unknownShort5 = (short)unkShort5UD.Value;
        }

        private void monLevelModifierUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].levelModifier = (short)monLevelModifierUD.Value;
        }

        private void monLevelCapUnusedUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].levelCapUnused = (short)monLevelCapUnusedUD.Value;
        }

        private void monUnkShort7UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkShort7 = (short)monUnkShort7UD.Value;
        }

        private void monUnkShort8UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkShort8 = (short)monUnkShort8UD.Value;
        }

        private void monUnkInt1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkInt1 = (int)monUnkInt1UD.Value;
        }

        private void monUnkByte1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex][monstersCB.SelectedIndex].unkByte1 = (byte)monUnkByte1UD.Value;
        }

        private void arrIdUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].arrangementId = (short)arrIdUD.Value;
        }

        private void arrSpawnDelayUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].arrangementDelay = (short)arrSpawnDelayUD.Value;
        }

        private void arrFormationUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].formation = (short)arrFormationUD.Value;
        }

        private void arrInitialCountUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].initialCount = (short)arrInitialCountUD.Value;
        }

        private void arrRespawnTriggerUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].respawnTrigger = (short)arrRespawnTriggerUD.Value;
        }

        private void arrUnkShort1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].unknownShort1 = (short)arrUnkShort1UD.Value;
        }

        private void arrUnkShort2UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].unknownShort2 = (short)arrUnkShort2UD.Value;
        }

        private void arrUnkShort3UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements[arrangementComboBox.SelectedIndex].unknownShort3 = (short)arrUnkShort3UD.Value;
        }

        private void SDSpawnNumberUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[spawnDataSelectionComboBox.SelectedIndex].spawnNum = (short)SDSpawnNumberUD.Value;
        }

        private void SDUnkShort1UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[spawnDataSelectionComboBox.SelectedIndex].unknownFlag1 = (short)SDUnkShort1UD.Value;
        }

        private void SDUnkShort2UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[spawnDataSelectionComboBox.SelectedIndex].unknownFlag2 = (short)SDUnkShort2UD.Value;
        }

        private void SDUnkShort3UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[spawnDataSelectionComboBox.SelectedIndex].unusedShort1 = (short)SDUnkShort3UD.Value;
        }

        private void SDUnkShort4UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[spawnDataSelectionComboBox.SelectedIndex].unusedShort2 = (short)SDUnkShort4UD.Value;
        }

        private void SDUnkShort5UD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData[spawnDataSelectionComboBox.SelectedIndex].unusedShort3 = (short)SDUnkShort5UD.Value;
        }

        private void addEntryButton_Click(object sender, EventArgs e)
        {
            var index = spawnEntryCB.SelectedIndex;
            //This needs to not just be an empty entry, it must have a blank entry in every sublist.
            SpawnEntry newEntry = new SpawnEntry();
            newEntry.monsters.Add(new List<MonsterEntry>());
            newEntry.monsters[0].Add(new MonsterEntry());
            newEntry.spawnData.Add(new SpawnData());
            newEntry.arrangements.Add(new Arrangement());
            internalFile.spawns.Insert(index + 1, newEntry);
            UpdateSpawnEntryList(index + 1);
        }

        private void removeEntryButton_Click(object sender, EventArgs e)
        {
            //Bad things happen if you delete the last one.
            if (internalFile.spawns.Count > 1)
            {
                var index = spawnEntryCB.SelectedIndex;
                internalFile.spawns.RemoveAt(index);
                UpdateSpawnEntryList(Math.Min(index, internalFile.spawns.Count - 1));
            }
        }

        private void addArrangementButton_Click(object sender, EventArgs e)
        {
            var index = arrangementComboBox.SelectedIndex;
            List<MonsterEntry> newEntries = new List<MonsterEntry>();
            newEntries.Add(new MonsterEntry());
            Arrangement newArrangement = new Arrangement();
            internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements.Insert(index + 1, new Arrangement());
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters.Insert(index + 1, newEntries);
            UpdateSpawnEntryList(spawnEntryCB.SelectedIndex);
            arrangementComboBox.SelectedIndex = index + 1;
        }

        private void removeArrangementButton_Click(object sender, EventArgs e)
        {
            //Bad things happen if you delete the last one.
            if (internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements.Count > 1)
            {
                var index = arrangementComboBox.SelectedIndex;
                internalFile.spawns[spawnEntryCB.SelectedIndex].arrangements.RemoveAt(index);
                internalFile.spawns[spawnEntryCB.SelectedIndex].monsters.RemoveAt(index);
                UpdateSpawnEntryList(spawnEntryCB.SelectedIndex);
                arrangementComboBox.SelectedIndex = Math.Min(index, arrangementComboBox.Items.Count - 1);
            }
        }

        private void addMonsterButton_Click(object sender, EventArgs e)
        {
            var index = monstersCB.SelectedIndex;
            int selectedArrangement = arrangementComboBox.SelectedIndex;
            int selectedSpawn = spawnEntryCB.SelectedIndex;
            internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex].Insert(index + 1, new MonsterEntry());
            UpdateSpawnEntryList(spawnEntryCB.SelectedIndex);
            arrangementComboBox.SelectedIndex = selectedArrangement;
            monstersCB.SelectedIndex = index + 1;
        }

        private void removeMonsterButton_Click(object sender, EventArgs e)
        {
            //Bad things happen if you delete the last one.
            if (internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex].Count > 1)
            {
                var index = monstersCB.SelectedIndex;
                int selectedArrangement = arrangementComboBox.SelectedIndex;
                int selectedSpawn = spawnEntryCB.SelectedIndex;
                internalFile.spawns[spawnEntryCB.SelectedIndex].monsters[arrangementComboBox.SelectedIndex].RemoveAt(index);
                UpdateSpawnEntryList(spawnEntryCB.SelectedIndex);
                arrangementComboBox.SelectedIndex = selectedArrangement;
                monstersCB.SelectedIndex = Math.Min(index, monstersCB.Items.Count - 1); 
            }
        }

        private void addSpawnDataButton_Click(object sender, EventArgs e)
        {
            var index = spawnDataSelectionComboBox.SelectedIndex;
            internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData.Insert(index + 1, new SpawnData());
            UpdateSpawnDataList(index + 1);
        }

        private void removeSpawnDataButton_Click(object sender, EventArgs e)
        {
            //Bad things happen if you delete the last one.
            if (internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData.Count > 1)
            {
                var index = spawnDataSelectionComboBox.SelectedIndex;
                int selectedArrangement = arrangementComboBox.SelectedIndex;
                int selectedSpawn = spawnEntryCB.SelectedIndex;
                internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData.RemoveAt(index);
                UpdateSpawnDataList(Math.Min(index, internalFile.spawns[spawnEntryCB.SelectedIndex].spawnData.Count - 1));
            }
        }
    }
}
