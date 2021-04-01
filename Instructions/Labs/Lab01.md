# Lab: Building the webApi application and run it as the Docker container locally
# Student lab manual

## Lab scenario

You're a developer and should implement Notifier API which sends the notifications to the customers and run it as a docker container.

## Lab setup

-   Estimated time: **1 hour**

## Instructions

### Before you start

#### Review the installed applications

 
-   Postman (or another API client)
-   Docker Desktop
-   Visual Studio

    
### Exercise 1: Create an image of Notifier API

#### Task 1: Create Dockerfile 
1. Open the **Notifier** project in VS (~\Allfiles\Labs\01\Notifier) and run it.
2. Create a ```.dockerignore``` file in your project folder and exclude files that shouldn't be copied into the container:
```
Dockerfile
[b|B]in
[O|o]bj
```
3. Create a ```Dockerfile``` in your project:
- **Specify image.** For content, the first thing we need to define is an image we want to base it on. We also need to set a working directory where we want the files to end up on the container. We do that with the command FROM and WORKDIR, like so:
```
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
```
- **Copy project file.** Next, we need to copy the project file ending in ```.csproj```. Additionally, we also need to call ```dotnet restore```, to ensure we install all specified dependencies:
```
COPY ["Notifier.csproj", ""]
RUN dotnet restore "./Notifier.csproj"
```
- **Copy and Build.** Next, we need to copy our app files and build our app:
```
COPY . .
WORKDIR "/src/."
RUN dotnet build "Notifier.csproj" -c Debug -o /app/build
RUN dotnet publish "Notifier.csproj" -c Debug -o /app/publish
```
- **Build runtime image.** Here we again specify our image and our working directory:
```
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
```
There is a difference though, this time we want to copy our built files to ```/app/publish```:
```
COPY --from=build /app/publish .
```
- **Starting the app.** Finally, we add a command for how to start up our app. We do that with the command ENTRYPOINT. ENTRYPOINT takes an array that transforms into a command-line invocation with arguments:
```
ENTRYPOINT ["dotnet", "Notifier.dll"]
```

 The ```Dockerfile``` in its entirety looks like this:
 ```
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Notifier.csproj", ""]
RUN dotnet restore "./Notifier.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Notifier.csproj" -c Debug -o /app/build
RUN dotnet publish "Notifier.csproj" -c Debug -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notifier.dll"]
 ```

#### Task 2: Create Docker image

1. Start Docker Desktop application. Make sure the app is running in 'Linux containers' mode.
1. Create a docker image
   - open CMD tool
   - run command ```cd {path where Dockerfile is located}```
   - run command ```docker build -t notifier .``` to create an image (it can take a few minutes)
   - run command ```docker images``` to check the image was created

### Exercise 2: Run Notifier API as a container

#### Task 1: Run Docker image as a container

1. Run command ```docker run --rm --name notifier -p 8080:80 -d notifier:latest``` to run Docker image as a container
1. Run command ```docker container ps``` to check the container was run
1. Check via Postman that the API is working (use URI http://localhost:8080/api/notify)
As you can see the response has a status code **500 (Internal Server Error)**.
   
#### Task 2: Fix the issue
To debug the app, do next steps:
1. Go to VS, open **NotifyController.cs** file and insert breakpoint inside **Send** method.
1. Open **Debug -> Attach to Process...**
1. Choose 
   - Connection type: **Docker (Linux Container)**
   - Connection target: **notifier**
   - Attach to: **Managed (.NET Core for Unix) code**
   - Process: **dotnet**
1. Press **Attach**
1. Make a request via Postman (see Task 1, step 3)
1. Using Debugger you can catch and fix the error (just remove custom error)
1. Rebuild an image and re-run a container (before run command ```docker container stop notifier``` to stop the container)
1. Check via Postman that the API is working now

#### Task 3: Inspect logs
Additional way to find the issue is to inspect logs. In order to inspect logs, run command '**docker logs shopapi**'. In logs you can see the same error 'System.Exception: Test exception'.

### Exercise 3: Create Dockerfile using Visual Studio
Alternative way to create Dockerfile is to use the next VS feature. 
In context menu click **Add -> Docker Support...** and choose **Target OS: Linux** in 'Docker File Options' dialog.
After file is created, compare content of both files (new one and you created before).

### Exercise 4: Clean up

   Run the next commands to delete created container and images
   - ```docker container stop notifier```
   - ```docker image rm notifier```
   - ```docker image prune -f```
