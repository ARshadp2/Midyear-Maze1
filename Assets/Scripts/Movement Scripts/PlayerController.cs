using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.Translate(move);

        // Additional logic for sensing or interacting with maze items could be implemented here
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoodItem"))
        {
            // Increase score by +10
            Debug.Log("Player got +10!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BadItem"))
        {
            // Decrease score by -10
            Debug.Log("Player hit -10!");
            Destroy(other.gameObject);
        }
    }
}
