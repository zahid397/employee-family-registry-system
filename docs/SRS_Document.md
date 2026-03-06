# Software Requirements Specification (SRS)
## Employee & Family Registry System

### 1. Introduction
#### 1.1 Purpose
This document specifies the requirements for the Employee & Family Registry System, a web application to manage employee records along with their spouse and children information.

#### 1.2 Scope
The system allows HR personnel to add, search, delete, and generate PDF profiles of employees. It enforces data validation (NID uniqueness, Bangladeshi phone format) and stores data in PostgreSQL.

#### 1.3 Definitions
- **Employee**: A person working in the organization.
- **Spouse**: Married partner of the employee (optional).
- **Child**: Offspring of the employee (zero or many).

### 2. Overall Description
#### 2.1 Product Perspective
The system is a new standalone application with a React frontend and ASP.NET Core backend. It uses Clean Architecture to separate concerns.

#### 2.2 User Characteristics
- **HR Manager**: Can view, add, delete employee records and generate PDFs.

#### 2.3 Constraints
- NID must be unique and exactly 10 or 17 digits.
- Phone must follow Bangladeshi format (+8801xxxxxxxxx or 01xxxxxxxxx).
- Frontend must be written in JavaScript (not TypeScript).

### 3. Functional Requirements
#### 3.1 Employee Management
- **FR1**: The system shall allow searching employees by name, NID, or department (with debounce).
- **FR2**: The system shall allow adding a new employee with name, NID, phone, department, basic salary, optional spouse, and multiple children.
- **FR3**: The system shall validate all inputs before saving.
- **FR4**: The system shall prevent duplicate NIDs.
- **FR5**: The system shall allow deleting an employee.
- **FR6**: The system shall generate a PDF profile containing all employee details, spouse (if any), and children list.

#### 3.2 API Endpoints
- `GET /api/employees/search?q=`
- `POST /api/employees`
- `DELETE /api/employees/{id}`
- `GET /api/employees/{id}/pdf`

### 4. Non-Functional Requirements
- **NFR1**: The backend shall follow Clean Architecture.
- **NFR2**: Database shall use unique index on NID.
- **NFR3**: The frontend shall use React with Vite and Tailwind CSS.
- **NFR4**: PDF generation shall use QuestPDF library.
- **NFR5**: The system shall be responsive and user-friendly.

### 5. Data Model
#### Entities
- **Employee**: Id (GUID), Name, NID (unique), Phone, Department, BasicSalary
- **Spouse**: Id, EmployeeId, Name, NID
- **Child**: Id, EmployeeId, Name, DateOfBirth

Relationships:
- Employee 1 ── 0..1 Spouse
- Employee 1 ── 0..* Child

### 6. System Architecture
The backend is structured in four layers:
- **Domain**: Core entities.
- **Application**: DTOs, commands, validators, interfaces.
- **Infrastructure**: Persistence, repositories, PDF service, seeder.
- **API**: Controllers and startup configuration.

The frontend uses components and services to communicate with the API via Axios.

### 7. Validation Rules
- **NID**: 10 or 17 digits only.
- **Phone**: Must start with +8801 or 01 followed by a digit 3-9 and 8 more digits.
- **Salary**: Positive decimal.
- **DateOfBirth**: Must be in the past.

### 8. Database Schema
Tables: Employees, Spouses, Children with foreign keys and indexes.

### 9. PDF Content
- Employee name, NID, phone, department, basic salary
- Spouse details (if any)
- List of children with names and dates of birth
- Generation date

### 10. Future Enhancements
- Edit employee functionality
- Authentication and authorization
- Export to Excel
- More advanced search filters
