using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState();

    public abstract void Update();

    public virtual void FixedUpdate() { }

    public virtual void ProcessAI() { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }

    public virtual void OnCollisionExit2D(Collision2D collision) { }
}
