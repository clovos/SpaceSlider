using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	[System.Serializable]
	public class CellCount
	{
		public int X;
		public int Y;
	};

	public bool ShowDebugInfo = false;
	public CellCount Cells;
	public Vector2 CellDimensions;

	private List<List<GridCell>> m_cells;

	void Awake()
	{
		Debug.Assert(Cells.X > 0 && Cells.Y > 0 && CellDimensions.x > 0 && CellDimensions.y > 0, "INVALID GRID! Gör om gör rätt!");

		Vector3 centerPosition = Camera.main.transform.position;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float centerY = (Cells.Y * CellDimensions.y) * 0.5f;
		float startX = centerPosition.x - centerX;
		float startY = centerPosition.y - centerY;

		m_cells = new List<List<GridCell>>(Cells.Y);
		for (int y = 0; y < Cells.Y; ++y) 
		{
			List<GridCell> newColumn = new List<GridCell>(Cells.X);
			for (int x = 0; x < Cells.X; ++x) 
			{
				GridCell cell = new GridCell();
				cell.SetDimensions(CellDimensions.x, CellDimensions.y);
				cell.SetPosition(startX + CellDimensions.x * 0.5f + (x * CellDimensions.x), startY + CellDimensions.y * 0.5f + (y * CellDimensions.y));
				newColumn.Add(cell);
			}
			m_cells.Add(newColumn);
		}		
	}

	void Start () 
	{
	}

	void Update ()
	{
		for (int y = 0; y < m_cells.Count; ++y) 
		{
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				m_cells[y][x].Update();
			}
		}	
	}

	void OnDrawGizmos()
	{
		if(ShowDebugInfo)
		{
			if(m_cells == null) { return; }

			for (int y = 0; y < m_cells.Count; ++y) 
			{
				for (int x = 0; x < m_cells[y].Count; ++x) 
				{
					GridCell cell = m_cells[y][x];
					Vector3 linePos = new Vector3(cell.GetPosition().x, cell.GetPosition().y, Camera.main.nearClipPlane);
					Gizmos.DrawWireCube(linePos, new Vector3(CellDimensions.x, CellDimensions.y, 0f));
				}
			}	
		}		
	}
	void OnGUI()
	{
		if(ShowDebugInfo)
		{
			if(m_cells == null) { return; }
			for (int y = 0; y < m_cells.Count; ++y) 
			{
				for (int x = 0; x < m_cells[y].Count; ++x) 
				{
					GridCell cell = m_cells[y][x];
					Vector3 worldPos = new Vector3(cell.GetPosition().x, cell.GetPosition().y + 0.8f, Camera.main.nearClipPlane);
					Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
					Rect screenRectangle = new Rect(screenPos.x - 18.0f, Screen.height - screenPos.y , 50.0f, 60.0f);

					string text = "x: " + cell.GetPosition().x.ToString() + "\ny: " + cell.GetPosition().y.ToString() + "\nz: " + Camera.main.nearClipPlane.ToString();
					GUI.Label(screenRectangle, text);
				}
			}	
		}
	}
}
