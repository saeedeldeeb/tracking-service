# tracking-service

This is the tracking service in the off-duty school bus system. 

## Techs:

- C#, .NET Core 8
- RabbitMQ
- Docker
- Background services
- Redis
- HttpClient
- Postman Mock Server

## About:
This service is responsible for tracking the school bus and sending the location to the main service. It uses RabbitMQ to communicate with the main service. It also uses Redis to cache the location data.
It uses a background service to get the location of the bus every 5 seconds.

The main service can be found at [this link](https://github.com/saeedeldeeb/the-off-duty-school-bus).