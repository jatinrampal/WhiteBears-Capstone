SELECT Task.title, Task.description, Task.startDate, Task.dueDate, Task.completionDate
FROM Task
JOIN Project P on Task.projectId = P.projectId
JOIN User_Task Task2 on Task.taskId = Task2.taskId
WHERE Task2.uName = 'Kish' AND P.projectId = '1';

SELECT * FROM [User]