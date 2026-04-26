using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{

    public static MenuSceneManager Instance;
    [SerializeField] AudioSource outroMonologue;
    [SerializeField] AudioSource writingSound;
    [SerializeField] int nextSceneIdx = 0;
    [SerializeField] GameObject subtitlesContainer;
    [SerializeField] List<TextMeshProUGUI> outrosubtitles;
    [SerializeField] List<float> outroSubtitlesDurations;

    [SerializeField] RectTransform outroPostcard;

    [SerializeField] Button nextLevelButton;
    [SerializeField] Button sendPostcardButton;

    [SerializeField] GameObject closeButton;
    [SerializeField] GameObject restartButton;

    const bool FESTIVAL_BUILD = true;
    float lastInputTimer = 0f;


    [System.Serializable]
    public class MonologueListWrapper
    {
        public List<TextMeshProUGUI> subtitles;
        public List<float> durations;
    }


    [SerializeField] Image fade;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        fade.DOFade(0f, 0.5f);
        if (!FESTIVAL_BUILD) {
            foreach (FestivalBuildOnly festivalBuildOnly in FindObjectsOfType<FestivalBuildOnly>())
            {
                festivalBuildOnly.gameObject.SetActive(false);
            }
        }
        //if (nextLevelButton != null) nextLevelButton.enabled = false;
    }

    void Start()
    {
        TriggerLevelStart();
    }

    void Update()
    {
        if (FESTIVAL_BUILD)
        {
            CheckGameIdling();
        }
    }

    void CheckGameIdling()
    {
        //Debug.Log("last input timer: " + lastInputTimer);
        if (!Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)
            && Input.mousePositionDelta.x < 0.0001f && Input.mousePositionDelta.y < 0.0001f)
        {
            lastInputTimer += Time.deltaTime;
            if (lastInputTimer > 180f && SceneManager.GetActiveScene().buildIndex != 0)
            {
                LoadScene(0);
            }
        }
        else
        {
            lastInputTimer = 0f;
        }
    }

    void TriggerLevelStart()
    {
        if (outroMonologue == null) return;
        Debug.Log("playing outro monologue");
        outroMonologue.Play();

        if (outrosubtitles.Count == 0)
        {
            StartCoroutine(nextLevelPostcardWiggle());
        }

        StartCoroutine(PlayOutroSubtitles());

        if (outrosubtitles.Count != 0)
            writingSound.Play();

        DOVirtual.DelayedCall(GetOutroSubtitlesLength(), () =>
        {
            TriggerLevelEnd();
            writingSound.Stop();
        });

    }

    float GetOutroSubtitlesLength()
    {
        float total = 0f;
        foreach (float duration in outroSubtitlesDurations)
        {
            total += duration;
        }
        return total;
    }

    IEnumerator PlayOutroSubtitles()
    {
        subtitlesContainer.SetActive(true);
        for (int i = 0; i < outrosubtitles.Count; i++)
        {
            outrosubtitles[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(outroSubtitlesDurations[i]);
            //outrosubtitles[i].gameObject.SetActive(false);
        }

        if (outrosubtitles.Count == 0)
        {
            yield break;
        }

        Vector3 sendPostCardButtonRotation = sendPostcardButton.gameObject.GetComponent<RectTransform>().rotation.eulerAngles;

        yield return new WaitForSeconds(1f);
        while (sendPostcardButton.enabled)
        {
            sendPostcardButton.gameObject.transform.DORotate(sendPostCardButtonRotation + new Vector3(0, 0, 1.5f), 0.1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                sendPostcardButton.gameObject.transform.DORotate(sendPostCardButtonRotation, 0.1f).SetEase(Ease.InOutSine);
            });
            yield return new WaitForSeconds(3f);
        }
        //sendPostcardButton.gameObject.transform.DORotate(sendPostCardButtonRotation + new Vector3(0, 0, 1), 0.2f).SetEase(Ease.InOutBounce);
        //subtitlesContainer.SetActive(false);
    }

    [ContextMenu("Trigger Level End")]
    void TriggerLevelEnd()
    {
        sendPostcardButton.enabled = true;
        //SendPostCard(2f);
    }

    public void LoadScene(int idx)
    {
        if (fade)
        {
            fade.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene(idx));
        }
        else
            SceneManager.LoadScene(idx);
    }

    public void NextScene()
    {
        Debug.Log("Next scene");
        LoadScene(nextSceneIdx);
    }

    public void SendPostCard()
    {
        float duration = 2f;
        sendPostcardButton.enabled = false;
        outroPostcard.DOMove(outroPostcard.position + new Vector3(3000, 400, 0), duration).SetEase(Ease.InSine);
        foreach (TextMeshProUGUI subtitle in outrosubtitles)
        {
            var subT = subtitle.GetComponent<RectTransform>();
            subT.DOMove(subT.position + new Vector3(3000, 400, 0), duration).SetEase(Ease.InSine);
            subT.DORotate(new Vector3(0, 0, -30), duration).SetEase(Ease.InSine);
        }
        outroPostcard.DORotate(new Vector3(0, 0, -30), duration).SetEase(Ease.InSine).OnComplete(() =>
        {
            if (nextLevelButton != null)
            {
                nextLevelButton.enabled = true;
                StartCoroutine(nextLevelPostcardWiggle());
            }
        });
    }

    IEnumerator nextLevelPostcardWiggle()
    {
        yield return new WaitForSeconds(1f);
        Vector3 nextPostCardButtonRotation = nextLevelButton.gameObject.GetComponent<RectTransform>().rotation.eulerAngles;
        while (nextLevelButton.enabled)
        {
            nextLevelButton.gameObject.transform.DORotate(nextPostCardButtonRotation + new Vector3(0, 0, 1.5f), 0.1f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    nextLevelButton.gameObject.transform.DORotate(nextPostCardButtonRotation, 0.1f).SetEase(Ease.InOutSine);
                });
            yield return new WaitForSeconds(3f);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void RevealCloseAndRestartButtons()
    {
        //if (restartButton == null || closeButton == null) return;
        DOVirtual.DelayedCall(3f, () =>
        {
            if (restartButton != null)
            {
                restartButton.SetActive(true);
                restartButton.GetComponent<Image>().DOFade(1f, 2f);
            }
            if (closeButton != null || !FESTIVAL_BUILD)
            {
                closeButton.SetActive(true);
                closeButton.GetComponent<Image>().DOFade(1f, 2f);
            }

        });
    }

}
