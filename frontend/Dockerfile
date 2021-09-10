FROM node:16.6.0-alpine3.14 as build

WORKDIR /app

COPY package.json ./

RUN npm install

COPY . .

RUN npm run build

FROM nginx:stable-alpine

# This fix react-route issue, see: https://stackoverflow.com/questions/43555282/react-js-application-showing-404-not-found-in-nginx-server/61753597#61753597
COPY nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 80

CMD [ "nginx", "-g", "daemon off;" ]