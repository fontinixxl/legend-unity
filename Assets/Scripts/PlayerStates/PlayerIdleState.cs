using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        base.EnterState(player);
        // Transition Animator to Idle (See Animator states graph)
        player.Animator.SetBool("IsWalking", false);
    }

    public override void Update()
    {
        // If any Input direction, transition to walking state
        if (Input.GetAxisRaw("Horizontal") != 0.0f || 
            Input.GetAxisRaw("Vertical") != 0.0f)
        {
            player.TransitionToState(player.WalkState);
        }

        // TODO: Add swingSword Input and transition to swing-sword state
    }
}
