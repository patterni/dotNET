using Moq;
using Patterni.TaskPlanner.Domain.Models;
using Patterni.TaskPlanner.DataAccess.Abstractions;

namespace Patterni.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        private Mock<IWorkItemsRepository> _mockRepository;
        private SimpleTaskPlanner _taskPlanner;

        public SimpleTaskPlannerTests()
        {
            _mockRepository = new Mock<IWorkItemsRepository>();
            _taskPlanner = new SimpleTaskPlanner(_mockRepository.Object);
        }

        [Fact]
        public void CreatePlan_ShouldSortTasksCorrectly()
        {
            var workItems = new[]
            {
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.Medium, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 2", DueDate = DateTime.Now.AddDays(-1), Priority = Priority.High, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 3", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Low, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 4", DueDate = DateTime.Now.AddDays(-2), Priority = Priority.Urgent, IsCompleted = true }
            };

            _mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var result = _taskPlanner.CreatePlan();

            Assert.Equal(3, result.Length); // Має бути 3 задачі
            Assert.Equal("Task 2", result[0].Title); // Найвищий пріоритет
            Assert.Equal("Task 1", result[1].Title); // Другий за пріоритетом
            Assert.Equal("Task 3", result[2].Title); // Найнижчий пріоритет
        }

        [Fact]
        public void CreatePlan_ShouldIncludeOnlyIncompleteTasks()
        {
            var workItems = new[]
            {
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.Medium, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 2", DueDate = DateTime.Now.AddDays(-1), Priority = Priority.High, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "Task 3", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Low, IsCompleted = true }
            };

            _mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var result = _taskPlanner.CreatePlan();

            Assert.Equal(2, result.Length); // Має бути 2 незавершені задачі
            Assert.All(result, task => Assert.False(task.IsCompleted)); // Переконатися, що жодна задача не завершена
        }
    }
}
