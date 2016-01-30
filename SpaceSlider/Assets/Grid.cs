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

	public class GridCell
	{
		public GridCell() { m_cellType = CellType.Empty; }
		public enum CellType
		{
			Empty,
			Movable,
			NonMovable,
			PowerUp,
		};
		private CellType m_cellType { get; set;}
	};

	public CellCount Cells;
	public Vector2 CellDimensions;

	private List<List<GridCell>> m_cells;

	// Use this for initialization
	void Start () 
	{
		Debug.Assert(Cells.X > 0 && Cells.Y > 0 && CellDimensions.x > 0 && CellDimensions.y > 0, "INVALID GRID! Gör om gör rätt!");

		m_cells = new List<List<GridCell>>(Cells.Y);
		for (int y = 0; y < m_cells.Count; ++y) 
		{
			m_cells[y] = new List<GridCell>(Cells.X);
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				m_cells[y][x] = new GridCell();
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
