import unittest
from flaskr import imgHandler
from PIL import Image


class TestImgHandler(unittest.TestCase):
    def test_image_to_byte_array(self):
        h = imgHandler.ImgHandler()
        img = Image.open('img_for_test/sheep.jpeg', mode='r')
        h.image_to_byte_array(img)
        print(img.size)

    def test_save_image(self):
        h = imgHandler.ImgHandler()
        img = Image.open('img_for_test/sheep.jpeg', mode='r')
        h.save_image(img)
