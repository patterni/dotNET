using Patterni.TaskPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterni.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] workItems)
        {
            return workItems
                .OrderByDescending(w => w.Priority) // Спершу сортуємо за пріоритетом у порядку спадання
                .ThenBy(w => w.DueDate)              // Далі сортуємо за DueDate у порядку зростання
                .ThenBy(w => w.Title)                // В кінці сортуємо за назвою в алфавітному порядку
                .ToArray();
        }
    }
}
