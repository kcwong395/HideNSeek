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
            for (int i = 0; i < indexList.Length; i++) Console.WriteLine(indexList[i]);
            int curByte = 0, curBit = 7;
            for (int x = indexList[0]; x < imgMap.Width; x += indexList[2])
            {
                for (int y = indexList[1]; y < imgMap.Height; y += indexList[3])
                {
                    Color pixelColor = imgMap.GetPixel(x, y);

                    // k represents rgb
                    for (int k = 0; k < 3; k++)
                    {
                        if(indexList[k + 4] == 1)
                        {
                            int bit = (textInByte[curByte] >> curBit--) & 1;
                            if(k == 0)
                            {
                                pixelColor = Color.FromArgb((pixelColor.R & 0xFE) + bit, pixelColor.G, pixelColor.B);
                            }
                            else if(k == 1)
                            {
                                pixelColor = Color.FromArgb(pixelColor.R, (pixelColor.G & 0xFE) + bit, pixelColor.B);
                            }
                            else
                            {
                                pixelColor = Color.FromArgb(pixelColor.R, pixelColor.G, (pixelColor.B & 0xFE) + bit);
                            }
                            if (curBit < 0)
                            {
                                curByte++;    // process to next byte
                                curBit = 7;  // reset the bit to 7

                                if (curByte == textInByte.Length)
                                {
                                    imgMap.SetPixel(x, y, pixelColor);
                                    return;
                                }
                            }
                        }
                    }
                    // updates every pixel
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

            int curByte = 0, curBit = 7;
            int[] mask = { 1, 2, 4, 8, 16, 32, 64, 128 };
            
            int endByte = 0;
            byte[] end = { 60, 69, 78, 68, 62 };

            int sum = 0;
            bool done = false;
            for (int x = indexList[0]; x < imgMap.Width && !done; x += indexList[2])
            {
                for (int y = indexList[1]; y < imgMap.Height && !done; y += indexList[3])
                {
                    Color pixelColor = imgMap.GetPixel(x, y);

                    if (indexList[4] == 1)
                    {
                        sum += (pixelColor.R & 1) * mask[curBit--];
                        if (curBit < 0)
                        {
                            curBit = 7;
                            tmp[curByte++] = (byte)sum;
                            if ((byte)sum == end[endByte])
                            {
                                endByte++;
                                if (endByte == 5)
                                {
                                    done = true;
                                    break;
                                }
                            }
                            else
                            {
                                endByte = 0;
                            }
                            sum = 0;
                        }
                    }

                    if (indexList[5] == 1)
                    {
                        sum += (pixelColor.G & 1) * mask[curBit--];
                        if (curBit < 0)
                        {
                            curBit = 7;
                            tmp[curByte++] = (byte)sum;

                            if ((byte)sum == end[endByte])
                            {
                                endByte++;
                                if (endByte == 5)
                                {
                                    done = true;
                                    break;
                                }
                            }
                            else
                            {
                                endByte = 0;
                            }

                            sum = 0;
                        }
                    }

                    if (indexList[6] == 1)
                    {
                        sum += (pixelColor.B & 1) * mask[curBit--];
                        if (curBit < 0)
                        {
                            curBit = 7;
                            tmp[curByte++] = (byte)sum;

                            if ((byte)sum == end[endByte])
                            {
                                endByte++;
                                if (endByte == 5)
                                {
                                    done = true;
                                    break;
                                }
                            }
                            else
                            {
                                endByte = 0;
                            }

                            sum = 0;
                        }
                    }
                }
            }

            byte[] textInByte = new byte[curByte - 10];
            for (int t = 0; t < textInByte.Length; t++)
            {
                textInByte[t] = tmp[t + 5];
            }
            return textInByte;
        }
    }
}
