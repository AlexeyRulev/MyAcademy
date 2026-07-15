using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace Studio3D_DbApp
{

  // 1. МОДЕЛЬ ДАННЫХ
  public class Customer
  {
    public int CustomerID { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public DateTime RegistrationDate { get; set; }
  }


  // 2. ENTITY FRAMEWORK CONTEXT
  public class Studio3DDbContext : DbContext
  {
    private readonly string _connectionString;

    public Studio3DDbContext(string connectionString)
    {
      _connectionString = connectionString;
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_connectionString);
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      // Чтение строки подключения из appsettings.json
      var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

      IConfiguration config = builder.Build();
      string connectionString = config.GetConnectionString("DefaultConnection");

      Console.WriteLine("---ТЕСТ ADO.NET ---");
      TestAdoNet(connectionString);

      Console.WriteLine("\n--- ТЕСТ ENTITY FRAMEWORK ---");
      TestEntityFramework(connectionString);

      Console.WriteLine("\nПрограмма завершена.");
    }

    // РЕАЛИЗАЦИЯ CRUD ЧЕРЕЗ ADO.NET 
    static void TestAdoNet(string connString)
    {
      using (SqlConnection conn = new SqlConnection(connString))
      {
        conn.Open();

        // CREATE
        string insertSql = "INSERT INTO Customers (FullName, Phone) VALUES (@Name, @Phone); SELECT SCOPE_IDENTITY();";
        using (SqlCommand cmd = new SqlCommand(insertSql, conn))
        {
          cmd.Parameters.AddWithValue("@Name", "Новый Клиент ADO");
          cmd.Parameters.AddWithValue("@Phone", "+70001112233");
          var newId = Convert.ToInt32(cmd.ExecuteScalar());
          Console.WriteLine($"[ADO] Добавлен клиент с ID: {newId}");
        }

        // READ
        string selectSql = "SELECT TOP 1 CustomerID, FullName, Phone FROM Customers ORDER BY CustomerID DESC";
        int lastId = 0;
        using (SqlCommand cmd = new SqlCommand(selectSql, conn))
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
          if (reader.Read())
          {
            lastId = reader.GetInt32(0);
            Console.WriteLine($"[ADO] Прочитан клиент: {reader.GetString(1)} ({reader.GetString(2)})");
          }
        }

        // UPDATE
        if (lastId > 0)
        {
          string updateSql = "UPDATE Customers SET Phone = @Phone WHERE CustomerID = @Id";
          using (SqlCommand cmd = new SqlCommand(updateSql, conn))
          {
            cmd.Parameters.AddWithValue("@Phone", "+79999999999");
            cmd.Parameters.AddWithValue("@Id", lastId);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[ADO] Телефон клиента {lastId} обновлен.");
          }
        }

        // DELETE
        if (lastId > 0)
        {
          string deleteSql = "DELETE FROM Customers WHERE CustomerID = @Id";
          using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
          {
            cmd.Parameters.AddWithValue("@Id", lastId);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[ADO] Клиент {lastId} удален.");
          }
        }
      }
    }


    // РЕАЛИЗАЦИЯ CRUD ЧЕРЕЗ ENTITY FRAMEWORK
    static void TestEntityFramework(string connString)
    {
      using (var db = new Studio3DDbContext(connString))
      {
        // CREATE
        var newCustomer = new Customer
        {
          FullName = "Новый Клиент EF",
          Phone = "+78887776655",
          RegistrationDate = DateTime.Now
        };
        db.Customers.Add(newCustomer);
        db.SaveChanges();
        Console.WriteLine($"[EF] Добавлен клиент с ID: {newCustomer.CustomerID}");

        // READ
        var customer = db.Customers.OrderByDescending(c => c.CustomerID).FirstOrDefault();
        if (customer != null)
        {
          Console.WriteLine($"[EF] Прочитан клиент: {customer.FullName} ({customer.Phone})");
        }

        // UPDATE
        if (customer != null)
        {
          customer.FullName = "Измененный Клиент EF";
          db.SaveChanges();
          Console.WriteLine($"[EF] Имя клиента {customer.CustomerID} обновлено.");
        }

        // DELETE
        if (customer != null)
        {
          db.Customers.Remove(customer);
          db.SaveChanges();
          Console.WriteLine($"[EF] Клиент {customer.CustomerID} удален.");
        }
      }
    }
  }
}