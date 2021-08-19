from functools import reduce
from typing import List
from PIL import Image
from flaskr import textHandler


class ImgHandler:
    def __init__(self):
        pass

    # TODO: make the output name dynamic
    def embed_msg(self, image: Image, raw_msg: str) -> None:
        # get the msg in bit format
        h = textHandler.TextHandler()
        msg = h.insert_indicator(raw_msg)
        byte_arr = h.encode_msg(msg)

        # flatten the 2d list to 1d list
        byte_arr = reduce(lambda u, v: u + v, byte_arr)

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
                        val[k] = (val[k] & 254) + byte_arr[bit_idx]
                        bit_idx += 1
                image.putpixel((i, j), (val[0], val[1], val[2]))
            if bit_idx >= len(byte_arr):
                break

        # save the alter image, must be in png format to prevent data lose
        filename = 'out_img_for_test/sheep.png'
        image.save(filename)

    def extract_msg(self, image: Image) -> str:

        x, y = image.size
        byte_arr = []
        tmp = []
        h = textHandler.TextHandler()
        done = False

        for i in range(0, x):
            for j in range(0, y):
                pixel = image.getpixel((i, j))
                for k in range(3):
                    tmp.append(pixel[k] & 1)
                    if len(tmp) >= 8:
                        byte_arr.append(tmp)
                        tmp = []
                        # header and footer takes 10 byte
                        if len(byte_arr) >= len(h.header) + len(h.footer):
                            done = self.check_end(byte_arr)
                        if done:
                            break
                if done:
                    break
            if done:
                break

        # decode the message
        msg = h.decode_msg(byte_arr)

        # remove the indicator
        msg = h.remove_indicator(msg)

        return msg

    def check_end(self, byte_arr: List[List[int]]) -> bool:
        h = textHandler.TextHandler()
        footer_byte = h.encode_msg(h.footer)
        for i in range(len(h.footer)):
            if footer_byte[i] != byte_arr[len(h.footer) * -1 + i]:
                return False
        return True