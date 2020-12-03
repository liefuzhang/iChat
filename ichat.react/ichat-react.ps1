docker image build -t ichatreact . -f .\Dockerfile.prod
docker tag ichatreact registry.heroku.com/ichat-react/web
docker push registry.heroku.com/ichat-react/web
heroku container:release web -a ichat-react
