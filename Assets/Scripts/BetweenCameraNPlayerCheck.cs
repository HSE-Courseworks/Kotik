using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenCameraNPlayerCheck : MonoBehaviour
{
    private RaycastHit hit;
    public bool isBetweenCameraAndPlayer = false;
    public bool isCollidingWithCamera = false;
    private float alphaStartValue = 1.0f;
    private float alphaEndValue = 0.5f;
    private float duration = 0.5f;
    private MeshRenderer _meshRenderer;

    void Start () {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update () {

        // Check if object is between camera and player
        if (!isBetweenCameraAndPlayer && IsObjectBetweenCameraAndPlayer()) {
            isBetweenCameraAndPlayer = true;
            SetObjectTranslucent();
        }

        if (isBetweenCameraAndPlayer && !IsObjectBetweenCameraAndPlayer()) {
            isBetweenCameraAndPlayer = false;
            SetObjectOpaque();
        }

        // Check if object is colliding with camera
        if (!isCollidingWithCamera && IsObjectCollidingWithCamera()) {
            isCollidingWithCamera = true;
            SetObjectTranslucent();
        }

        if (isCollidingWithCamera && !IsObjectCollidingWithCamera()) {
            isCollidingWithCamera = false;
            SetObjectOpaque();
        }
    }

    bool IsObjectBetweenCameraAndPlayer() {
        Vector3 playerPosition = Camera.main.GetComponent<CameraManager>().target.transform.position;;
        if (Physics.Linecast(transform.position, playerPosition, out hit))
        {
            Debug.Log("Лайнкаст в между");
            if (hit.collider == GetComponent<Collider>()) {
                return true;
            }
        }
        return false;
    }

    bool IsObjectCollidingWithCamera() {
        float cameraHeight = Camera.main.transform.position.y;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            Debug.Log("Рейкаст в коллизии");
            if (hit.distance <= cameraHeight) {
                return true;
            }
        }
        return false;
    }

    void SetObjectTranslucent() {
        StartCoroutine(TransitTranslucence(alphaStartValue, alphaEndValue, duration));
    }
    
    void SetObjectOpaque() {
        StartCoroutine(TransitTranslucence(alphaEndValue, alphaStartValue, duration));
    }

    IEnumerator TransitTranslucence(float startValue, float endValue, float totalTime) {
        float elapsedTime = 0.0f;
        while (elapsedTime < totalTime) {
            elapsedTime += Time.deltaTime;
            float alphaValue = Mathf.Lerp(startValue, endValue, (elapsedTime / totalTime));
            Color color = _meshRenderer.material.color;
            color.a = alphaValue;
            _meshRenderer.material.color = color;
            yield return null;
        }
    }
}
