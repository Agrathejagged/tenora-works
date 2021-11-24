using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using PSULib.FileClasses.Missions;
using PSULib.FileClasses.Missions.Sets;

namespace psu_generic_parser.Forms.FileViewers.SetEditorSupportClasses
{
    public partial class HexMetadataEditor : UserControl
    {
        private SetFile.ObjectEntry objectEntry;
        MemoryStream metadataStream;

        public HexMetadataEditor(SetFile.ObjectEntry obj)
        {
            InitializeComponent();
            metadataHexEditor.StringDataVisibility = Visibility.Hidden;
            metadataHexEditor.BytesModified += metadataHexEditor_BytesModified;

            objectEntry = obj;
            metadataStream = new MemoryStream(100);
            reloadData();
        }

        public void setObjectEntry(SetFile.ObjectEntry obj)
        {
            objectEntry = obj;
            reloadData();
        }

        private void reloadData()
        {
            if (SetObjectDefinitions.definitions.ContainsKey(objectEntry.objID))
            {
                SetObjectDefinition def = SetObjectDefinitions.definitions[objectEntry.objID];
                metadataLengthLabel.Text = "AotI: " + def.metadataLengthAotI + " / " + "PSP2: " + def.metadataLengthPsp2;
            }
            else
            {
                metadataLengthLabel.Text = "INVALID OBJECT";
            }
            metadataLengthUD.Value = objectEntry.metadata.Length;
            metadataStream.SetLength(objectEntry.metadata.Length);
            metadataStream.Seek(0, SeekOrigin.Begin);
            metadataStream.Write(objectEntry.metadata, 0, objectEntry.metadata.Length);
            //For some reason, setting the stream from the constructor marks the stream unwritable, so it has to be set here.
            //If it's already been set, this won't do anything.
            metadataHexEditor.Stream = metadataStream;
            metadataHexEditor.ClearAllChange();
            metadataHexEditor.RefreshView();
        }

        private void metadataHexEditor_BytesModified(object sender, System.EventArgs e)
        {
            objectEntry.metadata = metadataHexEditor.GetAllBytes();
        }

        private void metadataLengthUD_ValueChanged(object sender, System.EventArgs e)
        {
            updateLength();
        }

        private void updateLength()
        {
            lock (metadataHexEditor)
            {
                if (metadataLengthUD.Value != objectEntry.metadata.Length)
                {
                    Array.Resize(ref objectEntry.metadata, Convert.ToInt32(metadataLengthUD.Value));
                    metadataStream.SetLength(objectEntry.metadata.Length);
                    metadataHexEditor.RefreshView();
                }
            }
        }
    }
}
