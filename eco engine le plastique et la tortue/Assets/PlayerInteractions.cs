using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public Rigidbody2D turtleRb;
    public float pushForce = 5f;

    void Update()
    {
        if (Input.GetMouseButton(0)) // clic gauche maintenu ou répétitif
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pushDir = (turtleRb.position - (Vector2)mousePos).normalized;
            turtleRb.AddForce(pushDir * pushForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
