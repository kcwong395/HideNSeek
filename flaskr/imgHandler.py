from functools import reduce

from PIL import Image
import io
import textHandler


class ImgHandler:
    def __init__(self):
        pass

    # borrow from https://stackoverflow.com/questions/33101935/convert-pil-image-to-byte-array
    def image_to_byte_array(self, image: Image):
        img_byte_arr = io.BytesIO()
        image.save(img_byte_arr, format=image.format)
        img_byte_arr = img_byte_arr.getvalue()
        return img_byte_arr

    def embed_msg(self, image: Image, raw_msg: str) -> None:
        # get the msg in bit format
        h = textHandler.TextHandler()
        msg = h.insert_indicator(raw_msg)
        byte_arr = h.encode_msg(msg)

        # flatten the 2d list to 1d list
        reduce(lambda u, v: u + v, byte_arr)

        # insert the bit one by one
        x, y = image.size
        bit_idx = 0
        for i in range(0, x):
            for j in range(0, y):
                ori_pixel = image.getpixel((i, j))
                val = [ori_pixel[0], ori_pixel[1], ori_pixel[2]]
                for k in range(3):
                    if bit_idx >= len(byte_arr):
                        break
                    else:
                        # TODO: logic here need to be think carefully
                        val[i] = byte_arr[bit_idx]
                        bit_idx += 1
                image.putpixel((i, j), (val[0], val[1], val[2]))
            if bit_idx >= len(byte_arr):
                break
        # save the alter image
        filename = 'out_img_for_test/sheep.jpeg'
        image.save(filename, image.format)

    def extract_msg(self, image: Image) -> None:
        pass