using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Counter_Editor.Files;

namespace Counter_Editor
{
    public partial class MetadataEditor : UserControl
    {
        CounterArchive.CategoryInfo currentCategory;
        TreeNode owningNode;
        bool loaded = false;

        public MetadataEditor(CounterArchive.CategoryInfo cat, TreeNode node)
        {
            owningNode = node;
            currentCategory = cat;
            InitializeComponent();
            textBox1.Text = currentCategory.categoryDescription;
            nameTextBox.Text = currentCategory.categoryName;
            loaded = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                currentCategory.categoryDescription = textBox1.Text;
                this.Parent.Refresh();
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                currentCategory.categoryName = nameTextBox.Text;
                owningNode.Text = "Category " + (owningNode.Index - 1) + ": " + currentCategory.categoryName;
                this.Parent.Refresh();
            }
        }
    }
}
