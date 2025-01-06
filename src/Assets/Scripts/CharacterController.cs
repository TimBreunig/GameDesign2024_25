using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 550f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .1f;
	[SerializeField] private bool m_AirControl = false;
	[SerializeField] private LayerMask m_GroundLayer;
	[SerializeField] private Transform m_GroundCheck;

	const float k_GroundedRadius = .2f;
	private Animator m_animator;
	private bool m_IsGrounded;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_IsFacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_GroundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_IsGrounded = true;
			}
		}
		Debug.Log(m_IsGrounded);
	}


	public void Move(float move, bool jump)
	{
		if (m_IsGrounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if (move != 0)
			{
				m_animator.SetBool("isRunning", true);
				
				if (move > 0 && !m_IsFacingRight)
				{
					Flip();
				}
				else if (move < 0 && m_IsFacingRight)
				{
					Flip();
				}
			}
			else
			{
				m_animator.SetBool("isRunning", false);
			}
		}
		if (m_IsGrounded && jump)
		{
			m_IsGrounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

			m_animator.SetBool("isJumping", true);
		}
		else
		{
			m_animator.SetBool("isJumping", false);
		}
	}


	private void Flip()
	{
		m_IsFacingRight = !m_IsFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}