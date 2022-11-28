SELECT YEAR(o.OrderDate), SUM(od.Quantity * od.UnitPrice * (1 - od.Discount)) as 'Total facturado'
FROM 
dbo.[Order Details] od
INNER JOIN Orders o
ON o.OrderID = od.OrderID
INNER JOIN Employees e
ON e.EmployeeID = o.EmployeeID
WHERE e.FirstName = 'Steven'
GROUP BY YEAR(o.OrderDate)

