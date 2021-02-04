# Bridgit Take-Home C# Coding Challenge Base

Hello there!  
If you're looking at this repo there's a _very_ good chance you're taking one of our take-home coding challenge. If so, thanks again for your time & interest! 

## Challenge Details
All you need to know about this challenge and how to submit it when complete can be found in  [this pdf file](Take-Home-Instructions.pdf). The actual challenge you'll be tasked with will be provided by the Bridgit recruiter you're in contact with. Please read both documents thoroughly before you start!

## Readiness Check
Before you start, be sure to read through the PDF challenge document you received. There are a few different versions of the challenge which is why the text isn't included here.
### Test Build
If you want to check and see if you're ready to start you should be able to build this project in the root of the repo by running:
```
dotnet build
```
### Test Run
If the build completed successfully you should be able to start the project with: 
```
dotnet run
```
If that worked, a GET request to https://localhost:5001/api/users should return a 200 response with an empty JSON array.  
An example using curl would be:
```
curl --insecure -i https://localhost:5001/api/users
```
And the result should look like this:
```
HTTP/2 200 
date: Thu, 04 Feb 2021 15:35:09 GMT
content-type: application/json; charset=utf-8
server: Kestrel

[]
```

👍 **If you're here, you're ready to start!**

## Postman Collection
To simplify testing of the API there is a basic Postman collection available in this repo [here](/Postman/Tasklify.postman_collection.json)

## One Last Thing
Don't forget to read the challenge document thoroughly and send an email to the recipient specififed with your planned submission deadline. If you forget to do this we may assume you've elected to drop out of the hiring process!