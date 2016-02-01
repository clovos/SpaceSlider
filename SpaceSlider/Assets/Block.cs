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
		TotalAmountOfTypes
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
			Vector3 direction = (parentPos - transform.position);
			float length = direction.sqrMagnitude;
			direction.Normalize();

			if(length < 0.05f)
				transform.position = parentPos;
			else
				transform.position += direction * 2f * Time.deltaTime;
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
				currentScreenPos.y = Mathf.Lerp(currentScreenPos.y, Input.mousePosition.y, Time.deltaTime * 25f);
	
				Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(currentScreenPos);
				transform.position = currentWorldPos;

				Vector3 direction = (transform.position - m_parentCell.GetPosition());
				float currentLenght = direction.sqrMagnitude;
				currentLenght *= currentLenght;

				direction.Normalize();
				float limit = (m_parentCell.GetDimensions().y * 0.2f) * (m_parentCell.GetDimensions().y * 0.2f);
				if(currentLenght >= limit)
				{
					GameObject grid = GameObject.Find("Grid");
					Grid gridComponent = grid.GetComponent<Grid>();
					Vector3 worldPos = m_parentCell.GetPosition();
					worldPos += direction * m_parentCell.GetDimensions().y;

					GridCell cell = gridComponent.GetCellFromWorldPosition(worldPos);
					if(cell != null)
					{
						if(cell.GetBlock() == null)
						{
							m_parentCell.SetBlock(null);
							cell.SetBlock(this);						
						}
					}
				}
			}
		}
	}
}
