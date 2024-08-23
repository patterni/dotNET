using Patterni.TaskPlanner.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterni.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }


        public override string ToString()
        {
            string formattedDate = DueDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            string priorityString = Priority.ToString().ToLower();

            return $"{Title}: due {formattedDate}, {priorityString} priority";
        }
    }
}
