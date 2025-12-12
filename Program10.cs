using System;

public class MyVector<T>
{
    private T[] elementData;
    private int elementCount;
    private int capacityIncrement;

    public MyVector(int initialCapacity, int capacityIncrement)
    {
        if (initialCapacity < 0)
            throw new ArgumentOutOfRangeException("initialCapacity");
        if (capacityIncrement < 0)
            throw new ArgumentOutOfRangeException("capacityIncrement");

        elementData = new T[initialCapacity];
        elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }

    public MyVector(int initialCapacity) : this(initialCapacity, 0) { }

    public MyVector() : this(10, 0) { }

    public MyVector(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        elementData = new T[a.Length];
        for (int i = 0; i < a.Length; i++)
            elementData[i] = a[i];

        elementCount = a.Length;
        capacityIncrement = 0;
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (elementData.Length >= minCapacity)
            return;

        int newCapacity;
        if (elementData.Length == 0)
            newCapacity = minCapacity > 1 ? minCapacity : 1;
        else if (capacityIncrement > 0)
            newCapacity = elementData.Length + capacityIncrement;
        else
            newCapacity = elementData.Length * 2;

        if (newCapacity < minCapacity)
            newCapacity = minCapacity;

        T[] newArray = new T[newCapacity];
        for (int i = 0; i < elementCount; i++)
            newArray[i] = elementData[i];

        elementData = newArray;
    }

    public void Add(T e)
    {
        EnsureCapacity(elementCount + 1);
        elementData[elementCount] = e;
        elementCount++;
    }

    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        EnsureCapacity(elementCount + a.Length);
        for (int i = 0; i < a.Length; i++)
        {
            elementData[elementCount] = a[i];
            elementCount++;
        }
    }

    public void Clear()
    {
        elementCount = 0;
    }

    public bool Contains(object o)
    {
        for (int i = 0; i < elementCount; i++)
            if (Equals(elementData[i], o))
                return true;
        return false;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        for (int i = 0; i < a.Length; i++)
            if (!Contains(a[i]))
                return false;
        return true;
    }

    public bool IsEmpty()
    {
        return elementCount == 0;
    }

    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index == -1)
            return false;

        for (int i = index + 1; i < elementCount; i++)
            elementData[i - 1] = elementData[i];

        elementCount--;
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
                removed = Remove(a[i]);
        }
    }

    private bool IsInArray(T value, T[] a)
    {
        for (int i = 0; i < a.Length; i++)
            if (Equals(value, a[i]))
                return true;
        return false;
    }

    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");

        int newSize = 0;
        for (int i = 0; i < elementCount; i++)
        {
            if (IsInArray(elementData[i], a))
            {
                elementData[newSize] = elementData[i];
                newSize++;
            }
        }
        elementCount = newSize;
    }

    public int Size()
    {
        return elementCount;
    }

    public T[] ToArray()
    {
        T[] result = new T[elementCount];
        for (int i = 0; i < elementCount; i++)
            result[i] = elementData[i];
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < elementCount)
        {
            T[] result = new T[elementCount];
            for (int i = 0; i < elementCount; i++)
                result[i] = elementData[i];
            return result;
        }
        else
        {
            for (int i = 0; i < elementCount; i++)
                a[i] = elementData[i];
            return a;
        }
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException("index");

        EnsureCapacity(elementCount + 1);

        for (int i = elementCount; i > index; i--)
            elementData[i] = elementData[i - 1];

        elementData[index] = e;
        elementCount++;
    }

    public void AddAll(int index, T[] a)
    {
        if (a == null)
            throw new ArgumentNullException("a");
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException("index");

        int count = a.Length;
        EnsureCapacity(elementCount + count);

        for (int i = elementCount - 1; i >= index; i--)
            elementData[i + count] = elementData[i];

        for (int i = 0; i < count; i++)
            elementData[index + i] = a[i];

        elementCount += count;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException("index");

        return elementData[index];
    }

    public int IndexOf(object o)
    {
        for (int i = 0; i < elementCount; i++)
            if (Equals(elementData[i], o))
                return i;
        return -1;
    }

    public int LastIndexOf(object o)
    {
        for (int i = elementCount - 1; i >= 0; i--)
            if (Equals(elementData[i], o))
                return i;
        return -1;
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException("index");

        T removed = elementData[index];

        for (int i = index + 1; i < elementCount; i++)
            elementData[i - 1] = elementData[i];

        elementCount--;
        return removed;
    }

    public void Set(int index, T e)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException("index");

        elementData[index] = e;
    }

    public MyVector<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        int count = toIndex - fromIndex;
        T[] subArray = new T[count];

        for (int i = 0; i < count; i++)
            subArray[i] = elementData[fromIndex + i];

        return new MyVector<T>(subArray);
    }

    public T FirstElement()
    {
        if (elementCount == 0)
            throw new InvalidOperationException("Вектор пуст");
        return elementData[0];
    }

    public T LastElement()
    {
        if (elementCount == 0)
            throw new InvalidOperationException("Вектор пуст");
        return elementData[elementCount - 1];
    }

    public void RemoveElementAt(int pos)
    {
        if (pos < 0 || pos >= elementCount)
            throw new ArgumentOutOfRangeException("pos");

        for (int i = pos + 1; i < elementCount; i++)
            elementData[i - 1] = elementData[i];

        elementCount--;
    }

    public void RemoveRange(int begin, int end)
    {
        if (begin < 0 || end > elementCount || begin > end)
            throw new ArgumentOutOfRangeException();

        int count = end - begin;
        if (count <= 0)
            return;

        for (int i = end; i < elementCount; i++)
            elementData[i - count] = elementData[i];

        elementCount -= count;
    }
}

class Program
{
    static void PrintVector(MyVector<int> v, string title)
    {
        Console.WriteLine(title);
        int[] arr = v.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write(arr[i]);
            if (i != arr.Length - 1) Console.Write(" ");
        }
        Console.WriteLine();
        Console.WriteLine("size = " + v.Size());
        Console.WriteLine();
    }

    static void Main()
    {
        MyVector<int> v = new MyVector<int>(2, 3);
        v.Add(10);
        v.Add(20);
        v.Add(30);
        v.Add(40);
        PrintVector(v, "После Add 10 20 30 40:");

        v.AddAll(new int[] { 50, 60, 70, 40 });
        PrintVector(v, "После AddAll 50 60 70:");

        Console.WriteLine("Contains(30) = " + v.Contains(30));
        Console.WriteLine("Contains(99) = " + v.Contains(99));
        Console.WriteLine();

        Console.WriteLine("IndexOf(40) = " + v.IndexOf(40));
        Console.WriteLine("LastIndexOf(40) = " + v.LastIndexOf(40));
        Console.WriteLine();

        v.Add(2, 999);
        PrintVector(v, "После Add(index=2, 999):");

        v.AddAll(1, new int[] { 111, 222 });
        PrintVector(v, "После AddAll(index=1, {111,222}):");

        Console.WriteLine("Get(0) = " + v.Get(0));
        Console.WriteLine("FirstElement() = " + v.FirstElement());
        Console.WriteLine("LastElement() = " + v.LastElement());
        Console.WriteLine();

        int removed = v.Remove(3);
        PrintVector(v, "После Remove(index=3), удалено = " + removed + ":");

        bool removedVal = v.Remove((object)20);
        PrintVector(v, "После Remove(value=20), success = " + removedVal + ":");

        v.RemoveElementAt(0);
        PrintVector(v, "После RemoveElementAt(0):");

        v.RemoveRange(1, 3);
        PrintVector(v, "После RemoveRange(1,3):");

        v.Clear();
        Console.WriteLine("После Clear(): IsEmpty = " + v.IsEmpty());
        Console.WriteLine();

        MyVector<int> fromArr = new MyVector<int>(new int[] { 1, 2, 3, 4, 5 });
        PrintVector(fromArr, "Вектор из массива {1,2,3,4,5}:");

        MyVector<int> sub = fromArr.SubList(1, 4);
        PrintVector(sub, "SubList(1,4) от {1,2,3,4,5}:");

        fromArr.RetainAll(new int[] { 2, 5, 10 });
        PrintVector(fromArr, "После RetainAll({2,5,10}):");
    }
}
