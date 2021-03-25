# Lab: Building the webApi applications and run them as the Docker containers locally
# Student lab manual

## Lab scenario

You're the developer and should implement and run two APIs:
- Notifier which sends the notifications to the customers
- ShopApi which processes the orders and uses Notifier API

## Lab setup

-   Estimated time: **?? hour ?? minutes**

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
   insted of
   ```EXPOSE 80``` (should be on line 5)
   
   > **Note**: It allows to change default port 80 to custom 82
1. Create a docker image
   - open CMD tool
   - run command ```cd {path where Dockerfile is located}```
   - run command ```docker build -t notifier .``` to create an image
   - run command ```docker images``` to check the image was created

#### Task 3: Run Docker image as a container

1. Run command ```docker run --rm --name notifier -p 8082:82 -d notifier:latest``` to run Docker image as a container
1. Run command ```docker container ps``` to check the container was run
1. Check via Postman that the API is working (use URI http://localhost:8082/api/notify)


### Exercise 2: Build ShopApi back-end API

#### Prerequisites:
-  Notifier API should be running as a Docker container (see previous exercise)

#### Task 1: 

1. Open the **ShopApi** project in VS (~\Allfiles\Labs\01\ShopApi
1. Inspect the file **OrderController.cs**. There you can see that **Notifier** API is used into **Create** method through **HttpClient** (**appsettings.json** file contains URI for Notifier API).
1. Run the project and test it via Postman (create POST request with URI http://localhost:56085/api/order)

#### Task 2: Create Docker image 
1. Add Dockerfile to the project: in context menu **Add -> Docker Support...** (choose **Target OS: Linux** in 'Docker File Options' dialog).
1. Open **Dockerfile** and add the instructions below
   ```EXPOSE 82```
   ```ENV ASPNETCORE_URLS=http://*:82 ```
   insted of
   ```EXPOSE 80``` (should be on line 5)
   
   > **Note**: It allows to change default port 80 to custom 82
1. Create a docker image
   - open CMD tool
   - run command ```cd {path where Dockerfile is located}```
   - run command ```docker build -t notifier .``` to create an image
   - run command ```docker images``` to check the image was created

#### Task 3: Run Docker image as a container

1. Run command ```docker run --rm --name notifier -p 8082:82 -d notifier:latest``` to run Docker image as a container
1. Run command ```docker container ps``` to check the container was run
1. Check via Postman that the API is working (use URI http://localhost:8082/api/notify)

### Exercise 7: Clean up

#### Task 1: Open Azure Cloud Shell

1.  In the Azure portal, select the **Cloud Shell** icon to open a new shell instance.

1.  If Cloud Shell isn't already configured, configure the shell for Bash by using the default settings.

#### Task 2: Delete resource groups

1.  Enter the following command, and then select Enter to delete the **ManagedPlatform** resource group:

    ```
    az group delete --name ManagedPlatform --no-wait --yes
    ```

1.  Close the Cloud Shell pane in the portal.

#### Task 3: Close the active applications


#### Review

In this exercise, you cleaned up your subscription by removing the resource groups used in this lab.
