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
                if (internalFile.positionFrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("Position Frames"));
                }
                if (internalFile.list3FrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("Unknown 0 Frames"));
                }
                if (internalFile.list4FrameList[i] != null)
                {
                    node.Nodes.Add(new TreeNode("Unknown 1 Frames"));
                }

                treeView1.Nodes.Add(node);
            }
            treeView1.EndUpdate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<List<NomFile.NomFrame>> nomFrames = null;
            //frameDataListBox.BeginUpdate();
            frameDataListBox.Items.Clear();
            //Only handle if on a valid node
            switch(e.Node.Text)
            {
                case "Rotation Frames":
                    nomFrames = internalFile.rotationFrameList;
                    break;
                case "Position Frames":
                    nomFrames = internalFile.positionFrameList;
                    break;
                case "Unknown 0 Frames":
                    nomFrames = internalFile.list3FrameList;
                    break;
                case "Unknown 1 Frames":
                    nomFrames = internalFile.list4FrameList;
                    break;
                default:
                    return; //Leave if it's not a frame set
            }
            //Format
            for(int i = 0; i < nomFrames[(int)e.Node.Parent.Tag].Count; i++)
            {
                NomFile.NomFrame nomFrame = nomFrames[(int)e.Node.Parent.Tag][i];
                frameDataListBox.Items.Add(nomFrame.ToString());
            }
            //frameDataListBox.EndUpdate();
        }
    }
}
