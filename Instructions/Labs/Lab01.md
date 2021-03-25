# Lab: Building the webApi applications and run them as the Docker containers locally
# Student lab manual


## Lab setup

-   Estimated time: **?? hour ?? minutes**

## Instructions

### Before you start

#### Review the installed applications

 
-   Postman (or another API client)
-   Docker Desktop
-   Visual Studio

    > **Note**: Test note with **bold**.

### Exercise 1: Build Notifier back-end API

#### Task 1: 

1. Open the Notifier project in VS (~\Allfiles\Labs\01\Notifier) and run it.
1. Test API via Postman (create POST request with URI http://localhost:55592/api/notify)
2. 








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
