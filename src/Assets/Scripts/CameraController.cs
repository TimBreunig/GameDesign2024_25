using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float _dampTime = 0.5f;

    private Vector3 _cameraPos;
    private Vector3 _velocity = Vector3.zero;


    private void Update()
    {
        _cameraPos = new Vector3(0f, player.position.y + 1f, -10f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, _cameraPos, ref _velocity, _dampTime);
    }
}
