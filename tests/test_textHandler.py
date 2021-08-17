import unittest
from flaskr import textHandler


class TestTextHandler(unittest.TestCase):
    def test_encode(self):
        h = textHandler.TextHandler()
        receive_res = h.encodeMsg("français")
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
        self.assertEqual(receive_res, expected_res)

    def test_decode(self):
        h = textHandler.TextHandler()
        received_res = h.decodeMsg(h.encodeMsg("français"))
        expected_res = "français"
        self.assertEqual(received_res, expected_res)
