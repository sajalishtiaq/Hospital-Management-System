# Hospital Management System

A complete **Hospital Management System** developed in C# (.NET Windows Forms) with SQL Server backend for the Open Ended Lab (OEL) course.

## Features

- **Role-based Login**: Admin, Doctor, and Patient portals
- **Admin Dashboard**: Overview with statistics and quick access
- **Patient Management**: Add, update, search patients
- **Doctor Management**: Manage doctors and specializations
- **Appointment System**: Book, update, cancel appointments with conflict detection
- **Billing & Payments**: Generate bills and record payments
- **Medical Records**: Maintain patient diagnosis, treatment & prescriptions
- **Doctor Portal**: View and update appointment status
- **Patient Portal**: View personal appointments and bills
- **Reports**: Patient visits, billing, and appointment reports

## Technologies

- **Frontend**: C# Windows Forms
- **Backend**: SQL Server
- **Database Helper**: Custom ADO.NET layer
- **Architecture**: 3-Tier (Presentation, Business, Data)

## Setup Instructions

1. Restore the database using `Database.sql`
2. Update connection string in `DBHelper.cs`
3. Build and run the project

```bash
# Default Login Credentials
Admin     → admin / admin123
Doctor    → dr.ahmed / doc123
Patient   → patient1 / pat123
