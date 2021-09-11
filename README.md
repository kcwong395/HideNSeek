# HideNSeek
[![standard-readme compliant](https://img.shields.io/badge/HideNSeek--readme-OK-green.svg?style=flat-square)](https://github.com/kcwong395/HideNSeek)

A steganography tool to hide messages in an image

## Table of Contents
- [HideNSeek](#hidenseek)
  - [Table of Contents](#table-of-contents)
  - [Motivation](#motivation)
  - [Features](#features)
  - [Usage](#usage)
  - [Future Works:](#future-works)
  - [Maintainers](#maintainers)
  - [Contributing](#contributing)
  - [License](#license)

## Motivation

It is always difficult to transmit messages in an unsecure environment. Some suggest using Encryption while some say steganography. This application is a common type of steganographic skill which allow the user to embed a piece of message into an image. When receiving the image, another user can extract the message back.

TODO: To enhance the confidentiality, this tool combines steganography and encryption. A user can insert a key and the tool will encrypt the message before inserting the message into the targeted image.

## Features

- Confidentiality: all uploaded images will be processed in memory and will never be saved in storage
- Ease of Access: the service is deployed on a cloud platform and no installation required
- Free of Charge: you do not need to pay any fee to use this service

## Usage

<video width="100%" height="100%" controls>
    <source src="frontend/public/HideNSeek-Demo.mkv" type="video/mp4" />
    <source src="frontend/public/HideNSeek-Demo.webm" type="video/webm" />
    Your browser does not support the video tag.
</video> 

## Future Works:

- Add encryption features
- Add algorithms to determine next insertion point

## Maintainers

[@ElderHorse](https://github.com/kcwong395)

## Contributing
Feel free to open an issue if you hope to propose anything

## License
MIT © 2019 ElderHorse
