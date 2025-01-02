using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POMDP : MonoBehaviour
{
    public float[] belief;
    public float[,] observations;
    // Start is called before the first frame update
    void Start()
    {
        reset();
        // actions are checking, staying, leaving, or listening
        observations = new float[,] {{0, 0, 1, 0, 0}, {.2f, .5f, .2f, .1f, 0}, {0, .1f, .2f, .5f, .2f}}; // observations are bad, semi bad, nothing, semi good, good, and they correspond to the states
    }

    public float[] calculate(int obs) {
        float[] calc = new float[3];
        calc[0] = observations[0, obs] * belief[0];
        calc[1] = observations[1, obs] * belief[1];
        calc[2] = observations[2, obs] * belief[2];
        float total = calc[0] + calc[1] + calc[2];
        calc[0] /= total;
        calc[1] /= total;
        calc[2] /= total;
        belief = calc;
        // Expected Awards
        int yes = 1; // Getting good
        int no = -1; // Getting bad
        int maybe = -2; // Getting nothing
        float cost = .7f; // Listening cost
        // If good is the highest, pick the item
        // If bad, go away
        // If nothing, wait
        float expected_nothing = calc[0] * maybe;
        float expected_bad = calc[1] * yes + calc[2] * no - cost + calc[0] * maybe;
        float expected_good = calc[2] * yes + calc[1] * no - cost + calc[0] * maybe;
        Debug.Log("Prob Nothing: " + calc[0] + " Prob Bad: " + calc[1] + " Prob Good: " + calc[2]);
        Debug.Log("Exp Nothing: " + expected_nothing + " Exp Bad: " + expected_bad + " Exp Good: " + expected_good);
        return new float[] {expected_nothing, expected_bad, expected_good};
    }
    
    public void reset() {
        belief = new float[] {1/3f, 1/3f, 1/3f}; // states are nothing, bad, and good
    }
}
