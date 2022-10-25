using System;
public abstract class AbstractBossAction
{
    protected Boss parent;
    public AbstractBossAction(Boss _parent)
    {
        parent = _parent;
    }

    public abstract void Update();

    public virtual void FinishAction() { }
}
