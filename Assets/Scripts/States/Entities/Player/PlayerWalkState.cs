using UnityEngine;

public class PlayerWalkState : BaseState
{
    //private Vector2 movement;
    
    private PlayerStateManager _player;

    public PlayerWalkState(EntityStateManager player)
    {
        _player = (PlayerStateManager)player;
    }

    public override void EnterState()
    {
        _player.Animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
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
            _player.Direction = movement;
            _player.Animator.SetFloat("MoveX", horizontalMovement);
            _player.Animator.SetFloat("MoveY", verticalMovement);
        }
        else
        {
            // No motion, transition to Idle keeping the previous animation
            _player.TransitionToState(_player.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        Vector2 position = _player.Rigidbody2d.position;
        position += _player.WalkSpeed * _player.Direction * Time.fixedDeltaTime;

        _player.Rigidbody2d.MovePosition(position);

    }
}
