SELECT e.FirstName, e.LastName
FROM 
Employees e
LEFT OUTER JOIN employees em ON e.ReportsTo = em.EmployeeID 
WHERE e.ReportsTo is null;