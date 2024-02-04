--SELECT * FROM Employees;

--SELECT EmployeeID, FirstName, LastName FROM Employees

--SELECT TOP(5) FirstName, LastName FROM Employees;

--SELECT * 
--FROM Employees 
--WHERE JobTitle = 'Design Engineer';


--INSERT INTO Projects(Name, StartDate)
--	VALUES('Introduction to SQL Server', '1/1/2006');

--SELECT * FROM Projects WHERE Name = 'Introduction to SQL Server';

--UPDATE Projects
--SET EndDate = '8/31/2008'
--WHERE StartDate = '1/1/2006';

--DELETE FROM Projects
--WHERE StartDate = '1/1/2006';

--SELECT * FROM Departments;

--SELECT DepartmentID, Name FROM Departments;

--SELECT EmployeeID AS ID, FirstName, LastName FROM Employees;

--SELECT dep.DepartmentID, dep.Name, dep.ManagerID FROM Departments AS dep;

--SELECT FirstName + ' ' + LastName AS 'Full Name', EmployeeID AS 'No.'
--FROM Employees
