using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject m_Background;
    [SerializeField] private Transform m_Player;
    [SerializeField] private float m_DampTime = 0.5f;

    private Vector3 m_CameraPos;
    private Vector3 m_Velocity = Vector3.zero;


    private void Update()
    {
        m_CameraPos = new Vector3(0f, m_Player.position.y + 1f, -10f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, m_CameraPos, ref m_Velocity, m_DampTime);

        m_Background.transform.position = new Vector3(0, 0.96f * transform.position.y + 1.25f, 1f);
    }
}
