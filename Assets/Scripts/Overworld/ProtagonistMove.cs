using UnityEngine;

public class ProtagonistMove : MonoBehaviour
{
    public float moveSpeed = 4f;

    private Rigidbody2D body;

    void Awake()
    {
        this.body = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, v, 0);
        this.body.velocity = direction.normalized * this.moveSpeed;
    }
}