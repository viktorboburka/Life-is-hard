using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
    [SerializeField] AudioSource introMonologue;
    [SerializeField] AudioSource outroMonologue;
    [SerializeField] int nextSceneIdx = 0;

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

    void TriggerLevelStart() {
        if (introMonologue == null) return;
        Debug.Log("playing intro monologue");
        introMonologue.Play();
        DOVirtual.DelayedCall(introMonologue.clip.length, () => gameRunning = true);

    }

    [ContextMenu("Trigger Level End")]
    void TriggerLevelEnd()
    {
        gameRunning = false;
        PostCardFlipper postCardFlipper = FindAnyObjectByType<PostCardFlipper>();
        postCardFlipper.FlipPostCard();
        Debug.Log("playing outro monologue");
        DOVirtual.DelayedCall(postCardFlipper.flipDuration + 0.5f, () => outroMonologue.Play()).OnComplete(() => LoadScene(nextSceneIdx));
    }

    public void LoadScene (int idx) {
        if (fade) {
            fade.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene(idx));
        }
        else
            SceneManager.LoadScene(idx);
    }

}
