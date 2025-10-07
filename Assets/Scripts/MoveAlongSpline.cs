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
    float acceleration = 0.015f;
    SplineAnimate splineAnimate;

    [SerializeField] float tapMaxSpeed = 0.15f;
    [SerializeField] float tapMinSpeed = -0.15f;
    /*[SerializeField]*/ float tapSpeed = 0f;
    [SerializeField] float tapAcceleration = 0.003f;
    [SerializeField] float tapDecceleration = 0.005f;
    [SerializeField] float tapDeccelerationWhileMovingUp = 0.015f;

    public bool doneMoving = true;

    /*[HideInInspector]*/ public MouthPartHintBehavior hintBehavior;
    float showHintAfterIdleSeconds = 5f;

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

        /*if (progress > 0.95f) {
            hintBehavior.HideHint();
        }*/
        if (progress < 0.90f ) {
            hintBehavior.ShowHint();
        }
        /*if (lastKeyPressedTime + showHintAfterIdleSeconds < Time.timeSinceLevelLoad) {
            hintBehavior.ShowHint();
        }
        if (lastKeyPressed) {
            hintBehavior.HideHint();
        }*/
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
            if (progress < 0.01f) tapSpeed *= 1.5f;
            tapSpeed = Mathf.Clamp(tapSpeed, tapMinSpeed, tapMaxSpeed);

            progress += tapSpeed;

            SoundManager.Instance.PlayLetterClickSound();
        }
        else {
            //decceleration is 2x faster if the lip is moving up
            if (tapSpeed > 0f) tapSpeed -= tapDeccelerationWhileMovingUp * Time.deltaTime;
            else tapSpeed -= tapDecceleration * Time.deltaTime;
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
