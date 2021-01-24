using System;
using System.Collections.Generic;

namespace Matest
{
    /// <summary>
    /// Generates examples
    /// </summary>
    static class ExampleGenerator
    {
        // Contains a string name of a sign
        private static string operation;

        // Array with signs
        private static readonly char[] signs = { '+', '-', '*', '/', '^', 'V' };

        private static Random rnd = new Random();

        /// <summary>
        /// Generates new example
        /// </summary>
        /// <returns></returns>
        public static Example NewExample()
        {
            // Sign of an example
            char sign = GetSign();

            int minValue = Settings.IntSettings["min" + operation];
            int maxValue = Settings.IntSettings["max" + operation];

            // First operand
            double op1 = rnd.Next(minValue, maxValue);

            // Second operand
            double op2 = 0;
            // Checks
            switch (sign) 
            {
                case '/': // Gerenrates second operand for division example
                    while (op2 == 0 || op1 % op2 != 0)
                    {
                        op2 = rnd.Next(minValue, maxValue);
                    }
                    break;

                case 'V': // Genetates operand for sqrt example
                    op1 *= op1;
                    break;

                default:
                    op2 = rnd.Next(minValue, maxValue);

                    // Generate decimal part
                    if (sign != '^' && Settings.BoolSettings["enableDecimalNumbers"])
                    {
                        if (sign == '*')
                        {
                            op1 += GetRandomDouble(1);
                            op2 += GetRandomDouble(1);
                        }
                        else
                        {
                            op1 += GetRandomDouble(2);
                            op2 += GetRandomDouble(2);
                        }
                    }

                    // If negative result for '-' disabled
                    while (sign == '-' && !Settings.BoolSettings["enableNegativeResult"]
                        && op2 > op1)
                    {
                        op2 = rnd.Next(minValue, (int)op1);
                        if (Settings.BoolSettings["enableDecimalNumbers"])
                        {
                            op2 += GetRandomDouble(2);
                        }
                    }
                    break;
            }

            var ex = new Example(op1, op2, sign); 

            return ex;
        }

        /// <summary>
        /// Generates random sign of an example
        /// </summary>
        /// <returns></returns>
        private static char GetSign()
        {
            // Contains operations
            Dictionary<char, string> operations = new Dictionary<char, string>();
            operations.Add('+', "Plus");
            operations.Add('-', "Minus");
            operations.Add('*', "Multi");
            operations.Add('/', "Divide");
            operations.Add('^', "Sqr");
            operations.Add('V', "Sqrt");

            char sign;

            while (true)
            {
                // Random int value, which convertd to a sign
                var rnd = new Random();
                int s = rnd.Next(0, 6);

                sign = signs[s];
                operation = operations[sign];

                // If this sign is active
                if (Settings.BoolSettings["active" + operation])
                    break;
            }

            return sign;
        }

        /// <summary>
        /// Returns rounded by decimals random float number from 0 to 1
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        private static double GetRandomDouble(int decimals)
        {
            return Math.Round((double)rnd.NextDouble(), decimals);
        }
    }
}
