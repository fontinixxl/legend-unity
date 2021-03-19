using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(EntityStateManager entity) : base(entity){}

    public override void EnterState()
    {
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            player.TransitionToState(player.SwingSwordState);
        }
    }
}
