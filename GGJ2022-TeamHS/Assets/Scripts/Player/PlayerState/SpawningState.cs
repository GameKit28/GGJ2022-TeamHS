using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm
{
    public class SpawningState : MeFsmState<PlayerStateController>
    {
        protected override void EnterState()
        {
            GetComponent<PlayerMovementController>().enabled = true;
            GetComponent<PlayerHealthController>().SetImmortal(true);
            ParentFsm.respawnPoint.EnableSafetyZone(true);

            this.transform.position = ParentFsm.respawnPoint.transform.position;
            ParentFsm.respawnPoint.PlayRespawn();
        }

        protected override void ExitState()
        {
            ParentFsm.respawnPoint.EnableSafetyZone(false);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == SafetyZoneTag)
            {
                SwapState<PlayingState>();
            }
        }
    }
}
