using PSULib.FileClasses.Enemies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.Enemies
{
    public partial class DamageDataFileViewer : UserControl
    {
        private DamageDataFile damageDataFile;
        private List<DamageDataAnglePanel> anglePanels = new List<DamageDataAnglePanel>();
        public DamageDataFileViewer(DamageDataFile damageDataFile)
        {
            this.damageDataFile = damageDataFile;
            InitializeComponent();
            for(int i = 0; i < damageDataFile.DamageTypeEntries.Count; i++)
            {
                topLevelListComboBox.Items.Add("Top Level List " + i);
            }
            topLevelListComboBox.SelectedIndex = 0;
        }

        private void topLevelListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateListBox();
        }

        private void updateListBox()
        {
            damageDataEntryListBox.BeginUpdate();
            damageDataEntryListBox.Items.Clear();
            for(int i = 0; i < damageDataFile.DamageTypeEntries[topLevelListComboBox.SelectedIndex].Count; i++)
            {
                damageDataEntryListBox.Items.Add("Entry " + i);
            }
            damageDataEntryListBox.EndUpdate();
            damageDataEntryListBox.SelectedIndex = 0;
        }

        private void damageDataEntryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            reloadRightPanel();
        }

        private void reloadRightPanel()
        {
            int topLevelIndex = topLevelListComboBox.SelectedIndex;
            int secondLevelIndex = damageDataEntryListBox.SelectedIndex;
            if (topLevelIndex < damageDataFile.DamageTypeEntries.Count && secondLevelIndex < damageDataFile.DamageTypeEntries[topLevelIndex].Count)
            {
                angleTablePanel.SuspendLayout();
                var entry = damageDataFile.DamageTypeEntries[topLevelIndex][secondLevelIndex];
                eventTypeNumericUpDown.Value = entry.DamageType;
                RowStyle temp = angleTablePanel.RowStyles[0];
                angleTablePanel.RowStyles.Clear();
                angleTablePanel.Controls.Clear();
                anglePanels.Clear();

                for(int i = 0; i < entry.Angles.Count; i++)
                {
                    var newAngle = entry.Angles[i];
                    var anglePanel = new DamageDataAnglePanel(this, newAngle);
                    anglePanels.Add(anglePanel);
                    anglePanel.Dock = DockStyle.Fill;
                    angleTablePanel.Controls.Add(anglePanel);
                    angleTablePanel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                }
                angleTablePanel.RowCount = anglePanels.Count;
                angleTablePanel.ResumeLayout(true);
                angleTablePanel.AutoScroll = false;
                angleTablePanel.AutoScroll = true;
            }
        }

        public void RemoveAngle(DamageDataFile.DamageAngleEntry angleEntry, DamageDataAnglePanel anglePanel)
        {
            int topLevelIndex = topLevelListComboBox.SelectedIndex;
            int secondLevelIndex = damageDataEntryListBox.SelectedIndex;
            if (topLevelIndex < damageDataFile.DamageTypeEntries.Count && secondLevelIndex < damageDataFile.DamageTypeEntries[topLevelIndex].Count && anglePanels.Count > 1)
            {
                int entryIndex = damageDataFile.DamageTypeEntries[topLevelIndex][secondLevelIndex].Angles.IndexOf(angleEntry);
                damageDataFile.DamageTypeEntries[topLevelIndex][secondLevelIndex].Angles.Remove(angleEntry);
                reloadRightPanel();
            }
        }

        private void addAngleButton_Click(object sender, EventArgs e)
        {
            int topLevelIndex = topLevelListComboBox.SelectedIndex;
            int secondLevelIndex = damageDataEntryListBox.SelectedIndex;
            if (topLevelIndex < damageDataFile.DamageTypeEntries.Count && secondLevelIndex < damageDataFile.DamageTypeEntries[topLevelIndex].Count)
            {
                var newAngle = new DamageDataFile.DamageAngleEntry();
                damageDataFile.DamageTypeEntries[topLevelIndex][secondLevelIndex].Angles.Add(newAngle);
                reloadRightPanel();
            }
        }
    }
}
