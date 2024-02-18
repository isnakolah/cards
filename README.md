# Cards API Documentation

## Introduction
The Cards API allows users to create and manage tasks in the form of cards. This documentation covers the setup, configuration, and usage of the Cards API.

## Requirements
- .NET Core 8.0 SDK
- MSSQL Server/In Memory Database
- Docker (for containerization)

## Setup
1. **Clone the repository**:
```git clone [repository_url]```
2. **Navigate to the project directory**:
```cd CardsAPI```
3. **Restore dependencies**:
```dotnet restore```
4. **Apply database migrations**: 
```dotnet ef database update```

## Configuration

1. **AppSettings**:
   Update the `appsettings.json` file with the necessary configurations including database connection string, JWT settings, and any other required settings.

2. **JWT Settings**:
```json
"JwtOptions": {
  "Key": "your_jwt_key_here",
  "Issuer": "your_issuer_here"
}
```

## Build and Run
1. **Build the project**:
```dotnet build```
2. **Run the project**:
```dotnet run --project Cards.Api```
3. **Using Docker**:
- Build the Docker image:
```docker build -t cards-api .```
- Run the Docker container:
```docker run -d -p 8080:8080 -p 8081:8081 --name cards_api cardsapi```
- Usage
-----

1.  **Seed Users**: Send a `POST` request to `/auth/seed-users` with the user's email, password, and role.
2.  **Login**: Send a `POST` request to `/auth/login` with the user's email and password to receive a JWT token. Below is the default users:
>**Admin**: Email: ```admin@cards.com``` Password: ```StrongAdminPassword123```

>**Member 1**: Email: ```john.doe@cards.com``` Password: ```StrongMemberOnePassword123```

> **Member 2**: Email: ```jane.doe@cards.com``` Password: ```StrongMemberTwoPassword123```

3.  **Create a Card**: Send a `POST` request to `/cards` with the JWT token in the Authorization header and card details in the body.

4.  **Search Cards**: Send a `GET` request to `/cards` with search parameters and the JWT token in the Authorization header.

5.  **Update a Card**: Send a `PUT` request to `/cards/{id}` with the JWT token and updated card details in the body.

6.  **Delete a Card**: Send a `DELETE` request to `/cards/{id}` with the JWT token.


API Endpoints
-------------

*   `POST /auth/seed`: Seeds default users.
*   `POST /auth/login`: Authenticate a user and return a token.
*   `POST /cards`: Create a new card.
*   `GET /cards`: Retrieve accessible cards with optional search parameters.
*   `PUT /cards/{id}`: Update a card's details.
*   `DELETE /cards/{id}`: Delete a card.

Authorization
-------------

The Cards API uses role-based authorization. There are two roles: Member and Admin.

*   **Members** can only access cards they created.
*   **Admins** have access to all cards.

JWT tokens are used to manage sessions and authorization. Ensure the token is included in the `Authorization` header as a Bearer token for authenticated requests.

Limitations
-------------
- Test coverage is not comprehensive.