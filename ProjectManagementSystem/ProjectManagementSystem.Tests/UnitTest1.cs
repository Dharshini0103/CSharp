using NUnit.Framework;
using ProjectManagementSystem.dao;
using ProjectManagementSystem.entity;
using System.Collections.Generic;
using NUnit.Framework.Legacy;

namespace ProjectManagementSystem.Tests
{
    [TestFixture]
    public class Tests
    {
        private ProjectRepositoryImpl repo;

        [SetUp]
        public void Setup()
        {
            repo = new ProjectRepositoryImpl();
        }

        //1. Create Employee
        [Test]
        public void CreateEmployee_ShouldReturnTrue()
        {
            var emp = new Employee
            {
                Name = "Tester",
                Designation = "QA",
                Gender = "Female",
                Salary = 40000,
                Project_id = 3 
            };
            bool result = repo.CreateEmployee(emp);
            ClassicAssert.IsTrue(result);
        }

        //2. Create Task
        [Test]
        public void CreateTask_ShouldReturnTrue()
        {
            var task = new entity.Task
            {
                Task_name = "Test Payments",
                Project_id = 3,     
                Employee_id = 4,    
                Status = "Assigned"
            };
            bool result = repo.CreateTask(task);
            ClassicAssert.IsTrue(result);
        }

        // 3. Get tasks assigned to employee
        [Test]
        public void GetTasksForEmployee_ShouldReturnList()
        {
            var searchTask = new entity.Task
            {
                Project_id = 6,
                Employee_id =9 
            };
            var tasks = repo.GetAllTasks(searchTask);
            ClassicAssert.IsNotNull(tasks);
            ClassicAssert.IsTrue(tasks.Count > 0);
        }

        //4. EmployeeNotFoundException 
        [Test]
        public void DeleteEmployee_InvalidId_ShouldReturnFalse()
        {
            var emp = new Employee { Id = 9999 }; 
            bool result = repo.DeleteEmployee(emp);
            ClassicAssert.IsFalse(result);
        }
    }
}
