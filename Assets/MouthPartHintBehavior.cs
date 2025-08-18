using UnityEngine;
using DG.Tweening;
using TMPro;

public class MouthPartHintBehavior : MonoBehaviour
{

    [SerializeField] SpriteRenderer sprite;
    Sequence spriteSequence;
    [SerializeField] TextMeshPro text;
    Sequence textSequence;

    
    public void SetHint(KeyCode key) {
        text.text = key.ToString();
    }

    public void ShowHint() {
        if (spriteSequence != null && textSequence != null)
            if (spriteSequence.IsPlaying() || textSequence.IsPlaying()) return;

        if (sprite.color.a == 1) return;
        if (text.color.a == 1) return;

        spriteSequence = DOTween.Sequence();
        textSequence = DOTween.Sequence();
        spriteSequence.Join(sprite.DOFade(1f, 0.5f).SetEase(Ease.OutCirc));
        spriteSequence.Play();
        textSequence.Join(text.DOFade(1f, 0.5f).SetEase(Ease.OutCirc));
        textSequence.Play();
    }

    public void HideHint() {
        if (spriteSequence != null && textSequence != null)
            if (spriteSequence.IsPlaying() || textSequence.IsPlaying()) return;

        if (sprite.color.a == 0) return;
        if (text.color.a == 0) return;

        spriteSequence = DOTween.Sequence();
        textSequence = DOTween.Sequence();
        spriteSequence.Join(sprite.DOFade(0f, 0.5f).SetEase(Ease.OutCirc));
        spriteSequence.Play();
        textSequence.Join(text.DOFade(0f, 0.5f).SetEase(Ease.OutCirc));
        textSequence.Play();
    }

}
