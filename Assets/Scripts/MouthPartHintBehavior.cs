using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MouthPartHintBehavior : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> sprites;
    List<DG.Tweening.Sequence> spriteSequences;
    [SerializeField] TextMeshPro text;
    DG.Tweening.Sequence textSequence;

    public void SetHint(KeyCode key)
    {
        text.text = key.ToString();
    }

    void Start() {
        sprites = new();
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>(true)) {
            sprites.Add(sprite);
            //Debug.Log(sprite);
        }

        //want only hints, not mouth or mustache
        sprites.Remove(GetComponent<SpriteRenderer>());
        List<SpriteRenderer> mustaches = new();
        foreach (SpriteRenderer sprite in sprites) {
            if (sprite.gameObject.name.Contains("mustache")) {
                mustaches.Add(sprite);
            }
        }
        foreach (SpriteRenderer sprite in mustaches) {
            sprites.Remove(sprite);
        }
    }

    public void ShowHint()
    {
        if (spriteSequences == null)
        {
            spriteSequences = new List<DG.Tweening.Sequence>();
            foreach (SpriteRenderer sprite in sprites)
            {
                spriteSequences.Add(DOTween.Sequence());
            }
        }

        bool isPlaying = false;
        foreach (DG.Tweening.Sequence sequence in spriteSequences)
        {
            if (sequence.IsPlaying())
            {
                isPlaying = true;
                break;
            }
        }
        if (isPlaying || textSequence != null && textSequence.IsPlaying()) return;


        if (text.color.a == 1) return;

        foreach (DG.Tweening.Sequence sequence in spriteSequences)
        {
            sequence.Join(sprites[spriteSequences.IndexOf(sequence)].DOFade(1f, 0.5f).SetEase(Ease.OutCirc));
            sequence.Play();
        }

        textSequence = DOTween.Sequence();
        textSequence.Join(text.DOFade(1f, 0.5f).SetEase(Ease.OutCirc));
        textSequence.Play();
    }

    public void HideHint()
    {
        if (spriteSequences == null)
        {
            spriteSequences = new List<DG.Tweening.Sequence>();
            foreach (SpriteRenderer sprite in sprites)
            {
                spriteSequences.Add(DOTween.Sequence());
            }
        }

        bool isPlaying = false;
        foreach (DG.Tweening.Sequence sequence in spriteSequences)
        {
            if (sequence.IsPlaying())
            {
                isPlaying = true;
                break;
            }
        }

        if (isPlaying || (textSequence != null && textSequence.IsPlaying())) return;

        if (text.color.a == 0) return;

        foreach (DG.Tweening.Sequence sequence in spriteSequences)
        {
            sequence.Join(sprites[spriteSequences.IndexOf(sequence)].DOFade(0f, 0.5f).SetEase(Ease.OutCirc));
            sequence.Play();
        }

        textSequence = DOTween.Sequence();
        textSequence.Join(text.DOFade(0f, 0.5f).SetEase(Ease.OutCirc));
        textSequence.Play();
    }
}
