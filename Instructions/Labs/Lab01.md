# Lab: Building the webApi applications and run them as the Docker containers locally
# Student lab manual

## Lab scenario

You're a developer and should implement and run two APIs:
- Notifier which sends the notifications to the customers
- ShopApi which processes the orders and uses Notifier API

## Lab setup

-   Estimated time: **1 hour 15 minutes**

## Instructions

### Before you start

#### Review the installed applications

 
-   Postman (or another API client)
-   Docker Desktop
-   Visual Studio

    
### Exercise 1: Build Notifier back-end API

#### Task 1: 

1. Open the **Notifier** project in VS (~\Allfiles\Labs\01\Notifier) and run it.
1. Test API via Postman (create POST request with URI http://localhost:55592/api/notify)

#### Task 2: Create Docker image 
1. Add Dockerfile to the project: in context menu **Add -> Docker Support...** (choose **Target OS: Linux** in 'Docker File Options' dialog).
1. Open **Dockerfile** and add the instructions below
   ```EXPOSE 82```
   ```ENV ASPNETCORE_URLS=http://*:82 ```
   instead of
   ```EXPOSE 80``` (should be on line 5)
   
   > **Note**: It allows to change default port 80 to custom 82
1. Start Docker Desktop application. Make sure the app is running in 'Linux containers' mode.
1. Create a docker image
   - open CMD tool
   - run command ```cd {path where Dockerfile is located}```
   - run command ```docker build -t notifier .``` to create an image (it can take a few minutes)
   - run command ```docker images``` to check the image was created

#### Task 3: Run Docker image as a container

1. Run command ```docker run --rm --name notifier -p 8082:82 -d notifier:latest``` to run Docker image as a container
1. Run command ```docker container ps``` to check the container was run
1. Check via Postman that the API is working (use URI http://localhost:8082/api/notify)


### Exercise 2: Build ShopApi back-end API

#### Prerequisites:
-  Notifier API should be running as a Docker container (see previous exercise)

#### Task 1: 

1. Open the **ShopApi** project in VS (~\Allfiles\Labs\01\ShopApi)
1. Inspect the file **OrderController.cs**. There you can see that **Notifier** API is used into **Create** method through **HttpClient** (**appsettings.json** file contains URI for Notifier API).
1. Run the project and test it via Postman (create POST request with URI http://localhost:56085/api/order)

#### Task 2: Create Docker image 
1. Add Dockerfile to the project: in context menu **Add -> Docker Support...** (choose **Target OS: Linux** in 'Docker File Options' dialog).
1. Open **Dockerfile** and add the instructions below
   ```EXPOSE 81```
   ```ENV ASPNETCORE_URLS=http://*:81 ```
   instead of
   ```EXPOSE 80``` (should be on line 5)
   
   > **Note**: It allows to change default port 80 to custom 81

   In addition, **replace 'Release' mode with 'Debug'** in the lines with 'RUN dotnet build ...' and 'RUN dotnet publish ...' commands. It allows to debug the API later.
1. Create a docker image
   - open CMD tool
   - run command ```cd {path where Dockerfile is located}```
    > **Important:** Change a path to ShopApi project
   - run command ```docker build -t shopapi .``` to create an image
   - run command ```docker images``` to check the image was created

#### Task 3: Run Docker image as a container

1. Run command ```docker run --rm --name shopapi -p 8081:81 -d shopapi:latest``` to run Docker image as a container
1. Run command ```docker container ps``` to check the container was run (you should see both containers **notifier** and **shopapi**)
1. Check via Postman that the API is working (use URI http://localhost:8081/api/order)
  As you can see the response is 'Cannot assign requested address' with 400 status code.

#### Task 4: Fix the issue
To debug the app, do next steps:
1. Go to VS, open **OrderController.cs** file and insert breakpoint inside **Create** method.
1. Open **Debug -> Attach to Process...**
1. Choose 
   - Connection type: **Docker (Linux Container)**
   - Connection target: **shopapi**
   - Attach to: **Managed (.NET Core for Unix) code**
   - Process: **dotnet**
1. Press **Attach**
1. Make a request via Postman (see Task 3, step 3)
1. Using Debugger you can see the error 'System.Net.Http.HttpRequestException: Cannot assign requested address' was caught. It means 'Notifier' address can't be assigned because **localhost** for **shopapi** container is not the same as **localhost** of your computer.
1. To fix this issue, you can put these containers to one network
   - run commands ```docker container stop shopapi``` and ```docker container stop notifier``` to stop both containers
   - run command ```docker network create shop-net``` to create a new network named **shop-net**
   - run command ```docker run --rm --name notifier -p 8082:82 --network shop-net -d notifier:latest``` to create **notifier** container with **shop-net** network
   - run command ```docker run --rm --name shopapi -p 8081:81 --network shop-net -e "NotifierUrl=http://notifier:82/api/notify" -d shopapi:latest``` to create **shopapi** container with **shop-net** network and new **NotifierUrl** configuration parameter
3. Check via Postman that the API is working now (use URI http://localhost:8081/api/order)

> **Note:** Additional way to find the issue is to inspect logs. In order to inspect logs, run command '**docker logs shopapi**'. In logs you can see the same error 'System.Net.Http.HttpRequestException: Cannot assign requested address'.
### Exercise 7: Clean up

   Run the next commands to delete all created containers and images
   - ```docker container stop shopapi```
   - ```docker container stop notifier```
   - ```docker image rm shopapi```
   - ```docker image rm notifier```
   - ```docker image prune -f```
   - ```docker network rm shop-net```
