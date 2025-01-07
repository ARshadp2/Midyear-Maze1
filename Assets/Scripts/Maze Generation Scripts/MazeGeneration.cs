using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    private int width = 21;
    private int height = 21;
    private List<Vector2Int> frontiers;
    public Cell[,] map;
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    public void Generate() {
        map = new Cell[width, height];
        for (int a = 0; a < width; a++)
            for (int b = 0; b < height; b++) {
                map[a,b] = new Cell();
            }

        int x = Random.Range(1, width - 1);
        int y = Random.Range(1, height - 1);
        if (x % 2 == 0)
            x--;
        if (y % 2 == 0)
            y--;

        map[x, y].visit = true;
        map[x, y].wall = false;
        frontiers = new List<Vector2Int>();
        addFrontiers(x, y);
        Debug.Log("here1");
        while (frontiers.Count > 0) {
            int rand = Random.Range(0, frontiers.Count);
            Vector2Int wall = frontiers[rand];
            frontiers.RemoveAt(rand);
            NonOddWall(wall);
        }
        Debug.Log("here2");
        Display();
    }

    private void addFrontiers(int x, int y) {
        // The reason we use x - 2 is because x - 1 would mean that you can make the borders a frontier (which you shouldn't)
        if (x - 2 > 0) 
            frontiers.Add(new Vector2Int(x - 1, y));
        if (x + 2 < width - 1) 
            frontiers.Add(new Vector2Int(x + 1, y));
        if (y - 2 > 0)
            frontiers.Add(new Vector2Int(x, y - 1));
        if (y + 2 < height - 1)
            frontiers.Add(new Vector2Int(x, y + 1));
    }

    private void NonOddWall(Vector2Int wall) {
        // Bridges the connection between an unvisted and visited cell, and tries to get back to two odd numbers
        int x = wall.x;
        int y = wall.y;
        List<Vector2Int> neighbors = GetNeighbors(x, y); // relevant cells adjacent
        if (neighbors.Count == 2)
        {
            Cell cell1 = map[neighbors[0].x, neighbors[0].y];
            Cell cell2 = map[neighbors[1].x, neighbors[1].y];
            if (cell1.visit != cell2.visit)
            {
                map[x, y].wall = false;
                if (!cell1.visit)
                {
                    cell1.visit = true;
                    cell1.wall = false;
                    addFrontiers(neighbors[0].x, neighbors[0].y);
                }
                else
                {
                    cell2.visit = true;
                    cell2.wall = false;
                    addFrontiers(neighbors[1].x, neighbors[1].y);
                }
            }
        }
    }

    private List<Vector2Int> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        // The way this works is that all cells with both cooridnates being even are 100% walls
        // You start at two odd coordinates. If the frontier cell is 1 above, your y becomes even.
        // This means the relevant cells are above and below
        if (x % 2 == 1)
        {
            // Vertical wall
            if (y - 1 >= 0) neighbors.Add(new Vector2Int(x, y - 1));
            if (y + 1 < height) neighbors.Add(new Vector2Int(x, y + 1));
        }
        else if (y % 2 == 1)
        {
            // Horizontal wall
            if (x - 1 >= 0) neighbors.Add(new Vector2Int(x - 1, y));
            if (x + 1 < width) neighbors.Add(new Vector2Int(x + 1, y));
        }

        return neighbors;
    }

    private void Display()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x - width/2, 0f, y - height/2);
                Vector3 WallPos = new Vector3((x-width/2)+1.27f, 2f, (y-height/2)+.5f);

                if (map[x, y].wall)
                {
                    Instantiate(wallPrefab, WallPos, Quaternion.identity, transform);
                }
                else
                {
                    GameObject tile = Instantiate(floorPrefab, position, Quaternion.identity, transform);
                    Renderer render = tile.GetComponent<Renderer>();
                    if ((x+y) % 2 == 0)
                        render.material.SetColor("_Color", Color.yellow);
                    else
                        render.material.SetColor("_Color", Color.green);
                }
            }
        }
    }
    public int getWidth() {
        return width;
    }
    public int getHeight() {
        return height;
    }
}