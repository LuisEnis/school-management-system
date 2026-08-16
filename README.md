# School Management System

A full-stack **School Management System** built with **ASP.NET Core Web API** and **Angular**, designed to manage students, teachers, subjects, classes, and their relationships through a role-based system.

The project is being developed as a practical full-stack application with a focus on clean architecture, RESTful APIs, authentication, authorization, validation, and frontend/backend integration.

---

## 🚀 Tech Stack

### Backend

* **ASP.NET Core Web API**
* **.NET 10**
* **Entity Framework Core**
* **SQL Server**
* **JWT Authentication**
* **Role-Based Authorization**
* **AutoMapper**
* **Swagger / OpenAPI**
* Repository + Service layered architecture
* Custom exception handling middleware
* Password hashing with ASP.NET Core Identity
* Automatic database migration and initialization
* SignalR planned

### Frontend

* **Angular**
* Standalone Components
* TypeScript
* Reactive Forms
* Angular Router
* Route Guards
* HTTP Interceptors
* RxJS / Observables
* Role-based UI rendering
* Role-based route protection
* Responsive custom CSS

---

## 👥 User Roles

The system supports four roles:

| Role | Description |
| --- | --- |
| **Director** | Full access to the management system, including managing teachers and secretaries |
| **Secretary** | Access to management pages and data, but cannot manage teachers or secretaries |
| **Teacher** | Access to their dashboard, assigned classes and change password |
| **Student** | Access to their dashboard and change password |

Authorization is implemented both on the **backend** and **frontend**.

Frontend guards and UI restrictions improve the user experience, while backend authorization provides the actual security boundary.

---

## 📚 Main Features

### Authentication & Authorization

* User login with JWT authentication
* Current-user endpoint
* Role-based authorization policies
* Protected API endpoints
* Angular authentication guard
* Role-based Angular route guards
* JWT HTTP interceptor
* Role-based navigation
* Role-based UI actions
* Change password functionality
* Director profile management
* Secure password hashing
* Default Director initialization

### Students

* View students
* Create students
* Edit students
* Delete students
* Assign students to classes
* Remove student-class assignments
* Student dashboard

### Teachers

* View teachers
* Create teachers
* Edit teachers
* Delete teachers
* Assign teachers to subjects
* Create teaching assignments
* Teacher dashboard
* View classes and subjects taught
* View students belonging to classes they teach

Teacher creation, editing and deletion are restricted to the Director.

Secretaries can view teachers but cannot modify them.

### Secretaries

* View secretaries
* Create secretaries
* Edit secretaries
* Delete secretaries

Secretary management is restricted to the Director.

### Subjects

* Create subjects
* View subjects
* Edit subjects
* Delete subjects
* Assign subjects to teachers

Both Directors and Secretaries can manage subjects.

### Classes

* Create classes
* View classes
* Edit classes
* Delete classes
* View class details
* View students assigned to a class
* View subjects and teachers assigned to a class

Both Directors and Secretaries can manage classes.

Teachers can access class details only for classes where they are assigned to teach.

### Assignments

The system manages three main relationships:

* **Student → Class**
* **Teacher → Subject**
* **Teacher → Subject → Class**

These relationships have their own endpoints and validation rules to prevent invalid or duplicate assignments.

---

## 🖥️ Dashboard

The dashboard changes depending on the logged-in user's role.

### Student Dashboard

Students can see:

* Their assigned class
* Their subjects
* The teacher responsible for each subject

### Teacher Dashboard

Teachers can see:

* The classes where they teach
* The subject they teach in each class
* A **Go to class** action for each teaching assignment
* Students belonging to classes they teach

### Director & Secretary Dashboard

Management users have a dedicated management dashboard containing statistics and overview information about the school.

The dashboard provides management-level information such as:

* Total number of students
* Total number of teachers
* Total number of secretaries
* Total number of classes
* Total number of subjects

The management dashboard is provided through dedicated backend endpoints and an Angular dashboard service.

---

## 👤 Director Profile

The Director can update their own account information through the profile page.

The profile allows the Director to update:

* Username
* First Name
* Last Name
* Email

Password changes are handled separately through the existing **Change Password** functionality.

The profile page is protected so that only the Director can access it.

---

## 🔐 Role-Based Access

The application uses both backend and frontend authorization.

### Backend

ASP.NET Core authorization policies are used to protect API endpoints, including:

* `Management`
* `DirectorOnly`
* `TeacherOnly`
* `StudentOnly`
* `ClassView`

Business rules are also enforced inside the service layer.

For example:

* Secretaries can only create, update and delete students.
* Secretaries cannot create, update or delete teachers.
* Only Directors can manage secretaries.
* Only Directors can create, edit or delete teachers.
* Only Directors can update their profile.
* Teachers can access class details only for classes they teach.

### Frontend

Angular route guards prevent unauthorized users from navigating directly to protected pages.

The frontend uses guards for:

* Authentication
* Management pages
* Director-only pages
* Class details

Role-based UI rendering is also used to hide actions that the current user cannot perform.

Frontend authorization is not considered a security boundary by itself; the backend always performs the final authorization checks.

---

## 🏗️ Backend Architecture

The backend follows a layered architecture:

```text
SchoolManagement
│
├── Controllers
├── Services
├── Interfaces
│   ├── Services
│   └── Repositories
├── Repositories
├── Entities
├── DTOs
├── Data
├── Enums
├── Middleware
├── Mappings
├── Migrations
├── Exceptions
├── Properties
├── Responses
└── Settings
```

The general request flow is:
```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
SQL Server
```
DTOs are used between the API and clients instead of exposing database entities directly.
---
## 🛡️ Validation & Business Rules
The backend contains business validation for scenarios such as:
* Preventing duplicate class names
* Preventing duplicate subject names
* Preventing duplicate assignments
* Verifying that a user is actually a student before assigning them to a class
* Verifying that a user is actually a teacher before assigning them to a subject
* Verifying that subjects exist before creating related assignments
* Preventing deletion of classes that are still being used by teaching assignments
* Email uniqueness validation
* Restricting teachers to classes they actually teach
* Role-based endpoint authorization
API exceptions are handled centrally through custom exception handling middleware.
---
## 📖 API Documentation
The backend includes **Swagger / OpenAPI** documentation for testing and exploring the API.
Swagger can be used to:
* View available endpoints
* Inspect request/response models
* Test CRUD operations
* Authenticate using JWT
* Test role-protected endpoints
---
## 🗄️ Database
The application uses **SQL Server** with **Entity Framework Core**.
Main entities include:
* User
* SchoolClass
* Subject
* StudentClass
* TeacherSubject
* TeachingAssignment

The relationships between these entities allow the system to represent students, teachers, classes, subjects, and teaching assignments.

Automatic Database Initialization

The application contains a DatabaseInitializer that runs when the API starts.

It:

Applies any pending Entity Framework Core migrations automatically.
Checks whether a Director already exists.
Creates a default Director if no Director exists.

This allows a new database to be initialized automatically when the application starts.

The default Director information is configured through appsettings.json.

The application therefore does not require manually running Update-Database every time a new database is started, as pending migrations are automatically applied by the application.
---
## 📁 Frontend Structure
The Angular application is organized by features and shared core functionality:
```text
src/app
│
├── core
│   ├── guards
│   ├── interceptors
│   ├── models
│   └── services
│
├── features
│   ├── assignments
│   ├── auth
│   ├── dashboard
│   ├── schoolClasses
│   ├── students
│   ├── subjects
│   └── teachers
│
└── layout
    └── main-layout
```
The frontend uses Angular standalone components and Reactive Forms.
---
## 🔄 Current Development Status
### Completed
* Backend project architecture
* Database entities and relationships
* Entity Framework Core configuration
* Repositories and services
* DTOs
* CRUD operations
* Assignment functionality
* Business validation
* Exception handling middleware
* Password hashing
* JWT authentication
* Role-based authorization
* Swagger/OpenAPI
* Angular authentication
* Angular route protection
* JWT interceptor
* Student management UI
* Teacher management UI
* Subject management UI
* Class management UI
* Assignment management UI
* Change password
* Role-based sidebar
* Role-based frontend actions
* Student dashboard
* Teacher dashboard
* Director/Secretary management dashboard
* Teacher class access validation
* Backend/frontend integration and testing
### Planned / Remaining
* Final frontend/backend testing and cleanup
* SignalR integration
* Additional UI polishing and improvements
---
## 🧪 Testing
The API has been tested using **Swagger** and **Postman** during development.
Frontend functionality is tested through the Angular application by verifying:
* Authentication
* Role-based navigation
* CRUD operations
* Assignment workflows
* Form validation
* Dashboard data
* Authorization behavior
* API integration
---
## ⚙️ Running the Project
### Backend
1. Clone the repository.
2. Open the backend solution in Visual Studio.
3. Configure the SQL Server connection string and default director configuration in:
```text
appsettings.json
```
Example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SchoolManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "DefaultDirector": {
    "Username": "admin",
    "FirstName": "System",
    "LastName": "Administrator",
    "Email": "admin@schoolmanagement.com",
    "Password": "admin123"
  }
}
```
4. Run the ASP.NET Core API.

On startup, the application will automatically:

* Apply any pending Entity Framework Core migrations.
* Check whether a Director already exists.
* Create the default Director if no Director exists.

The default Director can then log in and change their profile information and password.

Note: Update-Database is not required for normal application startup because pending migrations are automatically applied by the DatabaseInitializer.

Security: The default Director password in appsettings.json is intended for development/initialization purposes. For a real deployment, the password should be stored securely using environment variables, user secrets, or another secure configuration mechanism.
Swagger will be available when the API is running.
### Frontend
Navigate to the Angular project:
```bash
cd frontend
```
Install dependencies:
```bash
npm install
```
Start the development server:
```bash
ng serve
```
Then open the application in the browser.
---
## 🔑 Authentication Flow
The application uses JWT-based authentication.
```text
Login
  ↓
ASP.NET Core API
  ↓
Credentials validated
  ↓
JWT generated
  ↓
Angular stores token
  ↓
HTTP Interceptor adds Bearer token
  ↓
Protected API endpoints
```
The authenticated user's role determines which functionality is available.
---
## 🎯 Project Goals
This project was created to practice and demonstrate full-stack development using technologies commonly used in modern enterprise applications.
The main goals are:
* Build a complete REST API with ASP.NET Core
* Apply layered architecture
* Work with Entity Framework Core and SQL Server
* Implement JWT authentication and authorization
* Build a modern Angular frontend
* Integrate Angular with a real backend API
* Apply business rules and validation
* Work with role-based applications
* Practice API testing with Swagger and Postman
* Implement automatic database initialization
* Build a project that can be extended with real-time functionality using SignalR
---
## 📌 Future Improvements
Possible future improvements include:
* SignalR notifications and real-time updates
* More detailed management dashboards
* Attendance management
* Grades and academic performance
* Additional reporting
* Improved error handling and user feedback
* More extensive automated testing
* Deployment to a cloud platform
---
## 👨‍💻 Author
**Enis Sejdini**
Full-Stack Software Developer focused primarily on **.NET / ASP.NET Core backend development**, with experience building Angular frontends and working with databases, APIs, authentication, Docker, messaging and cloud technologies.
 		
