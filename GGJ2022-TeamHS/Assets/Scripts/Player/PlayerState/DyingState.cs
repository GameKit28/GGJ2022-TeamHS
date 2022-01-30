using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm
{
    public class DyingState : MeFsmState<PlayerStateController>
    {
        private float timer;
        protected override void EnterState()
        {
            GetComponent<PlayerMovementController>().enabled = false;
            GetComponent<PlayerHealthController>().SetImmortal(true);
            timer = 2f;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if(timer < 0f)
            {
                SwapState<SpawningState>();
            }
        }
    }
}
