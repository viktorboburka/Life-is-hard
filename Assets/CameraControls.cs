using UnityEngine;
using DG.Tweening;

public class CameraControls : MonoBehaviour
{

    Vector3 originalPos;
    float originalZoom;
    float closeUpZoom = 4.79f;


    public float zoomDuration = 1.0f;
    public bool transitioning = false;

    void Start() {
        originalPos = transform.position;
        originalZoom = Camera.main.orthographicSize;
    }

    public void TransitionTo(Vector3 position) {
        transitioning = true;
        transform.DOMove(position + new Vector3(0, 0, -100), zoomDuration).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(closeUpZoom, zoomDuration).SetEase(Ease.InOutSine).OnComplete(() => transitioning = false);
    }

    public void TransitionToOriginal() {
        transitioning = true;
        transform.DOMove(originalPos, zoomDuration).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(originalZoom, zoomDuration).SetEase(Ease.InOutSine).OnComplete(() => transitioning = false);
    }
}
