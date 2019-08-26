using System;
using System.Drawing;

namespace HideNSeek
{
    class ImgProcessing
    {
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

                    if (indexList[4] == 1)
                    {
                        int bit = (textInByte[i] >> j--) & 1;
                        pixelColor = Color.FromArgb((pixelColor.R & 0xFE) + bit, pixelColor.G, pixelColor.B);
                        if (j < 0)
                        {
                            i++;
                            j = 7;
                            if (i == textInByte.Length) return;
                        }
                    }

                    if (indexList[5] == 1)
                    {
                        int bit = (textInByte[i] >> j--) & 1;
                        pixelColor = Color.FromArgb(pixelColor.R, (pixelColor.G & 0xFE) + bit, pixelColor.B);
                        if (j < 0)
                        {
                            i++;
                            j = 7;
                            if (i == textInByte.Length) return;
                        }
                    }

                    if(indexList[6] == 1)
                    {
                        int bit = (textInByte[i] >> j--) & 1;
                        pixelColor = Color.FromArgb(pixelColor.R, pixelColor.G, (pixelColor.B & 0xFE) + bit);
                        if (j < 0)
                        {
                            i++;
                            j = 7;
                            if (i == textInByte.Length) return;
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

            byte[] str = { 60, 83, 84, 82, 62 };
            byte[] end = { 60, 69, 78, 68, 62 };
            bool STR = false, END = false;
            int i = 0, j = 7, k = 0;
            int[] mask = { 1, 2, 4, 8, 16, 32, 64, 128 };
            int sum = 0;
            for (int x = indexList[0]; x < imgMap.Width && !END; x += indexList[2])
            {
                for (int y = indexList[1]; y < imgMap.Height && !END; y += indexList[3])
                {
                    if (i == tmp.Length)
                    {
                        END = true;
                        break;
                    }

                    Color pixelColor = imgMap.GetPixel(x, y);

                    if (indexList[4] == 1)
                    {
                        sum += (pixelColor.R & 1) * mask[j--];
                        if (j < 0)
                        {
                            j = 7;
                            if (STR && sum == 60)
                            {
                                END = true;
                                break;
                            }
                            if (STR) tmp[i++] = (byte)sum;
                            if (sum == 62) STR = true;
                            sum = 0;
                        }
                    }

                    if (indexList[5] == 1)
                    {
                        sum += (pixelColor.G & 1) * mask[j--];
                        if (j < 0)
                        {
                            j = 7;
                            if (STR && sum == 60)
                            {
                                END = true;
                                break;
                            }
                            if (STR) tmp[i++] = (byte)sum;
                            if (sum == 62) STR = true;
                            sum = 0;
                        }
                    }

                    if (indexList[6] == 1)
                    {
                        sum += (pixelColor.B & 1) * mask[j--];
                        if (j < 0)
                        {
                            j = 7;
                            if (STR && sum == 60)
                            {
                                END = true;
                                break;
                            }
                            if (STR) tmp[i++] = (byte)sum;
                            if (sum == 62) STR = true;
                            sum = 0;
                        }
                    }
                }
            }
            
            byte[] textInByte = new byte[i];
            Array.Copy(tmp, textInByte, textInByte.Length);

            return textInByte;
        }
    }
}
