from typing import List


class TextHandler:
    def __init__(self):
        pass


    def insert_indicator(self, msg: str) -> str:
        return '<STR>' + msg + '<END>'

    def encode_msg(self, msg: str) -> List[List[int]]:
        encoded_msg = msg.encode('utf-8')
        return [[(byte >> i) & 1 for i in range(7, -1, -1)] for byte in encoded_msg]

    def decode_msg(self, byte_arr: List[List[int]]) -> str:
        byte_str = bytearray()
        for byte in byte_arr:
            tmp = 0
            for bit in byte:
                tmp = tmp * 2 + bit
            byte_str.append(tmp)
        return byte_str.decode('utf-8')
