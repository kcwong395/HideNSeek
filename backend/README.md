# HideNSeek
[![standard-readme compliant](https://img.shields.io/badge/HideNSeek--readme-OK-green.svg?style=flat-square)](https://github.com/kcwong395/HideNSeek)

A steganography tool to hide messages in an image

## Table of Contents
- [Motivation](#motivation)
- [Maintenance](#maintenance)
- [Install](#install)
- [Usage](#usage)
- [Maintainers](#maintainers)
- [Contributing](#contributing)
- [License](#license)

## Motivation

It is always difficult to transmit messages in an unsecure environment. Some suggest using Encryption while some say steganography. This application is a common type of steganographic skill which allow the user to embed a piece of message into an image. When receiving the image, another user can extract the message back.

To enhance the confidentiality, this tool combines steganography and encryption. A user can insert a key and the tool will encrypt the message before inserting the message into the targeted image.

## Maintenance

### Update requirements.txt

pip3 freeze > requirements.txt

### Build docker image with dockerfile

sudo docker build -t hns-backend:v1.0 .

## Maintainers

[@ElderHorse](https://github.com/kcwong395)

## Contributing
Feel free to open an issue if you hope to propose anything

## License
MIT © 2019 ElderHorse
