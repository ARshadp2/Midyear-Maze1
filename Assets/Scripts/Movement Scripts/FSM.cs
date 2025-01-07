using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public string state; //states: idle, move, think, wait
    public GameObject manager;
    public GameObject player;
    public int current;
    private float start_time;
    private bool waiting = false;
    private bool grab = false;
    public GameObject freeze;
    private float launch_gap = 2f;
    private float saved_time_launch = 0;
    // Start is called before the first frame update
    void Start()
    {
        state = "idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - saved_time_launch >= launch_gap && Vector3.Distance(transform.position, player.transform.position) < 5) {
            transform.LookAt(player.transform.position);
            saved_time_launch = Time.time;
            GameObject projectile = Instantiate(freeze, transform.position, transform.rotation);
        }
        if (state.Equals("idle")) {
            if (manager.GetComponent<MazeManager>().interests.Count > 0)
                state = "move";
        }
        else if (state.Equals("move")) {
            GetComponent<NPCMovement>().Run();
            if (manager.GetComponent<MazeManager>().interests.Count == 0)
                state = "idle";
        }
        else if (state.Equals("think")) {
            grab = false;
            int stat = manager.GetComponent<MazeManager>().interest_state[current];
            float randVal = Random.value;
            int x = 0;
            if (stat == 1) {
            }
            if (stat == 2) {
            }
            if (randVal < GetComponent<POMDP>().observations[stat,0]) {
                x = 0;
            }
            else if (randVal < GetComponent<POMDP>().observations[stat,0] + GetComponent<POMDP>().observations[stat,1]) {
                x = 1;
            }
            else if (randVal < GetComponent<POMDP>().observations[stat,0] + GetComponent<POMDP>().observations[stat,1] 
            + GetComponent<POMDP>().observations[stat,2]) {
                x = 2;
            }
            else if (randVal < GetComponent<POMDP>().observations[stat,0] + GetComponent<POMDP>().observations[stat,1] 
            + GetComponent<POMDP>().observations[stat,2] + GetComponent<POMDP>().observations[stat,3]) {
                x = 3;
            }
            else {
                x = 4;
            }
            float[] vals = new float[] {0, 0, 0};
            vals = GetComponent<POMDP>().calculate(x);
            if (vals[1] > vals[2] && vals[1] > 0) {
            }
            else if (vals[2] > vals[1] && vals[2] < 0 || vals[1] == vals[2] || vals[1] > vals[2] && vals[1] < 0) {
            }
            else if (vals[2] > 0) {
                grab = true;
            }
            if (vals[1] > vals[2] && vals[1] > 0 || vals[2] > 0) {
                manager.GetComponent<MazeManager>().interest_state.RemoveAt(current);
                GetComponent<POMDP>().reset();
                state = "move";
            }
            else {
                state = "wait";
            }
        }
        else if (state.Equals("wait")) {
            if (waiting == false) {
                waiting = true;
                start_time = Time.time;
            }
            if (Time.time > start_time + 1) {
                waiting = false;
                state = "think";
            }
        }
    }

    public void think(int x) {
        state = "think";
        current = x;
    }
    void OnTriggerStay(Collider other)
    {
        if (grab == true) {
            if (other.CompareTag("GoodItem"))
            {
                manager.GetComponent<ScoreManager>().score_2 += 1;
                Destroy(other.gameObject);
                grab = false;
            }
            else if (other.CompareTag("BadItem"))
            {
                manager.GetComponent<ScoreManager>().score_2-= 1;
                Destroy(other.gameObject);
                grab = false;
            }
        }
    }
}
