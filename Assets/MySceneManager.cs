using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
    [SerializeField] AudioSource introMonologue;
    [SerializeField] AudioSource outroMonologue;
    [SerializeField] int nextSceneIdx = 0;
    [SerializeField] GameObject subtitlesContainer;
    [SerializeField] List<TextMeshProUGUI> introsubtitles;
    [SerializeField] List<float> introSubtitlesDurations;
    [SerializeField] List<TextMeshProUGUI> outrosubtitles;
    [SerializeField] List<float> outroSubtitlesDurations;
    [SerializeField] Image flash;
    [SerializeField] GameObject controlsHint;

    [System.Serializable] public class MonologueListWrapper {
        public List<TextMeshProUGUI> subtitles;
        public List<float> durations;
    }
    [SerializeField] List<MonologueListWrapper> monologuesSubtitles;

    List<HoverSwitchSprite> allFaces;

    [SerializeField] Image fade;

    public bool gameRunning;

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
    }

    void Start()
    {
        allFaces = new(FindObjectsByType<HoverSwitchSprite>(FindObjectsSortMode.None));
        TriggerLevelStart();
    }

    void Update()
    {
        if (allFaces.TrueForAll(face => face.done))
        {
            if (gameRunning && Camera.main.GetComponent<CameraControls>().transitioning == false)
                TriggerLevelEnd();
        }
    }

    void TriggerLevelStart()
    {
        if (introMonologue == null) return;
        Debug.Log("playing intro monologue");
        introMonologue.Play();

        StartCoroutine(PlayIntroSubtitles());

        DOVirtual.DelayedCall(GetIntroSubtitlesLength(), () => {
            gameRunning = true;        
        });

    }

    float GetIntroSubtitlesLength() {
        float total = 0f;
        foreach (float duration in introSubtitlesDurations) {
            total += duration;
        }
        return total;
    }

    float GetOutroSubtitlesLength() {
        float total = 0f;
        foreach (float duration in outroSubtitlesDurations) {
            total += duration;
        }
        return total;
    }

    IEnumerator PlayIntroSubtitles()
    {
        subtitlesContainer.SetActive(true);
        for (int i = 0; i < introsubtitles.Count; i++)
        {
            introsubtitles[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(introSubtitlesDurations[i]);
            introsubtitles[i].gameObject.SetActive(false);
        }
        subtitlesContainer.SetActive(false);
    }

    IEnumerator PlayOutroSubtitles()
    {
        subtitlesContainer.SetActive(true);
        for (int i = 0; i < outrosubtitles.Count; i++)
        {
            outrosubtitles[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(outroSubtitlesDurations[i]);
            outrosubtitles[i].gameObject.SetActive(false);
        }
        subtitlesContainer.SetActive(false);
    }

    [ContextMenu("Trigger Level End")]
    void TriggerLevelEnd()
    {
        gameRunning = false;
        PostCardFlipper postCardFlipper = FindAnyObjectByType<PostCardFlipper>();
        postCardFlipper.FlipPostCard();
        float delay = postCardFlipper.flipDuration + 0.5f;

        DOVirtual.DelayedCall(delay, () => {
            Debug.Log("playing outro monologue");
            StartCoroutine(PlayOutroSubtitles());
            outroMonologue.Play();
        });
        
        DOVirtual.DelayedCall(GetOutroSubtitlesLength() + delay, () => LoadScene(nextSceneIdx));
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

    public IEnumerator PlayMonologueSubtitles(int monologueIdx) {
        MonologueListWrapper monologue = monologuesSubtitles[monologueIdx];
        subtitlesContainer.SetActive(true);
        for (int i = 0; i < monologue.subtitles.Count; i++)
        {
            monologue.subtitles[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(monologue.durations[i]);
            monologue.subtitles[i].gameObject.SetActive(false);
        }
        subtitlesContainer.SetActive(false);
    }

    public void PlayCameraFlashAnimation() {
        flash.DOFade(1f, 0.05f).OnComplete(() => flash.DOFade(0f, 0.25f));
    }

    public float GetMonologueDuration(int idx) {
        float total = 0f;
        foreach (float duration in monologuesSubtitles[idx].durations) {
            total += duration;
        }
        return total;
    }

    public void ShowControlsHint() {
        controlsHint.SetActive(true);
    }

    public void HideControlsHint() {
        controlsHint.SetActive(false);
    }

}
