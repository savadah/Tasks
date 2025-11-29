using System;
using System.Collections.Generic;

public class MyPriorityQueue<T>
{
    private T[] queue;
    private int size;
    private IComparer<T> comparator;

    // 1) Конструктор с начальной ёмкостью 11
    public MyPriorityQueue()
    {
        queue = new T[11];
        size = 0;
        comparator = Comparer<T>.Default;
    }

    // 2) Конструктор из массива
    public MyPriorityQueue(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        queue = new T[a.Length];
        Array.Copy(a, queue, a.Length);
        size = a.Length;
        comparator = Comparer<T>.Default;

        BuildHeap();
    }

    // 3) Конструктор с указанной ёмкостью
    public MyPriorityQueue(int initialCapacity)
    {
        if (initialCapacity < 1) throw new ArgumentException("Ёмкость должна быть положительной");

        queue = new T[initialCapacity];
        size = 0;
        comparator = Comparer<T>.Default;
    }

    // 4) Конструктор с ёмкостью и компаратором
    public MyPriorityQueue(int initialCapacity, IComparer<T> comparator)
    {
        if (initialCapacity < 1) throw new ArgumentException("Ёмкость должна быть положительной");

        queue = new T[initialCapacity];
        size = 0;
        this.comparator = comparator ?? Comparer<T>.Default;
    }

    // 5) Копирующий конструктор
    public MyPriorityQueue(MyPriorityQueue<T> c)
    {
        if (c == null) throw new ArgumentNullException("Очередь не может быть null");

        queue = new T[c.queue.Length];
        Array.Copy(c.queue, queue, c.size);
        size = c.size;
        comparator = c.comparator;
    }

    // 6) Добавление элемента
    public void Add(T e)
    {
        if (e == null) throw new ArgumentNullException("Элемент не может быть null");

        EnsureCapacity();
        queue[size] = e;
        size++;
        SiftUp(size - 1);
    }

    // 7) Добавление массива элементов
    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        foreach (T item in a)
        {
            Add(item);
        }
    }

    // 8) Очистка очереди
    public void Clear()
    {
        for (int i = 0; i < size; i++)
        {
            queue[i] = default(T);
        }
        size = 0;
    }

    // 9) Проверка наличия элемента
    public bool Contains(object o)
    {
        if (o == null) return false;

        for (int i = 0; i < size; i++)
        {
            if (queue[i].Equals((T)o))
            {
                return true;
            }
        }
        return false;
    }

    // 10) Проверка наличия всех элементов
    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        foreach (T item in a)
        {
            if (!Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    // 11) Проверка пустоты
    public bool IsEmpty()
    {
        return size == 0;
    }

    // 12) Удаление элемента
    public bool Remove(object o)
    {
        if (o == null) return false;

        for (int i = 0; i < size; i++)
        {
            if (queue[i].Equals((T)o))
            {
                queue[i] = queue[size - 1];
                size--;
                SiftDown(i);
                return true;
            }
        }
        return false;
    }

    // 13) Удаление всех указанных элементов
    public bool RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        bool changed = false;
        foreach (T item in a)
        {
            if (Remove(item))
            {
                changed = true;
            }
        }
        return changed;
    }

    // 14) Оставить только указанные элементы
    public bool RetainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        bool changed = false;
        for (int i = size - 1; i >= 0; i--)
        {
            bool found = false;
            foreach (T item in a)
            {
                if (queue[i].Equals(item))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Remove(queue[i]);
                changed = true;
            }
        }
        return changed;
    }

    // 15) Получение размера
    public int Size()
    {
        return size;
    }

    // 16) Преобразование в массив
    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(queue, result, size);
        return result;
    }

    // 17) Преобразование в массив с указанием типа
    public T[] ToArray(T[] a)
    {
        if (a == null)
        {
            return ToArray();
        }

        if (a.Length < size)
        {
            a = new T[size];
        }

        Array.Copy(queue, a, size);
        if (a.Length > size)
        {
            a[size] = default(T);
        }
        return a;
    }

    // 18) Получение головного элемента (с исключением)
    public T Element()
    {
        if (size == 0) throw new InvalidOperationException("Очередь пуста");
        return queue[0];
    }

    // 19) Попытка добавления
    public bool Offer(T obj)
    {
        try
        {
            Add(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // 20) Получение головного элемента (без исключения)
    public T Peek()
    {
        if (size == 0) return default(T);
        return queue[0];
    }

    // 21) Удаление и возврат головного элемента
    public T Poll()
    {
        if (size == 0) return default(T);

        T result = queue[0];
        queue[0] = queue[size - 1];
        size--;
        SiftDown(0);
        return result;
    }

    // Вспомогательные методы
    private void EnsureCapacity()
    {
        if (size == queue.Length)
        {
            int newCapacity;
            if (queue.Length < 64)
            {
                newCapacity = queue.Length + 2;
            }
            else
            {
                newCapacity = queue.Length + queue.Length / 2;
            }

            T[] newQueue = new T[newCapacity];
            Array.Copy(queue, newQueue, size);
            queue = newQueue;
        }
    }

    private void BuildHeap()
    {
        for (int i = size / 2 - 1; i >= 0; i--)
        {
            SiftDown(i);
        }
    }

    private void SiftUp(int index)
    {
        int current = index;
        while (current > 0)
        {
            int parent = (current - 1) / 2;
            if (comparator.Compare(queue[parent], queue[current]) <= 0)
                break;

            T temp = queue[current];
            queue[current] = queue[parent];
            queue[parent] = temp;
            current = parent;
        }
    }

    private void SiftDown(int index)
    {
        int current = index;
        while (true)
        {
            int left = 2 * current + 1;
            int right = 2 * current + 2;
            int smallest = current;

            if (left < size && comparator.Compare(queue[left], queue[smallest]) < 0)
            {
                smallest = left;
            }

            if (right < size && comparator.Compare(queue[right], queue[smallest]) < 0)
            {
                smallest = right;
            }

            if (smallest == current)
                break;

            T temp = queue[current];
            queue[current] = queue[smallest];
            queue[smallest] = temp;
            current = smallest;
        }
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ВСЕХ 5 КОНСТРУКТОРОВ ===\n");

        // 1. Конструктор MyPriorityQueue() - пустая очередь с ёмкостью 11
        Console.WriteLine("1. Конструктор MyPriorityQueue():");
        var queue1 = new MyPriorityQueue<int>();
        Console.WriteLine($"   Создана пустая очередь, ёмкость: 11");
        Console.WriteLine($"   IsEmpty(): {queue1.IsEmpty()}");
        Console.WriteLine($"   Size(): {queue1.Size()}");
        Console.WriteLine($"   Peek(): {queue1.Peek()} (default для int)");

        queue1.Add(5);
        queue1.Add(3);
        queue1.Add(8);
        Console.WriteLine($"   После добавления [5, 3, 8]:");
        Console.WriteLine($"   Peek(): {queue1.Peek()} (минимум)");
        Console.WriteLine($"   Size(): {queue1.Size()}");
        Console.WriteLine();

        // 2. Конструктор MyPriorityQueue(T[] a) - из массива
        Console.WriteLine("2. Конструктор MyPriorityQueue(T[] a):");
        int[] numbers = { 15, 7, 22, 4, 10 };
        var queue2 = new MyPriorityQueue<int>(numbers);
        Console.WriteLine($"   Создана из массива: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"   IsEmpty(): {queue2.IsEmpty()}");
        Console.WriteLine($"   Size(): {queue2.Size()}");
        Console.WriteLine($"   Peek(): {queue2.Peek()} (минимум из массива)");

        Console.Write("   Элементы в порядке приоритета: ");
        while (!queue2.IsEmpty())
        {
            Console.Write(queue2.Poll() + " ");
        }
        Console.WriteLine("\n");

        // 3. Конструктор MyPriorityQueue(int initialCapacity) - с указанной ёмкостью
        Console.WriteLine("3. Конструктор MyPriorityQueue(int initialCapacity):");
        var queue3 = new MyPriorityQueue<string>(5);
        Console.WriteLine($"   Создана пустая очередь, ёмкость: 5");
        Console.WriteLine($"   IsEmpty(): {queue3.IsEmpty()}");

        queue3.Add("яблоко");
        queue3.Add("банан");
        queue3.Add("вишня");
        Console.WriteLine($"   После добавления строк:");
        Console.WriteLine($"   Peek(): '{queue3.Peek()}' (первая по алфавиту)");
        Console.WriteLine($"   Size(): {queue3.Size()}");
        Console.WriteLine();

        // 4. Конструктор с компаратором
        Console.WriteLine("4. Конструктор MyPriorityQueue(int initialCapacity, IComparer<T> comparator):");
        var queue4 = new MyPriorityQueue<int>(10, Comparer<int>.Default);
        Console.WriteLine($"   Создана очередь с обратным компаратором (max-куча)");

        queue4.Add(5);
        queue4.Add(3);
        queue4.Add(8);
        queue4.Add(1);
        Console.WriteLine($"   Добавлены элементы: [5, 3, 8, 1]");
        Console.WriteLine($"   Peek(): {queue4.Peek()} (максимум, а не минимум!)");
        Console.WriteLine($"   Size(): {queue4.Size()}");
        Console.WriteLine();

        // 5. Копирующий конструктор
        Console.WriteLine("5. Конструктор MyPriorityQueue(MyPriorityQueue<T> c):");
        var originalQueue = new MyPriorityQueue<int>(new int[] { 25, 10, 40, 5 });
        var queue5 = new MyPriorityQueue<int>(originalQueue);
        Console.WriteLine($"   Создана копия существующей очереди");
        Console.WriteLine($"   Оригинал Size(): {originalQueue.Size()}");
        Console.WriteLine($"   Копия Size(): {queue5.Size()}");
        Console.WriteLine($"   Оригинал Peek(): {originalQueue.Peek()}");
        Console.WriteLine($"   Копия Peek(): {queue5.Peek()}");

        // Копия
        queue5.Poll();
        Console.WriteLine($"   После удаления из копии:");
        Console.WriteLine($"   Оригинал Size(): {originalQueue.Size()} (не изменился)");
        Console.WriteLine($"   Копия Size(): {queue5.Size()} (уменьшился)");
        Console.WriteLine();

        // Тестовая очередь для демонстрации
        var testQueue = new MyPriorityQueue<string>(new string[] { "яблоко", "банан", "апельсин", "виноград" });

        // 6. add(T e) 
        Console.WriteLine("6. add(T e):");
        testQueue.Add("киви");
        Console.WriteLine($"   После add('киви'): [{string.Join(", ", testQueue.ToArray())}]");

        // 7. addAll(T[] a)
        Console.WriteLine("\n7. addAll(T[] a):");
        string[] newFruits = { "груша", "персик" };
        testQueue.AddAll(newFruits);
        Console.WriteLine($"   После addAll(['груша', 'персик']):");
        Console.WriteLine($"   Элементы: [{string.Join(", ", testQueue.ToArray())}]");

        // 8. clear()
        Console.WriteLine("\n8. clear():");
        var clearQueue = new MyPriorityQueue<int>(new int[] { 1, 2, 3 });
        Console.WriteLine($"   До clear: Size() = {clearQueue.Size()}, IsEmpty() = {clearQueue.IsEmpty()}");
        clearQueue.Clear();
        Console.WriteLine($"   После clear: Size() = {clearQueue.Size()}, IsEmpty() = {clearQueue.IsEmpty()}");

        // 9. contains(object o)
        Console.WriteLine("\n9. contains(object o):");
        Console.WriteLine($"   contains('банан'): {testQueue.Contains("банан")}");
        Console.WriteLine($"   contains('манго'): {testQueue.Contains("манго")}");

        // 10. containsAll(T[] a)
        Console.WriteLine("\n10. containsAll(T[] a):");
        string[] checkArray = { "яблоко", "банан" };
        string[] checkArray2 = { "яблоко", "манго" };
        Console.WriteLine($"   containsAll(['яблоко', 'банан']): {testQueue.ContainsAll(checkArray)}");
        Console.WriteLine($"   containsAll(['яблоко', 'манго']): {testQueue.ContainsAll(checkArray2)}");

        // 11. isEmpty()
        Console.WriteLine("\n11. isEmpty():");
        Console.WriteLine($"   testQueue.IsEmpty(): {testQueue.IsEmpty()}");
        Console.WriteLine($"   clearQueue.IsEmpty(): {clearQueue.IsEmpty()}");

        // 12. remove(object o)
        Console.WriteLine("\n12. remove(object o):");
        Console.WriteLine($"   До remove('банан'): [{string.Join(", ", testQueue.ToArray())}]");
        bool removed = testQueue.Remove("банан");
        Console.WriteLine($"   remove('банан'): {removed}");
        Console.WriteLine($"   После remove('банан'): [{string.Join(", ", testQueue.ToArray())}]");

        // 13. removeAll(T[] a)
        Console.WriteLine("\n13. removeAll(T[] a):");
        var removeAllQueue = new MyPriorityQueue<string>(new string[] { "яблоко", "банан", "апельсин", "виноград" });
        string[] toRemove = { "банан", "виноград" };
        Console.WriteLine($"   До removeAll: [{string.Join(", ", removeAllQueue.ToArray())}]");
        bool allRemoved = removeAllQueue.RemoveAll(toRemove);
        Console.WriteLine($"   removeAll(['банан', 'виноград']): {allRemoved}");
        Console.WriteLine($"   После removeAll: [{string.Join(", ", removeAllQueue.ToArray())}]");

        // 14. retainAll(T[] a)
        Console.WriteLine("\n14. retainAll(T[] a):");
        var retainQueue = new MyPriorityQueue<string>(new string[] { "яблоко", "банан", "апельсин", "виноград" });
        string[] toRetain = { "яблоко", "апельсин" };
        Console.WriteLine($"   До retainAll: [{string.Join(", ", retainQueue.ToArray())}]");
        bool retained = retainQueue.RetainAll(toRetain);
        Console.WriteLine($"   retainAll(['яблоко', 'апельсин']): {retained}");
        Console.WriteLine($"   После retainAll: [{string.Join(", ", retainQueue.ToArray())}]");

        // 15. size()
        Console.WriteLine("\n15. size():");
        Console.WriteLine($"   testQueue.Size(): {testQueue.Size()}");

        // 16. toArray()
        Console.WriteLine("\n16. toArray():");
        string[] array = testQueue.ToArray();
        Console.WriteLine($"   toArray(): [{string.Join(", ", array)}]");

        // 17. toArray(T[] a)
        Console.WriteLine("\n17. toArray(T[] a):");
        string[] targetArray = new string[10]; // Массив большего размера
        string[] resultArray = testQueue.ToArray(targetArray);
        Console.WriteLine($"   toArray(массив[10]): [{string.Join(", ", resultArray)}]");
        Console.WriteLine($"   Длина результата: {resultArray.Length}");

        // 18. element()
        Console.WriteLine("\n18. element():");
        try
        {
            string element = testQueue.Element();
            Console.WriteLine($"   element(): {element}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"   element(): Исключение - {ex.Message}");
        }

        // 19. offer(T obj)
        Console.WriteLine("\n19. offer(T obj):");
        bool offered = testQueue.Offer("манго");
        Console.WriteLine($"   offer('манго'): {offered}");
        Console.WriteLine($"   После offer: [{string.Join(", ", testQueue.ToArray())}]");

        // 20. peek()
        Console.WriteLine("\n20. peek():");
        string peeked = testQueue.Peek();
        Console.WriteLine($"   peek(): {peeked}");

        // 21. poll()
        Console.WriteLine("\n21. poll():");
        Console.WriteLine($"   До poll: [{string.Join(", ", testQueue.ToArray())}]");
        string polled = testQueue.Poll();
        Console.WriteLine($"   poll(): {polled}");
        Console.WriteLine($"   После poll: [{string.Join(", ", testQueue.ToArray())}]");
    }
}