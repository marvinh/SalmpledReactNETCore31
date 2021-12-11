#!/bin/bash 
heroku container:login
docker build -t salmpled-backend . 
heroku container:push -a salmpled-backend web 
heroku container:release -a salmpled-backend web

