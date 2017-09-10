using System;
using UnityEngine;

public class PlayerControllerComponent : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    protected override void ComputeVelocity()
    {
        Vector2 movement = Vector2.zero;

        movement.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
            }
        }

        targetVelocity = movement * maxSpeed;
    }
}