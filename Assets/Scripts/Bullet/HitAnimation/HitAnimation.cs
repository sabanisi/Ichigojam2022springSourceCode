using UnityEngine;
using System.Collections;

public class HitAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly string blueEffect = "HitAnimation1";
    private static readonly string yellowEffect = "HitAnimation2";

    public void Initialize(HitAnimationEnum animationEnum,Vector3 pos)
    {
        transform.position = pos;
        SetAnimation(animationEnum);
    }

    private void SetAnimation(HitAnimationEnum animationEnum)
    {
        switch (animationEnum)
        {
            case HitAnimationEnum.Blue:
                StartCoroutine(AnimationCoroutine(blueEffect));
                break;
            case HitAnimationEnum.Yellow:
                StartCoroutine(AnimationCoroutine(yellowEffect));
                break;
        }
    }

    private IEnumerator AnimationCoroutine(string animationName)
    {
        animator.Play(animationName);
        yield return null;
        yield return new WaitAnimation(animator, 0);

        gameObject.SetActive(false);
    }
}
