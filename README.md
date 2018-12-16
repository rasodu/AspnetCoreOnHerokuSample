### Deploy on Heroku
- Clone the project to your computer: ```git clone "git@github.com:rasodu/AspnetCoreOnHerokuSample.git"```
- Create new app in Heroku ```heroku apps:create <app-name>```
- Set "Config Vars" for the Heroku application:
  ```
  Logging__LogLevel__Default=Information
  Logging__LogLevel__Microsoft=Warning
  Logging__LogLevel__System=Warning
  TZ=UTC
  ```
- Add Heroku git remote to your project : ```heroku git:remote -a <app-name>```
- Set Heroku project to use contaier: ```heroku stack:set container```
- Push code to Heroku: ```git push heroku master```
- Open website: ```heroku open```
- #### Required files
  - DockerfileWeb
  - heroku.yml

### Enable automatic migration on deployment
- Add 'Heroku Postgres' addin to the Heroku project
- Set additional "Config Vars" for the Heroku application:
  ```
  ConnectionStrings__DefaultConnection=Host=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx;Database=xxxxxxxxxxxxxx;Username=xxxxxxxxxxxxxx;Password=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx;Integrated Security=true;Pooling=true;SslMode=Require;Trust Server Certificate=true;
  DefaultConnectionAutoMigrate=Enabled
  ```
- #### Required files
  - scripts/release-phase.sh

### Enable partial CI
- #### Required files
  - scripts/test.sh

### Setting up Log monitoring
- Default Heroku [Logplex](https://devcenter.heroku.com/articles/logplex) only saves last 1500 entries.
- Enable [Logentries](https://elements.heroku.com/addons/logentries) add-in over Logplex to save log for logger time and to have web UI to see/search logs.

### Background tasks
- Background tasks should be implemented using hosted services.
- A single Asp.net Core web host process should be able to utilize all CPU cores if all actions are async.

### Faq
- Which files do I need to copy to my project?
  - Copy 'DockerfileWebapp' and 'heroku.yml' to root of your project directory
- How do I build image locally?
  ```
  docker build -f DockerfileWeb -t web .
  ```
- How can I run locally built image?
  ```
  docker run --rm -it --expose=5000 -p=5000:5000 -e="PORT=5000" -e="ASPNETCORE_ENVIRONMENT=Development" --net="host" web /bin/bash -c "cd src/Webapp/bin/Release/netcoreapp2.1/publish/ && ASPNETCORE_URLS=http://*:$PORT dotnet Webapp.dll"
  ```
