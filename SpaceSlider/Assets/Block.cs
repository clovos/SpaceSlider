using UnityEngine;
using System.Collections;

[System.Serializable]
public class Block : MonoBehaviour {

	[System.Serializable]
	public enum BlockProperty
	{
		Empty,
		Movable,
		NonMovable,
		PowerUp,
	};

	public BlockProperty BlockType;
	private GridCell m_parentCell;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_parentCell != null && !MapEditor.Instance)
		{
			Vector3 parentPos = m_parentCell.GetPosition();
			Vector3 direction = (parentPos - transform.position).normalized;
			transform.position += direction * 1f * Time.deltaTime;
		}
	}

	public void SetParentCell(GridCell parentCell)
	{
		m_parentCell = parentCell;
	}

	public void UpdateMovement()
	{
		if(m_parentCell != null)
		{
			if(BlockType == BlockProperty.Movable)
			{
				Vector3 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
				currentScreenPos.y = Input.mousePosition.y;
	
				Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(currentScreenPos);
				transform.position = currentWorldPos;
			}
		}
	}
}
