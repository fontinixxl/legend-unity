using UnityEngine;

public class EnemyKnockedState : BaseState
{
    private readonly EnemyStateManager _enemy;
    public EnemyKnockedState(EntityStateManager enemy)
    {
        _enemy = (EnemyStateManager)enemy;
    }

    public override void EnterState()
    {
        _enemy.Animator.SetBool("IsWalking", false);
        _enemy.Rigidbody2d.velocity = Vector2.zero;
        KnockYourself();
    }

    public override void FixedUpdate()
    {
        if (_enemy.Rigidbody2d.velocity.sqrMagnitude <= 0.2f)
        {
            _enemy.Rigidbody2d.velocity = Vector2.zero;

            _enemy.TransitionToState(_enemy.WalkState);
        }
    }

    public void KnockYourself()
    {
        // Make the eneemy look at oposite direction the attack came from
        // so it's facing the player before continuing movement
        _enemy.Animator.SetFloat("MoveX", _enemy.HitDirection.x * -1);
        _enemy.Animator.SetFloat("MoveY", _enemy.HitDirection.y * -1);

        _enemy.Rigidbody2d.velocity = Vector2.zero;
        _enemy.Rigidbody2d.AddForce(_enemy.HitDirection * _enemy.ThrustForce, ForceMode2D.Impulse);
    }
}
