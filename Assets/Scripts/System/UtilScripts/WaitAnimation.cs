using System;
using UnityEngine;

public class WaitAnimation:CustomYieldInstruction
{
    private Animator m_animator;
    private int m_lastStateHash = 0;
    private int m_layerNo = 0;

    public WaitAnimation(Animator animator,int layerNo)
    {
        Init(animator, layerNo, animator.GetCurrentAnimatorStateInfo(layerNo).fullPathHash);
    }

    private void Init(Animator animator,int layerNo,int hash)
    {
        m_animator = animator;
        m_lastStateHash = hash;
        m_layerNo = layerNo;
    }

    public override bool keepWaiting
    {
        get
        {
            var currentAnimationState = m_animator.GetCurrentAnimatorStateInfo(m_layerNo);
            return currentAnimationState.fullPathHash == m_lastStateHash &&
                (currentAnimationState.normalizedTime < 1);
        }
    }
}
