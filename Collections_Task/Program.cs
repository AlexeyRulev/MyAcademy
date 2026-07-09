using System;
using System.Collections;
using System.Collections.Generic;

namespace Collections_Task
{
  /// <summary>
  /// Представляет структуру данных "Умный стек" на базе обычного массива.
  /// </summary>
  /// <typeparam name="T">Тип элементов в стеке.</typeparam>
  public class SmartStack<T> : IEnumerable<T>
  {
    private T[] _items;
    private int _count;

    /// <summary>
    /// Возвращает количество элементов, находящихся в стеке.
    /// </summary>
    public int Count
    {
      get
      {
        return _count;
      }
    }

    /// <summary>
    /// Возвращает текущую емкость внутреннего массива.
    /// </summary>
    public int Capacity
    {
      get
      {
        return _items.Length;
      }
    }

    /// <summary>
    /// Позволяет получить или изменить элемент по его глубине (0 - вершина стека, Count-1 - основание).
    /// </summary>
    /// <param name="index">Индекс элемента по глубине стека.</param>
    /// <returns>Элемент на указанной глубине.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если индекс находится вне границ стека.</exception>
    public T this[int index]
    {
      get
      {
        if (index < 0 || index >= _count)
        {
          throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне границ стека.");
        }

        // Вершина стека находится на позиции (_count - 1), поэтому идем вглубь
        return _items[_count - 1 - index];
      }
      set
      {
        if (index < 0 || index >= _count)
        {
          throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне границ стека.");
        }

        _items[_count - 1 - index] = value;
      }
    }

    /// <summary>
    /// Инициализирует новый экземпляр стека с начальной емкостью в 4 элемента.
    /// </summary>
    public SmartStack()
    {
      _items = new T[4];
      _count = 0;
    }

    /// <summary>
    /// Инициализирует новый экземпляр стека с указанной начальной емкостью.
    /// </summary>
    /// <param name="capacity">Начальная емкость стека.</param>
    public SmartStack(int capacity)
    {
      if (capacity < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(capacity), "Емкость не может быть отрицательной.");
      }

      _items = new T[capacity];
      _count = 0;
    }

    /// <summary>
    /// Инициализирует новый экземпляр стека, копируя элементы из указанной коллекции в обратном порядке.
    /// </summary>
    /// <param name="collection">Коллекция, элементы которой будут скопированы в стек.</param>
    public SmartStack(IEnumerable<T> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      _items = new T[4];
      _count = 0;

      // Вызываем исправленный PushRange для соблюдения порядка из ТЗ
      PushRange(collection);
    }

    /// <summary>
    /// Добавляет элемент на вершину стека. При нехватке места емкость массива удваивается.
    /// </summary>
    /// <param name="item">Элемент для добавления.</param>
    public void Push(T item)
    {
      if (_count == _items.Length)
      {
        Resize(_items.Length == 0 ? 4 : _items.Length * 2);
      }

      _items[_count] = item;
      _count++;
    }

    /// <summary>
    /// Добавляет содержимое коллекции на вершину стека в порядке, обратном их следованию в коллекции.
    /// </summary>
    /// <param name="collection">Коллекция элементов для добавления.</param>
    public void PushRange(IEnumerable<T> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      // Переводим во временный список, чтобы иметь возможность обойти коллекцию с конца к началу
      var temp = new List<T>(collection);

      for (var i = temp.Count - 1; i >= 0; i--)
      {
        Push(temp[i]);
      }
    }

    /// <summary>
    /// Удаляет и возвращает элемент с вершины стека.
    /// </summary>
    /// <returns>Элемент, удаленный с вершины стека.</returns>
    /// <exception cref="InvalidOperationException">Выбрасывается, если стек пуст.</exception>
    public T Pop()
    {
      if (_count == 0)
      {
        throw new InvalidOperationException("Стек пуст.");
      }

      _count--;
      var item = _items[_count];

      // Очищаем ячейку для сборщика мусора
      _items[_count] = default;

      return item;
    }

    /// <summary>
    /// Возвращает элемент с вершины стека без его удаления.
    /// </summary>
    /// <returns>Элемент на вершине стека.</returns>
    /// <exception cref="InvalidOperationException">Выбрасывается, если стек пуст.</exception>
    public T Peek()
    {
      if (_count == 0)
      {
        throw new InvalidOperationException("Стек пуст.");
      }

      return _items[_count - 1];
    }

    /// <summary>
    /// Проверяет наличие элемента в стеке.
    /// </summary>
    /// <param name="item">Элемент для поиска.</param>
    /// <returns>Значение true, если элемент найден; иначе — false.</returns>
    public bool Contains(T item)
    {
      var comparer = EqualityComparer<T>.Default;

      for (var i = 0; i < _count; i++)
      {
        if (comparer.Equals(_items[i], item))
        {
          
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Реализация интерфейса IEnumerable для обхода элементов от вершины к основанию.
    /// </summary>
    public IEnumerator<T> GetEnumerator()
    {
      for (var i = _count - 1; i >= 0; i--)
      {
        yield return _items[i];
      }
    }

    /// <summary>
    /// Неявная реализация интерфейса IEnumerable.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    /// Вспомогательный метод для изменения размера внутреннего массива.
    /// </summary>
    private void Resize(int newCapacity)
    {
      var newArray = new T[newCapacity];
      Array.Copy(_items, 0, newArray, 0, _count);
      _items = newArray;
    }


  }

  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("=== Тестирование структуры данных SmartStack ===\n");

      // 1. Проверяем базовый конструктор и добавление элементов
      var stack = new SmartStack<int>();
      stack.Push(10);
      stack.Push(20);
      stack.Push(30);

      Console.WriteLine($"После Push 10, 20, 30. Количество: {stack.Count}, Емкость: {stack.Capacity}");
      Console.WriteLine($"Вершина стека через Peek: {stack.Peek()}");

      // 2. Проверяем индексатор глубины (задание 12)
      Console.WriteLine($"Элемент на глубине 0 (вершина): {stack[0]}"); // Должно быть 30
      Console.WriteLine($"Элемент на глубине 2 (основание): {stack[2]}"); // Должно быть 10

      // 3. Проверяем обход foreach (от вершины к основанию)
      Console.Write("Содержимое стека (от вершины): ");
      foreach (var item in stack)
      {
        Console.Write($"{item} ");
      }
      Console.WriteLine("\n");

      // 4. Проверяем PushRange в обратном порядке (пункт 5)
      var batch = new List<int> { 40, 50, 60 };
      Console.WriteLine("Добавляем коллекцию { 40, 50, 60 } через PushRange.");
      stack.PushRange(batch);

      Console.WriteLine($"Новая вершина стека (должна быть 40, так как порядок обратный): {stack.Peek()}");

      // 5. Проверяем удаление из стека
      Console.WriteLine($"Удаляем элемент через Pop: {stack.Pop()}");
      Console.WriteLine($"Следующий элемент через Pop: {stack.Pop()}");
      Console.WriteLine($"Текущее количество элементов после удаления: {stack.Count}");
      Console.WriteLine($"Убеждаемся, что реальная емкость не уменьшилась (пункт 6): {stack.Capacity}");

      // 6. Проверка Contains
      Console.WriteLine($"Содержит ли стек число 20? {stack.Contains(20)}");
      Console.WriteLine($"Содержит ли стек число 100? {stack.Contains(100)}");

      Console.WriteLine("\nТестирование успешно завершено!");
    }
  }
}