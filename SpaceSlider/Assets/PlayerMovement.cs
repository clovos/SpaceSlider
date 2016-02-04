using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float Acceleration;
	public float MaximumDistance;
	public float DragFactor;

	private Vector3 m_currentVelocity;
	private bool m_isColliding = false;

	// Use this for initialization
	void Start () 
	{
		MaximumDistance *= MaximumDistance;
	}

	// Update is called once per frame
	void Update () 
	{
		if(!MapEditor.Instance)
		{
			if(!m_isColliding)
			{
				Vector3 camPos = Camera.main.transform.position;
				camPos.z = transform.position.z;
				Vector3 direction = (camPos - transform.position).normalized;
				m_currentVelocity += (direction * Acceleration * Time.deltaTime) * CalculatedCameraDrag();
				transform.position += m_currentVelocity;				
			}
		}
	}

	float CalculatedCameraDrag()
	{
		Vector3 camPos = Camera.main.transform.position;
		camPos.z = transform.position.z;
		float distance = (camPos - transform.position).sqrMagnitude;
		float calculatedDrag = (distance / MaximumDistance) * DragFactor;
		return calculatedDrag;
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
	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<SlowPowerUpBlock>() != null)
		{
			collision.gameObject.GetComponent<SlowPowerUpBlock>().OnCollision();
		}
	}
//	void OnTriggerStay2D(Collider2D collision)
//	{
//		if(collision.gameObject.GetComponent<BlockBase>().BlockType == BlockBase.BlockProperty.PowerUp)
//		{
//
//		}
//	}
//	void OnTriggerExit2D(Collider2D collision)
//	{
//		if(collision.gameObject.GetComponent<BlockBase>().BlockType == BlockBase.BlockProperty.PowerUp)
//		{
//			GameObjectPool.Instance.AddToPool(collision.gameObject);
//		}
//	}
}
