# Authentication & Authorization
Nummi uses JWTs (JSON Web Tokens) as a means of checking if a user is allowed to perform a certain action. 
After logging in with the `user/login` API, the caller receives a JWT that encodes their identity and privileges.
All subsequent API calls will include that token so Nummi can check if the user has permissions to perform a certain task.

## Roles
Nummi currently defines two roles. 

| Role    | Description                                   | Claims |
|---------|-----------------------------------------------|--------|
| User    | The basic role that all users have            |        |
| Support | Elevated access meant for general maintenance |        |
| Admin   | Access to everything                          |        |

Look into Refresh Tokens, wondering if claim values are updated how will the JWT know

## Claims
| Claim | Description |
|-------|-------------|
|   |             |
|  |             |

## Policies

## Permissions
CreateDynamicState - allow users to define a strategy state with dynamic properties

Things that might depend on permissions and claims...
Different javascript libraries
Different types
Dynamic objects
Longer execution time