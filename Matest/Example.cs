using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matest
{
    /// <summary>
    /// Contains example
    /// </summary>
    class Example
    {
        /// <summary>
        /// Full example in a string
        /// </summary>
        public string ExampleStr { get; }
        /// <summary>
        /// First Operand
        /// </summary>
        public double Operand1 { get; }
        /// <summary>
        /// Second operand
        /// </summary>
        public double Operand2 { get; }
        /// <summary>
        /// Sign of an example
        /// </summary>
        public char Sign { get; }
        /// <summary>
        /// Answer of an example
        /// </summary>
        public double Answer { get; }

        public Example(double op1, double op2, char sign)
        {
            Operand1 = op1;
            Operand2 = op2;
            Sign = sign;

            // Get example string
            if (Sign == '^')
                ExampleStr = $"{Operand1} {Sign} 2";
            else if (Sign == 'V')
                ExampleStr = $"√{Operand1}";
            else
                ExampleStr = $"{Operand1} {Sign} {Operand2}";

            // Set answer
            switch (Sign)
            {
                case '+':
                    Answer = Operand1 + Operand2;
                    break;

                case '-':
                    Answer = Operand1 - Operand2;
                    break;

                case '*':
                    Answer = Operand1 * Operand2;
                    break;

                case '/':
                    Answer = Operand1 / Operand2;
                    break;

                case '^':
                    Answer = Operand1 * Operand1;
                    break;

                case 'V':
                    Answer = (int)Math.Sqrt(Operand1);
                    break;
            }
        }

        public override string ToString()
        {
            return ExampleStr;
        }
    }
}
