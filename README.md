# Employee & Family Registry System

Full-stack application for managing employees and their family members.

## Tech Stack

- **Backend**: ASP.NET Core 8, Clean Architecture, PostgreSQL, EF Core, FluentValidation, QuestPDF
- **Frontend**: React, Vite, JavaScript, Tailwind CSS, Axios

## Features

- CRUD for employees with spouse and children
- Search employees (debounced)
- Generate PDF profile per employee
- Validation for NID (10/17 digits) and Bangladeshi phone numbers

## Setup Instructions

### Backend

1. Navigate to `EmployeeRegistry` folder.
2. Update `appsettings.json` with your PostgreSQL connection string.
3. Run the following commands:
