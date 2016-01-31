using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridCell
{		
	private Block m_currentBlock;

	private SerializableVector3 m_position;
	private SerializableVector2 m_dimensions;

	public GridCell() 
	{ 
		m_currentBlock = null;
	}

	public void SetBlock(Block block) 
	{ 
		m_currentBlock = block;
		m_currentBlock.SetParentCell(this);
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
		
	public void UpdateEditorInput()
	{
		if(Input.GetMouseButtonUp(0))
		{
			if(Input.mousePosition.x > Screen.width * 0.80 && Input.mousePosition.y > Screen.height - (Screen.height * 0.25))
			{
				return;
			}
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
			if(Inside(mousePos.x, mousePos.y))
			{
				if(m_currentBlock == null)
				{
					string blockTypeId = "BlockPrefab" + Random.Range(1, 4).ToString();
					GameObject block = GameObjectPool.Instance.GetFromPool(blockTypeId, true);
					block.transform.position = m_position;
					SetBlock(block.GetComponent<Block>());

					GameObject grid = GameObject.Find("Grid");
					Grid gridComponent = grid.GetComponent<Grid>();
					gridComponent.SetIsSaved(false);
				}
				else
				{
					
				}
			}
		}		
	}
	public void UpdateInput()
	{
		if(MapEditor.Instance)
		{
			UpdateEditorInput();
		}
		else
		{
			if(Input.GetMouseButton(0))
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
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
	public void Update()
	{	
		UpdateInput();
	}

	void PromptBlockType()
	{
	}
};
