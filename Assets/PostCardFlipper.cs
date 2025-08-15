using DG.Tweening;
using UnityEngine;

public class PostCardFlipper : MonoBehaviour
{

    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] GameObject backWriting;
    [SerializeField] AudioSource flipSound;
    SpriteRenderer sr;
    [SerializeField] float backWritingDuration = 10f;

    bool flipping = false;

    [SerializeField] public float flipDuration;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = front;
    }

    public void FlipPostCard()
    {
        if (flipping) return;
        flipping = true;
        flipSound.Play();
        DOVirtual.DelayedCall(flipDuration / 2f, () =>
        {
            if (sr.sprite == front)
            {
                sr.sprite = back;
                //TODO: only show old writing
                backWriting.SetActive(true);
            }
            else
            {
                sr.sprite = front;
                backWriting.SetActive(false);
            }

            foreach (Transform child in transform)
            {
                if (child.gameObject != backWriting)
                    child.gameObject.SetActive(false);
            }
        });
        transform.DORotate(new Vector3(0, 180, 0), flipDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            flipping = false;
        });

        DOVirtual.DelayedCall(flipDuration * 2, () => {
            if (sr.sprite == back)
            {
                //TODO: show new writing
            }
            else
            {
            }
        });
    }

    public float GetDuration()
    {
        return flipDuration;
    }

    public float GetWritingDuration() {
        return backWritingDuration;
    }

}
