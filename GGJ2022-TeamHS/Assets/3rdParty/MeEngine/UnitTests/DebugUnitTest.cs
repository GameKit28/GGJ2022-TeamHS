using UnityEngine;

namespace MeEngine.Internal.UnitTests.Debug
{
    public class DebugUnitTest : MonoBehaviour
    {

        //Stick this on any object in the scene to test.

        // Use this for initialization
        void Start()
        {

            MeDebug.Info("Performing Debug ConsolePro Tests. See console output. 10 Tests Total.");

            MeDebug.Log("1) - This is a standard log message. This is white.");
            MeDebug.Info("2) - This is an info message. This is blue.");
            MeDebug.Warning("3) - This is a warning. This is yellow.");
            MeDebug.Log("#Custom#4) - This is a custom filter. No specific color.");
            MeDebug.Info("Test 5aa will only appear if USER_KIT is set in the environmental variables.");
            MeDebug.Kit.Trace("5aa) - This is Kit's custom log. This is green.");

            MeDebug.Kit.Trace("The next two tests (5ab & 5ac) will display a variable and thier values. Both Green.");
            int testVar = 5;
            //Random testRandom = null;
            MeDebug.Kit.Quick(testVar);
            //MeDebug.Kit.Quick(testRandom);

            MeDebug.Info("Test 5b will only appear if USER_DEFAULT is set in the environmental variables.");
            MeDebug.User.Trace("5b) - This is a new developer's custom log. This is green.");

            MeDebug.Assert(false, "6) - This is an assert. This is red.");
            MeDebug.Error("7) - This is an error. This is red.");
            throw new System.Exception("8) - This is an exeption. This is red.");

            //MeDebug.Info("The next three tests (9 & 10) will output increading numbers. And will be Green");
        }

        int updateCount = 0;
        void Update()
        {
            MeDebug.Kit.Watch("9) - UpdateCount: " + updateCount++ + ". This is Green.");
            MeDebug.Kit.LimitedTrace("10) - Only 5 total updates: " + updateCount + ". This is Green.", 5);
            if (updateCount == 5)
            {
                MeDebug.Info("Unit Test Complete.");
            }
        }
    }
}
