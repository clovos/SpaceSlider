using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float Acceleration;

	private Vector3 m_currentVelocity;
	private bool m_isColliding = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!MapEditor.Instance)
		{
			if(!m_isColliding)
			{
				CameraMovement camMove = Camera.main.GetComponent<CameraMovement>();
				Vector3 vel = camMove.GetCurrentVelocity();
				float currentSpeed = m_currentVelocity.sqrMagnitude;
				float targetSpeed = vel.sqrMagnitude;
				if(Mathf.Abs(currentSpeed - targetSpeed) < 0.01f)
				{
					m_currentVelocity = vel;
				}
				Vector3 target = vel - m_currentVelocity;
				m_currentVelocity += (target.normalized * Acceleration * Time.deltaTime);
				transform.position += m_currentVelocity;				
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		m_isColliding = true;
	}
	void OnCollisionExit2D(Collision2D collision)
	{
		m_currentVelocity.x = 0f;
		m_isColliding = false;
	}
}
