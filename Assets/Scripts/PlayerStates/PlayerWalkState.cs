using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    private Vector2 movement;

    public override void EnterState(PlayerController player)
    {
        base.EnterState(player);
        player.Animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        // Allow the character to move one direction at time!
        movement = Vector2.zero;
        if (horizontalMovement != 0.0f)
        {
            movement.x = horizontalMovement;
        }
        else if (verticalMovement != 0.0f)
        {
            movement.y = verticalMovement;
        }

        // Update the Animator only when motion
        if (movement != Vector2.zero)
        {
            player.Animator.SetFloat("MoveX", horizontalMovement);
            player.Animator.SetFloat("MoveY", verticalMovement);
        }
        else
        {
            // No motion, transition to Idle keeping the previous animation
            player.TransitionToState(player.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        Vector2 position = player.Rigidbody2d.position;
        position += player.speed * movement * Time.fixedDeltaTime;

        player.Rigidbody2d.MovePosition(position);

    }
}
