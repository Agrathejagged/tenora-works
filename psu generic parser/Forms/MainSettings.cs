using PSULib.FileClasses.Archives;
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
            ExportPNGCheckBox.Checked = mainForm.batchPngExport;
            exportMetaDataCheckBox.Checked = mainForm.exportMetaData;
            BatchExportSubContainersCheckBox.Checked = mainForm.batchExportSubArchiveFiles;
            BatchExportSubDirectoriesCheckBox.Checked = mainForm.batchRecursive;

            switch(NblLoader.NmllCompressionOverride)
            {
                case NblLoader.CompressionOverride.ForceCompress: alwaysCompressNmllRadioButton.Checked = true; break;
                case NblLoader.CompressionOverride.ForceDecompress: alwaysDecompressNmllRadioButton.Checked = true; break;
                default: useOriginalNmllCompressionRadioButton.Checked = true; break;
            }

            switch (NblLoader.TmllCompressionOverride)
            {
                case NblLoader.CompressionOverride.ForceCompress: alwaysCompressTmllRadioButton.Checked = true; break;
                case NblLoader.CompressionOverride.ForceDecompress: alwaysDecompressTmllRadioButton.Checked = true; break;
                default: useOriginalTmllCompressionRadioButton.Checked = true; break;
            }
        }
        private void ExportPNGCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.batchPngExport = ExportPNGCheckBox.Checked;
        }

        private void exportMetaDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.exportMetaData = exportMetaDataCheckBox.Checked;
        }

        private void BatchExportSubContainersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.batchExportSubArchiveFiles = BatchExportSubContainersCheckBox.Checked;
        }

        private void BatchExportSubDirectories_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.batchRecursive = BatchExportSubDirectoriesCheckBox.Checked;
        }

        private void nmllChunkOverrideOptions_CheckedChanged(object sender, EventArgs e)
        {
            NblLoader.CompressionOverride compressionOverride = NblLoader.CompressionOverride.UseFileSetting;
            if(alwaysCompressNmllRadioButton.Checked)
            {
                compressionOverride = NblLoader.CompressionOverride.ForceCompress;
            } else if(alwaysDecompressNmllRadioButton.Checked)
            {
                compressionOverride = NblLoader.CompressionOverride.ForceDecompress;
            }
            mainForm.setNmllCompressOverride(compressionOverride);
        }

        private void tmllChunkOverrideOptions_CheckedChanged(object sender, EventArgs e)
        {
            NblLoader.CompressionOverride compressionOverride = NblLoader.CompressionOverride.UseFileSetting;
            if (alwaysCompressTmllRadioButton.Checked)
            {
                compressionOverride = NblLoader.CompressionOverride.ForceCompress;
            }
            else if (alwaysDecompressTmllRadioButton.Checked)
            {
                compressionOverride = NblLoader.CompressionOverride.ForceDecompress;
            }
            mainForm.setTmllCompressOverride(compressionOverride);
        }
    }
}
