using Patterni.TaskPlanner.Domain.Models;
using System;
using System.Globalization;

namespace Patterni.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        // Нова властивість Id
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public WorkItem()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            string formattedDate = DueDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            string priorityString = Priority.ToString().ToLower();

            return $"{Title}: due {formattedDate}, {priorityString} priority";
        }

        public WorkItem Clone()
        {
            return new WorkItem
            {
                Id = this.Id, 
                CreationDate = this.CreationDate,
                DueDate = this.DueDate,
                Priority = this.Priority,
                Complexity = this.Complexity,
                Title = this.Title,
                Description = this.Description,
                IsCompleted = this.IsCompleted
            };
        }
    }
}
