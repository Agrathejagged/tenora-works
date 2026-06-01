using PSULib.FileClasses.Archives;
using PSULib.FileClasses.General;
using PSULib.FileClasses.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Set_Object_Manager
{
    /// <summary>
    /// A very minimal implementation for PSU's texture/cutscene files. Basically parses it out far enough to identify textures/models being loaded from the same archive.
    /// </summary>
    internal class PartialEffectFile
    {
        public static Dictionary<string, int> textureToValue = new Dictionary<string, int>();
        public static Dictionary<string, RawFile> textures = new Dictionary<string, RawFile>();

        public static ParticleMetadata /*????*/ readParticleEntries(RawFile file)
        {
            MemoryStream inStream = new MemoryStream(file.fileContents);
            BinaryReader inReader = new BinaryReader(inStream);

            ParticleMetadata particle = new ParticleMetadata();
            particle.fileSize = file.fileContents.Length;
            inStream.Seek(0x4, SeekOrigin.Begin);
            particle.particleType = inReader.ReadInt32();
            inStream.Seek(0xC, SeekOrigin.Begin);
            int sentinel = inReader.ReadInt32();
            if (sentinel == 0x12C)
            {
                particle.externalReferences = readStringTable(inReader);
                particle.filenames = readStringTable(inReader);

                int entryCount = inReader.ReadInt32();
                for (int i = 0; i < entryCount; i++)
                {
                    ParticleEntry entry = new ParticleEntry();
                    int entryType = inReader.ReadInt32();
                    entry.type = entryType;
                    int entryLength = inReader.ReadInt32();
                    for (int j = 0; j < entryLength; j += 4)
                    {
                        int currentInt = inReader.ReadInt32();
                        entry.fields.Add(currentInt);
                    }
                    particle.entries.Add(entry);
                }
                int group2Count = inReader.ReadInt32();
                for (int i = 0; i < group2Count; i++)
                {
                    ParticleGroup2Entry entry = new ParticleGroup2Entry();
                    entry.type = inReader.ReadInt32();
                    int entryLength = inReader.ReadInt32();
                    for (int j = 0; j < entryLength; j += 4)
                    {
                        int currentInt = inReader.ReadInt32();
                        entry.fields.Add(currentInt);
                    }
                    particle.entries2.Add(entry);
                }
                int group3Count = inReader.ReadInt32();
                for (int i = 0; i < group3Count; i++)
                {
                    ParticleGroup3Entry entry = new ParticleGroup3Entry();
                    entry.nextEntryTop = inReader.ReadInt32();
                    entry.nextEntryBottom = inReader.ReadInt32();
                    entry.effectId = inReader.ReadInt32();
                    entry.unknownId = inReader.ReadInt32();
                    entry.startTime = inReader.ReadInt32();
                    entry.endTime = inReader.ReadInt32();

                    entry.translateX = inReader.ReadSingle();
                    entry.translateY = inReader.ReadSingle();
                    entry.translateZ = inReader.ReadSingle();

                    entry.rotateX = inReader.ReadSingle();
                    entry.rotateY = inReader.ReadSingle();
                    entry.rotateZ = inReader.ReadSingle();

                    entry.unknownInt1 = inReader.ReadInt32();
                    entry.unknownInt2 = inReader.ReadInt32();
                    entry.unknownInt3 = inReader.ReadInt32();
                    entry.unknownInt4 = inReader.ReadInt32();

                    particle.entries3.Add(entry);
                }
                if (inStream.Position < (inStream.Length - 8))
                {
                    Console.Out.WriteLine("File length mismatch");
                }
            }

            return particle;
        }

        private static List<string> readStringTable(BinaryReader inReader)
        {
            List<string> strings = new List<string>();
            int boneCount = inReader.ReadInt32();
            int[] boneLengths = new int[boneCount + 1];
            for (int i = 0; i <= boneCount; i++)
            {
                boneLengths[i] = inReader.ReadInt32();
            }
            for (int i = 0; i < boneCount; i++)
            {
                byte[] bytes = inReader.ReadBytes(boneLengths[i + 1] - boneLengths[i]);
                string boneName = ASCIIEncoding.ASCII.GetString(bytes).TrimEnd('\0');

                strings.Add(boneName);
            }
            return strings;
        }

        public class ParticleMetadata
        {
            public int fileSize;
            public int particleType;
            public List<string> externalReferences = new List<string>();
            public List<string> filenames = new List<string>();
            public List<ParticleEntry> entries = new List<ParticleEntry>();
            public List<ParticleGroup2Entry> entries2 = new List<ParticleGroup2Entry>();
            public List<ParticleGroup3Entry> entries3 = new List<ParticleGroup3Entry>();
        }

        public class ParticleGroup2Entry
        {
            public int type;
            public List<int> fields = new List<int>();
            //Arguments may not make sense...
            public string ToString(List<string> bones, List<string> filenames)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Type: ");
                sb.Append(type);
                sb.Append(" (name: ");
                sb.Append(getParticleType(type));
                sb.Append(") (length: ");
                sb.Append(fields.Count);
                sb.Append("); ");
                if (type == 0)
                {
                    //value 39 (0-based) appears to be the count for sub-entries
                    //values 0-45 appear to be header
                    //each entry is 31 bytes long
                    int entryCount = fields[39];
                    if (entryCount * 31 + 46 != fields.Count)
                    {
                        throw new Exception();
                    }
                    List<int> headerData = fields.GetRange(0, 46);
                    sb.Append("Value 0: ");
                    sb.Append(headerData[0]);
                    sb.Append("; Initial floats: {");
                    sb.Append(string.Join(",", headerData.GetRange(1, 5).ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                    sb.Append("}; ints: {");
                    sb.Append(string.Join(",", headerData.GetRange(6, 3)));
                    sb.Append("}; blend mode: {");
                    sb.Append(string.Join(",", headerData.GetRange(9, 1)));

                    sb.Append("}; ints: {");
                    sb.Append(string.Join(",", headerData.GetRange(10, 1)));
                    sb.Append("}; flags?: ");
                    sb.Append(headerData[11].ToString("X2"));
                    sb.Append("; more floats: {");
                    sb.Append(string.Join(",", headerData.GetRange(12, 27).ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                    //sb.AppendLine(string.Join(",", headerData.ConvertAll(x => x.ToString("X2"))));
                    sb.Append("}; more ints: ");
                    sb.Append(string.Join(",", headerData.GetRange(39, 2)));
                    sb.Append("; more floats: {");
                    sb.Append(string.Join(",", headerData.GetRange(41, 3).ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                    sb.Append("}; last ints: ");
                    sb.AppendLine(string.Join(",", headerData.GetRange(44, 2)));

                    //sb.AppendLine(string.Join(",", headerData.ConvertAll(x => x.ToString("X2"))));
                    for (int i = 0; i < entryCount; i++)
                    {
                        int mysteryInt1 = fields[46 + i * 31];
                        int mysteryInt2 = fields[46 + i * 31 + 29];
                        int mysteryInt3 = fields[46 + i * 31 + 30];
                        sb.Append("\tEntry ");
                        sb.Append(i);
                        sb.Append("; Val0 = ");
                        sb.Append(mysteryInt1);
                        sb.Append("; Floats: {");
                        for (int j = 1; j < 29; j++)
                        {
                            sb.Append(BitConverter.ToSingle(BitConverter.GetBytes(fields[46 + i * 31 + j]), 0));
                            if (j < 28)
                            {
                                sb.Append(", ");
                            }
                        }
                        sb.Append("}; val1 = ");
                        sb.Append(mysteryInt2);
                        sb.Append("; val2 = ");
                        sb.Append(mysteryInt3);
                        sb.AppendLine();
                    }
                }
                else if (type == 1) //appears to be three ints, output in decimal
                {
                    sb.Append(string.Join(",", fields));
                }
                else if (type == -2) // one int...
                {
                    sb.Append(string.Join(",", fields));
                    if (filenames.Count > fields[0])
                    {
                        sb.Append(" // ");
                        sb.Append(filenames[fields[0]]);
                    }
                }
                else if (type == -3) // five(?) ints, eight floats
                {
                    sb.Append(string.Join(",", fields.GetRange(0, 5)));
                    sb.Append("; ");
                    sb.Append(string.Join(",", fields.GetRange(5, 8).ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                }
                else if (type == -4) // five ints
                {
                    sb.Append(string.Join(",", fields));
                    if (bones.Count > fields[1] && fields[1] > -1)
                    {
                        sb.Append(" // ");
                        sb.Append(bones[fields[1]]);
                        sb.Append(" string ");
                        sb.Append(fields[2]);
                        sb.Append(":");
                        sb.Append(fields[3]);
                    }
                }
                else if (type == -8) // two ints, rest floats
                {
                    if (fields.Count != 31)
                    {
                        throw new Exception("Unexpected length; type -8, expected 31 args, received " + fields.Count);
                    }
                    sb.Append(string.Join(",", fields.GetRange(0, 31)));
                }
                else if (type == -9) // two ints, rest floats
                {
                    if (fields.Count != 34)
                    {
                        throw new Exception("Unexpected length; type -9, expected 34 args, received " + fields.Count);
                    }
                    sb.Append(string.Join(",", fields.GetRange(0, 2)));
                    sb.Append("; ");
                    sb.Append(string.Join(",", fields.GetRange(2, fields.Count - 3).ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                }
                else if (type == -11) //6 ints, 1 float, 1 int, 20 floats, 10 ints
                {
                    if (fields.Count != 38)
                    {
                        throw new Exception("Unexpected length; type -9, expected 38 args, received " + fields.Count);
                    }
                    sb.Append(string.Join(",", fields.GetRange(0, 6)));
                    sb.Append("; float = ");
                    sb.Append(BitConverter.ToSingle(BitConverter.GetBytes(fields[6]), 0));
                    sb.Append("; int = ");
                    sb.Append(fields[7]);
                    sb.Append("; floats = ");
                    sb.Append(string.Join(",", fields.GetRange(8, 20).ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                    sb.Append("; ints = ");
                    sb.Append(string.Join(",", fields.GetRange(28, 10)));
                }
                else if (type == -13) // one float
                {
                    sb.Append(string.Join(",", fields.ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                }
                else if (type == -20) // 3 floats --lighting related, floats are R/G/B
                {
                    sb.Append(string.Join(",", fields.ConvertAll(x => BitConverter.ToSingle(BitConverter.GetBytes(x), 0))));
                }
                else
                {
                    sb.Append(string.Join(",", fields.ConvertAll(x => x.ToString("X2"))));
                }
                return sb.ToString();
            }

            string getParticleType(int particleType)
            {
                switch (particleType)
                {
                    case -20: return "TYPDParamAmbientLight";
                    case -19: return "TYPDParamLensflare";
                    case -18: return "TYPDParamAlphaTest";
                    case -17: return "TYPDParamBoid";
                    case -16: return "TYPDParamShadowVolume";
                    case -15: return "TYPDParamDepthOfField";
                    case -14: return "TYPDParamScreenFog";
                    case -13: return "TYPDParamBlur";
                    case -12: return "TYPDParamPlayInfo";
                    case -11: return "TYPDParamThunder";
                    case -10: return "TYPDParamScript";
                    case -9: return "TYPDParamSpotLight";
                    case -8: return "TYPDParamPlayer";
                    case -7: return "TYPDParamSE";
                    case -6: return "TYPDParamADXStreamSound";
                    case -5: return "TYPDParamStream";
                    case -4: return "TYPDParamText";
                    case -3: return "TYPDParamLight";
                    case -2: return "?play_animation?";
                    case 0: return "?generate_particle?";
                    case 1: return "?apply_model?";
                    default: return "unknown_classname";
                }
            }
        }

        public class ParticleGroup3Entry
        {
            //These have to be more complicated than "above you" and "below you"
            public int nextEntryTop, nextEntryBottom;

            public int effectId;
            public int unknownId;

            public int startTime, endTime;

            //According to internal materials, sets are XZY, but these are XYZ. Accordingly, since I call sets XYZ, these are XZY...
            public float translateX, translateZ, translateY;
            public float rotateX, rotateZ, rotateY;
            public int unknownInt1, unknownInt2, unknownInt3, unknownInt4;

            public override bool Equals(object? obj)
            {
                return obj is ParticleGroup3Entry entry &&
                       nextEntryTop == entry.nextEntryTop &&
                       nextEntryBottom == entry.nextEntryBottom &&
                       effectId == entry.effectId &&
                       unknownId == entry.unknownId &&
                       startTime == entry.startTime &&
                       endTime == entry.endTime &&
                       translateX == entry.translateX &&
                       translateZ == entry.translateZ &&
                       translateY == entry.translateY &&
                       rotateX == entry.rotateX &&
                       rotateZ == entry.rotateZ &&
                       rotateY == entry.rotateY &&
                       unknownInt1 == entry.unknownInt1 &&
                       unknownInt2 == entry.unknownInt2 &&
                       unknownInt3 == entry.unknownInt3 &&
                       unknownInt4 == entry.unknownInt4;
            }

            public override int GetHashCode()
            {
                int hashCode = -721353231;
                hashCode = hashCode * -1521134295 + nextEntryTop.GetHashCode();
                hashCode = hashCode * -1521134295 + nextEntryBottom.GetHashCode();
                hashCode = hashCode * -1521134295 + effectId.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownId.GetHashCode();
                hashCode = hashCode * -1521134295 + startTime.GetHashCode();
                hashCode = hashCode * -1521134295 + endTime.GetHashCode();
                hashCode = hashCode * -1521134295 + translateX.GetHashCode();
                hashCode = hashCode * -1521134295 + translateZ.GetHashCode();
                hashCode = hashCode * -1521134295 + translateY.GetHashCode();
                hashCode = hashCode * -1521134295 + rotateX.GetHashCode();
                hashCode = hashCode * -1521134295 + rotateZ.GetHashCode();
                hashCode = hashCode * -1521134295 + rotateY.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownInt1.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownInt2.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownInt3.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownInt4.GetHashCode();
                return hashCode;
            }

            //Arguments may not make sense...
            public string ToString(List<string> bones, List<string> filenames)
            {
                StringBuilder sb = new StringBuilder();
                //6 ints, some floats, 4 ints?
                sb.AppendFormat("Next entries={{ {0:X4}, {1:X4} }}\r\n", nextEntryTop, nextEntryBottom);
                sb.Append("\tEffect ID = ");
                sb.AppendLine(effectId.ToString());
                sb.Append("\tUnknown ID = ");
                sb.AppendLine(unknownId.ToString());
                sb.Append("\tTimespan = ");
                sb.Append(startTime);
                sb.Append("-");
                sb.AppendLine(endTime.ToString());
                sb.AppendFormat("\ttranslation = [ {0}, {1}, {2} ]\r\n", translateX, translateZ, translateY);
                sb.AppendFormat("\trotation = [ {0}, {1}, {2} ]\r\n", rotateX, rotateZ, rotateY);
                sb.AppendFormat("\tUnknown ints = {{ {0}, {1}, {2}, {3} }}\r\n", unknownInt1, unknownInt2, unknownInt3, unknownInt4);
                return sb.ToString();
            }


        }

        public class ParticleEntry
        {
            public int type;
            public List<int> fields = new List<int>();

            public string ToString(List<string> bones, List<string> filenames)
            {
                if (type == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Texture: ");
                    sb.Append("(" + fields[0] + "/" + filenames[fields[0]] + ")");
                    sb.Append("; ");
                    sb.Append(fields[1].ToString("X2"));
                    sb.Append("; extra count=");
                    sb.Append(fields[2]);
                    if (fields[2] > 0)
                    {
                        sb.Append(": {");
                        for (int i = 0; i < fields[2]; i++)
                        {
                            sb.Append("[");
                            sb.Append(fields[3 + 4 * i]);
                            sb.Append(", ");
                            sb.Append(fields[4 + 4 * i]);
                            sb.Append(", ");
                            sb.Append(BitConverter.ToSingle(BitConverter.GetBytes(fields[5 + 4 * i]), 0).ToString("0.0"));
                            sb.Append(", ");
                            sb.Append(fields[6 + 4 * i]);
                            sb.Append("]");
                        }
                        sb.Append("}");
                    }
                    return sb.ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Model: ");
                    sb.Append("XNJ=");
                    sb.Append(filenames[fields[0]]);
                    sb.Append("; field count=");
                    sb.Append(fields[1]);
                    sb.Append("; animation count=");
                    sb.Append(fields[2]);
                    if (fields[1] > 0)
                    {
                        sb.Append("; core files: {");
                        sb.Append(string.Join(",", fields.GetRange(3, fields[1]).Select(field => field != -1 ? filenames[field] : "none")));
                        sb.Append("}");
                    }
                    if (fields[2] > 0)
                    {
                        sb.Append("; animations: {");
                        sb.Append(string.Join(",", fields.GetRange(3 + fields[1], fields[2]).Select(field => field != -1 ? filenames[field] : "none")));
                        sb.Append("}");
                    }
                    if (fields.Count != 3 + fields[1] + fields[2])
                    {
                        sb.Append("MALFORMED");
                    }
                    return sb.ToString();
                }
                return type + ": " + string.Join(", ", fields.Select(field => field.ToString("X2")));
            }

            public override string ToString()
            {
                return type + ": " + string.Join(", ", fields.Select(field => field.ToString("X2")));
            }
        }
    }
}
