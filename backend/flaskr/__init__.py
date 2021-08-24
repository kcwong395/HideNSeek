import os

from flask import Flask, request, url_for, redirect, render_template, jsonify, send_file, send_from_directory
from flask_cors import CORS
from flaskr import service


def create_app(test_config=None):

    # create and configure the app
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_mapping(
        SECRET_KEY='dev',
        DATABASE=os.path.join(app.instance_path, 'flaskr.sqlite'),
    )
    CORS(app)

    if test_config is None:
        # load the instance config, if it exists, when not testing
        app.config.from_pyfile('config.py', silent=True)
    else:
        # load the test config if passed in
        app.config.from_mapping(test_config)

    # ensure the instance folder exists
    try:
        os.makedirs(app.instance_path)
    except OSError:
        pass

    @app.route('/api/hide', methods=['GET', 'POST'])
    def hide():
        msg = request.form.get('message')
        img = request.files.get('image')
        s = service.Service()
        return s.hide(img, msg)


    @app.route('/api/seek', methods=['POST'])
    def seek():
        img = request.files.get('image')
        s = service.Service()
        return s.seek(img)

    return app


if __name__ == '__main__':
    app = create_app()
    app.run(host="0.0.0.0", port=5000)
