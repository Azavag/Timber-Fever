using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField] private Image hintArrowImage;
    [Header("Scale tween")]
    [SerializeField] private float tweenScaleTime;
    [SerializeField] private Ease tweenScaleEase;
    [Header("Move tween")]
    [SerializeField] private float tweenMoveTime;
    [SerializeField] private Ease tweenMoveEase;
    [SerializeField] private float endYPos = 65;
    Tween moveTween;

    private void Start()
    {
        hintArrowImage.enabled = false;
        MoveObjectTween(hintArrowImage.transform, endYPos, tweenMoveTime);
    }

    public void ShowHint()
    {
        hintArrowImage.enabled = true;
        hintArrowImage.transform.localScale = Vector3.zero;
        Tween scaleTween = ScaleObjectTween(hintArrowImage.transform,
            1f, tweenScaleTime);
        scaleTween.OnComplete(ShowHintCallback);
        scaleTween.Play();
    }
    void ShowHintCallback()
    {
        moveTween.Play();
    }

    public void HideHint() 
    {
        Tween scaleTween = ScaleObjectTween(hintArrowImage.transform, 0f, tweenScaleTime);
        scaleTween.OnComplete(HideHintCallback);
        scaleTween.Play();
        
    }

    void HideHintCallback()
    {
        hintArrowImage.enabled = false;
        moveTween.Rewind();
    }

    void MoveObjectTween(Transform obj, float endPoseValue, float moveTime)
    {
        moveTween = obj.transform.DOLocalMoveY(endPoseValue, moveTime)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(tweenMoveEase)
            .SetAutoKill();
    }
   
    Tween ScaleObjectTween(Transform obj, float scaleValue, float scaleTime) 
    {
        return obj.transform.DOScale(scaleValue, scaleTime)
            .SetEase(tweenScaleEase)
            .SetAutoKill();
    }

   
}
