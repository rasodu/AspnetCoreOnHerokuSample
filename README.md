### Deploy on Heroku
- Clone the project to your computer: ```git clone "git@github.com:rasodu/AspnetCoreOnHerokuSample.git"```
- Create new app in Heroku
- Add Heroku git remote to your project : ```heroku git:remote -a aspnet-core-on-heroku-sample```
- Set Heroku project to use contaier: ```heroku stack:set container```
- Push code to Heroku: ```git push heroku master```

### Faq
- Which files do I need to copy to my project?
  - Copy 'DockerfileWebapp' and 'heroku.yml' to root of your project directory
- How do I build image locally?
  ```
  docker build -f DockerfileWeb -t web .
  ```
- How can I run locally built image?
  ```
  docker run --rm -it --expose=5000 -p=5000:5000 -e="PORT=5000" --net="host" web
  ```
