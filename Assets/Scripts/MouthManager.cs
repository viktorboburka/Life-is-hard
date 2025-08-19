using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouthManager : MonoBehaviour
{

    [SerializeField] HoverSwitchSprite hoverSwitchSprite;
    List<MoveAlongSpline> pieces = new();

    float holdTime = 2f;
    float currentHoldTime = 0f;

    bool ended = false;
    public bool started = false;

    void OnEnable() {
        ended = false;
    }

    void Start()
    {
        foreach (Transform child in transform) {
            MoveAlongSpline piece = child.GetComponent<MoveAlongSpline>();
            if (piece == null) continue;
            pieces.Add(child.GetComponent<MoveAlongSpline>());
        }
    }
    void Update()
    {
        if (ended || !started) return;

        float totalProgress = 0f;
        foreach (MoveAlongSpline piece in pieces) {
            totalProgress += piece.progress;
        }
        totalProgress /= pieces.Count;
        
        if (totalProgress >= 0.9f) {
            currentHoldTime += Time.deltaTime;
        }
        else {
            currentHoldTime = 0f;
        }
        if (currentHoldTime >= holdTime) {
            MinigameEnding();
        }

    }

    void MinigameEnding() {
        ended = true;
        foreach (MoveAlongSpline piece in pieces) {
            piece.doneMoving = true;
        }
        DOVirtual.DelayedCall(0.5f, () => SoundManager.Instance.PlayCameraSound());
        DOVirtual.DelayedCall(0.75f, () => MySceneManager.Instance.PlayCameraFlashAnimation());
        DOVirtual.DelayedCall(3.0f, () => hoverSwitchSprite.ReturnToBigPicture());
    }

    public void SetStarted(bool b) {
        started = b;
        foreach (MoveAlongSpline piece in pieces) {
            piece.doneMoving = !b;
        }

    }
}
