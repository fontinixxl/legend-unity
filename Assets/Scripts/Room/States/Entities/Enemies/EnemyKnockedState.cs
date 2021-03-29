using System.Collections;
using UnityEngine;

public class EnemyKnockedState : EnemyBaseState
{
    public EnemyKnockedState(EnemyStateManager entity) : base(entity) { }

    public override void EnterState()
    {
        enemy.Animator.SetBool("IsWalking", false);

        // Face the player
        enemy.Animator.SetFloat("MoveX", enemy.HitDirection.x * -1);
        enemy.Animator.SetFloat("MoveY", enemy.HitDirection.y * -1);

        enemy.Rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        enemy.Rigidbody2d.AddForce(enemy.HitDirection * enemy.ThrustForce, ForceMode2D.Impulse);

        enemy.StartCoroutine(GoWalking());
    }

    private IEnumerator GoWalking()
    {
        yield return new WaitForSeconds(.3f);
        enemy.Rigidbody2d.velocity = Vector2.zero;
        enemy.Rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
        enemy.TransitionToState(enemy.WalkState);

    }
}
