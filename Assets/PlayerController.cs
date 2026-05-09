using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float velocidade = 5f;
    private Rigidbody2D rb;
    private Vector2 movimento;

    void Start() => rb = GetComponent<Rigidbody2D>();

    void Update() {
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movimento.normalized * velocidade * Time.fixedDeltaTime);
    }
}