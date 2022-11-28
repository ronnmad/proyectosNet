SELECT c.ContactName
FROM 
Customers c
INNER JOIN Orders o
ON o.CustomerID = c.CustomerID
INNER JOIN Employees e
ON e.EmployeeID = o.EmployeeID
WHERE e.EmployeeID = 1