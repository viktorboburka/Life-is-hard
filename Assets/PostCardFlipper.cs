using DG.Tweening;
using UnityEngine;

public class PostCardFlipper : MonoBehaviour
{

    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    SpriteRenderer sr;

    bool flipping = false;

    [SerializeField] public float flipDuration;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = front;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) FlipPostCard();
    }

    public void FlipPostCard() {
        if (flipping) return;
        flipping = true;
        DOVirtual.DelayedCall(flipDuration/2f, () => {
            if (sr.sprite == front) sr.sprite = back;
            else sr.sprite = front;

            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
            }
        });
        transform.DORotate(new Vector3(0, 180, 0), flipDuration).SetEase(Ease.InOutSine).OnComplete(() => {
            flipping = false;
        });
    }

    public float GetDuration() {
        return flipDuration;
    }

}
