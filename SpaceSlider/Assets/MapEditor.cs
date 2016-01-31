using UnityEngine;
using System.Collections;

public class MapEditor : MonoBehaviour {

	public static MapEditor Instance { get; private set; }
	public bool Active = true;

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
		}
		else
		{
			Instance = this;
			for(int i = 0; i < transform.childCount; ++i)
				transform.GetChild(i).gameObject.SetActive(true);
		}

	}

	void OnGUI()
	{
		if(!Instance)
		{
			return;
		}
	}
}
