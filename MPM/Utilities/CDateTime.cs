// author: unknown
// purpose: utility class to handle unix ticks from date-time
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Utilities
{
    class CDateTime
    {
        public static DateTime UnixStartTime = new DateTime(1970, 1, 1, 0, 0, 0);

        //public static string RemoveSpecialCharacters(string str)
        //{
        //    StringBuilder stringBuilder = new StringBuilder(str.Length);
        //    foreach (char c in str)
        //    {
        //        if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c))
        //            stringBuilder.Append(c);
        //    }
        //    return stringBuilder.ToString();
        //}

        //public static string RemoveNewLineCharactersFromString(string input)
        //{
        //    return CDateTime.RemoveChars(CDateTime.RemoveChars(input, "\r"), "\n");
        //}

        //public static string RemoveChars(string inputString, string stringToRemove)
        //{
        //    for (int startIndex = inputString.IndexOf(stringToRemove); startIndex >= 0; startIndex = inputString.IndexOf(stringToRemove))
        //        inputString = inputString.Remove(startIndex, 1);
        //    if (inputString.Length > 0 && !char.IsLetter(inputString[0]))
        //        inputString = inputString.Remove(0, 1);
        //    return inputString;
        //}

        public static long UnixTicksFromDate(DateTime dateTime)
        {
            return (long)(dateTime - CDateTime.UnixStartTime).TotalSeconds;
        }

        //public static string FrontFill(int input, int length = 2)
        //{
        //    string str = input.ToString();
        //    while (str.Length < length)
        //        str = "0" + str;
        //    return str;
        //}
    }
}
