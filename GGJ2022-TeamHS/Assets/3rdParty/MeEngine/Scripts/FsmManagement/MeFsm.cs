using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeReferences;

namespace MeEngine.FsmManagement
{
    public abstract class MeFsm : MonoBehaviour
    {
        private MeFsmStateBase CurrentState;
        public string CurrentStateName { get { if(CurrentState != null) { return CurrentState.GetType().Name; } else { return null; } } }

        //Used for swapping states
        private SerializableType NextStateType;
        private bool isExitingState = false;

        //This is set via a dropdown in the inspector. Will be the first state we enter unless otherwise specified.
        public SerializableType StartingState;

        protected virtual void Start() { }

        protected virtual void Awake()
        {
            //Swap to our starting state
            _SwapState(StartingState);
        }

        protected virtual void LateUpdate()
        {
            //State swapping occurs during the LateUpdate
            _HandleSwapState();
        }

        /// <summary>
        /// Swap to another state. This will call ExitState on the current state.
        /// This is a protected method because generally its unwise to let another fsm change your state for you.
        /// Instead, you should be responding to a message or providing a static accessor.
        /// </summary>
        protected void SwapState<TNextState>() where TNextState : MeFsmStateBase
        {
            _SwapState(typeof(TNextState));
        }

        protected internal void _SwapState(System.Type nextStateType)
        {
            //Have we already called SwapState this frame?
            //if (isExitingState) //TODO: This appears to be firing when SwapState is called during an enter/exit state, which should be fine
            //{
            //    //Logs.WriteWarning("Swapstate was called twice this frame for " + this.GetType().ToString() +
            //    //	". The swap to " + NextState.GetType().ToString() + " has been trumped by the swap to " + nextState.GetType().ToString() +
            //    //	". Ensure that only one SwapState is called on a given frame. Try checking IsExitingState() first.");
            //}

            isExitingState = true;
            NextStateType = nextStateType;
        }

        /// <summary>
        /// Are we currently in a state of the specified type? Note: Will return true if we are in a subclass of the specified state.
        /// </summary>
        protected bool IsCurrentState<T>() where T : MeFsmStateBase
        {
            System.Type cStateType = CurrentState.GetType();
            return typeof(T) == cStateType || cStateType.IsSubclassOf(typeof(T));
        }

        /// <summary>
        /// True if we are in a null state.
        /// </summary>
        protected bool IsNullState()
        {
            return IsCurrentState<NullState>();
        }

        /// <summary>
        /// Return the current state as the specified type.
        /// </summary>
        protected T GetCurrentState<T>() where T : MeFsmStateBase
        {
            return CurrentState as T;
        }


        internal void _SwapState<TNextState>() where TNextState : MeFsmStateBase
        {
            //This function is just used to end-run the protected access of swapState internally
            SwapState<TNextState>();
        }

        //Called in LateUpdate
        internal void _HandleSwapState()
        {
            //Ignore if we've already called swapstate this frame
            if (isExitingState)
            {
                //Call our old state's ExitState() function
                if (CurrentState != null)
                {
                    CurrentState._DoExitState();
                    Destroy(CurrentState);
                }

                // It is important to clear the flags after we called ExitState() above, so that SwapState() calls from inside that 
                // ExitState will be cleared
                isExitingState = false;
                CurrentState = null;
            }
            if (NextStateType != null)
            {
                //Swap State
                CurrentState = (MeFsmStateBase)gameObject.AddComponent(NextStateType.Type);
                NextStateType = null;

                //Call our new state's EnterState() function
                CurrentState._SetParent(this);
                CurrentState._DoEnterState();
            }
        }

        /// <summary>
        /// A default state for all Fsms where nothing happens.
        /// </summary>
        protected class NullState : MeFsmStateBase { }
    }
}
