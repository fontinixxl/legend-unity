using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyIdleState : BaseState
{
    private readonly EnemyStateManager _enemy;
    
    // used for AI waiting
    private float _waitDuration;
    private float _waitTimer;

    public EnemyIdleState(EnemyStateManager entity)
    {
        _enemy = entity;
    }

    public override void EnterState()
    {
        //Debug.Log("Ghost: Entering Idle state");

        _enemy.Animator.SetBool("IsWalking", false);
        _waitDuration = _waitTimer = 0;
    }

    public override void Update()
    {
        if (_waitDuration == 0)
        {
            // TODO: make it a class property of _enemy
            _waitDuration = Random.Range(1, 3);
        }
        else
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer > _waitDuration)
            {
                _enemy.TransitionToState(_enemy.WalkState);

            }
        }
    }
}
