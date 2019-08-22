using System;
using System.Drawing;

namespace HideNSeek
{
    class ImgProcessing
    {
        public static void InsertMsg(byte[] textInByte, Bitmap imgMap)
        {
            /*
            // ensure that the photo has enough space for the message
            if (textInByte.Length > (imgMap.Width * imgMap.Height * 3) / 8.0)
            {
                status.Text = "Not Enough Space";
                return;
            }
            */
            Console.WriteLine("str");
            foreach(byte b in textInByte)
            {
                Console.WriteLine(b);
            }
            Console.WriteLine("end");

            int i = 0, j = 7;
            for (int x = 0; x < imgMap.Width; x++)
            {
                for (int y = 0; y < imgMap.Height; y++)
                {
                    if (i == textInByte.Length) return;

                    Color pixelColor = imgMap.GetPixel(x, y);

                    int bit = (textInByte[i] >> j--) & 1;
                    Color newColor = Color.FromArgb((pixelColor.R & 0xFE) + bit, pixelColor.G, pixelColor.B);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                        if (i == textInByte.Length) return;
                    }

                    bit = (textInByte[i] >> j--) & 1;
                    newColor = Color.FromArgb(newColor.R, (pixelColor.G & 0xFE) + bit, pixelColor.B);
                    imgMap.SetPixel(x, y, newColor);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                        if (i == textInByte.Length) return;
                    }

                    bit = (textInByte[i] >> j--) & 1;
                    newColor = Color.FromArgb(newColor.R, newColor.G, (pixelColor.B & 0xFE) + bit);
                    imgMap.SetPixel(x, y, newColor);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                        if (i == textInByte.Length) return;
                    }
                }
            }
        }

        /*
            Input:  the image map generated from the image selected and the prepossed text input
        */
        public static byte[] ExtractByte(Bitmap imgMap)
        {
            byte[] tmp = new byte[(int)Math.Ceiling((imgMap.Width * imgMap.Height * 3) / 8.0)];

            byte[] str = { 60, 83, 84, 82, 62 };
            byte[] end = { 60, 69, 78, 68, 62 };
            bool STR = false, END = false;
            int i = 0, j = 7, k = 0;
            int[] mask = { 1, 2, 4, 8, 16, 32, 64, 128 };
            int sum = 0;
            for (int x = 0; x < imgMap.Width && !END; x++)
            {
                for (int y = 0; y < imgMap.Height && !END; y++)
                {
                    if (i == tmp.Length)
                    {
                        END = true;
                        break;
                    }

                    Color pixelColor = imgMap.GetPixel(x, y);

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

            byte[] textInByte = new byte[i];
            Array.Copy(tmp, textInByte, textInByte.Length);

            Console.WriteLine(textInByte.Length);
            return textInByte;
        }
    }
}
