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
ECommerce.sln
├── API # Presentation layer (Controllers, Middleware, Swagger)
├── Core # Entities, DTOs, Interfaces, Specifications
├── Infrastructure # EF Core, Repositories, Identity, Data Seeding


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
cd ecommerce-backend
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

## Coming soon
Admin dashboard for product/order management

Email notifications for orders

Multi-language support

Advanced search with ElasticSearch

## 👨‍💻 Author

**Mohamed Hamed**  
_Backend Developer – ASP.NET Core & Clean Architecture Enthusiast_  

📧 **Email:** [mh1191128@gmail.com](mailto:mh1191128@gmail.com)  
🔗 **LinkedIn:** [linkedin.com/in/muhamed-hamed-muhamed-3a2a25250](https://www.linkedin.com/in/muhamed-hamed-muhamed-3a2a25250/)  
💻 **GitHub:** [https://github.com/muhamedhamedvl](https://github.com/muhamedhamedvl)
