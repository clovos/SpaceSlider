using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public bool ShowDebugInfo = false;

    public Vector3 Velocity;
	public float Acceleration;
	private Vector3 m_currentVelocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(ShowDebugInfo)
		{
			if(Acceleration == 0)
				Debug.LogError("The cameras acceleration is ZERO, resulting in no movement!");
		}

		if(!Game.Instance.MapEditorMode)
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
		UpdateFreeFly();
	}
	private void UpdateFreeFly()
	{
		if(Game.Instance.MapEditorMode)
		{
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
	}

	void OnGUI()
	{
		if(ShowDebugInfo || Game.Instance.MapEditorMode)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
			Rect screenRectangle = new Rect(0, Screen.height - 20, Screen.width, 20.0f);
			string text = "Camera position(" + transform.position.x.ToString() + ", " + transform.position.y.ToString() + ", " + transform.position.z.ToString() + ")"
				+ ", velocity(" + m_currentVelocity.x.ToString() + ", " + m_currentVelocity.y.ToString() + ", " + m_currentVelocity.z.ToString() + ")"
				+ ", screenMousePos(" + Input.mousePosition.x.ToString() + ", " + Input.mousePosition.y.ToString() + ")"
				+ ", worldMousePos(" + mousePos.x.ToString() + ", " + mousePos.y.ToString() + ")";
			GUI.Label(screenRectangle, text);

			if(Game.Instance.MapEditorMode)
			{
				screenRectangle = new Rect(Screen.width - 100f, 0f, 100f, 30.0f);
				text = "[EDITOR MODE]";
				GUI.Label(screenRectangle, text);
			}

//			//Draw arrow
//			Vector3 rayPos = transform.position;
//			rayPos.x -= m_currentVelocity.x * 0.5f;
//			rayPos.y -= m_currentVelocity.y * 0.5f;
//			rayPos.z = Camera.main.nearClipPlane + 0.1f;
//
//			Vector3 direction = m_currentVelocity.normalized;
//			Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 1, 1);
//			Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, -1, 1);
//
//			Debug.DrawLine(rayPos, direction, new Color(1, 0, 0));
//			Debug.DrawRay(rayPos + direction, right * 0.25f, new Color(1, 0, 0));
//			Debug.DrawRay(rayPos + direction, left * 0.25f, new Color(1, 0, 0));
		}
	}

}
