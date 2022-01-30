using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (PlayerMovementController))]
public class CatAnimator : MonoBehaviour
{

    const float motion_threshold_speed = 0.1f;
    // private reference to player movement type
    private PlayerMovementController movement_controller;
    // Cat animator type 
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // get a refernce to the player movement type:
        movement_controller = GetComponent<PlayerMovementController>();        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = movement_controller.inputVector;
        // TODO get forward velocity instead of input velocity
        
        bool is_moving = velocity.magnitude > motion_threshold_speed;
        if (is_moving)
            anim.SetBool("move", true);
        else    
            anim.SetBool("move", false);        

        anim.SetFloat("VelX", velocity.x);
        anim.SetFloat("VelY", velocity.y);        

        
        // Debug.Log(string.Format("Set animation coeff moving: {2} x:{0}, y:{1}", velocity.x, velocity.y, is_moving));
    }
}
