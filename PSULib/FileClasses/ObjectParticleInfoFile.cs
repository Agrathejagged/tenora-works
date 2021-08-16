using psu_generic_parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses
{
    public class ObjectParticleInfoFile : PsuFile
    {
        public class ParticleFileEntry
        {
            public int ParticleIndex { get; set; }
            public string ParticleName { get; set; }
            public string ParticleFileName { get; set; }
            public float mysteryFloat { get; set; }
            public int mysteryInt { get; set; }
        }

        public List<ParticleFileEntry> particleFileEntries = new List<ParticleFileEntry>();

        public ObjectParticleInfoFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            this.filename = inFilename;
            using (MemoryStream memoryStream = new MemoryStream(rawData))
            {
                BinaryReader reader = new BinaryReader(memoryStream);
                int identifier = reader.ReadInt32();
                int fileLength = reader.ReadInt32();
                int headerLoc = reader.ReadInt32();

                memoryStream.Seek(headerLoc, SeekOrigin.Begin);
                int listLoc = reader.ReadInt32() - baseAddr;
                int entryCount = reader.ReadInt32();

                for (int i = 0; i < entryCount; i++)
                {
                    memoryStream.Seek(listLoc + i * 20, SeekOrigin.Begin);
                    ParticleFileEntry entry = new ParticleFileEntry();
                    entry.ParticleIndex = reader.ReadInt32();
                    int nameLoc = reader.ReadInt32() - baseAddr;
                    int filenameLoc = reader.ReadInt32() - baseAddr;
                    entry.mysteryFloat = reader.ReadSingle();
                    entry.mysteryInt = reader.ReadInt32();

                    entry.ParticleName = reader.ReadAsciiString(nameLoc);
                    entry.ParticleFileName = reader.ReadAsciiString(filenameLoc);
                    particleFileEntries.Add(entry);
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(outStream);
            List<int> pointers = new List<int>();
            int[] nameLocs = new int[particleFileEntries.Count];
            int[] filenameLocs = new int[particleFileEntries.Count];

            outStream.Seek(0x10, SeekOrigin.Begin);
            for(int i = 0; i < particleFileEntries.Count; i++)
            {
                nameLocs[i] = (int)outStream.Position;
                writer.Write(Encoding.ASCII.GetBytes(particleFileEntries[i].ParticleName + "\0"));
                outStream.Seek((outStream.Position + 3) % 4, SeekOrigin.Current);
                filenameLocs[i] = (int)outStream.Position;
                writer.Write(Encoding.ASCII.GetBytes(particleFileEntries[i].ParticleFileName + "\0"));
                outStream.Seek((outStream.Position + 3) % 4, SeekOrigin.Current);
            }
            int listLoc = (int)outStream.Position;
            for(int i = 0; i < particleFileEntries.Count; i++)
            {
                writer.Write(particleFileEntries[i].ParticleIndex);
                pointers.Add((int)outStream.Position);
                writer.Write(nameLocs[i]);
                pointers.Add((int)outStream.Position);
                writer.Write(filenameLocs[i]);
                writer.Write(particleFileEntries[i].mysteryFloat);
                writer.Write(particleFileEntries[i].mysteryInt);
            }

            int headerLoc = (int)outStream.Position;
            pointers.Add((int)outStream.Position);
            writer.Write(listLoc);
            writer.Write(particleFileEntries.Count);

            int fileLength = (int)outStream.Position;
            outStream.Seek(0x0, SeekOrigin.Begin);
            writer.Write(0x52584E); //"NXR"
            writer.Write(fileLength);
            writer.Write(headerLoc);
            calculatedPointers = pointers.ToArray();
            return outStream.ToArray();
        }
    }
}
