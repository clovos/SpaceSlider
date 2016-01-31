using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCell
{		
	private Block m_currentBlock;
	private Vector3 m_position;
	private Vector2 m_dimensions;

	public GridCell() 
	{ 
		m_currentBlock = null;
	}

	public void SetBlock(Block block) 
	{ 
		m_currentBlock = block; 
	}

	public Block GetBlock() 
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

	public void SetPosition(float x, float y, float z) 
	{ 
		m_position.x = x; 
		m_position.y = y; 
		m_position.z = z; 
	}
	public Vector3 GetPosition() 
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
		if(Game.Instance.MapEditorMode)
		{
			if(Input.GetMouseButtonUp(0))
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
				if(Inside(mousePos.x, mousePos.y))
				{
					if(m_currentBlock == null)
					{
						string blockTypeId = "BlockPrefab" + Random.Range(1, 4).ToString();
						GameObject block = GameObjectPool.Instance.GetFromPool(blockTypeId, true);
						block.transform.position = m_position;
						m_currentBlock = block.GetComponent<Block>();
						m_currentBlock.SetParentCell(this);
					}
				}
			}		
		}
		else
		{
			if(Input.GetMouseButton(0))
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
				if(Inside(mousePos.x, -mousePos.y))
				{	
					if(m_currentBlock != null)
					{
						m_currentBlock.UpdateMovement();
					}
				}
			}
		}
	}

	void PromptBlockType()
	{
		
	}
};
