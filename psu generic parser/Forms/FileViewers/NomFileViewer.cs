using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace psu_generic_parser
{
    public partial class NomFileViewer : UserControl
    {
        NomFile internalFile;

        public NomFileViewer(NomFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;

            //Populate TreeView
            treeView1.BeginUpdate();
            for(int i = 0; i < internalFile.boneNames.Length; i++)
            {
                TreeNode node = new TreeNode($"({i}) {internalFile.boneNames[i]}");
                node.Tag = i;

                //Add frame list nodes, if they exist
                if(internalFile.rotationFrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("Rotation Frames"));
                }
                if (internalFile.xPositionFrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("X Position Frames"));
                }
                if (internalFile.yPositionFrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("Y Position Frames"));
                }
                if (internalFile.zPositionFrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("Z Position Frames"));
                }

                treeView1.Nodes.Add(node);
            }
            treeView1.EndUpdate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<List<NomFile.NomFrame>> nomFrames = null;
            //frameDataListBox.BeginUpdate();
            frameDataTextBox.Clear();
            //Only handle if on a valid node
            switch(e.Node.Text)
            {
                case "Rotation Frames":
                    nomFrames = internalFile.rotationFrameList;
                    break;
                case "X Position Frames":
                    nomFrames = internalFile.xPositionFrameList;
                    break;
                case "Y Position Frames":
                    nomFrames = internalFile.yPositionFrameList;
                    break;
                case "Z Position Frames":
                    nomFrames = internalFile.zPositionFrameList;
                    break;
                default:
                    return; //Leave if it's not a frame set
            }
            //Format
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nomFrames[(int)e.Node.Parent.Tag].Count; i++)
            {
                NomFile.NomFrame nomFrame = nomFrames[(int)e.Node.Parent.Tag][i];
                sb.AppendLine(nomFrame.ToString());
            }
            frameDataTextBox.Text = sb.ToString();
            //frameDataListBox.EndUpdate();
        }
    }
}
