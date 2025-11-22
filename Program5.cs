using System;
using System.Collections.Generic;

public class Heap<T> where T : IComparable<T>
{
    private List<T> items;
    private bool isMaxHeap;

    public int Count => items.Count;
    public bool IsEmpty => items.Count == 0;

    public Heap(T[] array = null, bool isMaxHeap = true)
    {
        this.items = new List<T>();
        this.isMaxHeap = isMaxHeap;

        if (array != null)
        {
            foreach (T item in array)
            {
                items.Add(item);
            }

            for (int i = items.Count / 2 - 1; i >= 0; i--)
            {
                SiftDown(i);
            }
        }
    }

    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Куча пуста");

        return items[0];
    }

    public T Extract()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Куча пуста");

        T result = items[0];
        items[0] = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);

        if (!IsEmpty)
            SiftDown(0);

        return result;
    }

    public void ChangeKey(int index, T newValue)
    {
        if (index < 0 || index >= items.Count)
            throw new ArgumentOutOfRangeException("Неверный индекс");

        T oldValue = items[index];
        items[index] = newValue;

        if (ShouldMoveUp(newValue, oldValue))
            SiftUp(index);
        else
            SiftDown(index);
    }

    public void Add(T item)
    {
        items.Add(item);
        SiftUp(items.Count - 1);
    }

    public Heap<T> Merge(Heap<T> other)
    {
        if (other == null)
            throw new ArgumentNullException("Другая куча не может быть null");

        var newHeap = new Heap<T>(isMaxHeap: this.isMaxHeap);

        foreach (T item in this.items)
        {
            newHeap.Add(item);
        }

        foreach (T item in other.items)
        {
            newHeap.Add(item);
        }

        return newHeap;
    }

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (IsCorrectOrder(parentIndex, index))
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void SiftDown(int index)
    {
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int selectedChild = index;

            if (leftChild < items.Count && !IsCorrectOrder(selectedChild, leftChild))
                selectedChild = leftChild;

            if (rightChild < items.Count && !IsCorrectOrder(selectedChild, rightChild))
                selectedChild = rightChild;

            if (selectedChild == index)
                break;

            Swap(index, selectedChild);
            index = selectedChild;
        }
    }

    private bool IsCorrectOrder(int firstIndex, int secondIndex)
    {
        return IsCorrectOrder(items[firstIndex], items[secondIndex]);
    }

    private bool IsCorrectOrder(T first, T second)
    {
        int comparison = first.CompareTo(second);
        return isMaxHeap ? comparison >= 0 : comparison <= 0;
    }

    private bool ShouldMoveUp(T newValue, T oldValue)
    {
        return IsCorrectOrder(newValue, oldValue);
    }

    private void Swap(int i, int j)
    {
        T temp = items[i];
        items[i] = items[j];
        items[j] = temp;
    }

    public override string ToString()
    {
        return string.Join(", ", items);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine(" Демонстрация Max-кучи ");

        int[] numbers = { 3, 1, 6, 5, 2, 4 };
        var maxHeap = new Heap<int>(numbers, true);

        Console.WriteLine($"Куча: {maxHeap}");
        Console.WriteLine($"Максимум: {maxHeap.Peek()}");

        maxHeap.Add(8);
        Console.WriteLine($"После добавления 8: {maxHeap}");

        Console.WriteLine($"Удаляем: {maxHeap.Extract()}");
        Console.WriteLine($"Теперь куча: {maxHeap}");

        maxHeap.ChangeKey(2, 10);
        Console.WriteLine($"После изменения: {maxHeap}");

        Console.WriteLine("\n=== Демонстрация Min-кучи ===");
        var minHeap = new Heap<int>(isMaxHeap: false);
        minHeap.Add(5);
        minHeap.Add(2);
        minHeap.Add(8);
        minHeap.Add(1);

        Console.WriteLine($"Min-куча: {minHeap}");
        Console.WriteLine($"Минимум: {minHeap.Peek()}");
        Console.WriteLine($"Удаляем: {minHeap.Extract()}");
        Console.WriteLine($"Теперь минимум: {minHeap.Peek()}");
    }
}