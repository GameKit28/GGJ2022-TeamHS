using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovementController))]
public class CatAnimator : MonoBehaviour
{
    private AudioSource audioSource;
    private Vector2 velocity;

    const float motion_threshold_speed = 0.1f;
    // private reference to player movement type
    private PlayerMovementController movement_controller;
    // Cat animator type 
    private Animator anim;

    [SerializeField] private AudioClip[] footstepSounds;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        // get a refernce to the player movement type:
        movement_controller = GetComponent<PlayerMovementController>();        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 veloIn = movement_controller.inputVector;
        // TODO get forward velocity instead of input velowwwwwwwwcity
        
        bool is_moving = veloIn.magnitude > motion_threshold_speed;
        if (is_moving)
            anim.SetBool("Move", true);
        else    
            anim.SetBool("Move", false);

        velocity = new Vector2(Mathf.Lerp(velocity.x,veloIn.x,0.1f),Mathf.Lerp(velocity.y,veloIn.y,0.2f));
        anim.SetFloat("VelX", velocity.x);
        anim.SetFloat("VelY", velocity.y);        

        
        // Debug.Log(string.Format("Set animation coeff moving: {2} x:{0}, y:{1}", velocity.x, velocity.y, is_moving));
    }

    public void DoFootstepSound() {
        if(footstepSounds.Length == 0) {
            Debug.Log("No footstep sounds were added to the character.\n\nLoser.");
            return;
        }
        audioSource.clip = footstepSounds[Random.Range(1, 3)];
        audioSource.Play();
    }

}
