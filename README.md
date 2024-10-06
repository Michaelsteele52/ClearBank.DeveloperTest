### Implementation Comments
I made the assumption that the business logic should not change. Which is why I started with the end to end tests. Ensuring I captured the starting behaviour.

Features I've used:
- Strategy Pattern
- Factory Pattern
- Activity tags for logging
- Data annotation for request validation

There are other factors I would like to consider if the scope was bigger, and extended beyond the payment service.
- error handling inside the repository, caught by a middleware with more useful messaging back to users.
- Having the factory situated closer to the DI and configuration and not in the service itself.
- Request validation before the service, decoupling that concern from within the service itself.
- The current handling of an invalid request is inadequate due to the limitation of the interface provided.
- Idempotency should play a role, could the request be expanded to handle duplicate requests, or do we implement something with datetime field + account number.

### Testing Notes
Running the tests
`dotnet test`

#### Unit tests
The tests are fairly standard, scoped to what they are testing with Mocks for dependencies.

#### E2E tests
I included a PaymentServiceE2ETests file, which gives you the opportunity to see the payment service working with all the strategies implemented.