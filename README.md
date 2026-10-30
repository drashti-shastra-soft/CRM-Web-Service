

# 🚀 CRM Web Service – RESTful API

**CRM Web Service** is a RESTful ASP.NET Core Web API developed as the backend for a Customer Relationship Management (CRM) platform.

It provides secure and structured API endpoints for managing leads, contacts, deals, tasks, follow-ups, and reports. The application follows a clean layered architecture to maintain separation of concerns, scalability, and code maintainability.

The API uses JWT authentication, role-based authorization, Entity Framework Core, validation, and logging to support secure and reliable application operations.

## ✨ Key Features

- **Authentication & Authorization:** Secure API access using JWT authentication.
- **Role-Based Authorization:** Controls access based on user roles and permissions.
- **Lead Management:** Provides API operations for managing CRM leads.
- **Contact Management:** Supports contact-related operations.
- **Deal Management:** Handles sales deals and related information.
- **Task Management:** Provides task management functionality.
- **Follow-Up Management:** Supports customer follow-up activities.
- **Reports:** Provides reporting-related API functionality.
- **Entity Framework Core:** Handles database operations and data access.
- **Validation:** Supports request validation.
- **Logging:** Provides application logging.
- **Layered Architecture:** Separates domain, infrastructure, persistence, repository, service, and API responsibilities.

## 📦 Main Modules

### 1. Lead Management

- Create and manage leads
- Retrieve lead information
- Update lead details
- Support lead-related operations

### 2. Contact Management

- Manage customer contacts
- Retrieve contact information
- Update contact details

### 3. Deal Management

- Manage sales deals
- Access deal information
- Support deal-related operations

### 4. Task Management

- Manage CRM tasks
- Retrieve task information
- Support task-related operations

### 5. Follow-Up Management

- Manage customer follow-ups
- Maintain follow-up information
- Support follow-up activities

### 6. Reports

- Provide reporting-related API endpoints
- Support CRM data retrieval for reports

## 🔐 Security

- JWT-based authentication
- Role-based authorization
- Protected API endpoints
- Request validation
- Application logging

## 🛠️ Technology Stack

- **Framework:** ASP.NET Core Web API
- **Language:** C#
- **ORM:** Entity Framework Core
- **Database:** As configured in the application
- **Authentication:** JWT (JSON Web Token)
- **Architecture:** Clean Layered Architecture
- **API Communication:** RESTful APIs
- **Development Tool:** Visual Studio

## 🏗️ Project Structure

The repository follows a layered architecture with the following projects:

```text
CRM-Web-Service/
│
├── CRMWebService.Domain/
│
├── CRMWebService.Infrastructure/
│
├── CRMWebService.Persistence/
│
├── CRMWebService.Repository/
│
├── CRMWebService.Service/
│
├── CRMWebService/
│
├── CRMWebService.sln
│
├── .gitattributes
│
└── .gitignore
```

### Folder Responsibilities

| Project                      | Description                                                           |
| ---------------------------- | --------------------------------------------------------------------- |
| CRMWebService.Domain         | Contains domain models and business-related definitions.              |
| CRMWebService.Infrastructure | Handles infrastructure-related implementations.                       |
| CRMWebService.Persistence    | Manages database persistence and Entity Framework Core configuration. |
| CRMWebService.Repository     | Handles repository and data access operations.                        |
| CRMWebService.Service        | Contains application services and business logic.                     |
| CRMWebService                | Contains the ASP.NET Core Web API application and API endpoints.      |

## ⚙️ Getting Started

### Prerequisites

- Visual Studio
- Compatible .NET SDK
- C# development environment
- SQL Server or the configured database provider
- NuGet Package Manager

### Installation

**1. Clone the repository**

```bash
git clone https://github.com/drashti-shastra-soft/CRM-Web-Service.git
```

**2. Navigate to the project directory**

```bash
cd CRM-Web-Service
```

**3. Open the solution**

Open the solution file in Visual Studio:

```text
CRMWebService.sln
```

**4. Restore NuGet Packages**

```bash
dotnet restore
```

**5. Configure Application Settings**

Configure the required database connection string, JWT settings, API configuration, and other environment-specific values.

Check the application's configuration files before running the project.

**Security Note:** Never commit production passwords, database credentials, JWT secrets, or API keys to GitHub.

**6. Build the Solution**

```bash
dotnet build
```

**7. Run the Web API**

Set `CRMWebService` as the startup project in Visual Studio and run the application.

The API will start using the configured launch settings.

## 🎯 Project Objective

The main objective of CRM Web Service is to provide a centralized and secure backend API for managing customer relationships and sales activities.

It helps the CRM platform handle leads, contacts, deals, tasks, follow-ups, and reports through a structured RESTful API.

The layered architecture makes the application easier to maintain, extend, and integrate with CRM frontend applications.
