# Payment Gateway Code

This API Gateway is responsible for creating, voiding, capturing and refunding payments

## Assumptions

Assuming total capture value can't be more than original total

Assuming we want to capture different captures and refunds for use in the future

Assuming a payment date to complete the transaction

Assuming we only want to refund the transactions we have captured. Not total value

## How to run

To run locally you can run the docker-compose up line on the docker-compose.debug.yml file

docker-compose -f "docker-compose.debug.yml" up -d --build 

## Still to do

1) Add unit test coverage
2) Improve Logging
3) Move Validation out of service
4) Update Readme
5) Add authentication
6) Move connection string to appsettings