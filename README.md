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
