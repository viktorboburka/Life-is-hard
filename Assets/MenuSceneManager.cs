using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
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


    [System.Serializable] public class MonologueListWrapper {
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

        if (nextLevelButton != null) nextLevelButton.enabled = false;
    }

    void Start()
    {
        TriggerLevelStart();
    }

    void TriggerLevelStart()
    {
        if (outroMonologue == null) return;
        Debug.Log("playing outro monologue");
        outroMonologue.Play();

        StartCoroutine(PlayOutroSubtitles());
        writingSound.Play();

        DOVirtual.DelayedCall(GetOutroSubtitlesLength(), () => {
            TriggerLevelEnd(); 
            writingSound.Stop();
        });

    }

    float GetOutroSubtitlesLength() {
        float total = 0f;
        foreach (float duration in outroSubtitlesDurations) {
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

    public void NextScene() {
        Debug.Log("Next scene");
        LoadScene(nextSceneIdx);
    }

    public void SendPostCard() {
        sendPostcardButton.enabled = false;
        outroPostcard.DOMove(new Vector2(3000, 400), 2f);
        outroPostcard.DORotate(new Vector3(0, 0, -30), 2f).OnComplete(() => {
            if (nextLevelButton != null) 
                nextLevelButton.enabled = true;
        });
    }

}
