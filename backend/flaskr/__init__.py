import os

from flask import Flask, request, url_for, redirect, render_template
from werkzeug.utils import secure_filename
from flaskr import fileHandler


def create_app(test_config=None):

    UPLOAD_FOLDER = '/tmp'

    # create and configure the app
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_mapping(
        SECRET_KEY='dev',
        DATABASE=os.path.join(app.instance_path, 'flaskr.sqlite'),
        UPLOAD_FOLDER=UPLOAD_FOLDER,
        MAX_CONTENT_LENGTH=16 * 1024 * 1024 # 16mb
    )

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

    @app.route('/', methods=['GET', 'POST'])
    def upload_file():
        h = fileHandler.FileHandler()
        if request.method == 'POST':
            file = request.files['file']
            if file and h.allowed_file(file.filename):
                filename = secure_filename(file.filename)
                file.save(os.path.join(app.config['UPLOAD_FOLDER'], filename))
                # return redirect(url_for('uploaded_file', filename=filename))
        return render_template('home.html')

    @app.route('/hello')
    def hello():
        return 'Hello, World!'

    return app