using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour {

	public enum EditorDrawMode
	{
		DrawEmpty,
		DrawMovable,
		DrawNonMovable,
		DrawBoost,
		DontDraw,
		Total
	}

	public static MapEditor Instance { get; private set; }
	public bool Active = true;

	private bool m_menuOpen = false;
	private Rect m_menuRect;

	private EditorDrawMode m_drawMode = EditorDrawMode.DontDraw;
	private GridCell m_lastDrawnCell = null;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!Active)
		{
			Instance = null;		
			for(int i = 0; i < transform.childCount; ++i)
				transform.GetChild(i).gameObject.SetActive(false);
			return;
		}
		if(Instance == null)
		{
			Instance = this;
			for(int i = 0; i < transform.childCount; ++i)
				transform.GetChild(i).gameObject.SetActive(true);
		}

		if(m_drawMode == EditorDrawMode.DontDraw)
		{
			if(Input.GetMouseButtonDown(0) && !m_menuOpen)
			{
//				if(m_menuOpen == true)
//					m_menuOpen = false;

				if(Input.mousePosition.y > Screen.height - (Screen.height * 0.08f))
				{
					return;
				}

				GameObject grid = GameObject.Find("Grid");
				Grid gridComponent = grid.GetComponent<Grid>();

				Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				GridCell cell = gridComponent.GetCellFromScreenPosition(screenPos);
				if(cell != null)
				{
					OpenBlockClickMenu(screenPos);
				}					
			}				
		}
		else
		{
			if(Input.GetMouseButton(0))
			{			
				GameObject grid = GameObject.Find("Grid");
				Grid gridComponent = grid.GetComponent<Grid>();
				Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				GridCell cell = gridComponent.GetCellFromScreenPosition(screenPos);
				if(cell != null)
				{
					if(cell != m_lastDrawnCell)
					{
						m_lastDrawnCell = cell;
						Block block = null;

						switch(m_drawMode)
						{
						case EditorDrawMode.DrawEmpty:
							break;
						case EditorDrawMode.DrawMovable:
							block = BlockFactory.Instance.CreateBlock(Block.BlockProperty.Movable);
							break;
						case EditorDrawMode.DrawNonMovable:
							block = BlockFactory.Instance.CreateBlock(Block.BlockProperty.NonMovable);
							break;
						case EditorDrawMode.DrawBoost:
							block = BlockFactory.Instance.CreateBlock(Block.BlockProperty.PowerUp);
							break;
						}
						if(block != null)
							block.transform.position = cell.GetPosition();

						if(cell.GetBlock() != null)
							GameObjectPool.Instance.AddToPool(cell.GetBlock().gameObject);	
						
						cell.SetBlock(block);
						gridComponent.SetIsSaved(false);
					}
				}
			}
		}
	}

	void OnGUI()
	{
		if(!Instance)
		{
			return;
		}
		if(m_menuOpen)
			m_menuRect = GUI.Window(0, m_menuRect, ShowRightClickMenu, "\nBlockTypes:\n");
	}

	void ShowRightClickMenu(int windowId)
	{	
		float spacing = 20f;
		for (int i = 0; i < (int)Block.BlockProperty.TotalAmountOfTypes; ++i) 
		{
			if(GUI.Button(new Rect((m_menuRect.width * 0.5f) - 70, 40 + (spacing * i), 140, 20), ((Block.BlockProperty)i).ToString()))
			{
				GameObject grid = GameObject.Find("Grid");
				Grid gridComponent = grid.GetComponent<Grid>();
				Vector2 screenPos = new Vector2(m_menuRect.x, (m_menuRect.y - Screen.height) * -1);
				GridCell cell = gridComponent.GetCellFromScreenPosition(screenPos);
				if(cell == null) continue;

				Block block = BlockFactory.Instance.CreateBlock(((Block.BlockProperty)i));
				if(block != null)
					block.transform.position = cell.GetPosition();

				if(cell.GetBlock() != null)
					GameObjectPool.Instance.AddToPool(cell.GetBlock().gameObject);	
				
				gridComponent.SetIsSaved(false);
				m_menuOpen = false;
				return;
			}				
		}
		if(GUI.Button(new Rect((m_menuRect.width * 0.5f) - 70, 40 + (spacing * (int)Block.BlockProperty.TotalAmountOfTypes), 140, 20), "Cancel"))
		{
			m_menuOpen = false;
		}
	}

	public void OpenBlockClickMenu(Vector2 position)
	{
		m_menuOpen = true;
		m_menuRect = new Rect(position.x, Screen.height - position.y, 150f, ((int)Block.BlockProperty.TotalAmountOfTypes + 1) * 20f + 45f);
		StartCoroutine(OnLeftClickGridCell());
	}

	IEnumerator OnLeftClickGridCell()
	{
		while(m_menuOpen)
		{
			yield return null;
		}
		yield return false;
	}

	//Draw Mode Toggeling
	public void ToggleDrawEmpty(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawEmpty;
	}
	public void ToggleDrawMovable(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawMovable;
	}
	public void ToggleDrawNonMovable(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawNonMovable;
	}
	public void ToggleDrawBoost(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawBoost;
	}
	public void ToggleDontDraw(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DontDraw;
	}
}
