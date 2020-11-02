# How to

## 1. How to run
### dotnetn --project ./SM.API/SM.API.csproj --configuration Release
or
### docker
docker build -t aspnetapp .
docker run -it --rm -p 5000:80 --name aspnetcore_sample aspnetapp

## 2. Go to Swagger page http://localhost:5000/api

## 3. Authentication
### 3.1. Get JWT token
API - /api/Account/authenticate

there is a predefined user:
{
  "email": "admin@email",
  "password": "Admin1!"
}
### 3.2. Authorize in Swagger by using jwt token gotten in the 3.1
template - Bearer jwt_token

## 4. Check CRUD operations Groups and Students
