using DG.Tweening;
using UnityEngine;

public class ui_twinkle : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        // 무한 반복: 0(투명) → 1(불투명) → 0(투명)
        canvasGroup.alpha = 1f; // 시작값
        FadeOutInLoop();
    }

    private void FadeOutInLoop()
    {
        canvasGroup.DOFade(0f, fadeDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                canvasGroup.DOFade(1f, fadeDuration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(FadeOutInLoop);
            });
    }
}
