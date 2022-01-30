using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm
{
    public class PlayingState : MeFsmState<PlayerStateController>
    {
        protected override void EnterState()
        {
            GetComponent<PlayerMovementController>().enabled = true;
            GetComponent<PlayerHealthController>().SetImmortal(false);
        }
    }
}
