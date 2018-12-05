# idsrv.api

The **idsrv.api** solution consists of a standalone identity server and a web api server to serve clients.

## Setup

Clone the repo and ensure you have at least version `>= 2.1.2` of the .NET Core SDK installed.

#### Provision DB
Under the `tools` directory, execute the powershell script `provision_db.ps1`, otherwise navigate to `src\idsrv.server` and execute
 the following in a command prompt:

    dotnet ef database update --context ConfigurationStoreContext
    dotnet ef database update --context UserDbContext

> **NOTE:** Identity server database data will be seeded on the first application run.

### Run

Under the `tools` directory, execute the powershell scripts `run_server.ps1` and `run_api.ps1` consecutively, otherwise execute `dotnet run` 
in the following directories  (_if there are Powershell Execution Policy issues_):

 1. `src\idsrv.server`
 2. `src\idsrv.api`

They will bind to ports `5000` and `50822` respectively.

> __**NB:** These ports need to be unused for binding.__
___
## Api Server
#### Login Page
Once the servers are running, navigate to `http://localhost:50822/Login.html`. This page will challenge for credentials:
1. alice , password
2. bob , password

#### User Page
On successful login, you will be directed to the user page, where the `access_token` is used from the cookie to make a request to the user endpoint.
A table will display the following info about the user:

|Field           |Bound From        |
|----------------|------------------|
|UserId          |Api/Server/Claim  |
|FirsName        |Api               |
|LastName        |Api               |
|Serial          |Server/Claim      |

#### Buttons
* ##### Grant/Revoke Serial
  * This will send the `serial` through to the server, relayed by the Api, to be activated/deactivated. All users on this serial, will be locked out.
 
* ##### Logout
  * This will end the user's current authenticated session by clearing the cookie and redirecting back to the login page.

* ##### Delete me
  * This will send the `userId` through to the server, relayed by the Api, to delete the user's existence.

____
## (Identity) Server
No direct interaction with this server is required. 
it runs autonomously, hosting the endpoints to serve authenticated tokens, validation and custom tenant serial/user management.
## Api Requests
For testing requests directly, the below may be called from an Http Client (ie. Postman, Fiddler)
#### Requesting an auth token

    POST /api/auth/token HTTP/1.1
    Host: localhost:50822
    Content-Type: application/json
    {
    	"userName":"alice",
    	"password":"password"
    }

#### Requesting user info

    GET /api/auth/user HTTP/1.1
    Host: localhost:50822
    Authorization: Bearer [access token]
    Content-Type: application/json

