using System;
using System.IO;

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
        for (int i = 0; i < a.Length; i++) elementData[i] = a[i];

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
        for (int i = 0; i < elementCount; i++) newArray[i] = elementData[i];
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

    public T Get(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException("index");
        return elementData[index];
    }
}

class Program
{
    static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    static bool IsBoundary(char c)
    {
        return !IsDigit(c) && c != '.';
    }

    static bool TryParseIPv4(string s, int start, int end, out string ip)
    {
        ip = null;

        int len = end - start;
        if (len < 7 || len > 15) return false;

        int[] parts = new int[4];
        int partIndex = 0;

        int i = start;
        while (i < end)
        {
            if (partIndex >= 4) return false;

            int value = 0;
            int digits = 0;

            if (i >= end || !IsDigit(s[i])) return false;
            {
                while (i < end && IsDigit(s[i]))
                {
                    value = value * 10 + (s[i] - '0');
                    digits++;
                    if (digits > 3) return false;
                    i++;
                }
            }

            if (value > 255) return false;

            parts[partIndex] = value;
            partIndex++;

            if (partIndex == 4) break;

            if (i >= end || s[i] != '.') return false;
            i++;
        }

        if (partIndex != 4) return false;
        if (i != end) return false;

        ip = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
        return true;
    }

    static void Main()
    {
        MyVector<string> lines = new MyVector<string>();
        MyVector<string> ips = new MyVector<string>();

        string[] fileLines = File.ReadAllLines("input.txt");
        for (int i = 0; i < fileLines.Length; i++)
            lines.Add(fileLines[i]);

        for (int li = 0; li < lines.Size(); li++)
        {
            string line = lines.Get(li);
            int n = line.Length;

            int i = 0;
            while (i < n)
            {
                if (IsDigit(line[i]))
                {
                    int start = i;

                    int j = i;
                    while (j < n && (IsDigit(line[j]) || line[j] == '.'))
                        j++;

                    int end = j;

                    bool leftOk = (start == 0) || IsBoundary(line[start - 1]);
                    bool rightOk = (end == n) || IsBoundary(line[end]);

                    if (leftOk && rightOk)
                    {
                        string ip;
                        if (TryParseIPv4(line, start, end, out ip))
                            ips.Add(ip);
                    }

                    i = end;
                }
                else
                {
                    i++;
                }
            }
        }

        string[] outLines = new string[ips.Size()];
        for (int i = 0; i < ips.Size(); i++)
            outLines[i] = ips.Get(i);

        File.WriteAllLines("output.txt", outLines);
    }
}

