using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace psu_generic_parser
{
    public partial class TextureViewer : UserControl
    {
        string imgType;
        string fileName;
        int unknown1 = 0;
        int fileChunks = 0;
        int filesize = 0;

        int flags;
        int imgStartLoc;
        bool hasMips = false;

        byte[] storedHeader;
        byte[] storedImg;

        short palUnkn1 = 0;
        short palSize = 0;
        short palUnkn2 = 0;
        short palColorDepth = 0;
        int imgHeaderLoc = 0;
        int palUnkn4 = 0;
        ColorPalette palette;
        Image palImage;
        short imgUnkn1 = 0;
        short imgWidth = 0;
        short imgHeight = 0;
        short imgColorDepth = 0;
        int imgLength = 0;
        int imgUnkn4 = 0;
        Bitmap internalImg;
        int pixelSize;

        bool hasIntHeader = true;

        public TextureViewer()
        {
            Dock = DockStyle.Fill;
            InitializeComponent();
        }

        public TextureViewer(byte[] header, byte[] toView, string newFilename) :this()
        {
            fileName = newFilename;
            storedHeader = header;
            storedImg = toView;
            MemoryStream headerStream = new MemoryStream(header);
            MemoryStream imageStream = new MemoryStream(toView);
            BinaryReader beta = new BinaryReader(imageStream);
            string fileIdentifier = new string(Encoding.ASCII.GetChars(header, 0, 4));
            imageStream.Seek(4, SeekOrigin.Begin);
            if (new string(Encoding.ASCII.GetChars(toView, 0, 4)) == "RIPC")
            {
                imgType = "RIPC";
                //label4.Text = Path.GetFileName(openFileDialog1.FileName);
                unknown1 = beta.ReadInt32();
                //label7.Text = unknown1.ToString();
                fileChunks = beta.ReadInt32();
                //label2.Text = fileChunks.ToString();
                //label2.Text = fileChunks.ToString();
                filesize = beta.ReadInt32();
                palUnkn1 = beta.ReadInt16();
                //label29.Text = palUnkn1.ToString();
                palSize = beta.ReadInt16();
                //label28.Text = palSize.ToString();
                palUnkn2 = beta.ReadInt16();
                //label27.Text = palUnkn2.ToString();
                palColorDepth = beta.ReadInt16();
                //label26.Text = palColorDepth.ToString();
                imgHeaderLoc = beta.ReadInt32();
                //label23.Text = imgHeaderLoc.ToString();
                palUnkn4 = beta.ReadInt32();
                //label22.Text = palUnkn4.ToString();
                palette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;
                palImage = new Bitmap(256, 256);
                for (int i = 0; i < palSize && imageStream.Position < (imgHeaderLoc + 16); i++)
                {
                    palette.Entries[i] = Color.FromArgb(beta.ReadInt32());
                    Graphics.FromImage(palImage).FillRectangle(new SolidBrush(palette.Entries[i]), (i % 16) * 16, (i / 16) * 16, 16, 16);
                }
                //pictureBox1.Image = palImage;
                if (imgHeaderLoc != 0 && imgHeaderLoc + 0x10 < filesize && (fileChunks & 1) != 0)
                {
                    imageStream.Seek(imgHeaderLoc + 0x10, SeekOrigin.Begin);
                    imgUnkn1 = beta.ReadInt16();
                    //label13.Text = imgUnkn1.ToString();
                    imgWidth = beta.ReadInt16();
                    //label14.Text = imgWidth.ToString();
                    imgHeight = beta.ReadInt16();
                    //label15.Text = imgHeight.ToString();
                    imgColorDepth = beta.ReadInt16();
                    //label16.Text = imgColorDepth.ToString();
                    imgLength = beta.ReadInt32();
                    //label19.Text = imgLength.ToString();
                    imgUnkn4 = beta.ReadInt32();
                    //label20.Text = imgUnkn4.ToString();
                    PixelFormat toUse = PixelFormat.Format8bppIndexed;
                    if (imgColorDepth == 4)
                        toUse = PixelFormat.Format4bppIndexed;
                    else if (imgColorDepth == 8)
                        toUse = PixelFormat.Format8bppIndexed;
                    internalImg = new Bitmap(imgWidth, imgHeight, toUse);
                    internalImg.Palette = palette;
                    byte[] imgData = beta.ReadBytes(imgLength);
                    BitmapData temp = internalImg.LockBits(new Rectangle(0, 0, imgWidth, imgHeight), ImageLockMode.ReadWrite, toUse);
                    Marshal.Copy(imgData, 0, temp.Scan0, imgLength - 0x10);
                    internalImg.UnlockBits(temp);
                    pictureBox1.Image = internalImg;
                }
                // Put .bin loading stuffz here
            }
            else if (fileIdentifier == "PVRT" || fileIdentifier == "GBIX")
            {
                imgType = "PVRT";
                //Texture gamma = new Texture( , 5, 5, 1, Usage.None, Format.Dxt5, Pool.Scratch);
                //if (fileIdentifier == "GBIX")
                    //alpha.Seek(0x14, SeekOrigin.Begin);
                label4.Text = BitConverter.ToInt32(header, 4).ToString("X4");
                label6.Text = BitConverter.ToInt32(header, 8).ToString("X4");
                label7.Text = BitConverter.ToInt32(header, 0xC).ToString("X4");
                label12.Text = BitConverter.ToInt32(header, 0x14).ToString("X8");
                label13.Text = BitConverter.ToInt32(toView, 0x1C).ToString("X8");
                label14.Text = toView.Length.ToString("X8");
                int fileLength = BitConverter.ToInt32(header, 0x14);// beta.ReadInt32();
                PixelFormat toUse = PixelFormat.Format16bppRgb565;
                flags = BitConverter.ToInt32(header, 0x18);
                //int padding = 0;
                bool twiddle = true;//(flags & 512) != 0;
                bool argb4444 = false;
                pixelSize = 2;
                label3.Text = flags.ToString("X4");
                if ((flags & 0xB04) == 0xB04)
                {
                    label2.Text = "DXT5";
                    return;
                }
                if ((flags & 0xE00) == 0x200)
                {
                    label2.Text = "DXT1";
                    return;
                }
                switch (flags & 7)
                {
                    //case 0:
                    //case 1:
                    case 2: toUse = PixelFormat.Format16bppArgb1555; label2.Text = "ARGB1555";  break;
                    case 3: toUse = PixelFormat.Format16bppRgb555; pixelSize = 2; label2.Text = "Rgb555"; break;
                    case 4: toUse = PixelFormat.Format32bppArgb; pixelSize = 4; argb4444 = true; label2.Text = "ARGB4444";  break; //argb4444
                    case 5: toUse = PixelFormat.Format16bppRgb565; pixelSize = 2; label2.Text = "Rgb565"; break;
                    case 6: toUse = PixelFormat.Format32bppArgb; pixelSize = 4; label2.Text = "32bppArgb";  break;
                    case 7: toUse = PixelFormat.Format32bppRgb; pixelSize = 4; label2.Text = "32bppRgb"; break;
                    default: break;
                }
                hasMips = ((flags & 0x100) != 0);
                imgWidth = BitConverter.ToInt16(header, 0x1C);
                imgHeight = BitConverter.ToInt16(header, 0x1E);
                internalImg = new Bitmap(imgWidth, imgHeight, toUse);
                int startLoc = 0;
                if (BitConverter.ToInt32(toView, 0) == 0x840001)
                {
                    hasIntHeader = true;
                    if (BitConverter.ToInt32(toView, 0x14) == 0)
                    {
                        imgStartLoc = 0x7E0;
                        imageStream.Seek(0x7E0, SeekOrigin.Begin);
                    }
                    else
                    {
                        imgStartLoc = 0x20;
                        imageStream.Seek(0x20, SeekOrigin.Begin);
                    }
                }
                else
                {
                    hasIntHeader = false;
                    imgStartLoc = 0;
                    imageStream.Seek(startLoc, SeekOrigin.Begin);
                }
                byte[] imgData;
                if (argb4444)
                    imgData = beta.ReadBytes(imgWidth * imgHeight * 2);
                else
                    imgData = beta.ReadBytes(imgWidth * imgHeight * pixelSize);
                byte[] imgArr = new byte[imgWidth * imgHeight * pixelSize];
                BitmapData temp = internalImg.LockBits(new Rectangle(0, 0, imgWidth, imgHeight), ImageLockMode.ReadWrite, toUse);
                if (twiddle)
                {
                    for (int i = 0; i < imgWidth * imgHeight && (((i + 1) * pixelSize) <= imgData.Length) || (argb4444 && ((i + 1) * 2) <= imgData.Length); i++)
                    {
                        int u = 0;
                        int v = 0;
                        int uOrig = (int)(i & 0xAAAAAAAA) >> 1;
                        int vOrig = i & 0x55555555;

                        for (int j = 0; j < 16; j++)
                        {
                            u |= ((uOrig & 1) << j);
                            v |= ((vOrig & 1) << j);
                            uOrig >>= 2;
                            vOrig >>= 2;
                        }
                        if (argb4444)
                        {
                            imgArr[(u * imgWidth + v) * 4 + 3] = (byte)(imgData[i * 2 + 1] & 0xF0);
                            imgArr[(u * imgWidth + v) * 4 + 2] = (byte)((imgData[i * 2 + 1] & 0xF) << 4);
                            imgArr[(u * imgWidth + v) * 4 + 1] = (byte)(imgData[i * 2] & 0xF0);
                            imgArr[(u * imgWidth + v) * 4] = (byte)((imgData[i * 2] & 0xF) << 4);
                        }
                        else
                            for (int j = 0; j < pixelSize; j++)
                            {
                                imgArr[(u * imgWidth + v) * pixelSize + j] = imgData[i * pixelSize + j];
                            }
                    }
                }
                else Array.Copy(imgData, imgArr, imgArr.Length);
                Marshal.Copy(imgArr, 0, temp.Scan0, imgWidth * imgHeight * pixelSize);
                internalImg.UnlockBits(temp);
                //Copy into the image after all the math is done!
                pictureBox1.Image = internalImg;
                // .xvr stuff here
            }
            pictureBox1.Width = imgWidth;
            pictureBox1.Height = imgHeight;
        }

        private void exportPng_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = fileName.Remove(fileName.Length - 4) + ".png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                internalImg.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }
        }

        private void import_Click(object sender, EventArgs e)
        {
            if (imgType == "PVRT")
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Stream tempStream = openFileDialog1.OpenFile();
                    Bitmap importedImg = new Bitmap(tempStream);
                    tempStream.Close();
                    byte[] rawData;
                    //int pixelSize;
                    /*
                    switch (importedImg.PixelFormat)
                    {
                        case PixelFormat.Format16bppArgb1555: // Direct match.
                            pixelSize = 2;
                            flags = (flags & 0x7000) | 2;
                            break;
                        case PixelFormat.Format16bppRgb555: // Store as 16bppArgb
                            pixelSize = 2;
                            flags = (flags & 0x7000) | 2;
                            break;
                        case PixelFormat.Format16bppRgb565: // Store as 16bppRgb565
                            pixelSize = 2;
                            flags = (flags & 0x7000) | 5;
                            break;
                        case PixelFormat.Format24bppRgb: //Shift up to 32bpp? Not sure if xvr has a 24bpp format, but probably!
                            return;     //You know, maybe I should just create a 32bpp image from it and then just use the 32bpp logic.
                            pixelSize = 4;
                            //rawData = new byte[importedImg.Width * importedImg.Height * pixelSize];
                            flags = (flags & 0x7000) | 6;
                            break;
                        case PixelFormat.Format32bppArgb: //Just copy directly!
                            pixelSize = 4;
                            //flags = (flags & 0x7000) | 6;
                            break;
                        default: //Not sure I need a default case here?
                            return; //You don't get to import things like that because fadskfljds
                            break; //Don't worry about it right now!
                    }
                     */
                    internalImg = new Bitmap(internalImg.Width, internalImg.Height, internalImg.PixelFormat);
                    //internalImg
                    Graphics.FromImage(internalImg).DrawImageUnscaled(importedImg, 0, 0);
                    //rawData = new byte[importedImg.Width * importedImg.Height * pixelSize];
                    //BitmapData bmpData = importedImg.LockBits(new Rectangle(0, 0, importedImg.Width, importedImg.Height), ImageLockMode.ReadOnly, importedImg.PixelFormat);
                    //Marshal.Copy(bmpData.Scan0, rawData, 0, rawData.Length);
                    //importedImg.UnlockBits(bmpData);

                    //rawData = new byte[internalImg.Width * internalImg.Height * pixelSize];
                    //BitmapData bmpData = internalImg.LockBits(new Rectangle(0, 0, internalImg.Width, internalImg.Height), ImageLockMode.ReadOnly, internalImg.PixelFormat);
                    //Marshal.Copy(bmpData.Scan0, rawData, 0, rawData.Length);
                    //internalImg.UnlockBits(bmpData);

                    int currLoc = imgStartLoc;
                    for (int mipLevel = internalImg.Width; (mipLevel == internalImg.Width) || (hasMips && mipLevel > 0); mipLevel >>= 1)
                    {
                        Bitmap tempImage = new Bitmap(mipLevel, mipLevel, internalImg.PixelFormat);
                        Graphics.FromImage(tempImage).DrawImage(internalImg, 0, 0, mipLevel, mipLevel);
                        rawData = new byte[mipLevel * mipLevel * pixelSize];
                        BitmapData bmpData = tempImage.LockBits(new Rectangle(0, 0, mipLevel, mipLevel), ImageLockMode.ReadOnly, tempImage.PixelFormat);
                        Marshal.Copy(bmpData.Scan0, rawData, 0, rawData.Length);
                        tempImage.UnlockBits(bmpData);
                        for (int i = 0; i < tempImage.Height; i++)
                        {
                            for (int j = 0; j < tempImage.Width; j++)
                            {
                                int destLoc = 0;
                                for (int p = 0; p < 16; p++)
                                {
                                    destLoc |= ((j & (1 << p)) << (p));
                                    destLoc |= ((i & (1 << p)) << (p + 1));
                                }
                                Array.Copy(rawData, (i * mipLevel + j) * pixelSize, storedImg, currLoc + (destLoc * pixelSize), pixelSize);
                            }
                        }
                        currLoc += rawData.Length;
                    }
                }
            }
            //Not allowing RIPC yet, that's gonna be a PAIN in the ass
        }

        //Export the xvr!
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //Import the xvr!
        private void button4_Click(object sender, EventArgs e)
        {

        }

    }
}