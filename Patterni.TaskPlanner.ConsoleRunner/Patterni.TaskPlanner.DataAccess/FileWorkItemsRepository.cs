using Newtonsoft.Json;
using Patterni.TaskPlanner.DataAccess.Abstractions;
using Patterni.TaskPlanner.Domain.Models;


namespace Patterni.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FilePath = "work-items.json";

        private readonly Dictionary<Guid, WorkItem> _workItems;

        public FileWorkItemsRepository()
        {
            if (File.Exists(FilePath) && new FileInfo(FilePath).Length > 0)
            {
                string json = File.ReadAllText(FilePath);
                var workItemsArray = JsonConvert.DeserializeObject<WorkItem[]>(json);
                _workItems = workItemsArray.ToDictionary(item => item.Id, item => item);
            }
            else
            {
                _workItems = new Dictionary<Guid, WorkItem>();
            }
        }

        public Guid Add(WorkItem workItem)
        {
            var newWorkItem = workItem.Clone();
            newWorkItem.Id = Guid.NewGuid();
            _workItems.Add(newWorkItem.Id, newWorkItem);
            return newWorkItem.Id;
        }

        public WorkItem Get(Guid id)
        {
            return _workItems.TryGetValue(id, out var workItem) ? workItem : null;
        }

        public WorkItem[] GetAll()
        {
            return _workItems.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (_workItems.ContainsKey(workItem.Id))
            {
                _workItems[workItem.Id] = workItem;
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            return _workItems.Remove(id);
        }

        public void SaveChanges()
        {
            var workItemsArray = _workItems.Values.ToArray();
            string json = JsonConvert.SerializeObject(workItemsArray, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        
    }


}
