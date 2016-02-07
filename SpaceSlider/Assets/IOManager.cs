﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class IOManager : MonoBehaviour 
{ 
	private Rect windowRect = new Rect((Screen.width * 0.5f) - 200f, (Screen.height * 0.5f) - 50f, 400f, 100f);
	private bool m_promptUnsavedLevel = false;

	public List<LoadLevelScript.Level> GetLevelsInDirectory(string directoryPath)
	{
		DirectoryInfo info = new DirectoryInfo(directoryPath);
		FileInfo[] fileInfo = info.GetFiles();

		List<LoadLevelScript.Level> levels = new List<LoadLevelScript.Level>();
		foreach (FileInfo file in fileInfo) 
		{
			if(file.Name.EndsWith(".lel"))
			{
				LoadLevelScript.Level l = new LoadLevelScript.Level(file.Name, file.FullName);
				levels.Add(l);	
			}
		}
			
		return levels;
	}

	public string Save()
	{
		string filePath = UnityEditor.EditorUtility.SaveFilePanel("Save current level", "/Assets/Levels/", "newLevel.lel", "lel");
		SaveFromPath(filePath);
		return filePath;
	}

	public void SaveFromPath(string filePath)
	{
		if(filePath == null || filePath == "" || filePath.Length == 0)
		{
			return;
		}

		GameObject grid = GameObject.Find("Grid");
		Grid gridComponent = grid.GetComponent<Grid>();
		List<List<GridCell>> cells = gridComponent.GetCells();

		StreamWriter writer = File.CreateText(filePath);
		writer.WriteLine(gridComponent.Cells.X.ToString());
		writer.WriteLine(gridComponent.Cells.Y.ToString());
		writer.WriteLine(gridComponent.CellDimensions.x.ToString());
		writer.WriteLine(gridComponent.CellDimensions.x.ToString());

		for (int y = 0; y < cells.Count; ++y) 
		{
			for (int x = 0; x < cells[y].Count; ++x) 
			{
				if(cells[y][x].GetBlock() != null)
					writer.WriteLine(cells[y][x].GetBlock().name);
				else
					writer.WriteLine("null");
			}
		}
		writer.Close();
		gridComponent.SetIsSaved(true);		
	}

	public void Load()
	{
		StartCoroutine(LoadAsync());
	}

	public void Load(string path)
	{
		StartCoroutine(LoadAsyncPath(path));
	}
	private void LoadFromPath(string path)
	{		
		if(!File.Exists(path))
		{
			Debug.LogError("File doesn't exist!");
			return;
		}

		GameObject grid = GameObject.Find("Grid");
		Grid gridComponent = grid.GetComponent<Grid>();
		List<List<GridCell>> cells = gridComponent.GetCells();

		StreamReader reader = File.OpenText(path);
		gridComponent.Cells.X = int.Parse(reader.ReadLine());
		gridComponent.Cells.Y = int.Parse(reader.ReadLine());
		gridComponent.CellDimensions.x = float.Parse(reader.ReadLine());
		gridComponent.CellDimensions.y = float.Parse(reader.ReadLine());

		Vector3 centerPosition = Camera.main.transform.position;
		//float centerX = (gridComponent.Cells.X * gridComponent.CellDimensions.x) * 0.5f;
		float centerY = (gridComponent.Cells.Y * gridComponent.CellDimensions.y) * 0.5f;
		float startX = Camera.main.transform.position.x; //centerPosition.x - centerX;
		float startY = centerPosition.y - centerY;

		gridComponent.Reset();
		cells.Clear();

		string readLine = "";
		cells = new List<List<GridCell>>(gridComponent.Cells.Y);
		for (int y = 0; y < gridComponent.Cells.Y; ++y) 
		{
			List<GridCell> newColumn = new List<GridCell>(gridComponent.Cells.X);
			for (int x = 0; x < gridComponent.Cells.X; ++x) 
			{
				GridCell cell = new GridCell();
				cell.SetDimensions(gridComponent.CellDimensions.x, gridComponent.CellDimensions.y);
				cell.SetPosition(startX + gridComponent.CellDimensions.x * 0.5f + (x * gridComponent.CellDimensions.x), (startY + gridComponent.CellDimensions.y * 0.5f + (y * gridComponent.CellDimensions.y)), Camera.main.nearClipPlane);

				readLine = reader.ReadLine();
				if(readLine != "null")
				{
					GameObject block = GameObjectPool.Instance.GetFromPool(readLine, true);
					block.transform.position = cell.GetPosition();

					if(cell.GetBlock() != null)
						GameObjectPool.Instance.AddToPool(cell.GetBlock().gameObject);	

					cell.SetBlock(block.GetComponent<BlockBase>());		
				}
				newColumn.Add(cell);
			}
			cells.Add(newColumn);
		}	
		gridComponent.SetCells(cells);
	}
	IEnumerator LoadAsync()
	{
		GameObject grid = GameObject.Find("Grid");
		Grid gridComponent = grid.GetComponent<Grid>();
		List<List<GridCell>> cells = gridComponent.GetCells();

		if(cells.Count > 0)
		{
			if(gridComponent.GetIsSaved() == false)
			{
				m_promptUnsavedLevel = true;
				while(m_promptUnsavedLevel)
				{
					yield return null;
				}
			}
		}

		string filePath = UnityEditor.EditorUtility.OpenFilePanel("Load a level", "/Assets/Levels/", "lel");
		if(filePath == null || filePath == "" || filePath.Length == 0)
		{
			yield return false;
		}
		LoadFromPath(filePath);
	}

	IEnumerator LoadAsyncPath(string filePath)
	{
		GameObject grid = GameObject.Find("Grid");
		Grid gridComponent = grid.GetComponent<Grid>();
		List<List<GridCell>> cells = gridComponent.GetCells();

		if(cells.Count > 0)
		{
			if(gridComponent.GetIsSaved() == false)
			{
				m_promptUnsavedLevel = true;
				while(m_promptUnsavedLevel)
				{
					yield return null;
				}
			}
		}
		if(filePath == null || filePath == "" || filePath.Length == 0)
		{
			yield return false;
		}
		LoadFromPath(filePath);
	}

	void OnGUI()
	{
		if(m_promptUnsavedLevel)

			windowRect = GUI.Window(0, windowRect, PromptUnsavedLevel, "\nThe current changes haven't been saved.\nWould you like to save them now?");
	}

	void PromptUnsavedLevel(int windowId)
	{
		GUI.FocusWindow(windowId);
		if(GUI.Button(new Rect((windowRect.width * 0.5f) - 100, 60, 80, 20), "Yes"))
		{
			m_promptUnsavedLevel = false;
			Save();
		}
		else if(GUI.Button(new Rect((windowRect.width * 0.5f) + 20, 60, 80, 20), "No"))
		{
			m_promptUnsavedLevel = false;
			return;			
		}
	}
}
