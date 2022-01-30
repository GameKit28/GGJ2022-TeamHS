using System.Diagnostics;

namespace MeEngine.Internal.Debug
{
    public static class LogExtensions
    {
        //Thanks to blooop from StackOverflow.com
        /// <summary>
        /// Uses Reflection to find the name of the object variable calling GetName(). IE, MyCoolVar.GetName() returns "MyCoolVar".
        /// Note: Slow and prone to errors, use only for debugging purposes.
        /// </summary>
        public static string GetReflectedName(this object thisSourceObject, int stackLevel = 1)
        {
            string searchString = ".GetReflectedName";

            StackFrame stackFrame = new StackTrace(true).GetFrame(stackLevel);
            string fileName = stackFrame.GetFileName();
            int lineNumber = stackFrame.GetFileLineNumber();

            string varName = string.Empty;

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                for (int i = 0; i < lineNumber - 1; i++)
                    file.ReadLine();
                varName = file.ReadLine();
                if (varName.Contains(searchString)) //If the line of code says "GetReflectedName"
                {
                    //Get the first "word" before ".GetReflectedName"
                    varName = varName.Substring(0, varName.IndexOf(searchString));
                    string[] splitStr = varName.Split(new char[] { '(', ')', ' ', '[', ']', '=', '+' });
                    varName = splitStr[splitStr.Length - 1];
                }
                else
                {
                    //In most cases, this is simply called in the format of "MeDebug.Log(var);"
                    varName = varName.Split(new char[] { '(', ')' })[1];
                }
            }
            catch
            {
                varName = "UnknownName";
            }
            return varName;
        }
    }
}
