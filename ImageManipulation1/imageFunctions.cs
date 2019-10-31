using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation1
{
    public class imageFunctions
    {

       public static Bitmap GetBitmapFromPixelList(List<pixel> pixelList, int width, int height) //convert a list of pixels into a BITMAP
       {
            Bitmap resultBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb); 
            //creates a new bitmap object (sets height/width & Format of object)

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            //The BitmapData class is a helper object used when calling the Bitmap.LockBits() method.
            //It contains iYou can't really "convert" between the two per se, as they don't represent the same information. 
            //You can obtain a BitmapData object from a Bitmap object simply by calling LockBits().
            //If you have a BitmapData object from some other Bitmap object, you can copy that data to a new Bitmap object by allocating one 
            //with the same format as the original, calling LockBits on that one too, and then just copying the bytes from one to the other.
            //Information about the locked bitmap, which you can use to inspect the pixel data within the bitmap.

            byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];
            //creates a byte array for pixels - the size is set using width(Stride) * height (sets array size)

            using (MemoryStream memoryStream = new MemoryStream(resultBuffer)) 
            { //creates a stream to the results buffer array
                memoryStream.Position = 0; //sets the start position to 0
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                //BinaryWriter object is used to write the data to the byte array
                foreach (pixel pixel in pixelList)
                { //cycle through each pixel in the list and get the colour bytes data and write it to the byte[] array
                    binaryWriter.Write(pixel.GetColorBytes()); 
                }
                binaryWriter.Close(); //close writer object
            }
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData); //unlock bits for use

            return resultBitmap; //return the result
        }

        public static List<pixel> GetPixelListFromBitmap(Bitmap sourceImage)
        {
            BitmapData sourceData = sourceImage.LockBits(new Rectangle(0, 0,
                        sourceImage.Width, sourceImage.Height),
                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            sourceImage.UnlockBits(sourceData);

            List<pixel> pixelList = new List<pixel>();

            int x = 0;
            int y = 0;

            using (MemoryStream memoryStream = new MemoryStream(sourceBuffer))
            {
                memoryStream.Position = 0;
                BinaryReader binaryReader = new BinaryReader(memoryStream);

                while (memoryStream.Position + 4 <= memoryStream.Length)
                {
                    pixel pixel = new pixel(binaryReader.ReadBytes(4), x, y);
                    pixelList.Add(pixel);

                    x += 1;

                    if (x >= sourceData.Width)//if x length is greater than image width...
                    {
                        x = 0; //reset x to 0
                        y += 1;//create a new line (y value)
                    }
                }

                binaryReader.Close();
            }

            return pixelList;
        }

        public static Bitmap CopyToSquareCanvas(Bitmap sourceBitmap, int canvasWidthLenght)
        {
            float ratio = 1.0f;
            int maxSide = sourceBitmap.Width > sourceBitmap.Height ? sourceBitmap.Width : sourceBitmap.Height; //determine the longest side

            ratio = (float)maxSide / (float)canvasWidthLenght; //determine the ratio


            Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height ? new Bitmap(canvasWidthLenght, (int)(sourceBitmap.Height / ratio)) : new Bitmap((int)(sourceBitmap.Width / ratio), canvasWidthLenght)); //create new bitmap with the new ratio

            using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
            {
                graphicsResult.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicsResult.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphicsResult.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphicsResult.DrawImage(sourceBitmap, new Rectangle(0, 0, bitmapResult.Width, bitmapResult.Height), new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), GraphicsUnit.Pixel);
                graphicsResult.Flush();//forces execution and returns result immeditley 
            } //draws the new image 

            return bitmapResult;
        }

    }
}
