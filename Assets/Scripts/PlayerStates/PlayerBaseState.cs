using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerController player;
    public virtual void EnterState(PlayerController player)
    {
        this.player = player;
    }

    public abstract void Update();

    // TODO: I don't like I am force to implement this when not all the subclasses will use
    public virtual void FixedUpdate() { }

    //public abstract void OnCollisionEnter();
}
