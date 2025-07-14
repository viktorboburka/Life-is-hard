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
            AudioSource music = SoundManager.Instance.musicSource;
            music.DOFade(0.0f, 0.5f);
            DOVirtual.DelayedCall(0.5f, () => {
                active = true;
                ResetSprites();
                music.Stop();
            });
            
        }
        if (Input.GetMouseButtonDown(0) && active && hoverSprite.activeInHierarchy) {
            Camera.main.GetComponent<CameraControls>().TransitionTo(transform.position);
            active = false;
            AudioSource music = SoundManager.Instance.musicSource;
            music.Play();
            music.volume = 0f;
            music.DOFade(1.0f, 0.5f);

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
