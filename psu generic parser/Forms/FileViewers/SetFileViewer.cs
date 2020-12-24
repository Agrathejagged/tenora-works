using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using static psu_generic_parser.SetFile;
using static psu_generic_parser.HexEditForm;
using psu_generic_parser.Forms.FileViewers.SetEditorSupportClasses;
using PSULib.FileClasses.Sets;

namespace psu_generic_parser
{
    public partial class SetFileViewer : UserControl
    {
        public SetFile internalFile;
        public ObjectEntry objectEntry;
        public HexEditForm objectListHeaderForm;
        public HexEditForm objectMetaData;
        private int currentMapIndex = -1; //to make sure it doesn't reload when we rename the current map

        DataContractJsonSerializer tempJson = new DataContractJsonSerializer(typeof(SetFile));
        public SetFileViewer(SetFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;

            InitializeDisplay();
        }

        private void InitializeDisplay()
        {
            areaIdComboBox.SelectedIndex = internalFile.areaID;
            //Initially load first set of first map list
            //Load Map List. This won't change again for this set file
            mapListCB.BeginUpdate();
            for (int i = 0; i < internalFile.mapData.Length; i++)
            {
                mapListCB.Items.Add(internalFile.mapData[i].mapNumber);
            }
            mapListCB.EndUpdate();
            mapListCB.SelectedIndex = 0;

            //Load Object Set from mapList 0; it should default to this
            updateObjectList();
        }

        //Updates object display visuals to current object's
        private void updateObjectDisplay()
        {
            if (objectMetaData != null)
            {
                objectMetaData.Close();
            }
            objectEntry = internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects[setObjectListBox.SelectedIndex];
            objIDUD.Value = objectEntry.objID;
            unkIntUD.Value = objectEntry.unkInt1;
            posXUD.Value = Convert.ToDecimal(objectEntry.objX);
            posYUD.Value = Convert.ToDecimal(objectEntry.objY);
            posZUD.Value = Convert.ToDecimal(objectEntry.objZ);
            rotXUD.Value = Convert.ToDecimal(objectEntry.objRotX);
            rotYUD.Value = Convert.ToDecimal(objectEntry.objRotY);
            rotZUD.Value = Convert.ToDecimal(objectEntry.objRotZ);
            headerInt1UD.Value = Convert.ToDecimal(objectEntry.headerInt1);
            headerInt2UD.Value = Convert.ToDecimal(objectEntry.headerInt2);
            headerInt3UD.Value = Convert.ToDecimal(objectEntry.headerInt3);
            headerShort1UD.Value = Convert.ToDecimal(objectEntry.headerShort1);

            if(SetObjectDefinitions.definitions.ContainsKey(objectEntry.objID))
            {
                objectNameLabel.Text = SetObjectDefinitions.definitions[objectEntry.objID].name;
            }
            else
            {
                objectNameLabel.Text = "INVALID OBJECT";
            }
            reloadMetadataEditor();
        }

        private void reloadMetadataEditor()
        {
            UserControl newControl = SetObjectMetadataEditors.getMetadataEditor(objectEntry, false);
            if (metadataGroupBox.Controls.Count == 0 || metadataGroupBox.Controls[0] != newControl)
            {
                metadataGroupBox.Controls.Clear();
                newControl.Dock = DockStyle.Fill;
                metadataGroupBox.Controls.Add(newControl);
            }
        }

        private void updateObjectList()
        {
            if (objectListHeaderForm != null)
            {
                objectListHeaderForm.Close();
            }
            objectListCB.BeginUpdate();
            objectListCB.Items.Clear();
            for(int i = 0; i < internalFile.mapData[mapListCB.SelectedIndex].headers.Length; i++)
            {
                objectListCB.Items.Add(i);
            }
            objectListCB.EndUpdate();
            objectListCB.SelectedIndex = 0;

            //Update Object List
            updateObjectListBox();
        }

        private void updateObjectListBox()
        {
            setObjectListBox.BeginUpdate();
            setObjectListBox.Items.Clear();
            for(int i = 0; i < internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects.Length; i++)
            {
                setObjectListBox.Items.Add("Object " + i);
            }
            setObjectListBox.EndUpdate();
            setObjectListBox.SelectedIndex = 0;

            //Load parameters from the first object of the object list
            updateObjectDisplay();
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
                SetFile tempSet = (SetFile)tempJson.ReadObject(tempStream);
                tempStream.Close();
                internalFile.mapData = tempSet.mapData;
                InitializeDisplay();
            }
        }

        private void setObjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateObjectDisplay();
        }

        private void editObjectSetHeaderBytesButton_Click(object sender, EventArgs e)
        {
            objectListHeaderForm = new HexEditForm(internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].headerBytes,
                $"Map List {mapListCB.SelectedIndex}, Set {objectListCB.SelectedIndex}, Object {setObjectListBox.SelectedIndex}", false, null);
            objectListHeaderForm.Show();
            objectListHeaderForm.setBytesDelegate = new SetBytesDelegate(this.setObjectSetHeaderBytes);
        }

        private void setObjectSetHeaderBytes(byte[] bytes)
        {
            internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].headerBytes = bytes;
        }

        private void mapListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentMapIndex != mapListCB.SelectedIndex)
            {
                currentMapIndex = mapListCB.SelectedIndex;
                mapListNumberUD.Value = internalFile.mapData[mapListCB.SelectedIndex].mapNumber;
                updateObjectList();
            }
        }

        private void objectListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateObjectListBox();
        }

        private void addMapListButton_Click(object sender, EventArgs e)
        {
            List<MapListing> newMapData = internalFile.mapData.ToList();
            
            ObjectEntry[] newObjectList = new ObjectEntry[1];
            newObjectList[0] = new ObjectEntry();
            newObjectList[0].metadata = new byte[0];

            ListHeader newListHeader = new ListHeader();
            newListHeader.headerBytes = new byte[36];
            newListHeader.objects = newObjectList;

            MapListing newMapListing = new MapListing();
            newMapListing.headers = new ListHeader[] { newListHeader };
            newMapData.Add(newMapListing);

            internalFile.mapData = newMapData.ToArray();

            mapListCB.BeginUpdate();
            mapListCB.Items.Add(internalFile.mapData[mapListCB.Items.Count - 1].mapNumber);
            mapListCB.EndUpdate();
            mapListCB.SelectedIndex = mapListCB.Items.Count - 1;
        }

        private void removeMapListButton_Click(object sender, EventArgs e)
        {
            if(internalFile.mapData.Length > 1)
            {
                int temp = 0;
                List<MapListing> newMapListing = internalFile.mapData.ToList();
                newMapListing.RemoveAt(mapListCB.SelectedIndex);
                internalFile.mapData = newMapListing.ToArray();

                mapListCB.BeginUpdate();
                if (mapListCB.SelectedIndex > 0)
                {
                    temp = mapListCB.SelectedIndex - 1;
                }
                mapListCB.Items.RemoveAt(mapListCB.SelectedIndex);
                mapListCB.EndUpdate();
                mapListCB.SelectedIndex = temp;
                mapListNumberUD.Value = internalFile.mapData[mapListCB.SelectedIndex].mapNumber;
                updateObjectList();
            } else
            {
                MessageBox.Show("You cannot remove the last Map List!");
            }
        }

        private void addObjectList_Click(object sender, EventArgs e)
        {
            List<ListHeader> newHeaderList = internalFile.mapData[mapListCB.SelectedIndex].headers.ToList();

            ObjectEntry[] newObjectList = new ObjectEntry[1];
            newObjectList[0] = new ObjectEntry();
            newObjectList[0].metadata = new byte[0];

            ListHeader newListHeader = new ListHeader();
            newListHeader.headerBytes = new byte[36];
            newListHeader.objects = newObjectList;
            newHeaderList.Add(newListHeader);

            internalFile.mapData[mapListCB.SelectedIndex].headers = newHeaderList.ToArray();

            objectListCB.BeginUpdate();
            objectListCB.Items.Add("New " + objectListCB.Items.Count);
            objectListCB.EndUpdate();
            objectListCB.SelectedIndex = objectListCB.Items.Count - 1;

        }

        private void removeObjectList_Click(object sender, EventArgs e)
        {
            if (internalFile.mapData[mapListCB.SelectedIndex].headers.Length > 1)
            {
                int temp = 0;
                List<ListHeader> newListHeaderList = internalFile.mapData[mapListCB.SelectedIndex].headers.ToList();
                newListHeaderList.RemoveAt(objectListCB.SelectedIndex);
                internalFile.mapData[mapListCB.SelectedIndex].headers = newListHeaderList.ToArray();

                objectListCB.BeginUpdate();
                if (objectListCB.SelectedIndex > 0)
                {
                    temp = objectListCB.SelectedIndex - 1;
                }
                objectListCB.Items.RemoveAt(objectListCB.SelectedIndex);
                objectListCB.EndUpdate();
                objectListCB.SelectedIndex = temp;
                
            }
            else
            {
                MessageBox.Show("You cannot remove the last Object List!");
            }
        }

        private void addObjectButton_Click(object sender, EventArgs e)
        {
            List<ObjectEntry> newObjList = internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects.ToList();
            ObjectEntry newObjectEntry = new ObjectEntry();
            newObjectEntry.metadata = new byte[4];
            newObjList.Add(newObjectEntry);
            internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects = newObjList.ToArray();

            setObjectListBox.BeginUpdate();
            setObjectListBox.Items.Add("New Object " + (setObjectListBox.Items.Count));
            setObjectListBox.EndUpdate();
            setObjectListBox.SelectedIndex = setObjectListBox.Items.Count - 1;

        }

        private void removeObjectButton_Click(object sender, EventArgs e)
        {
            if (internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects.Length > 1)
            {
                List<ObjectEntry> newObjList = internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects.ToList();
                newObjList.RemoveAt(setObjectListBox.SelectedIndex);
                internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects = newObjList.ToArray();

                setObjectListBox.BeginUpdate();
                if (setObjectListBox.SelectedIndex > 0)
                {
                    setObjectListBox.SelectedIndex = setObjectListBox.SelectedIndex - 1;
                }
                else
                {
                    setObjectListBox.SelectedIndex = 0;
                }
                setObjectListBox.Items.RemoveAt(setObjectListBox.Items.Count - 1);
                setObjectListBox.EndUpdate();
                
            }
            else
            {
                MessageBox.Show("You cannot remove the last Object!");
            }
        }

        private void clearObjectsButton_Click(object sender, EventArgs e)
        {
            List<ObjectEntry> newObjList = internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects.ToList();
            newObjList.Clear();
            ObjectEntry newObjectEntry = new ObjectEntry();
            newObjectEntry.metadata = new byte[0];
            newObjList.Add(newObjectEntry);
            internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects = newObjList.ToArray();

            setObjectListBox.BeginUpdate();
            setObjectListBox.Items.Clear();
            setObjectListBox.Items.Add("New Object " + (setObjectListBox.Items.Count));
            setObjectListBox.EndUpdate();
            setObjectListBox.SelectedIndex = 0;
        }

        private void duplicateObjectButton_Click(object sender, EventArgs e)
        {
            List<ObjectEntry> newObjList = internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects.ToList();
            List<byte> newMetaData = newObjList[setObjectListBox.SelectedIndex].metadata.ToList();

            ObjectEntry newObjectEntry = new ObjectEntry();
            newObjectEntry.headerInt1 = newObjList[setObjectListBox.SelectedIndex].headerInt1;
            newObjectEntry.headerInt2 = newObjList[setObjectListBox.SelectedIndex].headerInt2;
            newObjectEntry.headerInt3 = newObjList[setObjectListBox.SelectedIndex].headerInt3;
            newObjectEntry.headerShort1 = newObjList[setObjectListBox.SelectedIndex].headerShort1;
            newObjectEntry.objID = newObjList[setObjectListBox.SelectedIndex].objID;
            newObjectEntry.objRotX = newObjList[setObjectListBox.SelectedIndex].objRotX;
            newObjectEntry.objRotY = newObjList[setObjectListBox.SelectedIndex].objRotY;
            newObjectEntry.objRotZ = newObjList[setObjectListBox.SelectedIndex].objRotZ;
            newObjectEntry.objX = newObjList[setObjectListBox.SelectedIndex].objX;
            newObjectEntry.objY = newObjList[setObjectListBox.SelectedIndex].objY;
            newObjectEntry.objZ = newObjList[setObjectListBox.SelectedIndex].objZ;
            newObjectEntry.unkInt1 = newObjList[setObjectListBox.SelectedIndex].unkInt1;
            newObjectEntry.metadata = newMetaData.ToArray();
            newObjList.Add(newObjectEntry);
            internalFile.mapData[mapListCB.SelectedIndex].headers[objectListCB.SelectedIndex].objects = newObjList.ToArray();

            setObjectListBox.BeginUpdate();
            setObjectListBox.Items.Add("Dupe Object " + (setObjectListBox.Items.Count));
            setObjectListBox.EndUpdate();
            setObjectListBox.SelectedIndex = setObjectListBox.Items.Count - 1;
        }

        private void objIDUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objID = (short)objIDUD.Value;
            if(SetObjectDefinitions.definitions.ContainsKey(objectEntry.objID))
            {
                //TODO: properly detect Infinity files
                var def = SetObjectDefinitions.definitions[objectEntry.objID];
                if(def.metadataLengthAotI > objectEntry.metadata.Length)
                {
                    Array.Resize(ref objectEntry.metadata, def.metadataLengthAotI);
                }
            }
            reloadMetadataEditor();
        }

        private void unkIntUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.unkInt1 = (int)unkIntUD.Value;
        }

        private void posXUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objX = (float)posXUD.Value;
        }

        private void posYUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objY = (float)posYUD.Value;
        }

        private void posZUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objZ = (float)posZUD.Value;
        }

        private void rotXUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objRotX = (float)rotXUD.Value;
        }

        private void rotYUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objRotY = (float)rotYUD.Value;
        }

        private void rotZUD_ValueChanged(object sender, EventArgs e)
        {
            objectEntry.objRotZ = (float)rotZUD.Value;
        }

        private void mapListNumberUD_ValueChanged(object sender, EventArgs e)
        {
            internalFile.mapData[mapListCB.SelectedIndex].mapNumber = (short)mapListNumberUD.Value;
            mapListCB.Items[mapListCB.SelectedIndex] = mapListNumberUD.Value;
        }

        private void areaIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalFile.areaID = (short)areaIdComboBox.SelectedIndex;
        }
    }
}
