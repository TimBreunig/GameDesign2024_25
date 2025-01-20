using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot_Controller : MonoBehaviour
{
    [SerializeField] private int m_FallSpeed = 10;
    private Animator m_Animator;


    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }


    private void Update()
    {
        transform.position -= transform.up * Time.deltaTime * m_FallSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            m_Animator.SetBool("isColliding", true);
        }
        else if(collider.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
    }


    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
