using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int score_1 = 0;
    public int score_2 = 0;
    public TMP_Text player_p;
    public TMP_Text NPC_p;
    // Start is called befores the first frame update
    void Update()
    {
        player_p.SetText("Score: " + score_1); // Update the score display
        NPC_p.SetText("Score: " + score_2);
    }
    public void reset() {
        score_1 = 0;
        score_2 = 0;
    }
}
