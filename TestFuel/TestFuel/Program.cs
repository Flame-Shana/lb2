using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CrowdsourcingPlatform.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using CrowdsourcingPlatform.Data;

namespace CrowdsourcingPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var connectionString = config.GetConnectionString("CrowdsourcingPlatform");

            var options = new DbContextOptionsBuilder<CrowdsourcingPlatformContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var context = new CrowdsourcingPlatformContext(options);


            Console.WriteLine("2.1 Выборка всех данных из таблицы на стороне \"Один\" (берём Categories)");
            ShowAllCategories(context);
            Console.ReadLine();

            Console.WriteLine("2.2 Выборка из таблицы на стороне \"Один\" с фильтром (Customers с customer8@example.com)");
            ShowFilteredCustomers(context);
            Console.ReadLine();

            Console.WriteLine("2.3 Группировка по полю в таблице на стороне \"Многие\" (Tasks) — count, avg");
            ShowTaskGroupingByCategory(context);
            Console.ReadLine();

            Console.WriteLine("2.4 Выборка двух полей из связанных таблиц (Task.Title + Category.CategoryName)");
            ShowTaskTitlesWithCategory(context);
            Console.ReadLine();

            Console.WriteLine("2.5 Выборка из двух таблиц one-to-many с фильтром (Tasks с Budget > 100)");
            ShowHighBudgetTasksWithCustomer(context);
            Console.ReadLine();

            Console.WriteLine("2.6 Вставка в таблицу на стороне \"Один\" (Добавляем Category)");
            var newCategoryId = InsertCategory(context, "Proofreading", "Proofreading and editing tasks");
            Console.ReadLine();

            Console.WriteLine("2.7 Вставка в таблицу на стороне \"Многие\" (Добавляем Task)");
            InsertTask(context, newCategoryId);
            Console.ReadLine();

            Console.WriteLine("2.8 Удаление данных из таблицы на стороне \"Один\" (удаляем Category при отсутствии Tasks)");
            TryDeleteCategory(context, newCategoryId);
            Console.ReadLine();

            Console.WriteLine("2.9 Удаление данных из таблицы на стороне \"Многие\" (удаляем Task)");
            TryDeleteAnyTask(context);
            Console.ReadLine();

            Console.WriteLine("2.10 Обновление записей, удовлетворяющих условию (закрыть просроченные задачи)");
            CloseExpiredTasks(context);
            Console.ReadLine();

            Console.WriteLine("Готово.");
        }


        // -------------------------------------------
        // 2.1
        static void ShowAllCategories(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Все категории ===");
            var categories = context.Categories.ToList();
            foreach (var c in categories)
            {
                Console.WriteLine($"{c.CategoryID}: {c.CategoryName} — {c.CategoryDescription}");
            }
        }


        // -------------------------------------------
        // 2.2
        static void ShowFilteredCustomers(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Customers с customer8@example.com ===");
            var customers = context.Customers
                .Where(c => c.ContactEmail != null && c.ContactEmail.Contains("customer8@example.com"))
                .ToList();

            foreach (var c in customers)
                Console.WriteLine($"{c.CustomerID}: {c.CustomerName} — {c.ContactEmail}");

            Console.WriteLine($"Всего: {customers.Count}");
        }

        // -------------------------------------------
        // 2.3
        static void ShowTaskGroupingByCategory(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Группировка Tasks по CategoryID (count, avg budget) ===");
            var grouping = context.Tasks
            .GroupBy(t => t.CategoryID)
            .Select(g => new {
                CategoryId = g.Key,
                Count = g.Count(),
                AvgBudget = g.Average(t => (decimal?)t.Budget)
            })
            .ToList();


            foreach (var g in grouping)
            {
                Console.WriteLine($"Category {g.CategoryId}: Count={g.Count}, AvgBudget={g.AvgBudget}");
            }
        }

        // -------------------------------------------
        // 2.4
        static void ShowTaskTitlesWithCategory(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Task.Title + Category.CategoryName ===");
            var q = context.Tasks
            .Include(t => t.Category)
            .Select(t => new { t.Title, CategoryName = t.Category.CategoryName })
            .ToList();


            foreach (var x in q)
            {
                Console.WriteLine($"{x.Title} — {x.CategoryName}");
            }
        }

        // -------------------------------------------
        // 2.5
        static void ShowHighBudgetTasksWithCustomer(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Tasks (Budget > 100) + CustomerName ===");
            var q = context.Tasks
            .Include(t => t.Customer)
            .Where(t => t.Budget > 100)
            .Select(t => new { t.Title, t.Budget, CustomerName = t.Customer.CustomerName })
            .ToList();


            foreach (var r in q)
            {
                Console.WriteLine($"{r.Title} — {r.Budget} — Заказчик: {r.CustomerName}");
            }
        }

        // -------------------------------------------
        // 2.6
        static int InsertCategory(CrowdsourcingPlatformContext context, string name, string description)
        {
            Console.WriteLine("\n=== Вставка Category ===");
            var cat = new Category
            {
                CategoryName = name,
                CategoryDescription = description
            };
            context.Categories.Add(cat);
            context.SaveChanges();
            Console.WriteLine($"Добавлена категория ID={cat.CategoryID}");
            return cat.CategoryID;
        }


        // -------------------------------------------
        // 2.7
        static void InsertTask(CrowdsourcingPlatformContext context, int categoryId)
        {
            Console.WriteLine("\n=== Вставка Task (много-сторона) ===");

            var customer = context.Customers.FirstOrDefault();
            if (customer == null)
            {
                customer = new Customer { CustomerName = "Test Customer", ContactEmail = "test@example.com" };
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            var task = new Models.Task
            {
                CategoryID = categoryId,
                CustomerID = customer.CustomerID,
                Title = "Sample proofreading task",
                Description = "Short description",
                Deadline = DateTime.Now.AddDays(7),
                Budget = 75.00m,
                Complexity = "Low",
                CreatedAt = DateTime.Now,
                Status = "Open"
            };

            context.Tasks.Add(task);
            context.SaveChanges();
            Console.WriteLine($"Добавлена задача ID={task.TaskID}");
        }

        // -------------------------------------------
        // 2.8
        static void TryDeleteCategory(CrowdsourcingPlatformContext context, int categoryId)
        {
            Console.WriteLine("\n=== Попытка удалить Category ===");
            var cat = context.Categories
            .Include(c => EF.Property<IEnumerable<Models.Task>>(c, "Tasks"))
            .FirstOrDefault(c => c.CategoryID == categoryId);

            if (cat == null)
            {
                Console.WriteLine($"Категория {categoryId} не найдена.");
                return;
            }

            context.Categories.Remove(cat);
            context.SaveChanges();
            Console.WriteLine($"Категория {categoryId} удалена.");
        }

        // -------------------------------------------
        // 2.9
        static void TryDeleteAnyTask(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Удаление одной Task (много-сторона) ===");
            var task = context.Tasks.FirstOrDefault();
            if (task == null)
            {
                Console.WriteLine("Нет задач для удаления.");
                return;
            }


            var id = task.TaskID;
            context.Tasks.Remove(task);
            context.SaveChanges();
            Console.WriteLine($"Task {id} удалена.");
        }

        // -------------------------------------------
        // 2.10
        static void CloseExpiredTasks(CrowdsourcingPlatformContext context)
        {
            Console.WriteLine("\n=== Закрытие просроченных задач ===");
            var now = DateTime.Now;
            var expired = context.Tasks.Where(t => t.Deadline != null && t.Deadline < now && t.Status != "Closed").ToList();


            foreach (var t in expired)
            {
                Console.WriteLine($"Закрываем задачу {t.TaskID} (" + t.Title + ")");
                t.Status = "Closed";
            }


            context.SaveChanges();
            Console.WriteLine($"Обновлено записей: {expired.Count}");
        }
    }
}