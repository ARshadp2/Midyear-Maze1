using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
        if (other.tag == "Player") {
            other.GetComponent<ExtraHelper>().manager.GetComponent<ScoreManager>().score_1 -= 2;
        }
        if (other.tag != "AI")
            Destroy(gameObject);
    }
}
