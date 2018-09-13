### Deploy on Heroku
- Clone the project to your computer: ```git clone "git@github.com:rasodu/AspnetCoreOnHerokuSample.git"```
- Create new app in Heroku
- Add Heroku git remote to your project : ```heroku git:remote -a aspnet-core-on-heroku-sample```
- Set Heroku project to use contaier: ```heroku stack:set container```
- Push code to Heroku: ```git push heroku master```
- Scale worker dyno: ```heroku ps:scale worker1=1```
- Verify worker and web dynos are running: ```heroku ps```

### Faq
- Which files do I need to copy to my project?
  - Copy 'DockerfileWebapp' and 'heroku.yml' to root of your project directory
- How do I build image locally?
  ```
  docker build -f DockerfileWeb -t web .
  ```
- How can I run locally built image?
  ```
  docker run --rm -it --expose=5000 -p=5000:5000 -e="PORT=5000" --net="host" web /bin/bash -c "cd Webapp/bin/Release/netcoreapp2.1/ && ASPNETCORE_URLS=http://*:$PORT dotnet Webapp.dll"
  ```
- How can I run worker locally?
  ```
  docker run --rm -it web /bin/bash -c "cd Worker1/bin/Release/netcoreapp2.1/ && dotnet Worker1.dll"
  ```

**Note: Project is inspired by [Ikechi Michael](https://blog.devcenter.co/deploy-asp-net-core-2-0-apps-on-heroku-eea8efd918b6) article. This project couldn't have been possible had he not shared his knowledge.**
