using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HideNSeek
{
    class ImgProcessing
    {
        public static Boolean isImg(string path)
        {
            string[] ImageExtensions = new string[] { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            string curExtension = Path.GetExtension(path).ToUpper();
            foreach (string imgExtension in ImageExtensions)
            {
                if (curExtension.Equals(imgExtension))
                {
                    return true;
                }
            }
            return false;
        }

        /*
            Input:  receive the encoded msg, image map generated and indexList 
        */
        public static void InsertByte(byte[] textInByte, Bitmap imgMap, int[] indexList)
        {
            int i = 0, j = 7;
            for (int x = indexList[0]; x < imgMap.Width; x += indexList[2])
            {
                for (int y = indexList[1]; y < imgMap.Height; y += indexList[3])
                {
                    if (i == textInByte.Length) return;

                    Color pixelColor = imgMap.GetPixel(x, y);

                    for(int k = 0; k < 3; k++)
                    {
                        if(indexList[k + 4] == 1)
                        {
                            int bit = (textInByte[i] >> j--) & 1;
                            if(k + 4 == 4)
                            {
                                pixelColor = Color.FromArgb((pixelColor.R & 0xFE) + bit, pixelColor.G, pixelColor.B);
                            }
                            else if(k + 4 == 5)
                            {
                                pixelColor = Color.FromArgb(pixelColor.R, (pixelColor.G & 0xFE) + bit, pixelColor.B);
                            }
                            else
                            {
                                pixelColor = Color.FromArgb(pixelColor.R, (pixelColor.G & 0xFE) + bit, pixelColor.B);
                            }
                            if (j < 0)
                            {
                                i++;
                                j = 7;
                                if (i == textInByte.Length)
                                {
                                    imgMap.SetPixel(x, y, pixelColor);
                                    return;
                                }
                            }
                        }
                    }
                    imgMap.SetPixel(x, y, pixelColor);
                }
            }
        }

        /*
            Input:  the image map generated from the image selected and the indexList
            Return: the extract msg in byte format
        */
        public static byte[] ExtractByte(Bitmap imgMap, int[] indexList)
        {
            byte[] tmp = new byte[(int)Math.Ceiling((imgMap.Width * imgMap.Height * 3) / 8.0)];

            int i = 0, j = 7;
            int[] mask = { 1, 2, 4, 8, 16, 32, 64, 128 };
            int sum = 0;
            for (int x = indexList[0]; x < imgMap.Width; x += indexList[2])
            {
                for (int y = indexList[1]; y < imgMap.Height; y += indexList[3])
                {
                    Color pixelColor = imgMap.GetPixel(x, y);

                    if (indexList[4] == 1)
                    {
                        sum += (pixelColor.R & 1) * mask[j--];
                        if (j < 0)
                        {
                            j = 7;
                            tmp[i++] = (byte)sum;
                            sum = 0;
                        }
                    }

                    if (indexList[5] == 1)
                    {
                        sum += (pixelColor.G & 1) * mask[j--];
                        if (j < 0)
                        {
                            j = 7;
                            tmp[i++] = (byte)sum;
                            sum = 0;
                        }
                    }

                    if (indexList[6] == 1)
                    {
                        sum += (pixelColor.B & 1) * mask[j--];
                        if (j < 0)
                        {
                            j = 7;
                            tmp[i++] = (byte)sum;
                            sum = 0;
                        }
                    }
                }
            }

            /*
             <key>abc<key>ifc<idx>5,5,5,5,1,1,1<idx>
            */
            int k = 5;
            byte[] end = { 60, 69, 78, 68, 62 };
            for (; k < tmp.Length; k++)
            {
                int c = 0;
                for (int t = k; c < 5 && tmp[t] == end[c]; t++, c++) ;
                if (c == 5)
                {
                    break;
                }

            }
            byte[] textInByte = new byte[k - 5];
            for(int t = 0; t < k - 5; t++)
            {
                textInByte[t] = tmp[t + 5];
            }
            return textInByte;
        }
    }
}
