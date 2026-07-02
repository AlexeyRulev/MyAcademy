using System.Globalization;
using System.Text;

namespace DataType_Task_1
{

  public class InterestCalculator
  {
    /// <summary>
    /// Формирует строку с расчетом сложных процентов по годам.
    /// </summary>
    /// <param name="initialDeposit">Начальный вклад (положительное число).</param>
    /// <param name="years">Количество лет для расчета.</param>
    /// <param name="interestRate">Годовая процентная ставка в процентах.</param>
    /// <returns>Многострочный текст с результатами капитализации по годам.</returns>

    public string CalculateCompoundInterest(double initialDeposit, int years, double interestRate)
    {

      var resultBuilder = new StringBuilder();
      var currentBalance = initialDeposit;

      // В цикле for используем неявную типизацию var для счетчика i (в нашем случае year).
      for (var year = 1; year <= years; year++)
      {
        // Начисляем проценты за текущий год.
        currentBalance += currentBalance * (interestRate / 100);

        // Округляем до 2 знаков. InvariantCulture гарантирует точку в качестве разделителя.
        var formattedBalance = currentBalance.ToString("F2", CultureInfo.InvariantCulture);

        // Используем интерполяцию строк вместо сложения через "+".
        resultBuilder.AppendLine($"Год {year}: {formattedBalance} руб.");
      }

      // Возвращаем строку, удаляя последний перенос строки.
      return resultBuilder.ToString().TrimEnd();
    }
  }
  internal class Program
  {
    static void Main(string[] args)
    {
      // Создаем экземпляр класса, используя краткую форму new().
      InterestCalculator calculator = new();

      // Вызываем метод. Переменная явно типизирована, так как результат метода не очевиден из названия.
      string report = calculator.CalculateCompoundInterest(1000, 3, 10);

      Console.WriteLine(report);
    }
  }
}
