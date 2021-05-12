using System;
using System.Text;
using System.Collections.Generic;

namespace BinaryEncriptor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();

            //Getting the key from user
            string key = "";
            do
            {
                ColorfulConsole.Write("Input the key: ", ConsoleColor.White);
                key = ColorfulConsole.ReadLine(ConsoleColor.Red);
            }
            while (String.IsNullOrWhiteSpace(key) || String.IsNullOrEmpty(key) || key == "0");

            Console.WriteLine();

            //Getting the text from user
            ColorfulConsole.Write("Input the text to be encrypted: ", ConsoleColor.White);
            string input = ColorfulConsole.ReadLine(ConsoleColor.Yellow);
            Console.WriteLine();

            //Encrypting the text
            var encryptor = new StringEncryptor(key);
            encryptor.Encrypt(input);

            //Showing the results
            encryptor.PrintTable();
            Console.WriteLine();
            ColorfulConsole.Write($"Encrypted text: ", ConsoleColor.White);
            ColorfulConsole.Write(encryptor.EncryptedText, ConsoleColor.Green);

            Console.WriteLine();
            Console.WriteLine();
        }
    }

    class StringEncryptor
    {
        public string Key { get; set; }
        public string EncryptedText { get; private set; }

        private string[] firstLine;
        private string[] secondLine;
        private string[] fourthLine;
        private string[] resultLine;
        private bool IsTableReady;

        public StringEncryptor(string key)
        {
            IsTableReady = false;

            while (key[0] == '0')
                key = key.Remove(0, 1);

            Key = key;
        }

        public void Encrypt(string input)
        {
            IsTableReady = false;

            firstLine = (string[])ConvertInputToBinary(input);
            secondLine = new string[input.Length];
            fourthLine = new string[input.Length];
            resultLine = new string[input.Length];

            string tempresult = "0";

            for (int i = 0; i < input.Length; i++)
            {
                resultLine[i] = CalculateResultFor(firstLine[i],
                                                    ref secondLine[i],
                                                    ref fourthLine[i],
                                                    ref tempresult);
            }

            IsTableReady = true;
            EncryptedText = EncriptResult(resultLine);
        }

        public void PrintTable()
        {
            if (!IsTableReady)
            {
                Console.WriteLine("Calculation not completed!");
                return;
            }

            int length = firstLine.Length;
            const ConsoleColor rowNameColor = ConsoleColor.Magenta;
            const ConsoleColor valuesColor = ConsoleColor.White;

            ColorfulConsole.Write("Input:\t", rowNameColor);
            for (int i = 0; i < length; i++)
            {
                ColorfulConsole.Write(firstLine[i] + "\t", ConsoleColor.Yellow);
            }
            Console.WriteLine();

            ColorfulConsole.Write("2-nd:\t", rowNameColor);
            for (int i = 0; i < length; i++)
            {
                ColorfulConsole.Write(secondLine[i] + "\t", valuesColor);
            }
            Console.WriteLine();

            ColorfulConsole.Write("Key:\t", rowNameColor);
            for (int i = 0; i < length; i++)
            {
                ColorfulConsole.Write(this.Key + "\t", ConsoleColor.Red);
            }
            Console.WriteLine();

            ColorfulConsole.Write("4-th:\t", rowNameColor);
            for (int i = 0; i < length; i++)
            {
                ColorfulConsole.Write(fourthLine[i] + "\t", valuesColor);
            }
            Console.WriteLine();

            ColorfulConsole.Write("Result:\t", rowNameColor);
            for (int i = 0; i < length; i++)
            {
                ColorfulConsole.Write(resultLine[i] + "\t", ConsoleColor.Green);
            }
            Console.WriteLine();
        }

        //Put 65 here
        private string EncriptResult(string[] binaryResult)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < binaryResult.Length; i++)
            {
                sb.Append((char)Binary.BinaryToDecimal(binaryResult[i]));
                /*sb.Append((char)(Binary.BinaryToDecimal(binaryResult[i]) + 65));*/ //Uncomment this
            }

            return sb.ToString();
        }
        //Put 65 here
        private IEnumerable<string> ConvertInputToBinary(string input)
        {
            string[] temp = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                temp[i] = Binary.DecimalToBinary((int)input[i]);
                /*temp[i] = Binary.DecimalToBinary((int)(input[i] - 65));*/ //Uncomment this
            }

            return temp;
        }

        private string CalculateResultFor(string binaryNumber, ref string secondLineNum, ref string fourthLineNum, ref string result)
        {
            secondLineNum = Binary.AddBinaryNumbers(binaryNumber, result);
            fourthLineNum = Binary.AddBinaryNumbers(secondLineNum, this.Key);
            result = MoveToRight(fourthLineNum);

            return result;
        }

        private string MoveToRight(string binaryNumer)
        {
            var sb = new StringBuilder(binaryNumer);
            char firstDigit = binaryNumer[0];
            sb.Remove(0, 1);
            sb.Append(firstDigit);

            while (sb[0] == '0')
                sb.Remove(0, 1);

            return sb.ToString();
        }
    }

    static class Binary
    {
        public static string DecimalToBinary(int num)
        {
            var sb = new StringBuilder();

            if (num == 0)
                return "0";

            while (num > 0)
            {
                sb.Append(num % 2);
                num /= 2;
            }

            char[] arr = sb.ToString().ToCharArray();
            Array.Reverse(arr);

            return new string(arr);
        }

        public static int BinaryToDecimal(string binaryNumber)
        {
            if (binaryNumber == "0")
                return 0;

            int output = 0;

            int length = binaryNumber.Length - 1;
            for (int i = length; i >= 0; i--)
            {
                int num1 = int.Parse(binaryNumber[i].ToString());
                int num2 = (int)Math.Pow(2, length - i);
                output = output + (num1 * num2);
            }

            return output;

        }

        public static string AddBinaryNumbers(string s1, string s2)
        {
            return DecimalToBinary(BinaryToDecimal(s1) + BinaryToDecimal(s2));
        }
    }

    static class ColorfulConsole
    {
        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static string ReadLine(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            string s = Console.ReadLine();
            Console.ResetColor();

            return s;
        }
    }
}