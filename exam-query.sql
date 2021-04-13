CREATE PROCEDURE usp_getAllStudent
   as
   select * from Students
GO; 

exec usp_getAllStudent