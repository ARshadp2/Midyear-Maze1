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
    public GameObject AI;
    public float time;
    public GameObject manager;
    public GameObject freeze;
    private bool time_sound = false;
    
    // Footstep-related variables
    public AudioSource footstepAudioSource; // AudioSource component for footstep sounds
    public AudioClip walkClip; // Walking sound clip
    public AudioClip runClip; // Running sound clip
    public AudioClip observed;
    private bool isFootstepsPlaying = false; // Flag to prevent overlapping footstep sounds
    private float footstepTimer = 0f; // Timer to control footstep interval
    private float launch_gap = 2f;
    private float saved_time_launch = 0;

    
    void Update(){
        /*
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
        */
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) {
            transform.rotation = Quaternion.Euler(0, 315, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) {
            transform.rotation = Quaternion.Euler(0, 225, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) {
            transform.rotation = Quaternion.Euler(0, 45, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)) {
            transform.rotation = Quaternion.Euler(0, 135, 0);
        }
        if ((Input.GetKey(KeyCode.C)) && Time.time - saved_time_launch >= launch_gap) {
            saved_time_launch = Time.time;
            playerAnim.ResetTrigger("throw");
            playerAnim.SetTrigger("throw");
            GameObject projectile = Instantiate(freeze, transform.position, transform.rotation);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                playerAnim.ResetTrigger("run");
                playerAnim.SetTrigger("run");
                PlayFootsteps("walk");
                transform.position += transform.forward * 3f * Time.deltaTime;
            }
            else {
                playerAnim.ResetTrigger("walk");
                playerAnim.SetTrigger("walk");
                PlayFootsteps("run");
                transform.position += transform.forward * 1.5f * Time.deltaTime;
            }
        }
        else {
            playerAnim.enabled = false;
            playerAnim.enabled = true;
            playerAnim.ResetTrigger("idle");
            playerAnim.SetTrigger("idle");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StopFootsteps();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
            StopFootsteps();
        }
        if (time_sound == true)
            if (Time.time - 1 > time) {
                StopFootsteps();
                time_sound = false;
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
                footstepAudioSource.pitch = 1f;
                footstepAudioSource.clip = walkClip;
            }
            else if (movementType == "run")
            {
                footstepAudioSource.pitch = 1f;
                footstepAudioSource.clip = runClip;
            }

            footstepAudioSource.loop = true;  // Loop the footstep sound
            footstepAudioSource.Play();       // Start playing the footstep sound
        }
    }

    void OnTriggerStay(Collider other)
    {
        int stat = 0;
        if (other.CompareTag("GoodItem"))
        {
            stat = 2;
        }
        else if (other.CompareTag("BadItem"))
        {
            stat = 1;
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            Debug.Log("work2");
            float randVal = Random.value;
            footstepAudioSource.clip = observed;
            footstepAudioSource.loop = false;
            if (randVal < AI.GetComponent<POMDP>().observations[stat,0]) {
                footstepAudioSource.pitch = .25f;
                Debug.Log("test1");
                footstepAudioSource.Play();
            }
            else if (randVal < AI.GetComponent<POMDP>().observations[stat,0] + AI.GetComponent<POMDP>().observations[stat,1]) {
                footstepAudioSource.pitch = .5f;
                Debug.Log("test2");
                footstepAudioSource.Play();
            }
            else if (randVal < AI.GetComponent<POMDP>().observations[stat,0] + AI.GetComponent<POMDP>().observations[stat,1] 
            + AI.GetComponent<POMDP>().observations[stat,2]) {
                Debug.Log("test3");
                footstepAudioSource.Stop();
            }
            else if (randVal < AI.GetComponent<POMDP>().observations[stat,0] + AI.GetComponent<POMDP>().observations[stat,1] 
            + AI.GetComponent<POMDP>().observations[stat,2] + AI.GetComponent<POMDP>().observations[stat,3]) {
                footstepAudioSource.pitch = 1.5f;
                Debug.Log("test4");
                footstepAudioSource.Play();
            }
            else {
                footstepAudioSource.pitch = 2.5f;
                Debug.Log("test5");
                footstepAudioSource.Play();
            }
        }
        time = Time.time;
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (other.CompareTag("GoodItem"))
            {
                manager.GetComponent<ScoreManager>().score_1 += 1;
            }
            else if (other.CompareTag("BadItem"))
            {
                manager.GetComponent<ScoreManager>().score_1 -= 1;
            }
            Destroy(other.gameObject);
        }
    }
    
    // Stop footstep sounds when player stops moving
    private void StopFootsteps()
    {
        isFootstepsPlaying = false;
        footstepAudioSource.Stop();
    }
}
