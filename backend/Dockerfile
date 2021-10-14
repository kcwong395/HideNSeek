# syntax=docker/dockerfile:1

FROM python:3.8-slim-buster

WORKDIR /app

COPY requirements.txt requirements.txt
RUN pip3 install -r requirements.txt

COPY . .
RUN python -m unittest discover -s /app/tests -p "test_*.py" -v

ENV FLASK_APP=flaskr
ENV FLASK_ENV=production

CMD [ "flask", "run", "--host=0.0.0.0" ]

