using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float start;
    // Update is called once per frame
    void Start() {
        start = Time.time;
    }

    void Update()
    {
        transform.position += transform.forward * 3 * Time.deltaTime;
        if (Time.time - start >= 5) {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.tag == "AI") {
            other.GetComponent<FSM>().manager.GetComponent<ScoreManager>().score_2 -= 1;
        }
        if (other.tag != "Player")
            Destroy(gameObject);
    }
    
}
