using System;
using System.Text;

class Program
{
    // --- Завдання 2: Функції-конвертери ---
    static string IntToBase(int n, int baseNum)
    {
        if (n == 0) return "0";
        char[] digits = "0123456789ABCDEF".ToCharArray();
        bool isNegative = n < 0;
        int curr = Math.Abs(n);
        string res = "";

        while (curr > 0)
        {
            res = digits[curr % baseNum] + res;
            curr /= baseNum;
        }

        return isNegative ? "-" + res : res;
    }

    static string FracToBin(double f, int precision = 6)
    {
        string res = "0.";
        double curr = Math.Abs(f) - Math.Truncate(Math.Abs(f));

        for (int i = 0; i < precision; i++)
        {
            curr *= 2;
            int bit = (int)curr;
            res += bit;
            curr -= bit;
        }

        return res;
    }

    // --- Завдання 3: 8-бітний додатковий код та переповнення ---
    static (string BinStr, sbyte SignedVal, bool Overflow) AddTwosComplement8Bit(sbyte a, sbyte b)
    {
        byte a8 = (byte)a;
        byte b8 = (byte)b;
        byte res8 = (byte)(a8 + b8);

        int signA = (a8 >> 7) & 1;
        int signB = (b8 >> 7) & 1;
        int signRes = (res8 >> 7) & 1;

        bool overflow = (signA == signB) && (signA != signRes);
        sbyte signedRes = (sbyte)res8;

        string binStr = Convert.ToString(res8, 2).PadLeft(8, '0');
        return (binStr, signedRes, overflow);
    }

    static void Main()
    {
        // Налаштування UTF-8 для відображення кирилиці в консолі
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Вхідні дані Варіанта №2
        int N = 58;
        double F = 0.3;
        sbyte A = 60, B = 20;
        float C = 33.25f;

        Console.WriteLine("=== Крок 2: Переведення N та F ===");
        Console.WriteLine($"N = {N} -> BIN: {IntToBase(N, 2)}, OCT: {IntToBase(N, 8)}, HEX: {IntToBase(N, 16)}");
        Console.WriteLine($"F = {F} -> BIN (6 знаків): {FracToBin(F, 6)}");

        Console.WriteLine("\\n=== Крок 3: 8-бітне додавання A та B ===");
        var (binSum, decSum, ovf) = AddTwosComplement8Bit(A, B);
        Console.WriteLine($"A = {A}, B = {B} -> Сума бінарно: {binSum}, Десятково: {decSum}, Переповнення: {ovf}");

        Console.WriteLine("\\n=== Крок 4: Розбір IEEE 754 для C ===");
        uint bits = BitConverter.SingleToUInt32Bits(C);
        string bitsStr = Convert.ToString(bits, 2).PadLeft(32, '0');
        
        string sign = bitsStr.Substring(0, 1);
        string exp = bitsStr.Substring(1, 8);
        string mantissa = bitsStr.Substring(9);
        float restored = BitConverter.UInt32BitsToSingle(bits);

        Console.WriteLine($"C = {C}");
        Console.WriteLine($"32 біти: {bitsStr}");
        Console.WriteLine($"Знак: {sign}, Порядок: {exp}, Мантиса: {mantissa}");
        Console.WriteLine($"Відновлене значення: {restored}");

        Console.WriteLine("\\n=== Крок 5: Похибка представлення дробів ===");
        double v1 = 0.1, v2 = 0.2;
        double sumVal = v1 + v2;
        Console.WriteLine($"0.1 + 0.2 = {sumVal}");
        Console.WriteLine($"0.1 + 0.2 == 0.3: {sumVal == 0.3}");
    }
}