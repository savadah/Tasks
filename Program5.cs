using System;

namespace ConsoleApp4
{
    public class Heap<T> where T : IComparable<T>
    {
        private T[] data;
        private int size;

        public int Count
        {
            get { return size; }
        }

        public bool Empty
        {
            get { return size == 0; }
        }

        public Heap(int capacity = 10)
        {
            data = new T[capacity];
            size = 0;
        }

        public Heap(T[] array)
        {
            if (array == null)
                throw new ArgumentException("Массив не может быть null");

            data = new T[array.Length];
            Array.Copy(array, data, array.Length);
            size = array.Length;

            BuildHeap();
        }

        public T FindMax()
        {
            if (Empty)
                throw new InvalidOperationException("Куча пуста");
            return data[0];
        }

        public T ExtractMax()
        {
            if (Empty)
                throw new InvalidOperationException("Куча пуста");

            T max = data[0];
            data[0] = data[size - 1];
            size--;
            SiftDown(0);
            return max;
        }

        public void Insert(T value)
        {
            EnsureCapacity();
            data[size] = value;
            size++;
            SiftUp(size - 1);
        }

        public void Update(int index, T newValue)
        {
            if (index < 0 || index >= size)
                throw new IndexOutOfRangeException("Индекс вне диапазона");

            T oldValue = data[index];
            data[index] = newValue;

            if (newValue.CompareTo(oldValue) > 0)
                SiftUp(index);
            else
                SiftDown(index);
        }

        public Heap<T> Combine(Heap<T> other)
        {
            if (other == null)
                throw new ArgumentException("Другая куча не может быть null");

            int totalSize = this.size + other.size;
            T[] combined = new T[totalSize];

            Array.Copy(this.data, 0, combined, 0, this.size);
            Array.Copy(other.data, 0, combined, this.size, other.size);

            return new Heap<T>(combined);
        }

        private void BuildHeap()
        {
            for (int i = size / 2 - 1; i >= 0; i--)
            {
                SiftDown(i);
            }
        }

        private void SiftUp(int position)
        {
            int current = position;

            while (current > 0)
            {
                int parent = (current - 1) / 2;

                if (data[parent].CompareTo(data[current]) >= 0)
                    break;

                Swap(current, parent);
                current = parent;
            }
        }

        private void SiftDown(int position)
        {
            int current = position;

            while (HasLeftChild(current))
            {
                int maxChild = GetLeftChildIndex(current);

                if (HasRightChild(current) &&
                    data[GetRightChildIndex(current)].CompareTo(data[maxChild]) > 0)
                {
                    maxChild = GetRightChildIndex(current);
                }

                if (data[current].CompareTo(data[maxChild]) >= 0)
                    break;

                Swap(current, maxChild);
                current = maxChild;
            }
        }

        private bool HasLeftChild(int index)
        {
            return GetLeftChildIndex(index) < size;
        }

        private bool HasRightChild(int index)
        {
            return GetRightChildIndex(index) < size;
        }

        private int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        private int GetRightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        private void Swap(int i, int j)
        {
            T temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        private void EnsureCapacity()
        {
            if (size == data.Length)
            {
                int newCapacity = data.Length * 2;
                T[] newData = new T[newCapacity];
                Array.Copy(data, newData, size);
                data = newData;
            }
        }

        public void Display()
        {
            Console.Write("Элементы кучи: ");
            for (int i = 0; i < size; i++)
            {
                Console.Write(data[i] + " ");
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Тестирование Max-кучи");

            int[] numbers = { 3, 1, 6, 5, 2, 4 };
            var maxHeap = new Heap<int>(numbers);
            maxHeap.Display();
            Console.WriteLine("Максимум: " + maxHeap.FindMax());

            maxHeap.Insert(8);
            maxHeap.Display();

            Console.WriteLine("Извлечен: " + maxHeap.ExtractMax());
            maxHeap.Display();

            maxHeap.Update(2, 10);
            maxHeap.Display();

            Console.WriteLine("\nТестирование со строками");
            var stringHeap = new Heap<string>(new string[] { "apple", "banana", "cherry" });
            stringHeap.Display();
            Console.WriteLine("Максимум: " + stringHeap.FindMax());
        }
    }
}
