using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private Transform _player;
    [SerializeField] private float _dampTime = 0.5f;

    private Vector3 _cameraPos;
    private Vector3 _backgroundPos;
    private Vector3 _velocity = Vector3.zero;


    private void Update()
    {
        _cameraPos = new Vector3(0f, _player.position.y + 1f, -10f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, _cameraPos, ref _velocity, _dampTime);

        _background.transform.position = new Vector3(-0.05f * _player.position.x, 0.96f * transform.position.y + 1.25f, 1f);
    }
}
