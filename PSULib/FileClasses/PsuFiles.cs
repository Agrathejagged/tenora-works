using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using psu_generic_parser.FileClasses;

namespace psu_generic_parser
{
    public class PsuFiles
    {
        public static bool parseScripts = true; //Because you don't always want to parse script files.

        public static PsuFile FromRaw(string filename, byte[] rawData, byte[] inHeader, int[] ptrs, int baseAddr, bool bigEndian = false)
        {
            if (filename.EndsWith("xvr"))
                return new XvrTextureFile(inHeader, rawData, filename);
            else if (filename.EndsWith(".uvr"))
                return new UvrTextureFile(inHeader, rawData, filename);
            else if (filename.EndsWith(".gim") && GimSharp.GimTexture.Is(rawData))
                return new GimTextureFile(inHeader, rawData, filename);
            else if (filename.Contains("Weapon.bin") || filename.Contains("General.bin") || filename.Contains("Other.bin"))
                return new Psp2TextFile(filename, rawData);
            else if (filename.Equals("itemObjectDrop.xnr"))
                return new ObjectDropFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Equals("filelist.rel"))
                return new QuestListFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.EndsWith("filelist.rel"))
                return new ListFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Equals("itemEnemyDrop.xnr"))
                return new EnemyDropFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.StartsWith("set_r") && filename.EndsWith(".rel"))
                return new SetFile(filename, rawData, inHeader, ptrs, baseAddr, bigEndian);
            else if (filename.EndsWith(".xnt"))
                return new XntFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (parseScripts && (filename.Contains("script.bin") || filename.Contains("Tutor.bin")))
                return new ScriptFile(filename, rawData, bigEndian);
            else if ((filename.EndsWith(".k") || filename.Contains(".bin")) && BitConverter.ToInt32(rawData, 0) == rawData.Length && new string(ASCIIEncoding.ASCII.GetChars(rawData, 0, 4)) != "RIPC")
                return new TextFile(filename, rawData);
            else if (filename.Contains("item00ValueData"))
                return new ItemPriceFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Contains("itemTechParam") && !filename.Contains("EnemyB"))
                return new ItemTechParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Contains("itemBulletParam") && !filename.Contains("MagAt"))
                return new ItemBulletParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.StartsWith("Think") && filename.EndsWith("Dragon.xnr"))
                return new ThinkDragonFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Contains("itemSkillParam"))
                return new ItemSkillParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Contains("itemLineUnitParam"))
                return new ItemUnitParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            else if (filename.Contains(".nbl") || ASCIIEncoding.ASCII.GetString(rawData, 0, 4) == "NMLL")
            {
                NblLoader toReturn = new NblLoader(new MemoryStream(rawData));
                toReturn.filename = filename;
                return toReturn;
            }
            else if (ASCIIEncoding.ASCII.GetString(rawData, 0, 3) == "AFS")
            {
                AfsLoader toReturn = new AfsLoader(filename, new MemoryStream(rawData));
            }
            else if (new Regex("enemy.._...._r.\\.(xnr|rel|unr)").IsMatch(filename))
            {
                return new EnemyLayoutFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.Contains("itemWeaponParam"))
            {
                return new WeaponParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.Contains("itemCommonInfo.xnr"))
            {
                return new CommonInfoFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.StartsWith("EnemyLevelBaseParam."))
            {
                return new EnemyLevelParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.Equals("itemSuitParam.xnr") || filename.Equals("itemPartsParam.xnr"))
            {
                return new ItemSuitParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.EndsWith(".nom"))
            {
                return new NomFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.Contains("itemBulletParam_27MagAt"))
            {
                return new RmagBulletParamFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (filename.Equals("partsinfo.xnr"))
            {
                return new PartsInfoFile(filename, rawData, inHeader, ptrs, baseAddr);
            }
            else if (inHeader != null && ASCIIEncoding.ASCII.GetString(inHeader, 0, 4) == "NXIF" || ASCIIEncoding.ASCII.GetString(inHeader, 0, 4) == "NUIF" || ASCIIEncoding.ASCII.GetString(inHeader, 0, 4) == "NSIF")
            {
                return new PointeredFile(filename, rawData, inHeader, ptrs, baseAddr, false);
            }
            else if (inHeader != null && ASCIIEncoding.ASCII.GetString(inHeader, 0, 4) == "NYIF")
            {
                return new PointeredFile(filename, rawData, inHeader, ptrs, baseAddr, true);
            }
            return new UnpointeredFile(filename, rawData, inHeader);
        }
    }
}
