# entravel-tech-task

## Quick Start
To run all services pull repo and run next command
```bash
# Start all services (PostgreSql, RabbitMQ, Prometheus, OrderProcessing API Service, OrderProcessing Worker Service)
docker-compose up -d
```
## Architecture
Solutions implemented with microservices arhitecture wtih 2 services:
- API - handles http requests to POST, GET orders
- Worker - asynchronously process orders to unload API

Communitcation between services implemented according Event-driven arhitecture with RabbitMQ as message broker.
Message broker was preferred over caching because of assumption for scaling and more flexible extension (SAGA implementation).

## Services description
### API
API has a seeded user - b0f0d377-ee86-4bd3-a45c-4f05237407ce. All other customer Id cause a 500 error. ProductId - can be any just GUID format.
```bash
Available by http://localhost:8080
Swagger: http://localhost:8080/swagger
POST request body example:
{
    "customerId": "b0f0d377-ee86-4bd3-a45c-4f05237407ce",
    "totalAmount": 501,
    "items": [
        {
            "ProductId": "2ed0c1e1-da21-468d-8770-36ec0213c86b",
            "PricePerItem": 10,
            "Quantity": 768
        },
        {
            "ProductId": "83f3eddf-0e2d-4ac9-a657-71aea689fda3",
            "PricePerItem": 1.50,
            "Quantity": 4
        }
    ]
}
```
### Worker
Handles ProcessOrderMessages.Marks orders as processed and simulates some kind of discount calculation. If order TotalAmout > 500 applies discount 10. If order processed increases log counter for processed logs.
### Prometheus
Used for simple observability of processed orders
```bash
Available by http://localhost:9090
Processed orders amount is stored under 'processed_order_amount' key
```



