using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation1
{
    class imageEffects
    {

        public static Bitmap threshold_blackandwhite(Bitmap sourceImage)
        {
            List<pixel> pixelListSource = imageFunctions.GetPixelListFromBitmap(sourceImage);

            List<pixel> pixelListResult = new List<pixel>();



            foreach (pixel item in pixelListSource)
            {
                byte[] x = item.GetColorBytes();
                int coX = item.pixelX;
                int coY = item.pixelY;

                x[0] = x[0] < 128 ? (byte)0 : (byte)255;
                x[1] = x[0] < 128 ? (byte)0 : (byte)255;
                x[2] = x[0] < 128 ? (byte)0 : (byte)255;
                pixelListResult.Add(new pixel(x, coX, coY));
            }
            Bitmap resultBitmap = imageFunctions.GetBitmapFromPixelList(pixelListResult,
                       sourceImage.Width, sourceImage.Height);

            return resultBitmap;
        }

        public static Bitmap solarise(Bitmap sourceImage)
        {
            List<pixel> pixelListSource = imageFunctions.GetPixelListFromBitmap(sourceImage);

            List<pixel> pixelListResult = new List<pixel>();



            foreach (pixel item in pixelListSource)
            {
                byte[] x = item.GetColorBytes();
                int coX = item.pixelX;
                int coY = item.pixelY;

                if (x[0] <= 127.5){
                    x[0] = (byte) (255  - x[0]);
                }
                else { x[0] = x[0];}

                if (x[1] <= 127.5)
                {
                    x[1] = (byte)(255 - x[1]);
                }
                else { x[1] = x[1]; }

                if (x[2] <= 127.5)
                {
                    x[2] = (byte)(255 - x[2]);
                }
                else { x[0] = x[0]; }

                pixelListResult.Add(new pixel(x, coX, coY));
            }

            Bitmap resultBitmap = imageFunctions.GetBitmapFromPixelList(pixelListResult,
                       sourceImage.Width, sourceImage.Height);

            return resultBitmap;
        }

        public static Bitmap greyscale(Bitmap sourceImage, int colourRange) //-25 to 25
        {
            List<pixel> pixelListSource = imageFunctions.GetPixelListFromBitmap(sourceImage);

            List<pixel> pixelListResult = new List<pixel>();

            

            foreach (pixel item in pixelListSource)
            {
                byte[] x = item.GetColorBytes();
                int coX = item.pixelX;
                int coY = item.pixelY;

                double average = (x[0] + x[1] + x[2]) / 3 + colourRange;

                x[0] = (byte) average;
                x[1] = (byte)average;
                x[2] = (byte)average;
                pixelListResult.Add(new pixel(x, coX, coY));
            }


            Bitmap resultBitmap = imageFunctions.GetBitmapFromPixelList(pixelListResult,
                       sourceImage.Width, sourceImage.Height);

            return resultBitmap;
        }

        public static Bitmap colour_adjust(Bitmap sourceImage, int red,int green, int blue) //1 and 100
        {
            List<pixel> pixelListSource = imageFunctions.GetPixelListFromBitmap(sourceImage);

            List<pixel> pixelListResult = new List<pixel>();

            foreach (pixel item in pixelListSource)
            {
                byte[] x = item.GetColorBytes();
                int coX = item.pixelX;
                int coY = item.pixelY;

                double BluePercent = (blue / 50.0 * 100.0) / 100.0;
                x[0] = (byte)(x[0] * BluePercent);

                double GreenPercent = (green / 50.0 * 100.0) / 100.0;
                x[1] = (byte)(x[1] * GreenPercent);

                double RedPercent = (red / 50.0 * 100.0) / 100.0;
                x[2] = (byte)(x[2] * RedPercent);

                pixelListResult.Add(new pixel(x, coX, coY));
            }

            Bitmap resultBitmap = imageFunctions.GetBitmapFromPixelList(pixelListResult,
                       sourceImage.Width, sourceImage.Height);

            return resultBitmap;
        }





        public static Bitmap ApplyMatrix(Bitmap sourceImage, double[,] MatrixMaskThreeByThree)
        {
            List<pixel> pixelListSource = imageFunctions.GetPixelListFromBitmap(sourceImage);
            List<pixel> pixelListResult = new List<pixel>();

            double factor = 1.0;
            int bias = 0;
            int index = 0;

            foreach (pixel item in pixelListSource)
            {
                double blue = 0.0;
                double green = 0.0;
                double red = 0.0;

                List<pixel>  Matrix3x3GridPixels = FindGridForCentrePixel(pixelListSource, item,sourceImage,index);

                blue += (double) Matrix3x3GridPixels[0].blue * MatrixMaskThreeByThree[2, 2];
                blue += (double) Matrix3x3GridPixels[1].blue * MatrixMaskThreeByThree[2, 1];
                blue += (double) Matrix3x3GridPixels[2].blue * MatrixMaskThreeByThree[2, 0];
                blue += (double) Matrix3x3GridPixels[3].blue * MatrixMaskThreeByThree[1, 2];
                blue += (double) Matrix3x3GridPixels[4].blue * MatrixMaskThreeByThree[1, 1];
                blue += (double) Matrix3x3GridPixels[5].blue * MatrixMaskThreeByThree[1, 0];
                blue += (double) Matrix3x3GridPixels[6].blue * MatrixMaskThreeByThree[0, 2];
                blue += (double) Matrix3x3GridPixels[7].blue * MatrixMaskThreeByThree[0, 1];
                blue += (double) Matrix3x3GridPixels[8].blue * MatrixMaskThreeByThree[0, 0];

                green += (double) Matrix3x3GridPixels[0].green * MatrixMaskThreeByThree[2, 2];
                green += (double) Matrix3x3GridPixels[1].green * MatrixMaskThreeByThree[2, 1];
                green += (double) Matrix3x3GridPixels[2].green * MatrixMaskThreeByThree[2, 0];
                green += (double) Matrix3x3GridPixels[3].green * MatrixMaskThreeByThree[1, 2];
                green += (double) Matrix3x3GridPixels[4].green * MatrixMaskThreeByThree[1, 1];
                green += (double) Matrix3x3GridPixels[5].green * MatrixMaskThreeByThree[1, 0];
                green += (double) Matrix3x3GridPixels[6].green * MatrixMaskThreeByThree[0, 2];
                green += (double) Matrix3x3GridPixels[7].green * MatrixMaskThreeByThree[0, 1];
                green += (double) Matrix3x3GridPixels[8].green * MatrixMaskThreeByThree[0, 0];

                red += (double)Matrix3x3GridPixels[0].red * MatrixMaskThreeByThree[2, 2];
                red += (double)Matrix3x3GridPixels[1].red * MatrixMaskThreeByThree[2, 1];
                red += (double)Matrix3x3GridPixels[2].red * MatrixMaskThreeByThree[2, 0];
                red += (double)Matrix3x3GridPixels[3].red * MatrixMaskThreeByThree[1, 2];
                red += (double)Matrix3x3GridPixels[4].red * MatrixMaskThreeByThree[1, 1];
                red += (double)Matrix3x3GridPixels[5].red * MatrixMaskThreeByThree[1, 0];
                red += (double)Matrix3x3GridPixels[6].red * MatrixMaskThreeByThree[0, 2];
                red += (double)Matrix3x3GridPixels[7].red * MatrixMaskThreeByThree[0, 1];
                red += (double)Matrix3x3GridPixels[8].red * MatrixMaskThreeByThree[0, 0];

                blue = factor * blue + bias;
                green = factor * green + bias;
                red = factor * red + bias;

                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }

                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }

                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }

                byte[] x = item.GetColorBytes();
                int coX = item.pixelX;
                int coY = item.pixelY;
                x[0] = (byte)blue;
                x[1] = (byte)green;
                x[2] = (byte)red;

                pixelListResult.Add(new pixel(x, coX, coY));
                index++;
            }


            Bitmap resultBitmap = imageFunctions.GetBitmapFromPixelList(pixelListResult,
            sourceImage.Width, sourceImage.Height);
            return resultBitmap;
        }

        




        public static List<pixel> FindGridForCentrePixel(List<pixel> source, pixel centrePixel, Bitmap image, int currentIndex)
        {
            List<pixel> PixelGrid = new List<pixel>();
            int rowEnd = image.Width-1;

            pixel X00 = (currentIndex - rowEnd) - 1 > -1 ? source[(currentIndex - rowEnd) - 1]: source[currentIndex];
            pixel X01 = (currentIndex - rowEnd) > -1 ? source[(currentIndex - rowEnd)] : source[currentIndex];
            pixel X02 = (currentIndex - rowEnd) + 1 > -1 ? source[(currentIndex - rowEnd) + 1] : source[currentIndex];

            pixel X10 = currentIndex-1 > -1? source[currentIndex - 1]: source[currentIndex];
            pixel X11 = source[currentIndex];
            pixel X12 = currentIndex +1 < source.Count -1 ? source[currentIndex+1] : source[currentIndex];

            pixel X20 = (currentIndex + rowEnd) - 1 < source.Count-1? source[(currentIndex + rowEnd) - 1] : source[currentIndex];
            pixel X21 = (currentIndex + rowEnd) < source.Count - 1 ? source[(currentIndex + rowEnd)] : source[currentIndex];
            pixel X22 = (currentIndex + rowEnd) + 1 < source.Count - 1 ? source[(currentIndex + rowEnd) + 1] : source[currentIndex];

            PixelGrid.Add(X00);
            PixelGrid.Add(X01);
            PixelGrid.Add(X02);
            PixelGrid.Add(X10);
            PixelGrid.Add(X11);
            PixelGrid.Add(X12);
            PixelGrid.Add(X20);
            PixelGrid.Add(X21);
            PixelGrid.Add(X22);
            return PixelGrid;
        }






   

        public static Bitmap InvertColors(Bitmap sourceImage, ColourInversionType inversionType)
        {
            List<pixel> pixelListSource = imageFunctions.GetPixelListFromBitmap(sourceImage);

            List<pixel> pixelListResult = null;

            byte byte255 = 255;

            switch (inversionType)
            {
                case ColourInversionType.All:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = (byte)(byte255 - t.blue),
                             red = (byte)(byte255 - t.red),
                             green = (byte)(byte255 - t.green),
                             alpha = t.alpha,
                         }).ToList();
                        //foreach (pixel item in pixelListSource)
                        //{
                        //    byte[] x = item.GetColorBytes();
                        //    int coX = item.pixelX;
                        //    int coY = item.pixelY;
                        //    x[0] = (byte)(byte255 - x[0]);
                        //    x[1] = (byte)(byte255 - x[1]);
                        //    x[2] = (byte)(byte255 - x[2]);
                        //    pixelListResult.Add(new pixel(x,coX,coY));
                        //}
                        break;
                    }
                case ColourInversionType.Blue:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = (byte)(byte255 - t.blue),
                             red = t.red,
                             green = t.green,
                             alpha = t.alpha,
                         }).ToList();

                        break;
                    }
                case ColourInversionType.Green:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = t.blue,
                             red = t.red,
                             green = (byte)(byte255 - t.green),
                             alpha = t.alpha,
                         }).ToList();

                        break;
                    }
                case ColourInversionType.Red:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = t.blue,
                             red = (byte)(byte255 - t.green),
                             green = t.green,
                             alpha = t.alpha,
                         }).ToList();

                        break;
                    }
                case ColourInversionType.BlueRed:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = (byte)(byte255 - t.blue),
                             red = (byte)(byte255 - t.red),
                             green = t.green,
                             alpha = t.alpha,
                         }).ToList();

                        break;
                    }
                case ColourInversionType.BlueGreen:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = (byte)(byte255 - t.blue),
                             red = t.red,
                             green = (byte)(byte255 - t.green),
                             alpha = t.alpha,
                         }).ToList();

                        break;
                    }
                case ColourInversionType.RedGreen:
                    {
                        pixelListResult =
                        (from t in pixelListSource
                         select new pixel
                         {
                             blue = t.blue,
                             red = (byte)(byte255 - t.blue),
                             green = (byte)(byte255 - t.green),
                             alpha = t.alpha,
                         }).ToList();

                        break;
                    }
            }

            Bitmap resultBitmap = imageFunctions.GetBitmapFromPixelList(pixelListResult,
                        sourceImage.Width, sourceImage.Height);

            return resultBitmap;
        }
    }
    public enum ColourInversionType
    {
        All,
        Blue,
        Green,
        Red,
        BlueRed,
        BlueGreen,
        RedGreen,
    }
}
