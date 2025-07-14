using UnityEngine;
using DG.Tweening;

public class CameraControls : MonoBehaviour
{

    Vector3 originalPos;
    float originalZoom;
    float closeUpZoom = 4.79f;

    void Start() {
        originalPos = transform.position;
        originalZoom = Camera.main.orthographicSize;
    }

    public void TransitionTo(Vector3 position) {
        transform.DOMove(position + new Vector3(0, 0, -10), 0.5f).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(closeUpZoom, 0.5f).SetEase(Ease.InOutSine);
    }

    public void TransitionToOriginal() {
        transform.DOMove(originalPos, 0.5f).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(originalZoom, 0.5f).SetEase(Ease.InOutSine);
    }
}
