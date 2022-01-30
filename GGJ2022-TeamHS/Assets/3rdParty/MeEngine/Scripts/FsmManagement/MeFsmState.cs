using UnityEngine;
using System.Collections;

namespace MeEngine.FsmManagement
{
    public abstract class MeFsmState<T> : MeFsmStateBase where T : MeFsm
    {
        /// <summary>
        /// The parent fsm of this state
        /// </summary>
        protected T ParentFsm
        {
            get { return _parentFsm as T; }
        }
    }

    public abstract class MeFsmStateBase : MonoBehaviour
    {
        /// <summary>
        /// Called once each time the Fsm enters this state.
        /// Useful for initializing.
        /// </summary>
        protected virtual void EnterState() { }

        /// <summary>
        /// Called Every Unity Update.
        /// </summary>
        //protected virtual void UpdateState() { }

        /// <summary>
        /// Called once each time the Fsm leaves this state after SwapState has been called.
        /// Useful for cleaning up after yourself.
        /// </summary>
        protected virtual void ExitState() { }

        /// <summary>
        /// Swap to another state. This will call ExitState on the current state.
        /// This is a protected method because generally its unwise to let another fsm change your state for you.
        /// Instead, you should be responding to a message or providing a static accessor.
        /// </summary>
        protected void SwapState<TNextState>() where TNextState : MeFsmStateBase
        {
            _parentFsm._SwapState<TNextState>();
        }

        #region Internal
        protected internal MeFsm _parentFsm;

        internal void _SetParent(MeFsm parent)
        {
            _parentFsm = parent;
        }

        internal void _DoExitState()
        {
            ExitState();
        }

        internal void _DoEnterState()
        {
            EnterState();
        }

        //internal void _DoUpdateState()
        //{
        //    UpdateState();
        //}
        #endregion
    }
}


