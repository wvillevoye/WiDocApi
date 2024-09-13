USE master
GO

-- Stap 1: Maak de database 'SamplePersons'
CREATE DATABASE SamplePersons;
GO

-- Stap 2: Gebruik de nieuw gemaakte database
USE SamplePersons;
GO

-- Stap 3: Maak de tabel 'Persons' met extra kolommen voor State en ZipCode
CREATE TABLE Persons (
    PersonID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Age INT,
    Gender NVARCHAR(10),
    Email NVARCHAR(100),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10)
);
GO

-- Stap 4: Voeg 100 fictieve records toe aan de tabel 'Persons' met State en ZipCode
INSERT INTO Persons (FirstName, LastName, Age, Gender, Email, State, ZipCode)
VALUES
('John', 'Doe', 25, 'M', 'john.doe@example.com', 'California', '90001'),
('Jane', 'Smith', 30, 'F', 'jane.smith@example.com', 'New York', '10001'),
('Robert', 'Johnson', 45, 'M', 'robert.johnson@example.com', 'Texas', '73301'),
('Emma', 'Brown', 22, 'F', 'emma.brown@example.com', 'Florida', '33101'),
('Liam', 'Miller', 27, 'M', 'liam.miller@example.com', 'Illinois', '60601'),
('Olivia', 'Wilson', 35, 'F', 'olivia.wilson@example.com', 'Nevada', '89001'),
('Noah', 'Anderson', 40, 'M', 'noah.anderson@example.com', 'Washington', '98001'),
('Ava', 'Thomas', 29, 'F', 'ava.thomas@example.com', 'Virginia', '22001'),
('James', 'Jackson', 33, 'M', 'james.jackson@example.com', 'Georgia', '30301'),
('Sophia', 'White', 31, 'F', 'sophia.white@example.com', 'Arizona', '85001'),
('Benjamin', 'Harris', 38, 'M', 'benjamin.harris@example.com', 'Ohio', '44101'),
('Mia', 'Martin', 24, 'F', 'mia.martin@example.com', 'North Carolina', '27501'),
('Lucas', 'Lee', 36, 'M', 'lucas.lee@example.com', 'Colorado', '80001'),
('Isabella', 'Walker', 21, 'F', 'isabella.walker@example.com', 'Massachusetts', '02101'),
('Ethan', 'King', 28, 'M', 'ethan.king@example.com', 'Michigan', '48201'),
('Charlotte', 'Wright', 34, 'F', 'charlotte.wright@example.com', 'Oregon', '97001'),
('Mason', 'Hill', 32, 'M', 'mason.hill@example.com', 'New Jersey', '07001'),
('Amelia', 'Green', 26, 'F', 'amelia.green@example.com', 'Pennsylvania', '19001'),
('Jacob', 'Adams', 39, 'M', 'jacob.adams@example.com', 'South Carolina', '29001'),
('Harper', 'Baker', 22, 'F', 'harper.baker@example.com', 'Minnesota', '55101'),
('Michael', 'Gonzalez', 41, 'M', 'michael.gonzalez@example.com', 'Alabama', '35201'),
('Ella', 'Nelson', 27, 'F', 'ella.nelson@example.com', 'Louisiana', '70112'),
('Alexander', 'Carter', 30, 'M', 'alexander.carter@example.com', 'Kentucky', '40201'),
('Abigail', 'Mitchell', 28, 'F', 'abigail.mitchell@example.com', 'Missouri', '63101'),
('Daniel', 'Perez', 35, 'M', 'daniel.perez@example.com', 'Oklahoma', '73101'),
('Emily', 'Roberts', 23, 'F', 'emily.roberts@example.com', 'Wisconsin', '53201'),
('Henry', 'Turner', 29, 'M', 'henry.turner@example.com', 'Indiana', '46201'),
('Avery', 'Phillips', 24, 'F', 'avery.phillips@example.com', 'Mississippi', '39201'),
('Jackson', 'Campbell', 42, 'M', 'jackson.campbell@example.com', 'Arkansas', '72201'),
('Scarlett', 'Parker', 33, 'F', 'scarlett.parker@example.com', 'Tennessee', '37201'),
('Sebastian', 'Evans', 31, 'M', 'sebastian.evans@example.com', 'Iowa', '50301'),
('Madison', 'Edwards', 25, 'F', 'madison.edwards@example.com', 'Utah', '84101'),
('David', 'Collins', 36, 'M', 'david.collins@example.com', 'West Virginia', '25301'),
('Victoria', 'Stewart', 40, 'F', 'victoria.stewart@example.com', 'Kansas', '66101'),
('Samuel', 'Sanchez', 34, 'M', 'samuel.sanchez@example.com', 'Nebraska', '68101'),
('Luna', 'Morris', 21, 'F', 'luna.morris@example.com', 'Montana', '59001'),
('Elijah', 'Rogers', 37, 'M', 'elijah.rogers@example.com', 'Idaho', '83701'),
('Grace', 'Reed', 26, 'F', 'grace.reed@example.com', 'Maine', '04101'),
('Logan', 'Cook', 29, 'M', 'logan.cook@example.com', 'New Hampshire', '03301'),
('Chloe', 'Morgan', 23, 'F', 'chloe.morgan@example.com', 'South Dakota', '57101'),
('Oliver', 'Bell', 38, 'M', 'oliver.bell@example.com', 'North Dakota', '58101'),
('Zoey', 'Murphy', 27, 'F', 'zoey.murphy@example.com', 'Alaska', '99501'),
('Aiden', 'Bailey', 32, 'M', 'aiden.bailey@example.com', 'Delaware', '19901'),
('Penelope', 'Rivera', 28, 'F', 'penelope.rivera@example.com', 'Hawaii', '96801'),
('Matthew', 'Cooper', 30, 'M', 'matthew.cooper@example.com', 'Rhode Island', '02901'),
('Riley', 'Richardson', 25, 'F', 'riley.richardson@example.com', 'Vermont', '05601'),
('Joseph', 'Cox', 35, 'M', 'joseph.cox@example.com', 'New Mexico', '87501'),
('Layla', 'Howard', 22, 'F', 'layla.howard@example.com', 'Wyoming', '82001'),
('Joshua', 'Ward', 31, 'M', 'joshua.ward@example.com', 'Connecticut', '06101'),
('Hannah', 'Torres', 24, 'F', 'hannah.torres@example.com', 'Maryland', '21201'),
('Andrew', 'Peterson', 34, 'M', 'andrew.peterson@example.com', 'District of Columbia', '20001'),
('Lillian', 'Gray', 29, 'F', 'lillian.gray@example.com', 'Puerto Rico', '00901'),
('Ryan', 'Ramirez', 36, 'M', 'ryan.ramirez@example.com', 'Guam', '96910'),
('Addison', 'James', 23, 'F', 'addison.james@example.com', 'Nevada', '89002'),
('Gabriel', 'Watson', 40, 'M', 'gabriel.watson@example.com', 'Oregon', '97002'),
('Zoe', 'Brooks', 22, 'F', 'zoe.brooks@example.com', 'California', '90002'),
('Anthony', 'Kelly', 27, 'M', 'anthony.kelly@example.com', 'Texas', '73302'),
('Nora', 'Sanders', 26, 'F', 'nora.sanders@example.com', 'Florida', '33102'),
('Dylan', 'Price', 28, 'M', 'dylan.price@example.com', 'Illinois', '60602'),
('Lily', 'Bennett', 25, 'F', 'lily.bennett@example.com', 'New York', '10002'),
('Caleb', 'Wood', 31, 'M', 'caleb.wood@example.com', 'Georgia', '30302'),
('Sofia', 'Barnes', 21, 'F', 'sofia.barnes@example.com', 'Ohio', '44102'),
('Christian', 'Ross', 39, 'M', 'christian.ross@example.com', 'Washington', '98002'),
('Eleanor', 'Henderson', 23, 'F', 'eleanor.henderson@example.com', 'Virginia', '22002'),
('Nathan', 'Coleman', 33, 'M', 'nathan.coleman@example.com', 'North Carolina', '27502'),
('Hazel', 'Jenkins', 30, 'F', 'hazel.jenkins@example.com', 'Arizona', '85002'),
('Isaac', 'Perry', 28, 'M', 'isaac.perry@example.com', 'Massachusetts', '02102'),
('Aria', 'Powell', 24, 'F', 'aria.powell@example.com', 'Michigan', '48202'),
('Thomas', 'Long', 37, 'M', 'thomas.long@example.com', 'Colorado', '80002'),
('Ellie', 'Patterson', 25, 'F', 'ellie.patterson@example.com', 'New Jersey', '07002'),
('Christopher', 'Hughes', 35, 'M', 'christopher.hughes@example.com', 'Pennsylvania', '19002'),
('Mila', 'Flores', 27, 'F', 'mila.flores@example.com', 'South Carolina', '29002'),
('Landon', 'Washington', 26, 'M', 'landon.washington@example.com', 'Minnesota', '55102'),
('Lucy', 'Butler', 21, 'F', 'lucy.butler@example.com', 'Alabama', '35202'),
('Owen', 'Simmons', 38, 'M', 'owen.simmons@example.com', 'Louisiana', '70113'),
('Brooklyn', 'Foster', 29, 'F', 'brooklyn.foster@example.com', 'Kentucky', '40202'),
('Jack', 'Gonzales', 31, 'M', 'jack.gonzales@example.com', 'Missouri', '63102'),
('Aubrey', 'Bryant', 34, 'F', 'aubrey.bryant@example.com', 'Oklahoma', '73102'),
('Luke', 'Alexander', 28, 'M', 'luke.alexander@example.com', 'Wisconsin', '53202'),
('Evelyn', 'Russell', 23, 'F', 'evelyn.russell@example.com', 'Indiana', '46202'),
('Henry', 'Griffin', 40, 'M', 'henry.griffin@example.com', 'Mississippi', '39202'),
('Claire', 'Hayes', 22, 'F', 'claire.hayes@example.com', 'Arkansas', '72202'),
('Wyatt', 'Myers', 27, 'M', 'wyatt.myers@example.com', 'Tennessee', '37202'),
('Savannah', 'Ford', 33, 'F', 'savannah.ford@example.com', 'Iowa', '50302'),
('Levi', 'Hamilton', 29, 'M', 'levi.hamilton@example.com', 'Utah', '84102'),
('Stella', 'Graham', 25, 'F', 'stella.graham@example.com', 'West Virginia', '25302');
GO
