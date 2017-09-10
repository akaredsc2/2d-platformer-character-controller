using System;
using UnityEngine;

public class PlayerControllerComponent : PhysicsObject
{
    public float maxSpeed = 3;
    public float jumpTakeOffSpeed = 4;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override Vector2 ComputeVelocity()
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

        return movement * maxSpeed;
    }

    protected override void UpdateVisuals()
    {
        bool isFlipRequired = spriteRenderer.flipX ? targetVelocity.x > 0.01f : targetVelocity.x < 0.01f;
        if (isFlipRequired)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", isGrounded);
        animator.SetFloat("velocityX", Math.Abs(velocity.x) / maxSpeed);
    }
}