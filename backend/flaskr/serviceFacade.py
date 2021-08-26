import io

from PIL import Image
from flask import Response, jsonify, make_response
from werkzeug.datastructures import FileStorage

from imgHandler import ImgHandler
from textHandler import TextHandler


class ServiceFacade:
    @staticmethod
    def hide(img: FileStorage, msg: str) -> Response:
        msg_with_indicator = TextHandler.insert_indicator(msg)
        msg_byte = TextHandler.encode_msg(msg_with_indicator)

        image = Image.open(img)
        image_with_msg = ImgHandler.embed_msg(image, msg_byte)
        image_data = io.BytesIO()
        # save the alter image, must be in png format to prevent data lose
        image_with_msg.save(image_data, format='png')
        # config the response
        response = make_response()
        response.headers.set('Content-Type', 'image/png')
        response.data = image_data.getvalue()
        return response

    @staticmethod
    def seek(img: FileStorage) -> Response:
        image = Image.open(img)
        msg_byte_arr = ImgHandler.extract_msg(image)
        msg_with_indicator = TextHandler.decode_msg(msg_byte_arr)
        msg = TextHandler.remove_indicator(msg_with_indicator)
        return jsonify(message=msg)


