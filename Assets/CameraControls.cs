using UnityEngine;
using DG.Tweening;

public class CameraControls : MonoBehaviour
{

    Vector3 originalPos;
    float originalZoom;
    float closeUpZoom = 4.79f;

    public bool transitioning = false;

    void Start() {
        originalPos = transform.position;
        originalZoom = Camera.main.orthographicSize;
    }

    public void TransitionTo(Vector3 position) {
        transitioning = true;
        transform.DOMove(position + new Vector3(0, 0, -100), 0.5f).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(closeUpZoom, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => transitioning = false);
    }

    public void TransitionToOriginal() {
        transitioning = true;
        transform.DOMove(originalPos, 0.5f).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(originalZoom, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => transitioning = false);
    }
}
