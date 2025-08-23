using System.Collections.Generic;
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

    [SerializeField] public List<GameObject> characters;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = front;
        foreach (GameObject character in characters) {
            SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
            sr.color = new Vector4(sr.color.r, sr.color.g, sr.color.b, 0f);
        }
    }

    void Update()
    {
        if (done && Input.GetMouseButtonDown(0))
        {
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

            if (sr.sprite == front)
            {
                sr.sprite = back;
                backWritingIntro.SetActive(true);
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                sr.sprite = front;
                backWritingIntro.SetActive(false);
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
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

    public void SetDone(bool b)
    {
        done = b;
    }

}
