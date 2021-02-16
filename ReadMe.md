
me Coding Challenge

## Installing Dotnetcore
You need to install dotnet core 3.1 if you have not already on your operating system. It can be downloaded from the following link: https://dotnet.microsoft.com/download


## Running the main application
The main application can be found inside the bridgit-takehome directory. From there simply run the following command to build and run the application:

```
dotnet run
```

## Running Unit tests
From the root directory, run the following command:
```
dotnet test tests/takehome.tests.csproj
```
## Follwup Notes 

### General Feedback

Overall, I think this challenge was well designed. A lot of the base code to get the web server and basics up and running was already provided. This meant I could focus on the tasks given instead of having to spend a lot of time during the setup phase first. However, I still found some of the tasks somewhat time consuming, especially when adding unit tests for the various functionality. Admittedly, I went over the expected 4 hours. In total it took me nearly 6 hours to complete all the tasks + add unit tests for each of them. I understand it was not expected that all tasks be completed but I felt already invested a few hours in so figured I would complete them all. 

### Assumptions made during the challenge

The task to add a search by email did not specify what the API should be called and it needed to be distinquished from the user id search as two APIs cannot have the same routing. So I just called it GetUserByEmail. It can be accessed like so: https://localhost:5001/api/users/GetUserByEmail/{email}

The task to assign only valid assignee_id fields to tasks did not mention what to do in a corner case. Let us assume user 1 is in the system and so assignee_id=1 on any task is valid. Later user 1 is deleted. What should happen to those assignee_ids? I assumed they could be left alone and only restricted adding or editing tasks with currently invalid assignee_ids based off the current existing users. But if the assignee_ids become invalid after the fact, they are left alone in my current implementation. In a production setting I would ask for clarification as the client might want those now invalid assignee_ids to be deleted. 

The other task I found that had some ambiguity was the one that asked to to decouple the DALs for the TasksController class. I decided to add a business logic layer that contained the two DALs to accomplish this, but I am not certain if that was exactly what the task was looking for. For now my business logic layer has almost no additional logic in it, it just calls the task DAL methods directly. However, I thought it made sense to at least do the assignee_id validation within this layer since it was the one component that had acces to both DALs and this particular validation seemed more fitting under business logic. 
