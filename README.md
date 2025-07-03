# Employee-Access-Control-API

This is a simple RESTful Web API built with ASP.NET Core that allows HR personnel to manage employees and track their work shifts. It stores employee data, shift times, and calculates hours worked.

## 🔧 Technologies Used

- ASP.NET Core 7 Web API  
- Entity Framework Core with SQLite  
- Swagger / OpenAPI for API documentation  
- Visual Studio  
- Git & GitHub  

---

## 🚀 Features

- Add, update, delete, and view employee records  
- Start and end employee shifts manually  
- Auto-generate monthly shift schedules  
- Automatically calculate hours worked per shift  
- Track violations for late arrivals or early departures  
- View violation counts per employee  
- Clean architecture with services and interfaces  
- Swagger UI for easy testing and debugging  

---

## 📌 API Endpoints

### 👤 Employee Management

- `POST /AddEmployee` – Add a new employee  
- `GET /GetEmployee/{id}` – Get employee by ID (with shifts)  
- `PUT /UpdateEmployee` – Update employee details  
- `DELETE /DeleteEmployee/{id}` – Delete employee  

### 🕒 Shift Management

- `POST /StartShift` – Mark employee start time  
- `POST /EndShift` – Mark employee end time  
- `POST /GenerateShifts` – Auto-generate shifts for all employees (for current month)  

### 🚨 Violation Tracking

- `GET /GetViolations/{id}` – View violation count of a specific employee  
- `GET /GetAllViolations` – View all employees with their violation counts  

---

## 🔄 How to Run Locally

### 1. Clone the Repository

```bash
git clone https://github.com/faridun01/EmployeeAccessControlAPI.git
cd EmployeeAccessControlAPI
