
public class PlayerShiftState : PlayerBaseState
{
    public PlayerShiftState(EntityStateManager entity) : base(entity) { }

    public override void EnterState()
    {
        // Disable collider so player can go through the invisible walls (behind the doorways)
        player.Collider.enabled = false;
        player.Animator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        if (DungeonManager.Instance.Shifting == false)
        {
            player.Collider.enabled = true;
            player.TransitionToState(player.IdleState);
        }
    }
}
