using UnityEngine;

public class PlayerIdleState : BaseState
{
    private PlayerStateManager _player;

    public PlayerIdleState(EntityStateManager entity)
    {
        _player = (PlayerStateManager)entity;
    }
    public override void EnterState()
    {
        // Transition Animator to Idle (See Animator states graph)
        _player.Animator.SetBool("IsWalking", false);
    }

    public override void Update()
    {
        // If any Input direction, transition to walking state
        if (Input.GetAxisRaw("Horizontal") != 0.0f || 
            Input.GetAxisRaw("Vertical") != 0.0f)
        {
            _player.TransitionToState(_player.WalkState);
        }

        // TODO: Add swingSword Input and transition to swing-sword state
    }
}
