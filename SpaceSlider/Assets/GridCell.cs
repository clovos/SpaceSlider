using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCell
{		
	public enum CellType
	{
		Empty,
		Movable,
		NonMovable,
		PowerUp,
	};

	private GameObject m_currentBlock;
	private Vector2 m_position;
	private Vector2 m_dimensions;
	private CellType m_cellType { get; set; }

	public GridCell() 
	{ 
		m_cellType = CellType.Empty; 
		m_currentBlock = null;
	}

	public void SetBlock(GameObject block) 
	{ 
		m_currentBlock = block; 
	}

	public GameObject GetBlock() 
	{ 
		return m_currentBlock;
	}

	public void SetDimensions(float x, float y) 
	{ 
		m_dimensions.x = x; 
		m_dimensions.y = y; 
	}
	public Vector2 GetDimensions() 
	{ 
		return m_dimensions; 
	}

	public void SetPosition(float x, float y) 
	{ 
		m_position.x = x; 
		m_position.y = y; 
	}
	public Vector2 GetPosition() 
	{ 
		return m_position; 
	}

	public bool Inside(float x, float y)
	{
		float halfSizeX = m_dimensions.x * 0.5f;
		float halfSizeY = m_dimensions.x * 0.5f;
		if(x > m_position.x - halfSizeX && x < m_position.x + halfSizeX) 
			if(y > m_position.y - halfSizeY && y < m_position.y + halfSizeY) 
				return true;
		
		return false;
	}

	public void Update()
	{	
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
		if(Inside(mousePos.x, -mousePos.y))
		{
			if(Input.GetMouseButtonUp(0))
			{
				Debug.Log("MouseUp at position x: " + m_position.x.ToString() + "y: " + m_position.y.ToString());
				string blockTypeId = "Block" + Random.Range(1, 4).ToString();
				m_currentBlock = GameObjectPool.Instance.GetFromPool(blockTypeId, true);
				m_currentBlock.transform.position = new Vector3(m_position.x, -m_position.y, Camera.main.nearClipPlane);			
			}		
		}
	}
};
