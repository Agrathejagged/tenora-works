using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using psu_generic_parser.FileClasses;
using psu_generic_parser.FileClasses.XVR;

namespace psu_generic_parser
{
    public partial class XvrViewer : UserControl
    {
        TextureFile internalTexture;
        int currentMip = 0;
        int mipCount = 0;
        string filename = "";
        private PsuTexturePixelFormat[] permittedFormats = { PsuTexturePixelFormat.Argb1555, PsuTexturePixelFormat.Rgb555, PsuTexturePixelFormat.Argb4444, PsuTexturePixelFormat.Rgb565, PsuTexturePixelFormat.Argb8888, PsuTexturePixelFormat.Xrgb8888, PsuTexturePixelFormat.Rgb655, PsuTexturePixelFormat.Rgba8888, PsuTexturePixelFormat.Abgr8888 };

        XvrViewer()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        public XvrViewer(TextureFile toLoad) : this()
        {
            if(toLoad is PsuFile)
            {
                filename = (toLoad as PsuFile).filename;
            }
            if (toLoad is XvrTextureFile)
            {
                label3.Text = (toLoad as XvrTextureFile).OriginalPixelFormat.ToString();
                label5.Text = (toLoad as XvrTextureFile).OriginalTextureType.ToString();
                pixelFormatDropDown.DataSource = permittedFormats;
                pixelFormatDropDown.SelectedItem = (toLoad as XvrTextureFile).SavePixelFormat;
            }
            internalTexture = toLoad;
            currentMip = 0;
            mipCount = internalTexture.mipMaps.Length;
            refreshMip();
        }

        private void replaceMipButton_Click(object sender, EventArgs e)
        {
            //internalTexture.ReplaceMip(
            if (loadMipDialog.ShowDialog() == DialogResult.OK)
            {
                internalTexture.ReplaceMip(new Bitmap(loadMipDialog.FileName), currentMip);
            }
            refreshMip();
        }

        private void buttonUpMip_Click(object sender, EventArgs e)
        {
            if (mipCount > currentMip + 1)
            {
                currentMip++;
            }
            refreshMip();
        }

        private void refreshMip()
        {
            mipLabel.Text = currentMip.ToString();
            pictureBox1.Image = internalTexture.getPreviewMip(currentMip);
            pictureBox1.Size = pictureBox1.Image.Size;
            mipCount = internalTexture.mipMaps.Length;
            if (mipCount > currentMip + 1)
                buttonUpMip.Enabled = true;
            else
                buttonUpMip.Enabled = false;
            if (currentMip > 0)
                buttonDownMip.Enabled = true;
            else
                buttonDownMip.Enabled = false;
        }

        private void buttonDownMip_Click(object sender, EventArgs e)
        {
            if (currentMip > 0)
            {
                currentMip--;
            }
            refreshMip();
        }

        private void importImageButton_Click(object sender, EventArgs e)
        {
            if (importTextureDialog.ShowDialog() == DialogResult.OK)
            {
                if (importTextureDialog.FileName.EndsWith(".xvr") || importTextureDialog.FilterIndex == 2)
                {
                    using (Stream tempStream = importTextureDialog.OpenFile())
                    {
                        using (BinaryReader tempFile = new BinaryReader(tempStream))
                        {
                            try
                            {
                                internalTexture.loadXvrFile(tempFile.ReadBytes((int)tempStream.Length));
                                currentMip = 0;
                                refreshMip();
                            }
                            catch (Exception f)
                            {
                                MessageBox.Show("Error reading texture:" + exportTextureDialog.ToString(), "Image Size Error");
                                //Ignored for now. I need real error checking.
                            }
                        }
                    }
                }
                else if (importTextureDialog.FileName.EndsWith(".pvr") || importTextureDialog.FilterIndex == 3)
                {
                    //Don't have support for .pvr yet.
                }
                else
                {
                    Bitmap importedImage = new Bitmap(importTextureDialog.FileName);
                    if (!isPowerOfTwo(importedImage.Width) || !isPowerOfTwo(importedImage.Height))
                    {
                        MessageBox.Show("Texture width and height must be a power of 2. \nThe following sizes are permitted: 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048.", "Image Size Error");
                        return;
                    }
                    if (importedImage.Width > 4096 || importedImage.Height > 4096)
                    {
                        MessageBox.Show("Texture cannot be larger than 4096x4096.", "Image Size Error");
                        return;
                    }
                    internalTexture.loadImage(importedImage, rebuildMipsCheckbox.Checked);
                    currentMip = 0;
                    refreshMip();
                }
            }
        }

        private bool isPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            exportTextureDialog.FileName = filename.Replace(".xvr", ".png");
            if (exportTextureDialog.ShowDialog() == DialogResult.OK)
            {
                if(exportTextureDialog.FileName.EndsWith(".png"))
                {
                    internalTexture.mipMaps[0].Save(exportTextureDialog.FileName);
                }
            }
        }

        private void buttonSaveMip_Click(object sender, EventArgs e)
        {
            saveMipDialog.FileName = filename.Replace(".xvr", "") + "_mip_" + currentMip.ToString() + ".png";
            if (saveMipDialog.ShowDialog() == DialogResult.OK)
            {
                if (exportTextureDialog.FileName.EndsWith(".png"))
                {
                    internalTexture.mipMaps[currentMip].Save(saveMipDialog.FileName);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(internalTexture is XvrTextureFile)
            {
                PsuTexturePixelFormat fmt;
                Enum.TryParse<PsuTexturePixelFormat>(pixelFormatDropDown.SelectedValue.ToString(), out fmt);
                ((XvrTextureFile)internalTexture).SavePixelFormat = fmt;
                refreshMip();
            }
        }
    }
}
