SELECT *
, c.CategoryName 
FROM 
Products p
LEFT JOIN Categories c
ON c.CategoryID = p.CategoryID
WHERE p.CategoryID is not null 