using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HoverSwitchSprite : MonoBehaviour
{

    [SerializeField] GameObject hoverSprite;
    [SerializeField] GameObject normalSprite;
    [SerializeField] List<AudioSource> monologues = new List<AudioSource>();
    [SerializeField] int monologueIdx = 0;
    [SerializeField] MouthManager mouthManager;

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
        if (!active || done || !MySceneManager.Instance.gameRunning) return;
        hoverSprite.SetActive(true);
        normalSprite.SetActive(false);
    }

    void OnMouseExit()
    {
        if (!active || done || !MySceneManager.Instance.gameRunning) return;
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

        DOVirtual.DelayedCall(cam.zoomDuration, () =>
        {
            StartCoroutine(PlayMonologues());
            StartCoroutine(MySceneManager.Instance.PlayMonologueSubtitles(monologueIdx));
            DOVirtual.DelayedCall(MySceneManager.Instance.GetMonologueDuration(monologueIdx), () =>
            {
                AudioSource music = SoundManager.Instance.musicSource;
                music.Play();
                music.volume = 0f;
                music.DOFade(1.0f, 3.0f);

                mouthManager.SetStarted(true);
                MySceneManager.Instance.ShowControlsHint();
            });
        });
    }

    IEnumerator PlayMonologues()
    {
        foreach (AudioSource monologue in monologues)
        {
            monologue.Play();
            yield return new WaitForSeconds(monologue.clip.length);
            //while waiting for input
            while (true)
            {
                yield return null;
                if (Input.anyKeyDown) continue;
            }
        }
    }

    public void ReturnToBigPicture()
    {
        MySceneManager.Instance.HideControlsHint();
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
            MySceneManager.Instance.gameRunning = true;
        });
    }

}
