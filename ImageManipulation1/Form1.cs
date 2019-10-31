using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ImageManipulation1
{
    public partial class Form1 : Form
    {
        private Bitmap sourceBitmap = null; //bitmap pixel data for loaded image
        private Bitmap resultBitmap = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); //creates an open dialog box object
            ofd.Title = "Select an image file."; //sets title
            ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg"; //adds image filters
            ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if the ok button is clicked in the open dialog...
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);//opens up a stream to read the image, using the directory
                sourceBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream); //loads the image into the source bitmap variable
                streamReader.Close(); //closes the reader 

                pictureBox1.Image = imageFunctions.CopyToSquareCanvas(sourceBitmap, pictureBox1.Width); //sets picturebox 1 image
                resultBitmap = imageFunctions.CopyToSquareCanvas(sourceBitmap, pictureBox1.Width); //sets picturebox 1 image
                //ApplyFilter(true);
            }
        }
        


        


        

        private void button1_Click(object sender, EventArgs e) 
        {
            try
            {
                Bitmap bitmapResult = null;
                //bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Sobel3x3Horizontal);
                bitmapResult = imageEffects.threshold_blackandwhite(resultBitmap);

                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (resultBitmap != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Specify a file name and file path";
                sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                    ImageFormat imgFormat = ImageFormat.Png;

                    if (fileExtension == "BMP")
                    {
                        imgFormat = ImageFormat.Bmp;
                    }
                    else if (fileExtension == "JPG")
                    {
                        imgFormat = ImageFormat.Jpeg;
                    }

                    StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                    resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                    streamWriter.Flush();
                    streamWriter.Close();

                    resultBitmap = null;
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                bitmapResult = imageEffects.solarise(resultBitmap);
                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                bitmapResult = imageEffects.greyscale(resultBitmap,trackBar1.Value);
                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {

        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                bitmapResult = imageEffects.colour_adjust(resultBitmap, trackBar2.Value, trackBar3.Value, trackBar4.Value);
                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                bitmapResult = imageEffects.colour_adjust(resultBitmap, trackBar2.Value, trackBar3.Value, trackBar4.Value);
                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

        private void TrackBar4_Scroll(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                bitmapResult = imageEffects.colour_adjust(resultBitmap, trackBar2.Value, trackBar3.Value, trackBar4.Value);
                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                if (comboBox1.Text== "Laplacian3x3") { 
                        bitmapResult = imageEffects.ApplyMatrix(resultBitmap,matrix.Laplacian3x3);
                }
                else if (comboBox1.Text == "Gaussian3x3")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Gaussian3x3);
                }
                else if (comboBox1.Text == "edgeDetection")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.edgeDetection);
                }
                else if (comboBox1.Text == "sharpen")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.sharpen);
                }
                else if (comboBox1.Text == "Sobel3x3Horizontal")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Sobel3x3Horizontal);
                }
                else if (comboBox1.Text == "Sobel3x3Vertical")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Sobel3x3Vertical);
                }
                else if (comboBox1.Text == "Prewitt3x3Horizontal")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Prewitt3x3Horizontal);
                }
                else if (comboBox1.Text == "Prewitt3x3Vertical")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Prewitt3x3Vertical);
                }
                else if (comboBox1.Text == "Kirsch3x3Horizontal")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Kirsch3x3Horizontal);
                }
                else if (comboBox1.Text == "Kirsch3x3Vertical")
                {
                    bitmapResult = imageEffects.ApplyMatrix(resultBitmap, matrix.Kirsch3x3Vertical);
                }
                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmapResult = null;
                if (comboBox2.Text == "All")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.All);
                }
                else if (comboBox2.Text == "Blue")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.Blue);
                }
                else if (comboBox2.Text == "Green")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.Green);
                }
                else if (comboBox2.Text == "Red")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.Red);
                }
                else if (comboBox2.Text == "BlueRed")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.BlueRed);
                }
                else if (comboBox2.Text == "BlueGreen")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.BlueGreen);
                }
                else if (comboBox2.Text == "RedGreen")
                {
                    bitmapResult = imageEffects.InvertColors(resultBitmap, ColourInversionType.RedGreen);
                }


                pictureBox2.Image = imageFunctions.CopyToSquareCanvas(bitmapResult, pictureBox2.Width);
            }
             catch
            {
                MessageBox.Show("Unexpected Error");
            }
        }

    }
   
}
