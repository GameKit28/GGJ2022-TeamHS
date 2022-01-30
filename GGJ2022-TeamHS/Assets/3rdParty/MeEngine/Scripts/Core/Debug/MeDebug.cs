using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text;
using MeEngine.Internal.Debug;

namespace MeEngine
{
    /// <summary>
    /// Wraps Unity's basic Debug functions to allow for more customized logging.
    /// Ties in with the 3rd Party Tool, Editor Console Pro by Flyingworm.
    /// </summary>
    public partial class MeDebug
    {
        private const string ConditionalDebug = "DEBUG"; //This is specified by Unity

        private const string InfoLogPrefix = "[Info] ";
        private const string TraceLogPrefix = "[Trace] ";

        /// <summary>
        /// Throws an error if the condition is not met. Use when the data you are being provided is not data you require or you otherwise cannot proceed.
        /// Note: Be wary. Any message passed is still evaluated even if the assertion is not fired.
        /// </summary>
        [DebuggerHidden]
        public static void Assert(bool condition, object message, params object[] arguments)
        {
            Debug.AssertFormat(condition, message.ToString(), arguments);
        }

        /// <summary>
        /// Throws an error and writes it to the log. Use when something terribly wrong has happened and the application should not be allowed to continue. Errors should be investigated immedietly when found.
        /// </summary>
        [DebuggerHidden]
        public static void Error(object message, params object[] arguments)
        {
            Debug.LogErrorFormat(message.ToString(), arguments);
        }

        /// <summary>
        /// Writes a warning to the log. Use to notify us of potential problems but continue running the application.
        /// </summary>
        [DebuggerHidden]
        public static void Warning(object message, params object[] arguments)
        {
            Debug.LogWarningFormat(message.ToString(), arguments);
        }

        /// <summary>
        /// Writes helpful information to the log about normal operations. Ex. "User JohnDoe has joined the room. ID 844675."
        /// </summary>
        [DebuggerHidden]
        public static void Info(object message, params object[] arguments)
        {
            Debug.LogFormat(string.Concat(InfoLogPrefix, message.ToString()), arguments);
        }

        /// <summary>
        /// Writes development information to the log. Ignored outside of development builds. Use sparingly so as not to spam the console. Generally Log statements should be removed once the bug/feature is tested or upgraded to Info if they are genuinely helpful.
        /// </summary>
        [Conditional(ConditionalDebug)]
        [DebuggerHidden]
        public static void Log(object message, params object[] arguments)
        {
            Debug.LogFormat(message.ToString(), arguments);
        }

        #region developer_specific
        private const int LimitedTraceDefaultCount = 5; //When we only want to print a given log a handful of times, what's the default number of times to print it?
        private static Dictionary<string, int> _LimitedTraceDict;
        private static Dictionary<string, int> LimitedTraceDict
        {
            get
            {
                if (_LimitedTraceDict == null)
                {
                    _LimitedTraceDict = new Dictionary<string, int>();
                }
                return _LimitedTraceDict;
            }
        }

        //Writes debugging information to the log. Called from developer-specific functions so traces can only be seen by the developer who created them.
        [Conditional(ConditionalDebug)]
        [DebuggerHidden]
        private static void Trace(object message, params object[] arguments){
            Debug.LogFormat(string.Concat(TraceLogPrefix, message.ToString()), arguments);
        }

        //Watch a variable. This will only produce one log entry regardless of how many times it is logged, allowing you to track variables without spam.
        //Note: Relies on ConsoleProDebug API.
        [Conditional(ConditionalDebug)]
        [DebuggerHidden]
        private static void Watch(object message, int frameOffset = 0, params object[] arguments)
        {
            StackFrame frame = new StackTrace(true).GetFrame(1 + frameOffset);
            string key = string.Concat(frame.GetFileName(), frame.GetFileLineNumber());

            Debug.Log(string.Format(message.ToString(), arguments) + "\nCPAPI:{\"cmd\":\"Watch\" \"name\":\"" + key.RemoveSpecialCharacters() + "\"}");
        }

        // Writes development information to the log, but only a limited number of times. Useful for inside loops when you only need to see a handful of output examples and don't want to spam the console.
        [Conditional(ConditionalDebug)]
        [DebuggerHidden]
        private static void LimitedTrace(object message, int limit = LimitedTraceDefaultCount, int frameOffset = 0, params object[] arguments)
        {
            if (!HasSurpassedCount(limit, 1 + frameOffset))
            {
                //Fire again
                Trace(message, arguments);
            }
        }

        //Returns true if the function calling HasSurpassedCount has exceeded the number of times we are allowing it to be called.
        private static bool HasSurpassedCount(int count, int frameOffset = 0)
        {
            //Get the stack frame. We use this as a key to see if this particular MeDebug.LimitedLog has fired before.
            StackFrame frame = new StackTrace(true).GetFrame(1 + frameOffset);
            string key = string.Concat(frame.GetFileName(), frame.GetFileLineNumber()); //Multiple LimitedLogs on the same line number will fail

            //Find the remaining count for this log, or create a new entry with the provided count
            int remainingIterations;
            if (!LimitedTraceDict.TryGetValue(key, out remainingIterations))
            {
                LimitedTraceDict.Add(key, count);
                remainingIterations = count;
            }

            //If we haven't already fired too many times
            if(remainingIterations > 0)
            {
                //And subtract from our remaining count
                LimitedTraceDict[key] = remainingIterations - 1;
                return false;
            }
            else
            {
                return true;
            }
        }

        //Shorthand way to output the name, type, and value of a variable for debugging.
        //This is to prevent this code from running unless it is a debug build.
        //This level of reflection is slow.
        [Conditional(ConditionalDebug)]
        [DebuggerHidden]
        private static void Quick(object obj)
        {
            if (obj != null)
            {
                //Tell me the name, type, and value of the variable I just logged.
                string typeName = obj.GetType().Name;

                //Is this object an array, list, dictionary, or similar?
                if (obj is IEnumerable)
                {
                    StringBuilder str = new StringBuilder();

                    IEnumerable eMsg = obj as IEnumerable;

                    //Print out the collection information
                    str.AppendLine("(" + typeName + ") " + obj.GetReflectedName(3));
                    str.AppendLine();
                    int index = 0;

                    //Loop through each item in our collection
                    foreach (object eObj in eMsg)
                    {
                        //And print out the item's information
                        if (eObj != null)
                        {
                            string subTypeName = eObj.GetType().Name;
                            str.AppendLine("[" + index++ + "] - (" + subTypeName + ") " + eObj.ToString());
                        }
                        else
                        {
                            str.AppendLine("[" + index++ + "] - (Unknown) = NULL");
                        }
                    }
                    str.AppendLine("Items in collection = " + index);

                    Trace(str.ToString());
                }
                else
                {
                    //Not an array. Print the type, name, and value.
                    Trace("(" + typeName + ") " + obj.GetReflectedName(3) + " = " + obj.ToString());
                }
            }
            else
            {
                //It's null. Print what little info we have.
                Trace("(Unknown) " + obj.GetReflectedName(3) + " = NULL");
            }
        }
        #endregion
    }
}
