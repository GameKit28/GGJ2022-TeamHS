using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm
{
    public class SpawningState : MeFsmState<PlayerStateController>
    {
        float elapsedTime;

        protected override void EnterState()
        {
            elapsedTime = 2f;
            GetComponent<PlayerMovementController>().enabled = true;
            GetComponent<PlayerHealthController>().enabled = false;

            this.transform.position = ParentFsm.respawnPoint.transform.position;
            ParentFsm.respawnPoint.PlayRespawn();
        }

        private void Update()
        {
            elapsedTime -= Time.deltaTime;
            if(elapsedTime < 0f)
            {
                SwapState<PlayingState>();
            }
        }
    }
}
