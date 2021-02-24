using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    private Vector2 movement;

    public override void EnterState(PlayerController player)
    {
        base.EnterState(player);
        // Transition Animator to Walking state (See animator states graph)
        player.Animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        // While shifting to the next room return to IdleState
        if (DungeonManager.Instance.Shifting)
        {
            player.TransitionToState(player.IdleState);
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        // Force to allow the character to move on direction at time!
        movement = Vector2.zero;
        if (horizontalMovement != 0.0f)
        {
            movement.x = horizontalMovement;
        }
        else if (verticalMovement != 0.0f)
        {
            movement.y = verticalMovement;
        }

        // Update the Animator only when there is motion
        if (horizontalMovement != 0.0f || verticalMovement != 0.0f)
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

    public override void OnCollisionEnter()
    {
        throw new System.NotImplementedException();
    }

}
