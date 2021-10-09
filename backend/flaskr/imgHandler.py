from functools import reduce
from typing import List, Optional
from PIL import Image

from backend.flaskr.textHandler import TextHandler


class UnexpectedOutput(Exception):
    pass


class ImgHandler:

    # TODO: make the output name dynamic
    @staticmethod
    def embed_msg(image: Image, byte_arr: List[List[int]]) -> Image:

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

        return image

    @staticmethod
    def extract_msg(image: Image) -> Optional[List[List[int]]]:

        x, y = image.size
        byte_arr = []
        tmp = []

        for i in range(0, x):
            for j in range(0, y):
                pixel = image.getpixel((i, j))
                for k in range(3):
                    tmp.append(pixel[k] & 1)
                    if len(tmp) >= 8:
                        byte_arr.append(tmp)
                        tmp = []
                        # header and footer takes 10 byte
                        if ImgHandler.__is_end(byte_arr):
                            return byte_arr
        raise UnexpectedOutput("Failed to retrieve the result")

    @staticmethod
    def __is_end(byte_arr: List[List[int]]) -> bool:
        if len(byte_arr) < len(TextHandler.HEADER) + len(TextHandler.FOOTER):
            return False
        footer_byte = TextHandler.encode_msg(TextHandler.FOOTER)
        for i in range(len(TextHandler.FOOTER)):
            if footer_byte[i] != byte_arr[len(TextHandler.FOOTER) * -1 + i]:
                return False
        return True
