using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
    [SerializeField] AudioSource introMonologue;
    [SerializeField] AudioSource outroMonologue;

    List<HoverSwitchSprite> allFaces;

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
        Debug.Log("playing intro monologue");
        //introMonologue.Play();
        //DOVirtual.DelayedCall(introMonologue.clip.length, () => gameRunning = true);
        gameRunning = true;

    }

    [ContextMenu("Trigger Level End")]
    void TriggerLevelEnd()
    {
        gameRunning = false;
        PostCardFlipper postCardFlipper = FindAnyObjectByType<PostCardFlipper>();
        postCardFlipper.FlipPostCard();
        Debug.Log("playing outro monologue");
        DOVirtual.DelayedCall(postCardFlipper.flipDuration + 0.5f, () => outroMonologue.Play()).OnComplete(() => SceneManager.LoadScene(0));
        //DOVirtual.DelayedCall(outroMonologue.clip.length, () => SceneManager.LoadScene(0));
    }
}
