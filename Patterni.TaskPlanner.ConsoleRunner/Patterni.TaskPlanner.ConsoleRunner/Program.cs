using Patterni.TaskPlanner.Domain.Models;
using Patterni.TaskPlanner.Domain.Logic;
using Patterni.TaskPlanner.DataAccess;
using Patterni.TaskPlanner.DataAccess.Abstractions;

internal static class Program
{
    private static FileWorkItemsRepository repository = new FileWorkItemsRepository();
    private static SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner(repository);

    public static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nChoose an operation:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");

            string choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "A":
                    AddWorkItem();
                    break;
                case "B":
                    BuildPlan();
                    break;
                case "M":
                    MarkAsCompleted(repository);
                    break;
                case "R":
                    RemoveWorkItem(repository);
                    break;
                case "Q":
                    exit = true;
                    repository.SaveChanges();
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    // Метод для додавання нового WorkItem
    private static void AddWorkItem()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter description: ");
        string description = Console.ReadLine();

        Console.Write("Enter creation date (yyyy-MM-dd): ");
        DateTime creationDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Enter due date (yyyy-MM-dd): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Enter priority (None, Low, Medium, High, Urgent): ");
        Priority priority = Enum.Parse<Priority>(Console.ReadLine(), true);

        Console.Write("Enter complexity (None, Minutes, Hours, Days, Weeks): ");
        Complexity complexity = Enum.Parse<Complexity>(Console.ReadLine(), true);

        var workItem = new WorkItem
        {
            Title = title,
            Description = description,
            CreationDate = creationDate,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            IsCompleted = false
        };

        repository.Add(workItem);
        repository.SaveChanges();

        Console.WriteLine("Work item added successfully.");
    }

    // Метод для побудови плану
    private static void BuildPlan()
    {
        var sortedWorkItems = taskPlanner.CreatePlan(); // Викликаємо метод без аргументів

        Console.WriteLine("\n--- Sorted Work Items(Incomplete) ---");
        foreach (var workItem in sortedWorkItems)
        {
            Console.WriteLine(workItem);
        }
    }

    private static void MarkAsCompleted(IWorkItemsRepository repository)
    {
        var workItems = repository.GetAll()
                                  .Where(w => !w.IsCompleted)
                                  .ToArray();  // Фільтруємо лише незавершені завдання

        if (workItems.Length == 0)
        {
            Console.WriteLine("No incomplete work items available.");
            return;
        }

        Console.WriteLine("Select a work item to mark as completed:");
        for (int i = 0; i < workItems.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {workItems[i].Title} - Due: {workItems[i].DueDate.ToShortDateString()}");
        }

        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= workItems.Length)
        {
            var selectedWorkItem = workItems[index - 1];
            selectedWorkItem.IsCompleted = true;
            repository.Update(selectedWorkItem);
            repository.SaveChanges();
            Console.WriteLine("Work item marked as completed.");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }
    private static void RemoveWorkItem(IWorkItemsRepository repository)
    {
        var workItems = repository.GetAll();

        if (workItems.Length == 0)
        {
            Console.WriteLine("No work items available.");
            return;
        }

        Console.WriteLine("Select a work item to remove:");
        for (int i = 0; i < workItems.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {workItems[i].Title} - Due: {workItems[i].DueDate.ToShortDateString()}");
        }

        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= workItems.Length)
        {
            var selectedWorkItem = workItems[index - 1];
            repository.Remove(selectedWorkItem.Id);
            repository.SaveChanges();
            Console.WriteLine("Work item removed.");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }




}
