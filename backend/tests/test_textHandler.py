import unittest
from backend.flaskr.textHandler import TextHandler


class TestTextHandler(unittest.TestCase):

    EXPECTED = "français"

    def test_remove(self):
        result = TextHandler.remove_indicator(TextHandler.HEADER + self.EXPECTED + TextHandler.FOOTER)
        self.assertEqual(self.EXPECTED, result)

    def test_insert(self):
        result = TextHandler.insert_indicator(self.EXPECTED)
        self.assertEqual(TextHandler.HEADER + self.EXPECTED + TextHandler.FOOTER, result)

    def test_encode(self):
        receive_res = TextHandler.encode_msg(self.EXPECTED)
        expected_res = [
            [0, 1, 1, 0, 0, 1, 1, 0],
            [0, 1, 1, 1, 0, 0, 1, 0],
            [0, 1, 1, 0, 0, 0, 0, 1],
            [0, 1, 1, 0, 1, 1, 1, 0],
            [1, 1, 0, 0, 0, 0, 1, 1],
            [1, 0, 1, 0, 0, 1, 1, 1],
            [0, 1, 1, 0, 0, 0, 0, 1],
            [0, 1, 1, 0, 1, 0, 0, 1],
            [0, 1, 1, 1, 0, 0, 1, 1]
        ]
        self.assertEqual(expected_res, receive_res)

    def test_decode(self):
        expected_res = TextHandler.HEADER + self.EXPECTED + TextHandler.FOOTER
        received_res = TextHandler.decode_msg(TextHandler.encode_msg(expected_res))
        self.assertEqual(expected_res, received_res)
