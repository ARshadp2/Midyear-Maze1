using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public MazeGeneration mazeGenerator;
    public GameObject GoodItem;
    public GameObject BadItem;
    public List<Vector2Int> interests;
    public List<int> interest_state;


    // Distribution: 20% bad, 20% good, 60% neutral
    private float badProb = 0.2f;
    private float goodProb = 0.2f;

    void Start()
    {
        interests = new List<Vector2Int>();
        interest_state = new List<int>();
        // First, generate the maze
        mazeGenerator.Generate();

        // Instantiate Maze
        for (int x=0; x<mazeGenerator.getWidth(); x++)
        {
            for (int y=0; y<mazeGenerator.getHeight(); y++)
            {
                if (!mazeGenerator.map[x,y].wall)
                {
                    // This is a passage cell
                    // Randomly assign good/bad/neutral
                    float randVal = Random.value;
                    if (randVal < badProb)
                    {
                        Instantiate(BadItem, new Vector3(x - mazeGenerator.getWidth()/2, 0, y - mazeGenerator.getHeight()/2), Quaternion.identity);
                        interests.Add(new Vector2Int(x - mazeGenerator.getWidth()/2, y - mazeGenerator.getHeight()/2));
                        interest_state.Add(1);
                    }
                    else if (randVal < badProb + goodProb)
                    {
                        Instantiate(GoodItem, new Vector3(x - mazeGenerator.getWidth()/2, 0, y - mazeGenerator.getHeight()/2), Quaternion.identity);
                        interests.Add(new Vector2Int(x - mazeGenerator.getWidth()/2, y - mazeGenerator.getHeight()/2));
                        interest_state.Add(2);
                    }
                    
                    // else neutral: no item
                }
            }
        }

        // Place player & NPC at known start points (e.g. corners)
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(1-mazeGenerator.getWidth()/2,.2f,mazeGenerator.getHeight()/2-1);

        GameObject npc = GameObject.Find("NPC");
    }
}
