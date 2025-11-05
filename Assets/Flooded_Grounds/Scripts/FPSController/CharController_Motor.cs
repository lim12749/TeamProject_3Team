using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharController_Motor : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;

    [Header("References")]
    public GameObject cam;

    [Header("Settings")]
    public bool webGLRightClickRotation = true;

    private CharacterController character;
    private float moveFB, moveLR;
    private float rotX, rotY;
    private float gravity = -9.8f;

    void Start()
    {
        character = GetComponent<CharacterController>();

        // 🎯 카메라 자동 연결 (인스펙터에 비어 있으면)
        if (cam == null)
        {
            Camera foundCam = GetComponentInChildren<Camera>();
            if (foundCam != null)
            {
                cam = foundCam.gameObject;
                Debug.Log($"✅ CharController_Motor: Camera automatically assigned -> {cam.name}");
            }
            else
            {
                Debug.LogWarning("⚠️ CharController_Motor: No Camera found! Please assign one manually in the Inspector.");
            }
        }

        // 🎯 에디터 감도 보정
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity *= 1.5f;
        }
    }

    void Update()
    {
        // 🧩 안전한 입력 처리
        float h = SafeGetAxis("Horizontal");
        float v = SafeGetAxis("Vertical");
        float mouseX = SafeGetAxis("Mouse X");
        float mouseY = SafeGetAxis("Mouse Y");

        moveFB = h * speed;
        moveLR = v * speed;
        rotX = mouseX * sensitivity;
        rotY = mouseY * sensitivity;

        CheckForWaterHeight();

        Vector3 movement = new Vector3(moveFB, gravity, moveLR);

        // 🎯 카메라 회전 처리
        if (cam != null)
        {
            if (webGLRightClickRotation)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                    CameraRotation(cam, rotX, rotY);
            }
            else
            {
                CameraRotation(cam, rotX, rotY);
            }
        }

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            // 물속에서는 천천히 낙하
            gravity = -1.0f;
        }
        else
        {
            gravity = -9.8f;
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }

    // 🎯 Input 축이 없어도 예외 안 나도록
    float SafeGetAxis(string axisName)
    {
        try
        {
            return Input.GetAxis(axisName);
        }
        catch
        {
            return 0f;
        }
    }
}
