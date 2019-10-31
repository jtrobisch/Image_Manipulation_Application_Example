using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation1
{
    public class pixel
    {
        public int pixelX = 0; //pixel co-ordinates in the image
        public int pixelY = 0;

        public byte blue = 0; //between 0 and 255 (RGB settings)
        public byte green = 0;
        public byte red = 0;
        public byte alpha = 0; //transparency

        public pixel()
        {

        }

        //public pixel(byte[] colorComponents) //basic constructor to populate properties
        //{
        //    blue = colorComponents[0];
        //    green = colorComponents[1];
        //    red = colorComponents[2];
        //    alpha = colorComponents[3];
        //}

        public pixel(byte[] colorComponents, int x, int y) //basic constructor to populate all pixel properties
        {
            blue = colorComponents[0];
            green = colorComponents[1];
            red = colorComponents[2];
            alpha = colorComponents[3];

            pixelX = x;
            pixelY = y;
        }

        public byte[] GetColorBytes()
        {
            return new byte[] { blue, green, red, alpha }; //returns pixel colours in a byte array
        }

        public byte getSingleItem(int index) //return specific pixel property based on index given 
        {       switch (index)
                {
                    case 0: return blue;
                    case 1: return green;
                    case 2: return red;
                    case 3: return alpha;
                    default: return 0;
                }
         }
    }
}
