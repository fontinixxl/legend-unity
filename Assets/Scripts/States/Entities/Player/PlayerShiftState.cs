
public class PlayerShiftState : BaseState
{
    private PlayerStateManager _player;

    public PlayerShiftState(EntityStateManager player)
    {
        _player = (PlayerStateManager)player;
    }
    public override void EnterState()
    {
        // Disable collider so player can go through the invisible walls (behind the doorways)
        _player.Collider.enabled = false;
        // To feel the player is walking through the doorways.
        _player.Animator.SetBool("IsWalking", true);

    }

    public override void Update()
    {
        if (DungeonManager.Instance.Shifting == false)
        {
            _player.Collider.enabled = true;
            _player.TransitionToState(_player.IdleState);
        }
    }
}
