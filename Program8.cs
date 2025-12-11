using System;

public class MyArrayList<T>
{
    private T[] elementData;
    private int size;

    public MyArrayList()
    {
        elementData = new T[10];
        size = 0;
    }

    public MyArrayList(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        elementData = new T[a.Length];
        for (int i = 0; i < a.Length; i++)
            elementData[i] = a[i];

        size = a.Length;
    }

    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException("capacity");

        elementData = new T[capacity];
        size = 0;
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (elementData.Length < minCapacity)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;

            T[] newArray = new T[newCapacity];
            for (int i = 0; i < size; i++)
                newArray[i] = elementData[i];

            elementData = newArray;
        }
    }

    public void Add(T e)
    {
        EnsureCapacity(size + 1);
        elementData[size] = e;
        size++;
    }

    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        EnsureCapacity(size + a.Length);
        for (int i = 0; i < a.Length; i++)
        {
            elementData[size] = a[i];
            size++;
        }
    }

    public void Clear()
    {
        size = 0;
    }

    public bool Contains(object o)
    {
        for (int i = 0; i < size; i++)
        {
            if (Equals(elementData[i], o))
                return true;
        }
        return false;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        for (int i = 0; i < a.Length; i++)
        {
            if (!Contains(a[i]))
                return false;
        }
        return true;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index == -1)
            return false;

        for (int i = index + 1; i < size; i++)
            elementData[i - 1] = elementData[i];

        size--;
        return true;
    }

    public void RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        for (int i = 0; i < a.Length; i++)
        {
            bool removed = true;
            while (removed)
            {
                removed = Remove(a[i]);
            }
        }
    }

    private bool IsInArray(T value, T[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (Equals(value, a[i]))
                return true;
        }
        return false;
    }

    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        int newSize = 0;
        for (int i = 0; i < size; i++)
        {
            if (IsInArray(elementData[i], a))
            {
                elementData[newSize] = elementData[i];
                newSize++;
            }
        }
        size = newSize;
    }

    public int Size()
    {
        return size;
    }

    public T[] ToArray()
    {
        T[] result = new T[size];
        for (int i = 0; i < size; i++)
            result[i] = elementData[i];
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < size)
        {
            T[] result = new T[size];
            for (int i = 0; i < size; i++)
                result[i] = elementData[i];
            return result;
        }
        else
        {
            for (int i = 0; i < size; i++)
                a[i] = elementData[i];
            return a;
        }
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException("index");

        EnsureCapacity(size + 1);

        for (int i = size; i > index; i--)
            elementData[i] = elementData[i - 1];

        elementData[index] = e;
        size++;
    }

    public void AddAll(int index, T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException("index");

        int count = a.Length;
        EnsureCapacity(size + count);

        for (int i = size - 1; i >= index; i--)
            elementData[i + count] = elementData[i];

        for (int i = 0; i < count; i++)
            elementData[index + i] = a[i];

        size += count;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException("index");

        return elementData[index];
    }

    public int IndexOf(object o)
    {
        for (int i = 0; i < size; i++)
        {
            if (Equals(elementData[i], o))
                return i;
        }
        return -1;
    }

    public int LastIndexOf(object o)
    {
        for (int i = size - 1; i >= 0; i--)
        {
            if (Equals(elementData[i], o))
                return i;
        }
        return -1;
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException("index");

        T removed = elementData[index];

        for (int i = index + 1; i < size; i++)
            elementData[i - 1] = elementData[i];

        size--;
        return removed;
    }

    public void Set(int index, T e)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException("index");

        elementData[index] = e;
    }

    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        int count = toIndex - fromIndex;
        T[] subArray = new T[count];

        for (int i = 0; i < count; i++)
            subArray[i] = elementData[fromIndex + i];

        return new MyArrayList<T>(subArray);
    }
    static void PrintList(MyArrayList<int> list, string title)
    {
        Console.WriteLine(title);
        int[] data = list.ToArray();
        for (int i = 0; i < data.Length; i++)
        {
            Console.Write(data[i]);
            if (i != data.Length - 1)
                Console.Write(" ");
        }
        Console.WriteLine();
        Console.WriteLine("size = " + list.Size());
        Console.WriteLine();
    }
}

class Program
{
    static void PrintList(MyArrayList<int> list, string title)
    {
        Console.WriteLine(title);
        int[] data = list.ToArray();
        for (int i = 0; i < data.Length; i++)
        {
            Console.Write(data[i]);
            if (i != data.Length - 1)
                Console.Write(" ");
        }
        Console.WriteLine();
        Console.WriteLine("size = " + list.Size());
        Console.WriteLine();
    }
    static void Main()
    {
        MyArrayList<int> list = new MyArrayList<int>();
        list.Add(10);
        list.Add(20);
        list.Add(30);
        PrintList(list, "После Add(10,20,30):");

        int[] startArray = new int[] { 1, 2, 3, 4 };
        MyArrayList<int> listFromArray = new MyArrayList<int>(startArray);
        PrintList(listFromArray, "Список из массива {1,2,3,4}:");

        MyArrayList<int> listWithCapacity = new MyArrayList<int>(2);
        listWithCapacity.Add(100);
        listWithCapacity.Add(200);
        listWithCapacity.Add(300);
        PrintList(listWithCapacity, "Список с capacity=2 и тремя Add:");

        list.AddAll(new int[] { 40, 50 });
        PrintList(list, "После AddAll({40,50}):");

        bool contains20 = list.Contains(20);
        bool contains99 = list.Contains(99);
        Console.WriteLine("list.Contains(20) = " + contains20);
        Console.WriteLine("list.Contains(99) = " + contains99);
        Console.WriteLine();

        bool containsAll = list.ContainsAll(new int[] { 10, 20, 30 });
        bool containsAll2 = list.ContainsAll(new int[] { 10, 99 });
        Console.WriteLine("list.ContainsAll({10,20,30}) = " + containsAll);
        Console.WriteLine("list.ContainsAll({10,99}) = " + containsAll2);
        Console.WriteLine();

        bool isEmptyBefore = list.IsEmpty();
        Console.WriteLine("list.IsEmpty() до Clear = " + isEmptyBefore);
        list.Clear();
        bool isEmptyAfter = list.IsEmpty();
        Console.WriteLine("list.IsEmpty() после Clear = " + isEmptyAfter);
        Console.WriteLine();

        list.AddAll(new int[] { 1, 2, 3, 2, 4, 2, 5 });
        PrintList(list, "Новый список {1,2,3,2,4,2,5}:");

        bool removed2 = list.Remove((object)(2));
        PrintList(list, "После Remove(2):");
        Console.WriteLine("Remove(2) вернул " + removed2);
        Console.WriteLine();

        list.RemoveAll(new int[] { 2, 5 });
        PrintList(list, "После RemoveAll({2,5}):");

        list.AddAll(new int[] { 1, 2, 3, 4, 5, 6 });
        PrintList(list, "После добавления {1,2,3,4,5,6}:");

        list.RetainAll(new int[] { 1, 3, 5 });
        PrintList(list, "После RetainAll({1,3,5}):");

        list.Clear();
        list.AddAll(new int[] { 10, 20, 30, 40, 50 });
        PrintList(list, "Список {10,20,30,40,50}:");

        list.Add(2, 25);
        PrintList(list, "После Add(2,25):");

        list.AddAll(1, new int[] { 11, 12 });
        PrintList(list, "После AddAll(1,{11,12}):");

        int val0 = list.Get(0);
        int val3 = list.Get(3);
        Console.WriteLine("Get(0) = " + val0);
        Console.WriteLine("Get(3) = " + val3);
        Console.WriteLine();

        int idx30 = list.IndexOf(30);
        int idx100 = list.IndexOf(100);
        Console.WriteLine("IndexOf(30) = " + idx30);
        Console.WriteLine("IndexOf(100) = " + idx100);
        Console.WriteLine();

        list.AddAll(new int[] { 30, 30 });
        PrintList(list, "После добавления ещё двух 30:");
        int lastIdx30 = list.LastIndexOf(30);
        Console.WriteLine("LastIndexOf(30) = " + lastIdx30);
        Console.WriteLine();

        int removedAt2 = list.Remove(2);
        PrintList(list, "После Remove(2):");
        Console.WriteLine("Удалённый элемент = " + removedAt2);
        Console.WriteLine();

        list.Set(0, 999);
        PrintList(list, "После Set(0,999):");

        MyArrayList<int> sub = list.SubList(1, 4);
        PrintList(sub, "SubList(1,4) исходного списка:");
    }
}

    

