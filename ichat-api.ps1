docker image build -t ichatapis . -f .\iChat.Api\Dockerfile
docker tag ichatapis registry.heroku.com/ichat-apis/web
docker push registry.heroku.com/ichat-apis/web
heroku container:release web -a ichat-apis
