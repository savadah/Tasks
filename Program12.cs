using System;

public class MyVector<T>
{
    private T[] elementData;
    private int elementCount;
    private int capacityIncrement;

    public MyVector(int initialCapacity, int capacityIncrement)
    {
        if (initialCapacity < 0) throw new ArgumentOutOfRangeException("initialCapacity");
        if (capacityIncrement < 0) throw new ArgumentOutOfRangeException("capacityIncrement");

        elementData = new T[initialCapacity];
        elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }

    public MyVector(int initialCapacity) : this(initialCapacity, 0) { }
    public MyVector() : this(10, 0) { }

    public MyVector(T[] a)
    {
        if (a == null) throw new ArgumentNullException("a");

        elementData = new T[a.Length];
        for (int i = 0; i < a.Length; i++)
            elementData[i] = a[i];

        elementCount = a.Length;
        capacityIncrement = 0;
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (elementData.Length >= minCapacity) return;

        int newCapacity;
        if (elementData.Length == 0)
            newCapacity = minCapacity > 1 ? minCapacity : 1;
        else if (capacityIncrement > 0)
            newCapacity = elementData.Length + capacityIncrement;
        else
            newCapacity = elementData.Length * 2;

        if (newCapacity < minCapacity) newCapacity = minCapacity;

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

    public int Size()
    {
        return elementCount;
    }

    public bool IsEmpty()
    {
        return elementCount == 0;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException("index");
        return elementData[index];
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException("index");

        T removed = elementData[index];
        for (int i = index + 1; i < elementCount; i++)
            elementData[i - 1] = elementData[i];

        elementCount--;
        return removed;
    }
}

public class MyStack<T> : MyVector<T>
{
    public MyStack() : base() { }
    public MyStack(int initialCapacity) : base(initialCapacity) { }
    public MyStack(int initialCapacity, int capacityIncrement) : base(initialCapacity, capacityIncrement) { }
    public MyStack(T[] a) : base(a) { }

    public void Push(T item)
    {
        Add(item);
    }

    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty");

        return Remove(Size() - 1);
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty");

        return Get(Size() - 1);
    }

    public bool Empty()
    {
        return IsEmpty();
    }

    public int Search(T item)
    {
        int pos = 1;
        for (int i = Size() - 1; i >= 0; i--)
        {
            if (Equals(Get(i), item))
                return pos;
            pos++;
        }
        return -1;
    }
}

class Program
{
    static void Main()
    {
        MyStack<int> st = new MyStack<int>();

        st.Push(10);
        st.Push(20);
        st.Push(30);

        Console.WriteLine("Peek = " + st.Peek());       
        Console.WriteLine("Search 30 = " + st.Search(30)); 
        Console.WriteLine("Search 20 = " + st.Search(20)); 
        Console.WriteLine("Search 10 = " + st.Search(10)); 
        Console.WriteLine("Search 99 = " + st.Search(99)); 

        Console.WriteLine("Pop = " + st.Pop());        
        Console.WriteLine("Pop = " + st.Pop());       
        Console.WriteLine("Empty = " + st.Empty());     
        Console.WriteLine("Pop = " + st.Pop());         
        Console.WriteLine("Empty = " + st.Empty());     
    }
}
