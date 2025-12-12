using System;
using System.IO;

public class MyArrayList<T>
{
    private T[] data;
    private int size;

    public MyArrayList()
    {
        data = new T[10];
        size = 0;
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (data.Length < minCapacity)
        {
            int newCap = data.Length * 3 / 2 + 1;
            if (newCap < minCapacity)
                newCap = minCapacity;

            T[] newArr = new T[newCap];
            for (int i = 0; i < size; i++)
                newArr[i] = data[i];
            data = newArr;
        }
    }

    public void Add(T value)
    {
        EnsureCapacity(size + 1);
        data[size] = value;
        size++;
    }

    public int Size()
    {
        return size;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException("index");
        return data[index];
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException("index");
        for (int i = index + 1; i < size; i++)
            data[i - 1] = data[i];
        size--;
    }
}

class Program
{
    static bool IsValidTag(string tag)
    {
        if (tag.Length < 3) return false;
        if (tag[0] != '<') return false;
        if (tag[tag.Length - 1] != '>') return false;

        int pos = 1;
        if (pos < tag.Length - 1 && tag[pos] == '/')
            pos++;

        if (pos >= tag.Length - 1) return false;

        char first = tag[pos];
        if (!char.IsLetter(first)) return false;

        for (int i = pos + 1; i < tag.Length - 1; i++)
        {
            char c = tag[i];
            if (!char.IsLetterOrDigit(c))
                return false;
        }

        return true;
    }

    static string NormalizeTag(string tag)
    {
        string s = tag;

        if (s.Length >= 2 && s[0] == '<' && s[s.Length - 1] == '>')
            s = s.Substring(1, s.Length - 2);

        if (s.Length > 0 && s[0] == '/')
            s = s.Substring(1);

        s = s.ToLowerInvariant();
        return s;
    }

    static void Main()
    {
        MyArrayList<string> tags = new MyArrayList<string>();

        string[] lines = File.ReadAllLines("input.txt");

        for (int l = 0; l < lines.Length; l++)
        {
            string line = lines[l];

            int i = 0;
            while (i < line.Length)
            {
                if (line[i] == '<')
                {
                    int j = i + 1;
                    while (j < line.Length && line[j] != '>')
                        j++;

                    if (j >= line.Length)
                        break;

                    string candidate = line.Substring(i, j - i + 1);

                    if (IsValidTag(candidate))
                    {
                        tags.Add(candidate);
                    }

                    i = j + 1;
                }
                else
                {
                    i++;
                }
            }
        }

        int idx = 0;
        while (idx < tags.Size())
        {
            string current = tags.Get(idx);
            string normCurrent = NormalizeTag(current);

            int j = idx + 1;
            while (j < tags.Size())
            {
                string other = tags.Get(j);
                string normOther = NormalizeTag(other);

                if (normOther == normCurrent)
                {
                    tags.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }

            idx++;
        }

        for (int k = 0; k < tags.Size(); k++)
        {
            Console.WriteLine(tags.Get(k));
        }
    }
}
