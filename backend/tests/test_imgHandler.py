import unittest
from flaskr.imgHandler import ImgHandler
from flaskr.textHandler import TextHandler
from PIL import Image


class TestImgHandler(unittest.TestCase):
    def test_embed_image_1(self):
        img = Image.open('backend/tests/img_for_test/sheep.jpeg', mode='r')
        text = TextHandler.encode_msg(TextHandler.insert_indicator("français"))
        image = ImgHandler.embed_msg(img, text)
        msg = TextHandler.decode_msg(ImgHandler.extract_msg(image))
        msg = TextHandler.remove_indicator(msg)
        self.assertEqual("français", msg)

    def test_embed_image_2(self):
        img = Image.open('backend/tests/img_for_test/sheep.jpeg', mode='r')
        text = TextHandler.encode_msg(TextHandler.insert_indicator("I love Canada"))
        ImgHandler.embed_msg(img, text)
        image = ImgHandler.embed_msg(img, text)
        msg = TextHandler.decode_msg(ImgHandler.extract_msg(image))
        msg = TextHandler.remove_indicator(msg)
        self.assertEqual("I love Canada", msg)
