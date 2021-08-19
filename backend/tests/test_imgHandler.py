import unittest
from flaskr import imgHandler
from PIL import Image


class TestImgHandler(unittest.TestCase):
    def test_embed_image_1(self):
        h = imgHandler.ImgHandler()
        img = Image.open('img_for_test/sheep.jpeg', mode='r')
        h.embed_msg(img, "français")
        self.assertEqual("français", h.extract_msg(Image.open('out_img_for_test/sheep.png', mode='r')))

    def test_embed_image_2(self):
        h = imgHandler.ImgHandler()
        img = Image.open('img_for_test/sheep.jpeg', mode='r')
        h.embed_msg(img, "I love Canada")
        self.assertEqual("I love Canada", h.extract_msg(Image.open('out_img_for_test/sheep.png', mode='r')))