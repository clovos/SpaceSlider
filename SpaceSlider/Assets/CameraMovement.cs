using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public bool ShowDebugInfo = false;

    public Vector3 Velocity;
	public float Acceleration;
	private Vector3 m_currentVelocity;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(!MapEditor.Instance)
		{
			float currentSpeed = m_currentVelocity.sqrMagnitude;
			float targetSpeed = Velocity.sqrMagnitude;
			if(Mathf.Abs(currentSpeed - targetSpeed) < 0.01f)
			{
				m_currentVelocity = Velocity;
			}
			Vector3 target = Velocity - m_currentVelocity;
			m_currentVelocity += (target.normalized * Acceleration * Time.deltaTime);
			transform.position += m_currentVelocity;
		}
		else
		{
			UpdateFreeFly();			
		}
	}
	private void UpdateFreeFly()
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll != 0f)
		{
			float currentOrto = Camera.main.orthographicSize;
			currentOrto += (scroll * -3);
			currentOrto = Mathf.Clamp(currentOrto, 1f, 15f);
			Camera.main.orthographicSize = currentOrto;
		}
		if(Input.GetMouseButton(1))
		{
			Vector3 centerPos = new Vector3();
			centerPos.x = Screen.width * 0.5f;
			centerPos.y = Screen.height * 0.5f;
			centerPos.z = Input.mousePosition.z;

			Vector3 diff = Input.mousePosition - centerPos;
			Vector3 direction = diff.normalized;
			float speed = diff.sqrMagnitude * 0.0001f;

			transform.position += direction * speed * Time.deltaTime;
		}
	}

	void OnGUI()
	{
		if(ShowDebugInfo)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
			Rect screenRectangle = new Rect(0, Screen.height - 20, Screen.width, 20.0f);
			string text = "Camera position(" + transform.position.x.ToString() + ", " + transform.position.y.ToString() + ", " + transform.position.z.ToString() + ")"
				+ ", velocity(" + m_currentVelocity.x.ToString() + ", " + m_currentVelocity.y.ToString() + ", " + m_currentVelocity.z.ToString() + ")"
				+ ", screenMousePos(" + Input.mousePosition.x.ToString() + ", " + Input.mousePosition.y.ToString() + ")"
				+ ", worldMousePos(" + mousePos.x.ToString() + ", " + mousePos.y.ToString() + ")";
			GUI.Label(screenRectangle, text);

			if(Acceleration == 0)
				Debug.Log("The cameras acceleration is ZERO, resulting in no movement!");
		}
	}
}
