using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(EntityStateManager entity) : base(entity) { }

    public override void EnterState()
    {
        player.Animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.TransitionToState(player.SwingSwordState);
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        // Allow the character to move one direction at time!
        Vector2 movement = Vector2.zero;
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
            player.Direction = movement;
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
        player.KinematicController.MovePosition(player.Direction, player.WalkSpeed);
    }
}
