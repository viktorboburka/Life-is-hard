using System;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{

    [SerializeField] KeyCode key;
    bool lastKeyPressed;
    float lastKeyPressedTime = -Mathf.Infinity;
    public float progress = 0f;


    float maxSpeed = 0.02f;
    float minSpeed = 0.01f;
    float speed = 0f;
    float acceleration = 0.01f;
    SplineAnimate splineAnimate;

    float tapMaxSpeed = 0.15f;
    float tapMinSpeed = -0.15f;
    float tapSpeed = 0f;
    float tapAcceleration = 0.003f;
    float tapDecceleration = 0.005f;

    public bool doneMoving = true;

    MouthPartHintBehavior hintBehavior;
    float showHintAfterIdleSeconds = 3f;

    void Start()
    {
        doneMoving = true;
        splineAnimate = GetComponent<SplineAnimate>();
        hintBehavior = GetComponent<MouthPartHintBehavior>();
        hintBehavior?.SetHint(key);
    }

    void Update()
    {
        HintUpdate();
        //HoldControlsUpdate();
        TapControlsUpdate();
    }
    
    void HintUpdate() {
        if (!hintBehavior || doneMoving) return;
        if (lastKeyPressedTime + showHintAfterIdleSeconds < Time.timeSinceLevelLoad) {
            hintBehavior.ShowHint();
        }
        if (lastKeyPressed) {
            hintBehavior.HideHint();
        }
    }

    void TapControlsUpdate() {
        if (doneMoving) {
            splineAnimate.ElapsedTime = progress;
            return;
        }
        bool currentKeyPress = Input.GetKeyDown(key);
        if (currentKeyPress) {
            lastKeyPressedTime = Time.timeSinceLevelLoad;
            tapSpeed += tapAcceleration;
            tapSpeed = Mathf.Clamp(tapSpeed, tapMinSpeed, tapMaxSpeed);

            progress += tapSpeed;

            SoundManager.Instance.PlayLetterClickSound();
        }
        else {
            tapSpeed -= tapDecceleration * Time.deltaTime;
            //decceleration is 2x faster if the lip is moving up
            if (tapSpeed > 0f) tapSpeed -= tapDecceleration * Time.deltaTime;
            tapSpeed = Mathf.Clamp(tapSpeed, tapMinSpeed, tapMaxSpeed);

            progress += tapSpeed;
        }
        progress = Mathf.Clamp(progress, 0f, 0.999f);
        if (progress >= 0.998f) tapSpeed /= 2;
        if (progress < 0.001f) tapSpeed = 0;

        splineAnimate.ElapsedTime = progress;

        lastKeyPressed = Input.GetKey(key);

    }

    void HoldControlsUpdate()
    {
        if (doneMoving) {
            splineAnimate.ElapsedTime = progress;
            return;
        }
        bool currentKeyPress = Input.GetKey(key);
        if (currentKeyPress) {
            if (!lastKeyPressed) {
                speed = 0f;
            }
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            progress += speed;
        }
        else {
            if (lastKeyPressed) {
                speed = 0f;
            }
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            progress -= speed;
        }
        progress = Mathf.Clamp(progress, 0f, 0.999f);

        splineAnimate.ElapsedTime = progress;

        lastKeyPressed = Input.GetKey(key);
    }
}
