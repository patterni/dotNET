using Patterni.TaskPlanner.DataAccess.Abstractions;
using Patterni.TaskPlanner.Domain.Models;

namespace Patterni.TaskPlanner.Domain.Logic
{

    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository;
        }

        public WorkItem[] CreatePlan()
        {
            var workItems = _repository.GetAll();

            return workItems
                .Where(w => !w.IsCompleted) 
                .OrderByDescending(w => w.Priority) 
                .ThenBy(w => w.DueDate)              
                .ThenBy(w => w.Title)             
                .ToArray();
        }
    }
}