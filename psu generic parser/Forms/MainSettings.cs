using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class MainSettings : Form
    {
        public MainForm mainForm;
        public MainSettings(MainForm form)
        {
            mainForm = form;
            InitializeComponent();

            CompressNMLLCheckButton.Checked = mainForm.compressNMLL;
            CompressTMLLCheckButton.Checked = mainForm.compressTMLL;
            ExportPNGCheckBox.Checked = mainForm.batchPngExport;
            exportMetaDataCheckBox.Checked = mainForm.exportMetaData;
            BatchExportSubContainersCheckBox.Checked = mainForm.batchExportSubArchiveFiles;
            BatchExportSubDirectoriesCheckBox.Checked = mainForm.batchRecursive;
        }
        private void ExportPNGCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.batchPngExport = ExportPNGCheckBox.Checked;
        }

        private void CompressNMLLCheckButton_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.compressNMLL = CompressNMLLCheckButton.Checked;
        }

        private void CompressTMLLCheckButton_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.compressTMLL = CompressTMLLCheckButton.Checked;
        }

        private void exportMetaDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.exportMetaData = exportMetaDataCheckBox.Checked;
        }

        private void BatchExportSubContainersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.batchExportSubArchiveFiles = BatchExportSubContainersCheckBox.Checked;
        }

        private void MainSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BatchExportSubDirectories_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.batchRecursive = BatchExportSubDirectoriesCheckBox.Checked;
        }

        private void ForceSaveNBLInOldFormat_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.forceNblToPsu = ForceSaveNBLInOldFormat.Checked;
        }
    }
}
