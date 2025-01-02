using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour
{
    // This is a placeholder for a more complex system
    // NPC can attempt pathfinding with partial knowledge (A*) or POMDP
    // For simplicity, we can simulate random wandering or a simple BFS/A* if known
    public float speed = 3f;
    public GameObject player;
    private NavMeshAgent agent;
    public GameObject manager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
    }

    public void Run()
    {
        float shortest = 10000;
        int num = 0;
        for (int x = 0; x < manager.GetComponent<MazeManager>().interests.Count; x++) {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, 
            new Vector3(manager.GetComponent<MazeManager>().interests[x].x, 0, manager.GetComponent<MazeManager>().interests[x].y), NavMesh.AllAreas, path);
            float length = distance(path);
            if (length < shortest) {
                shortest = length;
                num = x;
            }
        }
        agent.SetDestination(new Vector3(manager.GetComponent<MazeManager>().interests[num].x, 0, manager.GetComponent<MazeManager>().interests[num].y));
        if (manager.GetComponent<MazeManager>().interests[num].x + .3 >= transform.position.x && 
        transform.position.x >= manager.GetComponent<MazeManager>().interests[num].x - .3 &&
        manager.GetComponent<MazeManager>().interests[num].y + .3 >= transform.position.z && 
        transform.position.z >= manager.GetComponent<MazeManager>().interests[num].y - .3) {
            Debug.Log(manager.GetComponent<MazeManager>().interests[num]);
            Debug.Log(manager.GetComponent<MazeManager>().interest_state[num]);
            manager.GetComponent<MazeManager>().interests.RemoveAt(num);
            GetComponent<FSM>().think(num);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoodItem"))
        {
            Debug.Log("NPC got +10!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BadItem"))
        {
            Debug.Log("NPC hit -10!");
            Destroy(other.gameObject);
        }
    }

    private float distance(NavMeshPath path) {
        float distance = 0;
        for (int x = 0; x < path.corners.Length-1; x++)
            distance += Vector3.Distance(path.corners[x], path.corners[x+1]);
        return distance;
    }
}