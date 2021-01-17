using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matest
{
    /// <summary>
    /// Generates examples
    /// </summary>
    abstract class ExampleGenerator
    {
        // Contains a string name of a sign
        private static string operation;

        // Array with signs
        private static char[] signs = { '+', '-', '*', '/', '^', 'V' };

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

            var rnd = new Random();
            // First operand
            int op1 = rnd.Next(minValue, maxValue);

            // Second operand
            int op2 = 0;
            // Checks
            if (sign == '/') // Gerenrates second operand for division example
            {
                while (op2 == 0 || op1 % op2 != 0)
                {
                    op2 = rnd.Next(minValue, maxValue);
                }
            }
            else if (sign == 'V') // Genetates operand for sqrt example
            {
                op1 *= op1;
            }
            else 
            {
                op2 = rnd.Next(minValue, maxValue);

                // If negative result for '-' disabled
                if (sign == '-' && !Settings.BoolSettings["enableNegativeResult"]
                    && op2 > op1)
                    op2 = rnd.Next(0, op1);
            }

            // TODO: пофиксить баг с генерацией примеров sqr
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
    }
}
