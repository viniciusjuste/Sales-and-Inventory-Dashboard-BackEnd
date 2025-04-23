# 💡 Inventory and Sales Management System

A simple and straightforward system for product registration, inventory control, sales registration and report generation. Ideal for small businesses, independent sellers or even as a base for larger projects.

## ✨ Features

- Product registration
- Automatic inventory control
- Sales recording
- Report generation with revenue summary
- RESTful API with organized endpoints

## ⚙️ Technologies Used

- **Backend**: .NET Minimal API
- **Database**:
  - SQLite (recommended for testing and simplicity)
  - SQL Server (for production)
  - JSON (optional for simpler versions)

## 🚀 How to Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/viniciusjuste/Sales-and-Inventory-Dashboard-BackEnd.git

2. Navigate to the project folder:
   ```bash
   cd SalesAndInventoryDashboard-BE
   ```

3. Restore dependencies and run the project:
   ```bash
   dotnet restore
   dotnet run
   ```

4. The API will be available at: https://localhost:5001 or http://localhost:5000

## 🔑 Available Endpoints

| Method | Route             | Description                        |
|--------|-------------------|------------------------------------|
| POST   | /api/products     | Register a new product             |
| GET    | /api/products     | List all products                  |
| PUT    | /api/products/{id}| Update an existing product         |
| DELETE | /api/products/{id}| Delete a product                   |
| POST   | /api/sales        | Register a sale (decrease stock)   |
| GET    | /api/sales        | List all sales                     |
| GET    | /api/sales/{id}   | Retrieve sale by ID                |
| GET    | /api/reports      | Get sales summary report           |

## 📦 Project Structure

```
/Data
/Models
/Endpoints
/Migrations
Program.cs
appsettings.json
```

## 📌 Possible Future Improvements

- Authentication and authorization (JWT)
- Integration with payment systems
- Generating reports in PDF

## 👨‍💻 Author

Developed by Vinícius — Fullstack developer, backend enthusiast, and builder of practical everyday systems.

