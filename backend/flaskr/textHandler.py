from typing import List, Optional, Final


class TextHandler:

    _HEADER: Final = '<STR>'
    _FOOTER: Final = '<END>'

    @staticmethod
    def remove_indicator(msg: str) -> Optional[str]:
        if not msg.startswith(TextHandler._HEADER) or not msg.endswith(TextHandler._FOOTER):
            return None
        else:
            return msg[len(TextHandler._HEADER):len(TextHandler._FOOTER) * -1]

    @staticmethod
    def insert_indicator(msg: str) -> str:
        return TextHandler._HEADER + msg + TextHandler._FOOTER

    @staticmethod
    def encode_msg(msg: str) -> List[List[int]]:
        encoded_msg = msg.encode('utf-8')
        return [[(byte >> i) & 1 for i in range(7, -1, -1)] for byte in encoded_msg]

    @staticmethod
    def decode_msg(byte_arr: List[List[int]]) -> str:
        byte_str = bytearray()
        for byte in byte_arr:
            tmp = 0
            for bit in byte:
                tmp = tmp * 2 + bit
            byte_str.append(tmp)
        return byte_str.decode('utf-8')
