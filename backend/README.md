## Maintenance

### install required package

pip install -r backend/requirements.txt

### Update requirements.txt

pip3 freeze > backend/requirements.txt

### Build docker image with dockerfile

sudo docker build -t hns-backend:v1.0 .