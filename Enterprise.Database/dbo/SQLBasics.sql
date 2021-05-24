-- a SELECT statement is to query tables, apply some logical manipulation, and return a result.
--The clauses are logically pro-cessed in the following order:
--1.  FROM
--2.  WHERE
--3.  GROUP BY
--4.  HAVING
--5.  SELECT
--6.  ORDER BY

--Advice: make it a practice to terminate all statements with a semicolon.
--Advice: always schema-qualify object names in your code. 
-- By using indexes, SQL Server can sometimes get the required data with much less work compared to applying 
--                  full table scans. Query filters also reduce the network traffic created by returning all possible rows to the caller and filtering on the client side.

--T-SQL uses three-valued predicate logic, where logical expressions can evaluate to TRUE, FALSE, or UNKNOWN. 
-- page.32(60) groupby clause





