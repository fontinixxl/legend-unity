
public class PlayerShiftState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        base.EnterState(player);

        // Disable collider so player can go through the invisible walls (behind the doorways)
        player.Collider.enabled = false;
        // To feel the player is walking through the doorways.
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
