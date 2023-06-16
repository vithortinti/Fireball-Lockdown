using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Behavior _cameraBehavior;
    private Vector3 _direction;
    private Vector3 _realPosition;
    private GameObject _sliders;

    // Start is called before the first frame update
    void Start()
    {
        _sliders = GameObject.FindWithTag("Sliders");
        DisableSlidersBeforeCameraIsCorrect(true);
        _cameraBehavior = GetComponent<Behavior>();
        _realPosition = new Vector3(-0.01f, 0.1f, -10f);
        _direction = _cameraBehavior.GetDirection(_realPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyAnimation();
    }

    void ApplyAnimation()
    {
        _cameraBehavior.MoveTo(_direction);
        _cameraBehavior.Speed -= 0.0018f; // Deixa a movimentação da câmera mais suave

        if (Vector3.Distance(transform.position, _realPosition) <= 0.01f)
        {
            DisableSlidersBeforeCameraIsCorrect(false);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(_cameraBehavior);
            Destroy(this);
        }
    }

    void DisableSlidersBeforeCameraIsCorrect(bool active)
    {
        _sliders.SetActive(!active);
    }
}
