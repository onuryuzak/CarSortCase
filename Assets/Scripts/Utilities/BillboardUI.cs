using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class BillboardUI : MonoBehaviour
{
    private Transform camTransform;
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
        if (_camera)
        {
            camTransform = _camera.transform;
            if (TryGetComponent(out Canvas canvas) && !canvas.worldCamera)
                canvas.worldCamera = _camera;
        }
        else
            Debug.LogError("Main Camera is empty!");
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward, camTransform.rotation * Vector3.up);
    }
}