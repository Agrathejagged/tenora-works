using System;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class ListFileViewer : UserControl
    {
        private ListFile listFile;
        private string[] categories = { "XNJ", "XNT", "UNKNOWN", "UNKNOWN", "XNM", "UNKNOWN", "UNKNOWN", "XNR/REL", "UNKNOWN", "UNKNOWN", "UNKNOWN", "XNV"};

        public ListFileViewer(ListFile listFile)
        {
            InitializeComponent();
            for(int i = 0; i < listFile.filenames.Count; i++)
            {
                stringListBox.Items.Add("List " + i + ": " + (i < categories.Length ? categories[i] : "UNKNOWN"));
            }
            this.listFile = listFile;
        }

        private void stringListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = listFile.filenames[stringListBox.SelectedIndex];
        }
    }
}
