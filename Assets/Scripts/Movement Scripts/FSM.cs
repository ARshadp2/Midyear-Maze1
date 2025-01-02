using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public string state; //states: idle, move, think, wait
    public GameObject manager;
    public int current;
    private float start_time;
    private bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {
        state = "idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (state.Equals("idle")) {
            Debug.Log("idle");
            if (manager.GetComponent<MazeManager>().interests.Count > 0)
                state = "move";
        }
        else if (state.Equals("move")) {
            GetComponent<NPCMovement>().Run();
            if (manager.GetComponent<MazeManager>().interests.Count == 0)
                state = "idle";
        }
        else if (state.Equals("think")) {
            int stat = manager.GetComponent<MazeManager>().interest_state[current];
            float randVal = Random.value;
            int x = 0;
            if (stat == 1) {
                Debug.Log("actual: bad");
            }
            if (stat == 2) {
                Debug.Log("actual: good");
            }
            Debug.Log(randVal);
            if (randVal < GetComponent<POMDP>().observations[stat,0]) {
                x = 0;
                Debug.Log("bad");
            }
            else if (randVal < GetComponent<POMDP>().observations[stat,0] + GetComponent<POMDP>().observations[stat,1]) {
                x = 1;
                Debug.Log("semi-bad");
            }
            else if (randVal < GetComponent<POMDP>().observations[stat,0] + GetComponent<POMDP>().observations[stat,1] 
            + GetComponent<POMDP>().observations[stat,2]) {
                x = 2;
                Debug.Log("none");
            }
            else if (randVal < GetComponent<POMDP>().observations[stat,0] + GetComponent<POMDP>().observations[stat,1] 
            + GetComponent<POMDP>().observations[stat,2] + GetComponent<POMDP>().observations[stat,3]) {
                x = 3;
                Debug.Log("semi-good");
            }
            else {
                x = 4;
                Debug.Log("good");
            }
            float[] vals = new float[] {0, 0, 0};
            vals = GetComponent<POMDP>().calculate(x);
            if (vals[1] > vals[2] && vals[1] > 0) {
                Debug.Log("calculated bad");
            }
            else if (vals[2] > vals[1] && vals[2] < 0 || vals[1] == vals[2] || vals[1] > vals[2] && vals[1] < 0) {
                Debug.Log("need more info");
            }
            else if (vals[2] > 0) {
                Debug.Log("calculated good");
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
}
