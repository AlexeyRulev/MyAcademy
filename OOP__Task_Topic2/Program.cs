using System.Globalization;

namespace OOP__Task_Topic2
{

  /// <summary>
  /// Описывает товар на складе или в магазине.
  /// </summary>
  public class Product
  {
    // Используем встроенные ключевые слова типов и свойства в PascalCase
    public string Name { get; set; }
    public string Manufacturer { get; set; }

    // Для цен в C# всегда используется тип decimal, а не double, чтобы избежать ошибок округления
    public decimal Price { get; set; }

    // Срок годности можно хранить в днях (int)
    public int ShelfLifeDays { get; set; }
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Переопределяет стандартный метод ToString для красивого вывода информации о товаре.
    /// </summary>
    public override string ToString()
    {
      // Используем сырой строковый литерал (raw string literal) и интерполяцию для наглядного макета текста
      return $"""
                ========================================
                Товар:           {Name}
                Производитель:   {Manufacturer}
                Цена:            {Price.ToString("F2", CultureInfo.InvariantCulture)} руб.
                Срок годности:   {ShelfLifeDays} дней
                Дата производства: {ProductionDate.ToString("dd.MM.yyyy")}
                ========================================
                """;
    }
  }

  internal class Program
  {
    static void Main(string[] args)
    {
      // Устанавливаем инвариантную культуру для консоли, чтобы ввод чисел с точкой/запятой не ломал программу
      CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

      // Создаем экземпляр класса, используя современную краткую форму new()
      Product product = new();

      Console.WriteLine("=== Ввод данных о товаре ===");

      Console.Write("Введите наименование товара: ");
      product.Name = Console.ReadLine();

      Console.Write("Введите производителя: ");
      product.Manufacturer = Console.ReadLine();

      Console.Write("Введите цену (например, 150.50): ");
      product.Price = Convert.ToDecimal(Console.ReadLine());

      Console.Write("Введите срок годности (в днях): ");
      product.ShelfLifeDays = Convert.ToInt32(Console.ReadLine());

      Console.Write("Введите дату производства (в формате ГГГГ-ММ-ДД): ");
      product.ProductionDate = Convert.ToDateTime(Console.ReadLine());

      Console.WriteLine("\n=== Результат работы программы ===");

      // Выводим объект на консоль. Метод ToString() вызовется автоматически под капотом
      Console.WriteLine(product);
    }
  }
}
