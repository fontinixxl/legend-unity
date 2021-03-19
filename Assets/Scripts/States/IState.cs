using UnityEngine;

public interface IState
{
    void EnterState();
    void Update();
    void FixedUpdate();
    void OnCollisionEnter2D(Collision2D collision);
    void OnCollisionExit2D(Collision2D collision);
}

public abstract class EnemyBaseState : IState
{
    protected EnemyStateManager enemy;

    protected EnemyBaseState(EntityStateManager entity)
    {
        enemy = (EnemyStateManager)entity;
    }

    public abstract void EnterState();
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnCollisionEnter2D(Collision2D collision) { }
    public virtual void OnCollisionExit2D(Collision2D collision) { }

}

public abstract class PlayerBaseState: IState
{
    protected PlayerStateManager player;

    protected PlayerBaseState(EntityStateManager entity)
    {
        player = (PlayerStateManager)entity;
    }

    public abstract void EnterState();
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnCollisionEnter2D(Collision2D collision) { }
    public virtual void OnCollisionExit2D(Collision2D collision) { }

}