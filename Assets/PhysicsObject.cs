using System;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1f;

    protected bool isGrounded;
    protected Vector2 groundNormal;

    protected Vector2 targetVelocity;
    protected Rigidbody2D rigidbody2D;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter2D;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMovementDistance = 0.001f;
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

    private void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {
        
    }

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
        isGrounded = false;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        
        Vector2 surfaceMovementDirection = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 surfaceMovement = surfaceMovementDirection * deltaPosition.x;
        ApplyMovement(surfaceMovement, false);
        
        Vector2 verticalMovement = Vector2.up * deltaPosition.y;
        ApplyMovement(verticalMovement, true);
    }

    private void ApplyMovement(Vector2 movement, bool isVerticalMovement)
    {
        float distance = movement.magnitude;
        if (distance > minMovementDistance)
        {
            int count = rigidbody2D.Cast(movement, contactFilter2D, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            foreach (RaycastHit2D hit2D in hitBufferList)
            {
                Vector2 currentNormal = hit2D.normal;
                Debug.Log("Current normal " + currentNormal);
                if (currentNormal.y > minGroundNormalY)
                {
                    isGrounded = true;
                    Debug.Log("Is grounded");
                    if (isVerticalMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hit2D.distance - shellRadius;
                distance = Math.Min(modifiedDistance, distance);
            }
        }
        rigidbody2D.position = rigidbody2D.position + movement.normalized * distance;
    }
}