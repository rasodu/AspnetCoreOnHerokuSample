### Goals
- Reasons
  - Product owner, working on a new idea, want to deploy their application quickly. They want to validate their product.
  - Developing a solution that scales vertically allows developers to keep code simple.
  - Development time for software that will be scaled vertically is less compare to the development time for the software that scales horizontally.
  - Infrastructure need to deploy vertical application is less complicated compared to application that is design with horizontal scaling in mind.
  - Asp.net Core can handle tens of thousands requests per seconds with vertical scaling. Here is the [bechmark](https://www.techempower.com/benchmarks/) and [article](https://www.techempower.com/blog/2016/11/16/framework-benchmarks-round-13/). If you move your application to a large machine on a public cloud, then it can handle thousands to customers without you needing to scale horizontally for a long time.
- Strategy we will follow
  - Vertical scaling over horizontal scaling.(If application is successful, then developer can spend time fixing scaling problem.)
  - Deployment ease should be pay a role when choosing an implementation for a feature.

### Stories
- Should enable Docker support in visual studio project. So, the developers will not need to download .NET SDK to development on their machine.
  - **Will not be implemented yet.** Looks like Visual Studio support for Docker is in primitive stage. Currently, there is no way to run and debug unit tests in Docker container. More, [here](https://techblog.dorogin.com/running-and-debugging-net-core-unit-tests-inside-docker-containers-48476eda2d2a) and [here](https://github.com/Microsoft/DockerTools/issues/77)
  - Development SQL server should be started on Docker when project starts.
  - Development SQL server should be migrated on project startup.
  - Development SQL server should be seeded on project startup.
- Allow developers to use this project by simply copying files from this project to their project.
  - We have to allow ```webapp``` project name to be customized in docker file. We can implement this using ARG and ENV in docker file.
  - Stages that we want to have
    - Minimum for deployment: ```DockerfileWebapp``` and ```Heroku.yml```
    - Release phase and automatic migrations: ```release_phase.sh```
    - Partial CI - test code before deplyment: ```tests.sh``` 
- Look into a way to monitor application server's resource usage.(RAM, bandwidth, CPU usage)
  - Look into possibility to enable new Relic for Heroku. More [here](http://blog.avenuecode.com/tricks-for-configuring-new-relic-for-.net-core) and [here](https://docs.newrelic.com/docs/agents/net-agent/installation/new-relic-net-agent-install-introduction#common-installs)
  - Let developer know when it is time to scale fleet of dynos.
  - Let developer know about inefficient parts of their application.
- Look into a way to monitor resources used by DB server.
- Add files that list all tools needed when using the project and when developing the project.
- This about creating a NuGet package that will add the functionalities to existing project.
- Add steps to upgrade version of .NET
- Add steps to change project name. 
- Analyze how Heroku's [build limits](https://devcenter.heroku.com/articles/limits#build) can potentially effect our project.
- Make a note of Heroku's [load testing](https://devcenter.heroku.com/articles/load-testing-guidelines#common-runtime-limits) limits.
- How to async lock. One of the way is to use [Nito.AsyncEx](https://www.nuget.org/packages/Nito.AsyncEx/).

### Implemented
- Deploy on Heroku. Below, we are documenting the reasons to choose Heroku over other solutions(Kubernetes or other containerized workload management technology.)
  - The aim of the project is to provide fastest way to deploy the project. Heroku's services free developers from setting up a lot of things that they need to run production level web application. 
  - Production website should use SSL certificate. Heroku manages SSL. There is a lot of work involved in using and managing SSL certificates.
  - You will also need load balancer. Heroku takes care of this too.
  - Logging works out of the box. You would need a way to aggregate logs from multiple containers in Docker solutions.
  - Pipeline is another advantage. You can quickly setup test app. Without Heroku you need to setup mirror of production infrastructure for test releases.(Though, at this point Heroku doesn't allow pipeline upgrades for Docker projects)
  - Heroku also provide quick way to put website in maintenance mode. With other approaches you need to configure a way to do something similar.
  - Heroku push to deploy works out of the box. With Kubernetes would need to setup something separate to enable this. CD is implemented right into Heroku.
  - You would need to maintain the cluster with Kubernetes or other containerized workload management technology.
- Deploy using docker image.
  - There is a possibility that a build pack could fail. Docker provides very reliable way to run projects in production.
  - If it works on developer's machine, then it will always work on production too. Docker eliminate chance of missing production dependency during deployment.
  - Developers can easily upgrade .NET version by switching the version number in dockerfile. Developer can utilize latest technology without relying on developer of build pack to release the upgrade.
  - Developers can customize additional software on the image running your app. Docker provides reliable way to install additional software on the OS level using Dockerfile. Developers are free to install additional packages on the OS. This makes infrastructure customizable by developers.
- Upgrade policy. Our upgrade policy will follow [dotnet](https://dotnet.microsoft.com/platform/support-policy) upgrade policy.
  - We will only upgrade to Asp.net Core LTS version. We will ignore current versions. Though, we will add new features that we want to add to product in backlog based on the new features introduced in current version. But we will not upgrade to current version because features added in the release are usually unstable. Implementation of new feature can change from current to LTS version release.
- Create initial implementation of the project based on Ikechi Michael's [article](https://blog.devcenter.co/deploy-asp-net-core-2-0-apps-on-heroku-eea8efd918b6) to deploy Asp.net Core app with docker on Heroku.
- We are naming Dockerfile for the project "DockerfileWeb" instead of "Dockerfile.Web". "Dockerfile.Web" is Heroku naming convention. Command [heroku container:push --recursive](https://devcenter.heroku.com/articles/container-registry-and-runtime#pushing-multiple-images) will push all dockerfile in current directory that follow convention. We don't want these images to be pushed incase user typed the command by mistake. Deploying this way will not run release tasks. Naming file this way will make sure that the images are not pushed by mistake.  
- Enable CD on git push for the project.
  - Heroku's built in functionality to create new deployment seems most strait forward. Looks like the will be work for long time without breaking and is very easy to get started with.
  - We had looked at doing CD by pushing image from Azure DevOps CD pipeline. But we decided to not take that route. The primary reason was that it required configuration done using the UI tool. There doesn't seem to be way to put those steps in config file and committing the configuration to repository, like you can do with Azure DevOps CI.
- Add a way to easily add release tasks.
- DB migration on new release. We will use PostgreSQL as default DB.
  - Options
    - Code first
      - How to [seed](https://www.learnentityframeworkcore.com/migrations/seeding) database?
      - What are [snapshot](https://channel9.msdn.com/Blogs/EF/Migrations-Under-the-Hood) files?
      - How to resolve code [conflict](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/teams#merging)?
      - Should you use data annotation on model fluent API?
      - How to check snapshot file is in good condition when upgrading database package? - Run ```Add-migrations <name> -IgnoreChanges``` and make sure there are no changes in snapshot file.
    - DB first
    - Code first from existing DB
      - How to do this [here](https://channel9.msdn.com/blogs/ef/migrations-existing-databases) and [here](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/existing-database)?
    - SSDT project for database
      - This project types can not be compiled without MsBuild.
      - So if we want to use this we would need to change deployment strategy.
      - Since we have to use DB first approach with this method, it will also make code complicated.
  - Implementation
    - User need to explicitly specify if they need to run migration. This way app don't run migrations on release if developer want to run them manually. 
    - User's can easily switch DB by changing a single package and rerunning migrations.
    - Most app would need a way to persist data. PostgreSQL is supported and maintained by Heroku. Otherwise developers would need to setup and maintain DB solution somewhere else. It would be a time consuming task. We want to allow developer to get their app quickly off the ground.
- Add implementation for background tasks. There are two ways to implement background tasks. Advantages and disadvantages for each technique are discussed below. We want to provide developers with fastest and easiest way to deploy the initial implementation of their ideas. You can easily move to more advanced approach once you validate you idea and have more resources.
  1. Background tasks will be run in a web dyno. We have opted for this approach. [More info](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2)
     - If the dyno doesn't receive traffic for 30 minutes, then the dyno will goto sleep even though the background process is running.
  1. Worker dyno to run background tasks in separate process.
     - Disadvantages
       - Currently there is no way to run only background tasks in the web project. Or to instruct project to start only web host.
       - So, we have to add a new project and create separate assembly. We will also have to create service container in both the projects. This will create complexity and it will take more time to develop.
     - Advantages
       - This will allow logs for web and background process to stay isolated.
       - Failure of one thread will not effect the other threads.
       - You can scale web and worker independently.
- Provide steps to perform logging.
  - There are a lot of third party addins. Currently, we are looking into implementing this by enabling [Logentries](https://elements.heroku.com/addons/logentries) addin over [Logplex](https://devcenter.heroku.com/articles/logplex).
- Implement CI. This will allow us to test code. We are not implementing full CI. Our implementation and impediment to implementing full CI are listed below.
  - Two technical ways to implement this. They are discussed in Microsoft [documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines/languages/docker?view=vsts&tabs=yaml#integrate-build-and-test-tasks). We are opting for Docker approach.
    1. Test in the build pipeline
       - With this approach the tool will be intelligently gather data from test run and create reports.
       - But you may have to customize given environment with additional tools to have your code build and tested.
    1. Test in docker
       - You are already building the image for deployment. So not much effort is needed to setup the environment.
       - Pipeline tool will not be able to pull test result.
  - Types of CI. To keep the setup simple. We have decided to not do full CI at this point.
    1. Full CI. This is what it means when we call CI.
       - Each pull request will run all unit tests and show their status on pull request.
       - There is no easy way to enable full CI. You need to purchase Heroku CI or user service like Azure DevOps which provide free CI time.
       - We have decided to not enable full CI at this point. It will make setting up project more complicated. And we want to stick with Heroku.
       - Developer can easy enable full CI support by making minor modifications.
    1. Run test before deployment.
       - This will make sure test are run successfully before each deployment. But it will not give developer feedback right when they are writing their code. In case tests fail, then developer may have to go back to the code that they have written earlier and fix the tests.
       - If a unit test is failing, then developer will be unable to deploy the code.
       - This approach will provide a runway that developers will be able to use to setup full CI.
