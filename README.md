# Employee-Access-Control-API

This is a simple RESTful Web API built with ASP.NET Core that allows HR personnel to manage employees and track their work shifts. It stores employee data, shift times, and calculates hours worked.

## 🔧 Technologies Used

- ASP.NET Core 7 Web API
- Entity Framework Core (with SQLite)
- Swagger / OpenAPI for API documentation
- Visual Studio 
- Git & GitHub

---

## 🚀 Features

- Add, update, delete, and view employee records
- Start and end employee shifts
- Calculate and store hours worked automatically
- View employee shift history
- Clean architecture with service interfaces
- Swagger UI for testing endpoints
  
##  📌 API Endpoints
👤 Employee Management

POST /AddEmployee – Add a new employee

GET /GetEmployee/{id} – Get employee by ID

PUT /UpdateEmployee – Update employee

DELETE /DeleteEmployee/{id} – Delete employee

🕒 Shift Management
POST /StartShift – Mark employee start time

POST /EndShift – Mark employee end time

---
## 🔄 How to Run

### 1. Clone the Repository

```bash
git clone https://github.com/faridun01/EmployeeAccessControlAPI.git
cd EmployeeAccessControlAPI

