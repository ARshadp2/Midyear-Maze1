using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
    public bool walking;
    public Transform playerTrans;
    
    // Footstep-related variables
    public AudioSource footstepAudioSource; // AudioSource component for footstep sounds
    public AudioClip walkClip; // Walking sound clip
    public AudioClip runClip; // Running sound clip
    private bool isFootstepsPlaying = false; // Flag to prevent overlapping footstep sounds
    private float footstepTimer = 0f; // Timer to control footstep interval
    
    void FixedUpdate()
{
    // Set velocity to zero if no movement keys are pressed
    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
        playerRigid.velocity = Vector3.zero;  // Stop movement if no key is pressed
    }

    // Apply movement when W is pressed
    if (Input.GetKey(KeyCode.W)) {
        playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
    }

    // Apply movement when S is pressed
    if (Input.GetKey(KeyCode.S)) {
        playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
    }
}

    
    void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            playerAnim.SetTrigger("walk");
            playerAnim.ResetTrigger("idle");
            walking = true;
            PlayFootsteps("walk");
        }
        
        if(Input.GetKeyUp(KeyCode.W)){
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
            walking = false;
            StopFootsteps();
        }
        
        if(Input.GetKeyDown(KeyCode.S)){
            playerAnim.SetTrigger("walkback");
            playerAnim.ResetTrigger("idle");
           
            PlayFootsteps("walkback");
        }
        
        if(Input.GetKeyUp(KeyCode.S)){
            playerAnim.ResetTrigger("walkback");
            playerAnim.SetTrigger("idle");
            StopFootsteps();
        }
        
        if(Input.GetKey(KeyCode.A)){
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }
        
        if(Input.GetKey(KeyCode.D)){
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }
        
        if(walking == true){                
            if(Input.GetKeyDown(KeyCode.LeftShift)){
          
                w_speed = w_speed + rn_speed;
                playerAnim.SetTrigger("run");
                playerAnim.ResetTrigger("walk");
                PlayFootsteps("run");
            }
            
            if(Input.GetKeyUp(KeyCode.LeftShift)){
               
                w_speed = olw_speed;
                playerAnim.ResetTrigger("run");
                playerAnim.SetTrigger("walk");
                PlayFootsteps("walk");
            }
        }
    }
    
    // Play footstep sounds based on movement
    private void PlayFootsteps(string movementType)
    {
        if (!isFootstepsPlaying)
        {
            isFootstepsPlaying = true;
            
            // Choose the correct sound clip based on the movement type (walking or running)
            if (movementType == "walk")
            {
                footstepAudioSource.clip = walkClip;
            }
            else if (movementType == "run")
            {
                footstepAudioSource.clip = runClip;
            }

            footstepAudioSource.loop = true;  // Loop the footstep sound
            footstepAudioSource.Play();       // Start playing the footstep sound
        }
    }
    
    // Stop footstep sounds when player stops moving
    private void StopFootsteps()
    {
        isFootstepsPlaying = false;
        footstepAudioSource.Stop();
    }
}
