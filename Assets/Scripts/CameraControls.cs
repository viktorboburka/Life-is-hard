
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using Unity.Mathematics;

public class CameraControls : MonoBehaviour
{

    Vector3 originalPos;
    float originalZoom;
    float closeUpZoom = 8f;//4.79f;


    public float zoomDuration = 1.0f;
    public bool transitioning = false;

    [SerializeField] SplineContainer spline;
    [SerializeField] float speed = 0.05f;
    Vector3 camPos;
    float time;
    bool zoomedIn;
    float zoomedInMultiplier = 0.1f;

    void Start() {
        originalPos = transform.position;
        originalZoom = Camera.main.orthographicSize;
        camPos = transform.position;
    }

    void Update() {
        if (!transitioning) {
            float speedMultiplier = speed;
            if (zoomedIn) {
                speedMultiplier *= zoomedInMultiplier;
            }
            time = (time + Time.deltaTime * speedMultiplier) % 1f;
            float3 position, tangent, up;
            spline.Evaluate(time, out position, out tangent,out up);
            Vector3 splinePos = new Vector3(position.x, position.y, 0);
            transform.position = camPos + splinePos;
        }
    }

    public void TransitionTo(Vector3 position) {
        transitioning = true;
        zoomedIn = true;
        camPos = position + new Vector3(0, 0, -100);
        time = 0f;
        transform.DOMove(position + new Vector3(0, 0, -100), zoomDuration).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(closeUpZoom, zoomDuration).SetEase(Ease.InOutSine).OnComplete(() => transitioning = false);
        DOVirtual.DelayedCall(zoomDuration - 0.001f, () => time = 0f);
    }

    public void TransitionToOriginal() {
        transitioning = true;
        zoomedIn = false;
        camPos = originalPos;
        time = 0f;
        transform.DOMove(originalPos, zoomDuration).SetEase(Ease.InOutSine);
        GetComponent<Camera>().DOOrthoSize(originalZoom, zoomDuration).SetEase(Ease.InOutSine).OnComplete(() => transitioning = false);
        DOVirtual.DelayedCall(zoomDuration, () => time = 0f);
    }
}
