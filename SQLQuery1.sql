create database Ticketsupportsystem

use Ticketsupportsystem 

CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(20) NOT NULL UNIQUE
        CHECK (Name IN ('MANAGER','SUPPORT','USER'))
);
select * from Roles
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role_Id INT NOT NULL,
    Created_At DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Users_Roles
        FOREIGN KEY (Role_Id)
        REFERENCES Roles(Id)
        ON DELETE NO ACTION
);

CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,

    Status VARCHAR(20) DEFAULT 'OPEN'
        CHECK (Status IN ('OPEN','IN_PROGRESS','RESOLVED','CLOSED')),

    Priority VARCHAR(20) DEFAULT 'MEDIUM'
        CHECK (Priority IN ('LOW','MEDIUM','HIGH')),

    Created_By INT NOT NULL,
    Assigned_To INT NULL,

    Created_At DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Tickets_Creator
        FOREIGN KEY (Created_By)
        REFERENCES Users(Id)
        ON DELETE NO ACTION, 

    CONSTRAINT FK_Tickets_Assigned
        FOREIGN KEY (Assigned_To)
        REFERENCES Users(Id)
        ON DELETE SET NULL
);

CREATE TABLE Ticket_Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Ticket_Id INT NOT NULL,
    User_Id INT NOT NULL,
    Comment TEXT NOT NULL,
    Created_At DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Comments_Ticket
        FOREIGN KEY (Ticket_Id)
        REFERENCES Tickets(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_Comments_User
        FOREIGN KEY (User_Id)
        REFERENCES Users(Id)
        ON DELETE CASCADE
);

CREATE TABLE Ticket_Status_Logs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Ticket_Id INT NOT NULL,

    Old_Status VARCHAR(20) NOT NULL
        CHECK (Old_Status IN ('OPEN','IN_PROGRESS','RESOLVED','CLOSED')),

    New_Status VARCHAR(20) NOT NULL
        CHECK (New_Status IN ('OPEN','IN_PROGRESS','RESOLVED','CLOSED')),

    Changed_By INT NOT NULL,
    Changed_At DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Logs_Ticket
        FOREIGN KEY (Ticket_Id)
        REFERENCES Tickets(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_Logs_User
        FOREIGN KEY (Changed_By)
        REFERENCES Users(Id)
        ON DELETE CASCADE
);


INSERT INTO roles (name) VALUES
('MANAGER'),
('SUPPORT'),
('USER');