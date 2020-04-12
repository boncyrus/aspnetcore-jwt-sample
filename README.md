
# ASP.NET Core API JWT Authentication and Refresh Token

This is a sample ASP.NET CORE API that uses JWT Bearer Authentication and Refresh Token feature.

## Build the API
`dotnet build`

## Run the API
`dotnet run`

A browser will open and will present you the swagger docs for you to check the endpoints.

## Endpoints
The API has the following controllers:
- **AuthController** - Contains the core authentication endpoints such as **SignIn**, **SignOut**, **SignUp** and **RefreshToken**
 
- **SecuredController** - This is just an extra controller that has a **GET** method and returns the list of users. You'll need to pass the token that you get from the AuthController to be able to access this resource.
