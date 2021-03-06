using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform _camTransform;

    private void Awake()
    {
        _camTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camTransform.rotation * Vector3.forward,
            _camTransform.rotation * Vector3.up);
    }



    [ContextMenu("Face To Camera")]
    public void FaceToCamera()
    {
        Awake();
        transform.LookAt(transform.position + _camTransform.rotation * Vector3.forward,
           _camTransform.rotation * Vector3.up);
    }
}