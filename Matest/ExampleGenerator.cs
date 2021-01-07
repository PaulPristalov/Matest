using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matest
{
    /// <summary>
    /// Generate examples
    /// </summary>
    class ExampleGenerator
    {
        /// <summary>
        /// Generate new example
        /// </summary>
        /// <returns></returns>
        public static Example NewExample()
        {
            // Sign of an example
            char sign = GetSign();

            /* TODO: создать файл с настройками
             * реализовать получение мин и макс значений у знака
             * реализовать генерацию операндов по настройкам
             */

            return null;

        }

        /// <summary>
        /// Generate random sign of an example
        /// </summary>
        /// <returns></returns>
        private static char GetSign()
        {
            char sign = '+';

            // Random int value, wich convert to char
            var rnd = new Random();
            int s = rnd.Next(1, 5);

            switch (s)
            {
                case 1:
                    sign = '+';
                    break;
                case 2:
                    sign = '-';
                    break;
                case 3:
                    sign = '*';
                    break;
                case 4:
                    sign = '/';
                    break;
            }

            return sign;
        }
    }
}
