using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace MeEngine
{
    public partial class MeDebug
    {
        //--Default logging options for new developers--
        //These logs will only be visible if the current user is USER_DEFAULT. (Set in the smcs.rsp file)
        public class User
        {
            private const string UserName = "USER_DEFAULT";

            /// <summary>
            /// Writes debugging info to the log. Use when diagnosing a feature your are adding or a bug your are investigating.
            /// Can only be seen by the specified developer. Usefull when you want to avoid spamming other developers with debug logs.
            /// </summary>
            [Conditional(UserName)]
            [DebuggerHidden]
            public static void Trace(object message, params object[] arguments)
            {
                MeDebug.Trace(message, arguments);
            }

            /// <summary>
            /// Watch a variable. This will only produce one log entry regardless of how many times it is logged, allowing you to track variables without spam.
            /// Can only be seen by the specified developer. Usefull when you want to avoid spamming other developers with debug logs.
            /// </summary>
            [Conditional(UserName)]
            [DebuggerHidden]
            public static void Watch(object message, params object[] arguments)
            {
                MeDebug.Watch(message, 1, arguments);
            }

            /// <summary>
            /// Writes development information to the log, but only a limited number of times. Useful for inside loops when you only need to see a handful of output examples and don't want to spam the console.
            /// Can only be seen by the specified developer. Usefull when you want to avoid spamming other developers with debug logs.
            /// </summary>
            [Conditional(UserName)]
            [DebuggerHidden]
            public static void LimitedTrace(object message, int limit = LimitedTraceDefaultCount, params object[] arguments)
            {
                MeDebug.LimitedTrace(message, limit, 1, arguments);
            }

            /// <summary>
            /// Shorthand way to output the name, type, and value of a variable for debugging.
            /// Can only be seen by the specified developer. Usefull when you want to avoid spamming other developers with debug logs.
            /// </summary>
            [Conditional(UserName)]
            [DebuggerHidden]
            public static void Quick(object obj)
            {
                MeDebug.Quick(obj);
            }
        }
    }
}
