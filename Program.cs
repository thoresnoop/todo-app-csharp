using System.Text.Json;

enum TaskStatus
{
    Pending = 0,
    Done = 1
}

class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public TaskStatus Status { get; set; }
}

class Program
{
    private const string DataFile = "tasks.json";

    static void Main()
    {
        var tasks = LoadTasks();

        while (true)
        {
            Console.WriteLine("\n=== TODO APP ===");
            Console.WriteLine("1) Listar tarefas");
            Console.WriteLine("2) Adicionar tarefa");
            Console.WriteLine("3) Concluir tarefa");
            Console.WriteLine("4) Remover tarefa");
            Console.WriteLine("0) Sair");
            Console.Write("Escolha: ");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ListTasks(tasks);
                    break;

                case "2":
                    AddTask(tasks);
                    SaveTasks(tasks);
                    break;

                case "3":
                    MarkDone(tasks);
                    SaveTasks(tasks);
                    break;

                case "4":
                    RemoveTask(tasks);
                    SaveTasks(tasks);
                    break;

                case "0":
                    Console.WriteLine("Até mais! 👋");
                    return;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void ListTasks(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Nenhuma tarefa cadastrada.");
            return;
        }

        Console.WriteLine("\n--- Tarefas ---");
        foreach (var t in tasks.OrderBy(t => t.Id))
        {
            Console.WriteLine($"{t.Id}) [{t.Status}] {t.Title}");
        }
    }

    static void AddTask(List<TaskItem> tasks)
    {
        Console.Write("Título da tarefa: ");
        var title = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Título não pode ser vazio.");
            return;
        }

        var nextId = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

        tasks.Add(new TaskItem
        {
            Id = nextId,
            Title = title,
            Status = TaskStatus.Pending
        });

        Console.WriteLine("Tarefa adicionada ✅");
    }

    static void MarkDone(List<TaskItem> tasks)
    {
        Console.Write("ID para concluir: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            Console.WriteLine("Tarefa não encontrada.");
            return;
        }

        task.Status = TaskStatus.Done;
        Console.WriteLine("Concluída ✅");
    }

    static void RemoveTask(List<TaskItem> tasks)
    {
        Console.Write("ID para remover: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            Console.WriteLine("Tarefa não encontrada.");
            return;
        }

        tasks.Remove(task);
        Console.WriteLine("Removida 🗑️");
    }

    static void SaveTasks(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(DataFile, json);
    }

    static List<TaskItem> LoadTasks()
    {
        if (!File.Exists(DataFile))
            return new List<TaskItem>();

        var json = File.ReadAllText(DataFile);

        return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
    }
}
