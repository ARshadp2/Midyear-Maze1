using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNav : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
}
