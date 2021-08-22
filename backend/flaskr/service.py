from PIL import Image
from flask import Response, send_file, jsonify
from werkzeug.datastructures import FileStorage

from flaskr import imgHandler
from flaskr import textHandler


class Service:
    def __init__(self):
        self.textHandler = textHandler.TextHandler()
        self.imgHandler = imgHandler.ImgHandler()

    def hide(self, img: FileStorage, msg: str) -> Response:
        msg_with_indicator = self.textHandler.insert_indicator(msg)
        msg_byte = self.textHandler.encode_msg(msg_with_indicator)

        image = Image.open(img)
        image_with_msg = self.imgHandler.embed_msg(image, msg_byte)
        # save the alter image, must be in png format to prevent data lose
        # TODO: without saving to a place
        filename = '/tmp/sheep.png'
        image_with_msg.save(filename)
        return send_file(filename, mimetype='image/png', as_attachment=True)

    def seek(self, img: FileStorage) -> Response:
        image = Image.open(img)
        msg_byte_arr = self.imgHandler.extract_msg(image)
        msg_with_indicator = self.textHandler.decode_msg(msg_byte_arr)
        msg = self.textHandler.remove_indicator(msg_with_indicator)
        return jsonify(message=msg)


