using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPasswordRecoveryPDF.Common
{
    public class ArrayMath
    {
        public ArrayMath()
        {
        }

        /// <summary>
        /// Checks whether the two arrays are equal.
        /// </summary>
        public bool ArraysAreEqual(byte[] userKey, byte[] uValue, int matchSize = 0)
        {
            bool arraysAreEqual = true;
            int minArrayLength = matchSize == 0 ? Math.Min(userKey.Length, uValue.Length) : matchSize;
            int idx = 0;

            while (idx < minArrayLength && arraysAreEqual)
            {
                if (userKey[idx] != uValue[idx])
                    arraysAreEqual = false;
                idx++;
            }
            return arraysAreEqual;
        }

        public byte[] ConcatByteArrays(params byte[][] arrays)
        {
            return arrays.SelectMany(x => x).ToArray();
        }
    }
}
