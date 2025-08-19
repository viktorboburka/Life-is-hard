using DG.Tweening;
using UnityEngine;

public class PostCardFlipper : MonoBehaviour
{

    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] GameObject backWritingIntro;
    [SerializeField] GameObject backWritingOutro;
    [SerializeField] AudioSource flipSound;
    SpriteRenderer sr;
    [SerializeField] float backWritingDuration = 10f;

    bool flipping = false;
    bool done = false;
    [SerializeField] public float flipDuration;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = front;
    }

    void Update()
    {
        if (done && Input.GetMouseButtonDown(0)) {
            MySceneManager.Instance.NextScene();
        } 
    }

    public void FlipPostCard()
    {
        if (flipping) return;
        flipping = true;
        flipSound.Play();
        DOVirtual.DelayedCall(flipDuration / 2f, () =>
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            if (sr.sprite == front)
            {
                sr.sprite = back;
                backWritingIntro.SetActive(true);
            }
            else
            {
                sr.sprite = front;
                backWritingIntro.SetActive(false);
            }

        });
        transform.DORotate(new Vector3(0, 180, 0), flipDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            flipping = false;
        });

        DOVirtual.DelayedCall(flipDuration * 2, () =>
        {
            if (sr.sprite == back)
            {
                backWritingOutro.SetActive(true);
            }
            else
            {
                backWritingOutro.SetActive(false);
            }
        });
    }

    public float GetDuration()
    {
        return flipDuration;
    }

    public float GetWritingDuration()
    {
        return backWritingDuration;
    }

    public void SetDone(bool b) {
        done = b;
    }

}
