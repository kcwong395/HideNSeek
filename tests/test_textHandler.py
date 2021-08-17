import unittest
from flaskr import textHandler


class TestTextHandler(unittest.TestCase):
    def test_insert(self):
        h = textHandler.TextHandler()
        self.assertEqual("<STR>français<END>", h.insert_indicator("français"))

    def test_encode(self):
        h = textHandler.TextHandler()
        receive_res = h.encode_msg("français")
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
        h = textHandler.TextHandler()
        received_res = h.decode_msg(h.encode_msg("<STR>français<END>"))
        expected_res = "<STR>français<END>"
        self.assertEqual(expected_res, received_res)
