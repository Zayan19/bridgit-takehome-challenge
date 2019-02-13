# Bridgit Take-Home C# Coding Challenge Base

Hello there!  
If you're looking at this repo there's a _very_ good chance you're taking one of our take-home coding challenge. If so, thanks again for your time & interest! 

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
HTTP/1.1 200 OK
Date: Wed, 13 Feb 2019 17:39:49 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

[]
```

👍 **If you're here, you're ready to start!**

## Potential Build Issues
Depending on the editor/IDE you choose you may get a warning about the netstandard version. You can safely add a reference to the AspNetCore.App dependency in the csproj file to resolve it. Note that this will introduce warnings in the 'dotnet build' output.
```
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.App" Version="2.2.2"/>
  </ItemGroup>
```


## One Last Thing
Don't forget to read the challenge document thoroughly and send an email to the recipient specififed with your planned submission deadline. If you forget to do this we may assume you've elected to drop out of the hiring process!