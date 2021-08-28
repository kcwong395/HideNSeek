import unittest
from flaskr.textHandler import TextHandler


class TestTextHandler(unittest.TestCase):
    def test_remove(self):
        self.assertEqual("français", TextHandler.remove_indicator("<STR>français<END>"))

    def test_insert(self):
        self.assertEqual("<STR>français<END>", TextHandler.insert_indicator("français"))

    def test_encode(self):
        receive_res = TextHandler.encode_msg("français")
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
        received_res = TextHandler.decode_msg(TextHandler.encode_msg("<STR>français<END>"))
        expected_res = "<STR>français<END>"
        self.assertEqual(expected_res, received_res)
