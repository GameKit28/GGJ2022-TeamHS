using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm
{
    public class DyingState : MeFsmState<PlayerStateController>
    {
        private float timer;
        private Animator anim;

        protected override void EnterState()
        {
            if (anim == null) {
                anim = GetComponent<Animator>();
                if (anim == null) {
                    return;
                }
            }
            anim.SetBool("isDead", true);

            GetComponent<PlayerMovementController>().enabled = false;
            GetComponent<PlayerHealthController>().SetImmortal(true);
            timer = 2f;
        }

        protected override void ExitState() {
            base.ExitState();

            anim.SetBool("isDead", false);

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
