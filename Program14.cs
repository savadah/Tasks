using System;

public class MyArrayDeque<T>
{
    private T[] elements;
    private int head;
    private int tail;
    private int count;

    public MyArrayDeque()
    {
        elements = new T[16];
        head = 0;
        tail = 0;
        count = 0;
    }

    public MyArrayDeque(T[] a)
    {
        if (a == null) throw new ArgumentNullException("a", "Массив a не должен быть null");
        int cap = a.Length;
        if (cap < 16) cap = 16;

        elements = new T[cap];
        head = 0;
        tail = 0;
        count = 0;

        addAll(a);
    }

    public MyArrayDeque(int numElements)
    {
        if (numElements < 0) throw new ArgumentOutOfRangeException("numElements", "Ёмкость не может быть отрицательной");
        if (numElements < 1) numElements = 1;

        elements = new T[numElements];
        head = 0;
        tail = 0;
        count = 0;
    }

    private int Capacity()
    {
        return elements.Length;
    }

    private int Dec(int i)
    {
        i--;
        if (i < 0) i = Capacity() - 1;
        return i;
    }

    private int Inc(int i)
    {
        i++;
        if (i >= Capacity()) i = 0;
        return i;
    }

    private int IndexAt(int pos)
    {
        int idx = head + pos;
        int cap = Capacity();
        idx %= cap;
        return idx;
    }

    private void EnsureCapacity(int need)
    {
        if (need <= Capacity()) return;

        int newCap = Capacity() * 2;
        while (newCap < need) newCap *= 2;

        T[] newArr = new T[newCap];
        for (int i = 0; i < count; i++)
            newArr[i] = elements[IndexAt(i)];

        elements = newArr;
        head = 0;
        tail = count;
    }

    public void add(T e)
    {
        addLast(e);
    }

    public void addAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("a", "Массив a не должен быть null");
        EnsureCapacity(count + a.Length);
        for (int i = 0; i < a.Length; i++)
            addLast(a[i]);
    }

    public void clear()
    {
        for (int i = 0; i < count; i++)
            elements[IndexAt(i)] = default(T);

        head = 0;
        tail = 0;
        count = 0;
    }

    public bool contains(object o)
    {
        for (int i = 0; i < count; i++)
        {
            T cur = elements[IndexAt(i)];
            if (Equals(cur, o)) return true;
        }
        return false;
    }

    public bool containsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("a", "Массив a не должен быть null");
        for (int i = 0; i < a.Length; i++)
            if (!contains(a[i])) return false;
        return true;
    }

    public bool isEmpty()
    {
        return count == 0;
    }

    public bool remove(object o)
    {
        return removeFirstOccurrence(o);
    }

    public void removeAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("a", "Массив a не должен быть null");
        for (int i = 0; i < a.Length; i++)
        {
            bool ok = true;
            while (ok)
                ok = removeFirstOccurrence(a[i]);
        }
    }

    private bool IsInArray(T val, T[] a)
    {
        for (int i = 0; i < a.Length; i++)
            if (Equals(val, a[i])) return true;
        return false;
    }

    public void retainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("a", "Массив a не должен быть null");

        T[] newArr = new T[Capacity()];
        int newCount = 0;

        for (int i = 0; i < count; i++)
        {
            T cur = elements[IndexAt(i)];
            if (IsInArray(cur, a))
            {
                newArr[newCount] = cur;
                newCount++;
            }
        }

        elements = newArr;
        head = 0;
        tail = newCount;
        count = newCount;
    }

    public int size()
    {
        return count;
    }

    public T[] toArray()
    {
        T[] res = new T[count];
        for (int i = 0; i < count; i++)
            res[i] = elements[IndexAt(i)];
        return res;
    }

    public T[] toArray(T[] a)
    {
        if (a == null || a.Length < count)
        {
            T[] res = new T[count];
            for (int i = 0; i < count; i++)
                res[i] = elements[IndexAt(i)];
            return res;
        }
        else
        {
            for (int i = 0; i < count; i++)
                a[i] = elements[IndexAt(i)];
            return a;
        }
    }

    public T element()
    {
        if (count == 0) throw new InvalidOperationException("Очередь пуста");
        return elements[head];
    }

    public bool offer(T obj)
    {
        addLast(obj);
        return true;
    }

    public T peek()
    {
        if (count == 0) return default(T);
        return elements[head];
    }

    public T poll()
    {
        return pollFirst();
    }

    public void addFirst(T obj)
    {
        EnsureCapacity(count + 1);
        head = Dec(head);
        elements[head] = obj;
        count++;
        if (count == 1) tail = Inc(head);
    }

    public void addLast(T obj)
    {
        EnsureCapacity(count + 1);
        elements[tail] = obj;
        tail = Inc(tail);
        count++;
        if (count == 1) head = Dec(tail);
    }

    public T getFirst()
    {
        if (count == 0) throw new InvalidOperationException("Очередь пуста");
        return elements[head];
    }

    public T getLast()
    {
        if (count == 0) throw new InvalidOperationException("Очередь пуста");
        int last = Dec(tail);
        return elements[last];
    }

    public bool offerFirst(T obj)
    {
        addFirst(obj);
        return true;
    }

    public bool offerLast(T obj)
    {
        addLast(obj);
        return true;
    }

    public T pop()
    {
        return removeFirst();
    }

    public void push(T obj)
    {
        addFirst(obj);
    }

    public T peekFirst()
    {
        if (count == 0) return default(T);
        return elements[head];
    }

    public T peekLast()
    {
        if (count == 0) return default(T);
        return elements[Dec(tail)];
    }

    public T pollFirst()
    {
        if (count == 0) return default(T);

        T val = elements[head];
        elements[head] = default(T);
        head = Inc(head);
        count--;

        if (count == 0)
        {
            head = 0;
            tail = 0;
        }

        return val;
    }

    public T pollLast()
    {
        if (count == 0) return default(T);

        tail = Dec(tail);
        T val = elements[tail];
        elements[tail] = default(T);
        count--;

        if (count == 0)
        {
            head = 0;
            tail = 0;
        }

        return val;
    }

    public T removeLast()
    {
        if (count == 0) throw new InvalidOperationException("Очередь пуста");
        return pollLast();
    }

    public T removeFirst()
    {
        if (count == 0) throw new InvalidOperationException("Очередь пуста");
        return pollFirst();
    }

    public bool removeFirstOccurrence(object obj)
    {
        if (count == 0) return false;

        int pos = -1;
        for (int i = 0; i < count; i++)
        {
            if (Equals(elements[IndexAt(i)], obj))
            {
                pos = i;
                break;
            }
        }
        if (pos == -1) return false;

        T[] newArr = new T[Capacity()];
        int newCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (i == pos) continue;
            newArr[newCount] = elements[IndexAt(i)];
            newCount++;
        }

        elements = newArr;
        head = 0;
        tail = newCount;
        count = newCount;

        return true;
    }

    public bool removeLastOccurrence(object obj)
    {
        if (count == 0) return false;

        int pos = -1;
        for (int i = count - 1; i >= 0; i--)
        {
            if (Equals(elements[IndexAt(i)], obj))
            {
                pos = i;
                break;
            }
        }
        if (pos == -1) return false;

        T[] newArr = new T[Capacity()];
        int newCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (i == pos) continue;
            newArr[newCount] = elements[IndexAt(i)];
            newCount++;
        }

        elements = newArr;
        head = 0;
        tail = newCount;
        count = newCount;

        return true;
    }
}

class Program
{
    static void Main()
    {
        MyArrayDeque<int> d = new MyArrayDeque<int>();
        d.addLast(10);
        d.addFirst(5);
        Console.WriteLine(d.getFirst());
        Console.WriteLine(d.getLast());
    }
}
