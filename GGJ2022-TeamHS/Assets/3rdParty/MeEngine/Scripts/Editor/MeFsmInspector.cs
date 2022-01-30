using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace MeEngine.FsmManagement
{
    /// <summary>
    /// Instructs the Inspector on how to draw MeFsms.
    /// </summary>
    [CustomEditor(typeof(MeFsm), true)]
    class MeFsmInspector : Editor
    {
        //Recursively grab all the nested classes for ourselves and any base classes
        private void GetNestedClasses(ref List<System.Type> currentList, System.Type currentType)
        {
            currentList.AddRange(currentType.GetNestedTypes(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).Reverse()); //We reverse this list because it seems GetNestedTypes returns the bottommost classes in a file first
            if(currentType.BaseType != null) { GetNestedClasses(ref currentList, currentType.BaseType); }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //Allow easy access to our MeFsm
            MeFsm myTarget = target as MeFsm;

            //---
            //Gather a list of all states valid for this FSM
            List<System.Type> states = new List<System.Type>();

            //The nested types/classes in the derived Fsm and all its base classes
            GetNestedClasses(ref states, myTarget.GetType());

            //Filter out invalid classes
            states = states.Where(c =>
                !c.IsAbstract //Only classes we can make an instance of are valid
                && c.IsSubclassOf(typeof(MeFsmStateBase)) //Only States are valid
                ).ToList();

            //Where in our dropdown list are we currently?
            int currentIndex = states.IndexOf(myTarget.StartingState);
            if (currentIndex == -1) currentIndex = 0; //If nothing was found, default to the first item

            //Display a popup listing all valid states (as strings)
            currentIndex = EditorGUILayout.Popup("Starting State", currentIndex, states.Select(s => s.Name.ToString()).ToArray());

            //Set our starting state
            myTarget.StartingState = states[currentIndex];
            //---

            //Display our current state
            string displayedState = myTarget.CurrentStateName;
            if (displayedState != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current State");
                EditorGUILayout.LabelField(displayedState);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
