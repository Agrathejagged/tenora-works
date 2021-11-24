using PSULib.FileClasses.Missions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.SetEditorSupportClasses
{
    class SetObjectMetadataEditors
    {
        //For speed purposes, this is going to be cached--it takes _forever_ to create a new one.
        private static HexMetadataEditor cachedEditor = null;

        public static UserControl getMetadataEditor(SetFile.ObjectEntry setObject, bool usePortableMode)
        {
            if(cachedEditor == null)
            {
                cachedEditor = new HexMetadataEditor(setObject);
            }
            else
            {
                cachedEditor.setObjectEntry(setObject);
            }
            return cachedEditor;
        }
    }
}
