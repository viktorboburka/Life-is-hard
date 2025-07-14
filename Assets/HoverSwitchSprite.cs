using DG.Tweening;
using UnityEngine;

public class HoverSwitchSprite : MonoBehaviour
{

    [SerializeField] GameObject hoverSprite;
    [SerializeField] GameObject normalSprite;

    public bool active = true;


    void Start()
    {
        ResetSprites();
    }

    void Update()
    {
        if (!active && Input.GetMouseButtonDown(1)) {
            Camera.main.GetComponent<CameraControls>().TransitionToOriginal();
            DOVirtual.DelayedCall(0.5f, () => {
                active = true;
                ResetSprites();
            });
        }
        if (Input.GetMouseButtonDown(0) && active && hoverSprite.activeInHierarchy) {
            Camera.main.GetComponent<CameraControls>().TransitionTo(transform.position);
            active = false;
        }
    }

    void OnMouseEnter() {
        if (!active) return;
        hoverSprite.SetActive(true);
        normalSprite.SetActive(false);
    }

    void OnMouseExit() {
        if (!active) return;
        hoverSprite.SetActive(false);
        normalSprite.SetActive(true);
    }

    void ResetSprites() {
        normalSprite.SetActive(true);
        hoverSprite.SetActive(false);
    }

}
