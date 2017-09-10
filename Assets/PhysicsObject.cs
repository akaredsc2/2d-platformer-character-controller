using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f;

    protected Rigidbody2D rigidbody2D;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter2D;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMovementDistance = 0.01f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        contactFilter2D.useTriggers = false;
        contactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter2D.useLayerMask = true;
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
        float distance = movement.magnitude;
        if (distance > minMovementDistance)
        {
            rigidbody2D.Cast(movement, contactFilter2D, hitBuffer, distance + shellRadius);
        }
        rigidbody2D.position += movement;
    }
}