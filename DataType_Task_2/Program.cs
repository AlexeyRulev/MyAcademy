using System.Text;

namespace DataType_Task_2
{

  public class ShapePrinter
  {
    /// <summary>
    /// Формирует строку, содержащую полый ромб из символов 'X'.
    /// </summary>
    /// <param name="diagonalLength">Длина диагонали ромба (положительное нечётное число).</param>
    /// <returns>Многострочный текст с визуализацией ромба.</returns>
    public string DrawRhombus(int diagonalLength)
    {
      // Используем StringBuilder для сборки больших текстов в циклах
      var rhombusBuilder = new StringBuilder();
      var center = diagonalLength / 2;

      // Внешний цикл отвечает за строки (row)
      for (var row = 0; row < diagonalLength; row++)
      {
        // Внутренний цикл отвечает за столбцы (col) в текущей строке
        for (var col = 0; col < diagonalLength; col++)
        {
          // Проверяем, является ли текущая точка центром фигуры
          var isCenterPoint = row == center && col == center;

          // Математическое условие отрисовки границы ромба
          var isRhombusBoundary = Math.Abs(row - center) + Math.Abs(col - center) == center;

          // Если это граница и не сам центр — ставим 'X', иначе — пробел
          if (isRhombusBoundary && !isCenterPoint)
          {
            rhombusBuilder.Append('X');
          }
          else
          {
            rhombusBuilder.Append(' ');
          }
        }

        // Переходим на новую строку после завершения отрисовки текущей строки
        rhombusBuilder.AppendLine();
      }

      // Возвращаем итоговую строку без лишнего переноса на конце
      return rhombusBuilder.ToString().TrimEnd();
    }
  }

  internal class Program
  {
    static void Main(string[] args)
    {
      // Создаем экземпляр класса, используя краткую форму new()
      ShapePrinter printer = new();

      // Генерируем ромб с диагональю 7
      string rhombusImage = printer.DrawRhombus(7);

      // Выводим результат на экран
      Console.WriteLine(rhombusImage);
    }
  }
}
