using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f;

    protected Rigidbody2D rigidbody2D;
    protected Vector2 velocity;

    private void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 movement = Vector2.up * deltaPosition.y;
        ApplyMovement(movement);
    }

    private void ApplyMovement(Vector2 movement)
    {
        rigidbody2D.position += movement;
    }
}