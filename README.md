# 🛒 E-Commerce Backend – ASP.NET Core API

This is the backend for an E-Commerce web application built using **ASP.NET Core 8**, following **Clean Architecture** principles with scalable design patterns and secure payment integration.  
The backend is responsible for **product management**, **shopping basket handling**, **orders processing**, **payment integration**, and **user authentication/authorization**.

---

## 🚀 Tech Stack
- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (Code-First Migrations)
- **SQL Server** (Main Database)
- **Redis** (Basket Caching)
- **Stripe API** (Payments)
- **JWT Authentication**
- **AutoMapper**
- **Swagger / OpenAPI**
- **Specification Pattern**
- **Repository & Unit of Work Pattern**

---

## 📂 Project Structure

```
ECommerce.sln                     # Solution file

├── API/                           # Presentation Layer
│   ├── Controllers/               # API endpoints
│   ├── Extensions/                # Service & middleware extensions
│   ├── Middleware/                 # Global error handling, logging
│   ├── Helpers/                    # Pagination, filtering helpers
│   └── Program.cs                  # App startup

├── Core/                          # Domain Layer
│   ├── Entities/                   # Domain models (Product, Order, Basket, etc.)
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Interfaces/                 # Abstractions for repositories & services
│   └── Specifications/             # Query filtering logic

├── Infrastructure/                # Data Access Layer
│   ├── Data/                        # EF Core DbContext, Migrations
│   ├── Identity/                    # ASP.NET Core Identity setup
│   ├── Repositories/                # Repository implementations
│   └── Seed/                        # Initial database seeding

```



---

## ✨ Features
- **Authentication & Authorization**  
  Registration/Login with ASP.NET Identity, JWT-based authentication, email verification, password reset.
- **Product Management**  
  List products with filtering, sorting, and pagination; get product details; seed initial data.
- **Basket (Shopping Cart)**  
  Add, update, remove items; store basket in Redis; auto-create basket for new users.
- **Orders**  
  Create orders from basket, save history, track status (`Pending`, `Paid`, `Failed`).
- **Payments**  
  Stripe integration for PaymentIntent creation, webhook handling, linking payment to orders.
- **Architecture & Performance**  
  Repository + Unit of Work pattern, Specification pattern, Global Exception Middleware, Response Caching, Swagger API Documentation.

---

## ⚙️ Installation & Setup

### 1️⃣ Clone the repository
```bash
git clone https://github.com/muhamedhamedvl/EcommApi
cd EcommApi
```

### 2️⃣ Apply migrations & seed data
```bash
dotnet ef database update
```

### 3️⃣ Run the project
```bash
dotnet run --project API
```

#### 🏗 API Endpoints Overview

| Endpoint                | Method | Description                 | Auth |
| ----------------------- | ------ | --------------------------- | ---- |
| `/api/products`         | GET    | List all products           | ❌    |
| `/api/products/{id}`    | GET    | Get product details         | ❌    |
| `/api/basket`           | GET    | Get basket                  | ✅    |
| `/api/basket`           | POST   | Create/Update basket        | ✅    |
| `/api/orders`           | POST   | Create new order            | ✅    |
| `/api/payments`         | POST   | Create Stripe PaymentIntent | ✅    |
| `/api/account/login`    | POST   | Login                       | ❌    |
| `/api/account/register` | POST   | Register                    | ❌    |


### Backend Flow Diagram
## flowchart LR
    User[Frontend User] -->|HTTP Request| API[ASP.NET Core API]
    API -->|Query| Repo[Repository Layer]
    Repo -->|EF Core| DB[(SQL Server)]
    API -->|Cache Basket| Redis[(Redis Cache)]
    API -->|PaymentIntent| Stripe[(Stripe API)]
    Stripe -->|Webhook| API

## 👨‍💻 Author

**Mohamed Hamed**  
_Backend Developer – ASP.NET Core 

📧 **Email:** [mh1191128@gmail.com](mailto:mh1191128@gmail.com)  
🔗 **LinkedIn:** [linkedin.com/in/muhamed-hamed-muhamed-3a2a25250](https://www.linkedin.com/in/muhamed-hamed-muhamed-3a2a25250/)  
💻 **GitHub:** [https://github.com/muhamedhamedvl](https://github.com/muhamedhamedvl)
