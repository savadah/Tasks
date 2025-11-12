using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program
{
    static void Main()
    {
        Console.Write("Введите имя файла (input.txt): ");
        string fileName = Console.ReadLine();

        if (!File.Exists(fileName))
        {
            Console.WriteLine("Файл не найден");
            return;
        }
        string[] lines = File.ReadAllLines(fileName);
        int n = int.Parse(lines[0]); // размерность - первая строка

        // динамическое выделение памяти
        double[,] G = new double[n, n];
        double[] x = new double[n];

        // чтение матрицы G
        for(int i = 0; i < n; i++)
        {
            string[] parts = lines[i + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for(int j = 0; j < n; j++)
            {
                G[i, j] = double.Parse(parts[j]);
            }
        }
        // чтение вектора x
        string[] vectorParts = lines[n + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < n; i++)
        {
            x[i] = double.Parse(vectorParts[i]);
        }
        //проверка симметричности G
        bool symmetric = true;
        for(int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (G[i, j] != G[j, i])
                {
                    symmetric = false; 
                    break;
                }
            }
            if(!symmetric)
                break;
        }

        if (!symmetric)
        {
            Console.WriteLine("Матрица G не симметрична");
            return;
        }
        //вычисление длины по формуле
        double sum = 0.0;
        for(int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                sum += x[i] * G[i, j] * x[j];
            }
        }

        double length = Math.Sqrt(sum);
        Console.WriteLine($"Длина вектора = {length}");
    }
    
}
