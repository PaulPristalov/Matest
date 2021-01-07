using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matest
{
    /// <summary>
    /// Contain example
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
        public int Operand1 { get; }
        /// <summary>
        /// Second operand
        /// </summary>
        public int Operand2 { get; }
        /// <summary>
        /// Sign of an example
        /// </summary>
        public char Sign { get; }

        public Example(int op1, int op2, char sign)
        {
            Operand1 = op1;
            Operand2 = op2;
            Sign = sign;

            ExampleStr = $"{Operand1} {Sign} {Operand2}";
        }


    }
}
