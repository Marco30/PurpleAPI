# PurpleAPI
Making a API that create document on specific event 
This is an example API built with .NET 6 that provides a single test endpoint.

# Getting started

To get started with the API, follow these steps:

    Clone the repository:

    bash

git clone https://github.com/Marco30/PurpleAPI.git

Open the solution in Visual Studio or your preferred IDE.

Build the solution to restore the NuGet packages:

dotnet build

# Run Code

Start your docker

Run this the command i the console "docker-compose up" 

Will create an image that runs RabbitMQ, it's is a messaging broker

To run the API then we need to have it running in docker. 

When RabbitMQ is up and running

Go to the project and open it in Visual Studio 

Run the API in Visual Studio 

It should now be running and accessible at https://localhost:44360/swagger/index.html.

# DB
I use JSON files as DB in order to do a quick prototype.

# Endpoint

The API provides a single test endpoint at /User/StartTest that returns a simple JSON response:

bash

GET https://localhost:44360/User/StartTest

Response:

json

{
  "customerNumber": "67890",
  "documentNumber": "0ae0e0c6-5003-42e1-a7ea-67bdea11b5c6",
  "documentText": "This is the first document for customer 67890"
}


