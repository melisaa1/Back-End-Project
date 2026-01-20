# 🎬 RateNow API

RateNow API is a **RESTful Web API** developed with **ASP.NET Core 8** as the **Final Project** for the *Backend Development* course.
The project provides a backend system where users can **track movies and TV series**, **rate them**, **write reviews**, and **manage personal watchlists** with secure authentication.

---

##  Live Deployment

The application is deployed on **Render** and is publicly accessible.

* **Base URL**
  [https://back-end-project-nhnh.onrender.com](https://back-end-project-nhnh.onrender.com)

* **Swagger UI**
  [https://back-end-project-nhnh.onrender.com/swagger](https://back-end-project-nhnh.onrender.com/swagger)

---

##  Team Members

- **Melisa**  
  Controllers , Database , Model, 

- **Rahmah**  
  Services , Middleware , DTOs

-The rest is together

---

##  Project Features

* RESTful API architecture
* User registration and login
* JWT-based authentication
* Secure token-based access to protected endpoints
* Movie and TV Series management
* Rating system for movies and series
* User reviews
* Personal watchlist management
* Entity relationships (Users, Movies, Series, Ratings, Reviews, Watchlist)
* Global exception handling middleware
* Structured logging
* Entity Framework Core (Code First)
* Database migrations
* Swagger / OpenAPI documentation
* Live cloud deployment

---

##  Technologies Used

| Technology                | Description               |
| ------------------------- | ------------------------- |
| ASP.NET Core 8            | Web API framework         |
| Entity Framework Core     | ORM                       |
| SQLite                    | Relational database       |
| JWT Bearer Authentication | Security                  |
| Swagger / OpenAPI         | API documentation         |
| Render                    | Cloud deployment platform |

---

##  Database & ORM

* **Database:** SQLite
* **ORM:** Entity Framework Core (Code First)

The database schema is created and managed using **EF Core Migrations**.
A SQLite database is used for persistence, managed via Entity Framework Core.


---

##  Authentication & Authorization

Authentication is implemented using **JWT Bearer Tokens**.

### Authentication Flow

**Register**

```http
POST /api/auth/register
```

**Login**

```http
POST /api/auth/login
```

After successful login, a JWT token is returned.

### Token Usage

The token must be included in the `Authorization` header for all protected endpoints:

```http
Authorization: Bearer <JWT_TOKEN>
```

Unauthorized requests are automatically rejected by the API.

---

##  API Endpoints (Summary)

### Authentication

* `POST /api/auth/register`
* `POST /api/auth/login`

### Movies

* `GET /api/movies`
* `GET /api/movies/{id}`
* `POST /api/movies`
* `PUT /api/movies/{id}`
* `DELETE /api/movies/{id}`

### TV Series

* `GET /api/series`
* `GET /api/series/{id}`
* `POST /api/series`
* `PUT /api/series/{id}`
* `DELETE /api/series/{id}`

### Ratings

* `GET /api/ratings`
* `POST /api/ratings`
* `PUT /api/ratings/{id}`
* `DELETE /api/ratings/{id}`

### Reviews

* `POST /api/reviews`
* `GET /api/reviews`

### Watchlist

* `GET /api/watchlist`
* `POST /api/watchlist`
* `DELETE /api/watchlist/{id}`

All endpoints are fully documented and testable via **Swagger UI**.

---

##  Configuration & Environment Management

* Configuration is managed via `appsettings.json`
* Environment-specific settings are supported
* Sensitive data such as JWT keys:

  * Are **not hardcoded**
  * Are managed via configuration settings
* Strongly-typed configuration is used where applicable

---

##  Logging & Error Handling

* Global exception handling middleware is implemented
* Proper HTTP status codes are returned
* Meaningful error messages are provided
* Logging is structured and categorized by severity level

---

##  Deployment Notes (Docker Clarification)

The application is deployed on **Render**.

* Render uses containerized infrastructure internally
* A `Dockerfile` exists **only for Render compatibility**
* Docker was **not manually configured or orchestrated**
* No `docker-compose.yml` is used
* Docker-related bonus requirements were **not implemented or claimed**

---

## Running Locally

### Prerequisites

* .NET 8 SDK

### Run the Application

```bash
git clone https://github.com/melisaa1/Back-End-Project.git
cd RateNowApi
dotnet run
```

### Swagger (Local)

```
https://localhost:<5232>/swagger

```

---

## Course Compliance

This project **fully complies with all Core Requirements** defined in the
*Backend Development – Final Project Guidelines*.

* RESTful API design
* Entity Framework Core with relationships
* JWT-based authentication
* Clean controller & service structure
* Global error handling
* Version control usage
* Live cloud deployment

No optional bonus features were claimed.
(Added Docker but it is not working)

---

## License

This project was developed for **academic purposes** as part of the Backend Development course.

---