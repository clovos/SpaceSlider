using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour {

	//Singleton
	public static GameObjectPool Instance { get; private set; }
	public bool ShowDebugInfo = false;

	//A simple class that defines the prefab to hold in the pool and the amount of that gameobject
	[Serializable]
	public class PoolItem
	{
		public GameObject Prefab;
		public int PoolSize = 0;
	}

	//List of prefabs to initiate the pool with
	public PoolItem[] Prefabs;

	//The actual list of all objects
	public List<GameObject>[] m_poolOfObjects;

	//The holding gameobject / the container
	private GameObject m_containerObject;

	void Awake ()
	{
		Instance = this;
	}

	void Start () 
	{
		m_containerObject = new GameObject("GameObjectPool");
		m_poolOfObjects = new List<GameObject>[Prefabs.Length];

		int prefabIndex = 0;
		foreach (PoolItem poolItem in Prefabs)
		{
			Debug.Assert(poolItem.Prefab != null, "You need to specify a prefab / gameobject to initate the pool with, check element: " + prefabIndex.ToString());
			Debug.Assert(poolItem.PoolSize > 0, "You tried to initiate a pool with size 0 with object type: " + poolItem.Prefab.name);
			m_poolOfObjects[prefabIndex] = new List<GameObject>(); 

			for (int n = 0; n < poolItem.PoolSize; ++n)
			{
				GameObject newObj = Instantiate(poolItem.Prefab) as GameObject;
				newObj.name = poolItem.Prefab.name;
				AddToPool(newObj);
			}
			++prefabIndex;
		}
	}

	public GameObject GetFromPool(string objectType, bool instantiateIfEmpty)
	{
		for(int i = 0; i < Prefabs.Length; ++i)
		{
			PoolItem item = Prefabs[i];
			if(item.Prefab.name == objectType)
			{
				if(m_poolOfObjects[i].Count > 0)
				{
					GameObject pooledObject = m_poolOfObjects[i][0];
					m_poolOfObjects[i].RemoveAt(0);
					pooledObject.transform.parent = null;
					pooledObject.SetActive(true);
					return pooledObject;
				} 

				if(instantiateIfEmpty) 
				{
					return Instantiate(m_poolOfObjects[i][0]) as GameObject;
				}
				break;

			}
		}
		Debug.Assert(false, "The pool is empty and you didn't want to instantiate a new GameObject of type: " + objectType);
		return null;
	}

	public void AddToPool(GameObject gameObject)
	{
		for (int i = 0; i < Prefabs.Length; ++i)
		{
			if(Prefabs[i].Prefab.name == gameObject.name)
			{
				gameObject.SetActive(false);
				gameObject.transform.parent = m_containerObject.transform;
				m_poolOfObjects[i].Add(gameObject);
				return;
			}
		}
		Debug.Assert(false, "You tried to add an object to the pool that isn't available! Object: " + gameObject.name);
	}
		
	void Update () 
	{

	}

	void OnGUI()
	{
		if(ShowDebugInfo)
		{
			Rect screenRectangle = new Rect(0, 0, 400.0f, 200.0f);
			string text = "GameObject Pool:\n";

			int totalAvailableObjects = 0;
			int availablePerPrefabType = 0;
			for(int i = 0; i < m_poolOfObjects.Length; ++i)
			{
				availablePerPrefabType = m_poolOfObjects[i].Count;
				totalAvailableObjects += availablePerPrefabType;
				text += m_poolOfObjects[i][0].name + ": " + availablePerPrefabType.ToString() + "\n";
				availablePerPrefabType = 0;
			}
			text += "Total Available Objects: " + totalAvailableObjects.ToString();
			GUI.Label(screenRectangle, text);
		}
	}
}
