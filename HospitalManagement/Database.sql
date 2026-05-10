CREATE DATABASE HospitalDBOEL;

USE HospitalDBOEL;

CREATE TABLE Users (
    UserID       INT PRIMARY KEY IDENTITY(1,1),
    Username     NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Role         NVARCHAR(20)  NOT NULL CHECK (Role IN ('Admin','Doctor','Patient')),
    ReferenceID  INT NULL
);

CREATE TABLE Patients (
    PatientID INT PRIMARY KEY IDENTITY(1,1),
    FullName  NVARCHAR(150) NOT NULL,
    CNIC      NVARCHAR(20)  NOT NULL UNIQUE,
    Contact   NVARCHAR(20)  NOT NULL,
    Address   NVARCHAR(300) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Doctors (
    DoctorID       INT PRIMARY KEY IDENTITY(1,1),
    FullName       NVARCHAR(150) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    Contact        NVARCHAR(20)  NOT NULL,
    CreatedAt      DATETIME DEFAULT GETDATE()
);

CREATE TABLE Appointments (
    AppointmentID   INT PRIMARY KEY IDENTITY(1,1),
    PatientID       INT NOT NULL REFERENCES Patients(PatientID),
    DoctorID        INT NOT NULL REFERENCES Doctors(DoctorID),
    AppointmentDate DATE NOT NULL,
    AppointmentTime TIME NOT NULL,
    Status          NVARCHAR(20) NOT NULL DEFAULT 'Scheduled'
                    CHECK (Status IN ('Scheduled','Completed','Cancelled','Closed')),
    Remarks         NVARCHAR(500) NULL,
    CreatedAt       DATETIME DEFAULT GETDATE(),
    CONSTRAINT UQ_Doctor_DateTime UNIQUE (DoctorID, AppointmentDate, AppointmentTime)
);

CREATE TABLE MedicalRecords (
    RecordID     INT PRIMARY KEY IDENTITY(1,1),
    PatientID    INT NOT NULL REFERENCES Patients(PatientID),
    Diagnosis    NVARCHAR(500) NOT NULL,
    Treatment    NVARCHAR(500) NOT NULL,
    Prescription NVARCHAR(500) NOT NULL,
    RecordDate   DATE NOT NULL,
    CreatedAt    DATETIME DEFAULT GETDATE()
);

CREATE TABLE Billing (
    BillID        INT PRIMARY KEY IDENTITY(1,1),
    PatientID     INT NOT NULL REFERENCES Patients(PatientID),
    AppointmentID INT NOT NULL REFERENCES Appointments(AppointmentID),
    Amount        DECIMAL(10,2) NOT NULL,
    BillDate      DATE NOT NULL,
    Status        NVARCHAR(10) NOT NULL DEFAULT 'Unpaid' CHECK (Status IN ('Paid','Unpaid')),
    CreatedAt     DATETIME DEFAULT GETDATE()
);

CREATE TABLE Payments (
    PaymentID   INT PRIMARY KEY IDENTITY(1,1),
    BillID      INT NOT NULL REFERENCES Billing(BillID),
    AmountPaid  DECIMAL(10,2) NOT NULL,
    PaymentDate DATE NOT NULL,
    CreatedAt   DATETIME DEFAULT GETDATE()
);

INSERT INTO Users (Username, PasswordHash, Role, ReferenceID) VALUES
('admin',    'admin123', 'Admin',   NULL),
('dr.ahmed', 'doc123',   'Doctor',  1),
('dr.sara',  'doc456',   'Doctor',  2),
('patient1', 'pat123',   'Patient', 1);

INSERT INTO Doctors (FullName, Specialization, Contact) VALUES
('Dr. Ahmed Khan', 'Cardiology',  '0300-1234567'),
('Dr. Sara Ali',   'Neurology',   '0301-9876543'),
('Dr. Bilal Raza', 'Orthopedics', '0302-1122334');

INSERT INTO Patients (FullName, CNIC, Contact, Address) VALUES
('Ali Hassan',   '35202-1234567-1', '0311-1234567', 'House 5, Street 3, Rawalpindi'),
('Fatima Malik', '35202-7654321-2', '0322-9876543', 'Block B, Satellite Town, Rawalpindi'),
('Usman Tariq',  '35202-1122334-3', '0333-1122334', 'F-8/2, Islamabad');

INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status) VALUES
(1, 1, '2025-05-10', '09:00:00', 'Scheduled'),
(2, 2, '2025-05-10', '10:00:00', 'Completed'),
(3, 3, '2025-05-11', '11:00:00', 'Scheduled');

INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate) VALUES
(1, 'Hypertension', 'Lifestyle changes, medication', 'Amlodipine 5mg',  '2025-05-10'),
(2, 'Migraine',     'Pain relief, rest',             'Sumatriptan 50mg','2025-05-10');

INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status) VALUES
(1, 1, 2500.00, '2025-05-10', 'Unpaid'),
(2, 2, 3000.00, '2025-05-10', 'Paid'),
(3, 3, 1500.00, '2025-05-11', 'Unpaid');

INSERT INTO Payments (BillID, AmountPaid, PaymentDate) VALUES
(2, 3000.00, '2025-05-10');

