using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HoverSwitchSprite : MonoBehaviour
{

    [SerializeField] GameObject hoverSprite;
    [SerializeField] GameObject normalSprite;
    [SerializeField] List<AudioSource> monologues = new List<AudioSource>();

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
        MySceneManager.Instance.gameRunning = false;
        CameraControls cam = Camera.main.GetComponent<CameraControls>();
        cam.TransitionTo(transform.position);
        active = false;
        AudioSource music = SoundManager.Instance.musicSource;
        music.Play();
        music.volume = 0f;
        music.DOFade(1.0f, cam.zoomDuration);

        DOVirtual.DelayedCall(cam.zoomDuration, () =>
        {
            StartCoroutine(PlayMonologues());
        });
    }

    IEnumerator PlayMonologues() {
        foreach (AudioSource monologue in monologues) {
            monologue.Play();
            yield return new WaitForSeconds(monologue.clip.length);
            //while waiting for input
            while (true) {
                yield return null;
                if (Input.anyKeyDown) continue;
            }
        }
    }

    public void ReturnToBigPicture()
    {
        CameraControls cam = Camera.main.GetComponent<CameraControls>();
        done = true;
        cam.TransitionToOriginal();
        AudioSource music = SoundManager.Instance.musicSource;
        music.DOFade(0.0f, cam.zoomDuration);
        DOVirtual.DelayedCall(cam.zoomDuration, () =>
        {
            active = true;
            //ResetSprites();
            music.Stop();
        });
    }

}
