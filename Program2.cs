using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

struct Complex
{
    public double re; // вещественная часть
    public double im; // мнимая часть

    public Complex(double real, double imag)
    {
        re = real;
        im = imag;
    }

    public double Real() { return re; }
    public double Imag() { return im; }

    public double Modulus()
    {
        return Math.Sqrt(re * re + im * im);
    }

    public double Argument()
    {
        return Math.Atan2(im, re);
    }

    public Complex Add(Complex other)
    {
        return new Complex(this.re + other.re, this.im + other.im);
    }

    public Complex Sub(Complex other)
    {
        return new Complex(this.re - other.re, this.im - other.im);
    }

    public Complex Mul(Complex other)
    {
        double a = this.re, b = this.im;
        double c = other.re, d = other.im;
        return new Complex(a * c - b * d, a * d + b * c);
    }

    public Complex Div(Complex other)
    {
        double a = this.re, b = this.im;
        double c = other.re, d = other.im;
        double denom = c * c + d * d;

        if (denom == 0)
        {
            throw new DivideByZeroException("Деление на ноль в коплексных числах.");
        }
        return new Complex((a * c + b * d) / denom, (b * c -  a * d) / denom);
    }

    public override string ToString()
    {
        string sign = im >= 0 ? "+" : "-";
        double absIm = Math.Abs(im);
        return $"{re} {sign} {absIm}i";
    }
}

class Program
{
    static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = Console.ReadLine();

            if (double.TryParse(s.Replace(',','.'),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double value))
            {
                return value;
            }
            Console.WriteLine("Не число, Потворите ввод.");
        }
    }

    static Complex ReadComplex(string title)
    {
        Console.WriteLine(title);
        double a = ReadDouble("Введите вещественную часть a: ");
        double b = ReadDouble("Введите мнимую часть b: ");
        return new Complex(a, b);
    }

    static void PrintMenu(Complex current)
    {
        Console.WriteLine();
        Console.WriteLine("КОМПЛЕКСНЫЕ ЧИСЛА");
        Console.WriteLine($"Текущее число {current}");
        Console.WriteLine(" I - Ввод/Замена текущего числа");
        Console.WriteLine(" A - Сложить с другим числом");
        Console.WriteLine(" S - Вычесть другое число");
        Console.WriteLine(" M - Умножить на другое число");
        Console.WriteLine(" D - Разделить на другое число");
        Console.WriteLine(" T - Модуль текущего числа");
        Console.WriteLine(" G - Аргумент текущего числа");
        Console.WriteLine(" R - Показать вещественную часть");
        Console.WriteLine(" J - Показать мнимую часть");
        Console.WriteLine(" P - Печать текущего числа");
        Console.WriteLine(" Q - Выход");
        Console.Write("Команда: ");
    }

    static void Main()
    {
        // изначально 0 + 0i
        Complex z = new Complex(0, 0);

        while (true)
        {
            PrintMenu(z);
            string cmdLine = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cmdLine))
            {
                Console.WriteLine("Неизвестная команда.");
                continue;
            }

            char cmd = char.ToUpperInvariant(cmdLine[0]);

            try
            {
                switch (cmd)
                {
                    case 'I':
                        z = ReadComplex("Ввод текущего числа");
                        Console.WriteLine($"Установлено {z}");
                        break;
                
                 case 'A':
                    {
                        Complex w = ReadComplex("Сложение: введите второе число");
                        z = z.Add(w); // результат записываем обратно
                        Console.WriteLine($"Результат: {z}");
                    }
                    break;

                case 'S':
                    {
                        Complex w = ReadComplex("Вычитание: введите второе число");
                        z = z.Sub(w);
                        Console.WriteLine($"Результат: {z}");
                    }
                    break;

                case 'M':
                    {
                        Complex w = ReadComplex("Умножение: введите второе число");
                        z = z.Mul(w);
                        Console.WriteLine($"Результат: {z}");
                    }
                    break;

                case 'D':
                    {
                        Complex w = ReadComplex("Деление: введите второе число");
                        z = z.Div(w);
                        Console.WriteLine($"Результат: {z}");
                    }
                    break;

                case 'T':
                    Console.WriteLine($"Модуль |z| = {z.Modulus()}");
                    break;

                case 'G':
                    Console.WriteLine($"Аргумент arg(z) = {z.Argument()} (в радианах)");
                    break;

                case 'R':
                    Console.WriteLine($"Вещественная часть Re(z) = {z.Real()}");
                    break;

                case 'J': 
                    Console.WriteLine($"Мнимая часть Im(z) = {z.Imag()}");
                    break;

                case 'P':
                    Console.WriteLine($"z = {z}");
                    break;

                case 'Q':
                    return; // выходим из программы

                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
                }
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Неожиданная ошибка: " + ex.Message);
            }
        }
    }
}
            
        
   


