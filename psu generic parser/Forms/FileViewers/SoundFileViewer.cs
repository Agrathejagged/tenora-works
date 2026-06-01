using PSULib.FileClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers
{
    public partial class SoundFileViewer : UserControl
    {
        PsuSoundFile soundFile;
        List<MemoryStream> generatedWaves = new List<MemoryStream>();
        SoundPlayer player = new SoundPlayer();

        public SoundFileViewer(PsuSoundFile file)
        {
            soundFile = file;
            InitializeComponent();
            //We're gonna pre-gen the wav files here.
            for (int i = 0; i < soundFile.Sounds.Count; i++)
            {
                //Generate the header
                var sound = soundFile.Sounds[i];
                generatedWaves.Add(generateSound(soundFile.Sounds[i]));
                listBox1.Items.Add(i + " Unk: " + sound.unknownInt + " Sample Rate: " + sound.sampleRate + " Unk: " + sound.unknownShort1);
            }
        }

        private MemoryStream generateSound(PsuSoundFile.SoundEntry sound)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter wr = new BinaryWriter(ms);
            short numchannels = 1;
            short samplelength = 2;
            wr.Write(Encoding.ASCII.GetBytes("RIFF"));
            //these feel like they should be dynamic, but  not found them yet.
            wr.Write(36 + sound.sampleCount * numchannels * samplelength);
            wr.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            wr.Write(16);
            wr.Write((ushort)1);
            wr.Write(numchannels);
            wr.Write((int)sound.sampleRate);
            wr.Write(sound.sampleRate * samplelength * numchannels);
            wr.Write((ushort)(samplelength * numchannels));
            wr.Write((ushort)(8 * samplelength));
            wr.Write(Encoding.ASCII.GetBytes("data"));
            wr.Write(sound.sampleCount * samplelength);
            wr.Write(sound.rawSound);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public SoundFileViewer()
        {
            InitializeComponent();
        }

        private void playSoundButton_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                if(player != null)
                {
                    player.Stop();
                }
                MemoryStream ms = generatedWaves[listBox1.SelectedIndex];
                ms.Seek(0, SeekOrigin.Begin);
                player = new SoundPlayer(ms);
                player.Load();
                player.Play();
                //Generate a wav file 
            }
        }

        private void exportSoundButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MemoryStream ms = generatedWaves[listBox1.SelectedIndex];
                ms.Seek(0, SeekOrigin.Begin);
                byte[] bytes = ms.ToArray();
                File.WriteAllBytes(saveFileDialog1.FileName, bytes);
            }
        }

        private void exportAllButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                var baseFilename = Path.GetFileNameWithoutExtension(soundFile.filename);
                for (int i = 0; i < generatedWaves.Count; i++)
                {
                    MemoryStream ms = generatedWaves[i];
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = ms.ToArray();
                    File.WriteAllBytes(Path.Combine(path, String.Format("{0}_{1:X2}.wav", baseFilename, i)), bytes);
                }
            }
        }
    }
}
