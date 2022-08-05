using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MPM.DataAcquisition.Helpers
{
    public class Utilities
    {
        public static int SizeOfInt     = sizeof(int);
        public static int SizeOfShort   = sizeof(short);
        public static int SizeOfUShort  = sizeof(ushort);
        public static int SizeOfFloat   = sizeof(float);
        public static int SizeOfDouble  = sizeof(double);

        public static int ComboBoxPositionNone  = -1;
        public static int ComboBoxPositionFirst = 0;

        public static int BytesToInt(byte[] bArray, int startPosition)
        {
            return System.BitConverter.ToInt32(bArray, startPosition);
        }

        public static int BytesToInt(List<byte> bList, int startPosition)
        {
            if (bList.Count > 0)
            {
                return BytesToInt(bList[startPosition + 0],
                                bList[startPosition + 1],
                                bList[startPosition + 2],
                                bList[startPosition + 3]);
            }
            else
                return 0;
            
        }

        public static int BytesToInt(byte byteLowest0, byte byteLow1, byte byteHigh2, byte byteHighest3)
        {
            byte[] bytes = new byte[4] { byteLowest0, byteLow1, byteHigh2, byteHighest3 };

            return BytesToInt(bytes, 0);
        }

        public static ushort BytesToUShort(byte[] bArray, int startPosition)
        {
            return (ushort) System.BitConverter.ToInt16(bArray, startPosition);
        }

        public static short BytesToShort(List<byte> byteList, int startPosition)
        {
            if ( startPosition + 1 >= byteList.Count )
                return 0;

            byte[] bytes = new byte[2] { byteList[startPosition], byteList[startPosition+1] };

            return System.BitConverter.ToInt16(bytes, 0 );
        }

        public static short BytesToShort(byte[] byteArray, int startPosition, bool checkBoundaries = true)
        {
            if (checkBoundaries)
                if (startPosition + Utilities.SizeOfShort > byteArray.Length)
                    return (short)0;

            return System.BitConverter.ToInt16(byteArray, startPosition);
        }

        public static double BytesToDouble(byte[] byteArray, int startPosition)
        {
            return (startPosition + sizeof(double) <= byteArray.Length)
                            ? System.BitConverter.ToDouble(byteArray, startPosition)
                            : (double)0;
        }

        public static int ShiftLeft(byte b0, int bits)
        {
            int temp = b0;
            return temp << bits;
        }

        public static string ConvertByteArrayToString(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buffer)
            {
                sb.Append((char)b);
            }

            return sb.ToString();
        }

        internal static float BytesToFloat(byte[] data, int index)
        {
            return System.BitConverter.ToSingle(data, index);
        }

        public static byte GetByte(int value, int whichByte)
        {
            Debug.Assert(whichByte >= 0);
            Debug.Assert(whichByte < sizeof(int));

            return (byte)(value >> (8 * whichByte));
        }
    }
}
