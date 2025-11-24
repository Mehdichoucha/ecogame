using UnityEngine;
using UnityEngine.UI;

public class TurtleController : MonoBehaviour
{
    [Header("Références")]
    public Transform plastic;
    public float swimForce = 3f;    // vitesse de nage automatique vers le plastique
    public float pushForce = 5f;    // force appliquée par chaque clic/touche
    public Image pushIndicator;      // UI simple pour feedback visuel (optionnel)

    [Header("Limites")]
    public Vector2 minBounds = new Vector2(-8, -4);
    public Vector2 maxBounds = new Vector2(8, 4);

    [Header("Victoire")]
    public float safeZoneX = -6f;    // tortue doit atteindre cette position X pour “zone sûre”
    public float winTime = 3f;       // temps à rester dans zone sûre
    private float safeTimer = 0f;

    private Rigidbody2D rb;
    private bool gameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (pushIndicator != null)
            pushIndicator.color = Color.clear;
    }

    void FixedUpdate()
    {
        if (gameOver || plastic == null) return;

        // --- Nage automatique vers le plastique ---
        Vector2 dir = ((Vector2)plastic.position - rb.position).normalized;
        rb.AddForce(dir * swimForce, ForceMode2D.Force);

        // --- Masher : chaque clic/touche repousse la tortue ---
        bool pushed = false;
        if (Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2 pushDir = (rb.position - (Vector2)plastic.position).normalized;
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
            pushed = true;
        }

        // --- Feedback visuel simple ---
        if (pushIndicator != null)
            pushIndicator.color = pushed ? Color.green : Color.clear;

        // --- Limites d’écran ---
        Vector2 clampedPos = rb.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minBounds.x, maxBounds.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minBounds.y, maxBounds.y);
        rb.position = clampedPos;

        // --- Timer victoire ---
        if (rb.position.x <= safeZoneX)
        {
            safeTimer += Time.fixedDeltaTime;
            if (safeTimer >= winTime)
            {
                Win();
            }
        }
        else
        {
            safeTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plastic"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;
        rb.linearVelocity = Vector2.zero; // <-- correction Unity 6+
        Debug.Log("Game Over !");
        // Ici tu peux déclencher UI Game Over, son, animation, etc.
    }

    void Win()
    {
        gameOver = true;
        rb.linearVelocity = Vector2.zero; // <-- correction Unity 6+
        Debug.Log("Victoire !");
        // Ici tu peux déclencher UI victoire, son, animation, etc.
    }
}
