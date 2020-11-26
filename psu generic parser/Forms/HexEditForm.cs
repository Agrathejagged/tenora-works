using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class HexEditForm : Form
    {
        private byte[] internalBytes;
        private PointeredFile pointerSet;
        private bool isReadOnly;
        public delegate void SetBytesDelegate(byte[] bytes);
        public SetBytesDelegate setBytesDelegate;
        public HexEditForm(byte[] bytes, string name, bool readOnly, PointeredFile pointeredFile)
        {
            internalBytes = bytes;
            pointerSet = pointeredFile;
            InitializeComponent();
            this.Text = ("Hex Editor - " + name);
            this.Load += attachStream;
            isReadOnly = readOnly;
            toggleReadOnlyButton.Visible = false;
            toggleReadOnlyButton.Enabled = false;

            if (readOnly == true)
            {
                saveButton.Visible = false;
                saveButton.Enabled = false;
            }
         }

        private void attachStream(object sender, EventArgs e)
        {
            hexEditor.Stream = new MemoryStream(internalBytes);
            hexEditor.ReadOnlyMode = isReadOnly;
            hexEditor.Loaded += markData;
        }

        private void markData(object sender, EventArgs e)
        {
            if (pointerSet != null && !pointerSet.isBrokenFile)
            {
                Dictionary<long, long[]> pointerData = new Dictionary<long, long[]>();

                foreach(var chunk in pointerSet.splitData)
                {
                    int offset = chunk.chunkLocation;
                    if (offset != 0)
                    {
                        foreach (int pointerLoc in pointerSet.destinationToPointers[offset])
                        {
                            hexEditor.AddHighLight(pointerLoc, 0x4, true);
                            pointerData.Add(pointerLoc, new long[] { offset, chunk.contents.Length });
                        }
                    }
                }
                hexEditor.PointerData = pointerData;
            }
        }

        private void toggleReadOnlyButton_Click(object sender, EventArgs e)
        {
            hexEditor.ReadOnlyMode = !hexEditor.ReadOnlyMode;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            setBytesDelegate(hexEditor.GetAllBytes());
        }
    }
}
