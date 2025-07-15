using DG.Tweening;
using UnityEngine;

public class HoverSwitchSprite : MonoBehaviour
{

    [SerializeField] GameObject hoverSprite;
    [SerializeField] GameObject normalSprite;

    public bool active = true;
    public bool done = false;


    void Start()
    {
        ResetSprites();
    }

    void Update()
    {
        if (done || !MySceneManager.Instance.gameRunning) return;
        if (!active && Input.GetMouseButtonDown(1))
        {
            ReturnToBigPicture();
        }
        if (Input.GetMouseButtonDown(0) && active && hoverSprite.activeInHierarchy)
        {
            ZoomToFace();
        }
    }

    void OnMouseEnter()
    {
        if (!active || done) return;
        hoverSprite.SetActive(true);
        normalSprite.SetActive(false);
    }

    void OnMouseExit()
    {
        if (!active || done) return;
        hoverSprite.SetActive(false);
        normalSprite.SetActive(true);
    }

    void ResetSprites()
    {
        normalSprite.SetActive(true);
        hoverSprite.SetActive(false);
    }

    void ZoomToFace()
    {
        Camera.main.GetComponent<CameraControls>().TransitionTo(transform.position);
        active = false;
        AudioSource music = SoundManager.Instance.musicSource;
        music.Play();
        music.volume = 0f;
        music.DOFade(1.0f, 0.5f);
    }

    public void ReturnToBigPicture()
    {
        done = true;
        Camera.main.GetComponent<CameraControls>().TransitionToOriginal();
        AudioSource music = SoundManager.Instance.musicSource;
        music.DOFade(0.0f, 0.5f);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            active = true;
            //ResetSprites();
            music.Stop();
        });
    }

}
