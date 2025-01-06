using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POMDPSoundManager : MonoBehaviour
{
    public POMDP pomdpScript;
    public AudioClip soundA;
    public AudioSource audioSource; 

    private void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlaySoundBasedOnProbability();
        }
    }

    private void PlaySoundBasedOnProbability()
    {
        // Get the probabilities from the POMDP script
        float[] expectedRewards = pomdpScript.calculate(3);
        float probGood = pomdpScript.belief[2]; // Probability of the "good" state

        Debug.Log($"Probability of Good: {probGood}");

        // Play the appropriate sound based on the "good" probability
        if (probGood > 0.5f)
        {
            audioSource.clip = soundA;
        }

        // Play the sound
        audioSource.Play();
    }
}
