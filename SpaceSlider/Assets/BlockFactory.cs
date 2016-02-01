using UnityEngine;
using System;
using System.Collections.Generic;

public class BlockFactory
{
	private static BlockFactory m_instance;
	public static BlockFactory Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new BlockFactory();
			}
			return m_instance;
		}
		set { m_instance = value; }
	}

	public Block CreateBlock(Block.BlockProperty type)
	{
		string prefabName = null;

		switch(type)
		{
		case Block.BlockProperty.Empty:
			break;
		case Block.BlockProperty.Movable:
			prefabName = "Movable";
			break;
		case Block.BlockProperty.NonMovable:
			prefabName = "NonMovable";
			break;
		case Block.BlockProperty.PowerUp:
			prefabName = "PowerUp";
			break;
		default:
			break;
		}

		if(prefabName != null)
		{
			GameObject block = GameObjectPool.Instance.GetFromPool(prefabName, true);
			if(block == null)
				return null;
			return block.GetComponent<Block>();
		}
		return null;
	}
}

