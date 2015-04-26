using UnityEngine;
using System.Collections;
using System;

public class MouseSpawner : MonoBehaviour {

	private bool isEnabled = true;
	// Use this for initialization
	void Start () {
	
	}

	void OnGUI(){
		if (GUI.Button (new Rect (10, 10, 80, 25), "Save Map")) {
			WorldTiler.SaveMap("map.xml");
		}
		if (GUI.Button (new Rect (10, 35, 80, 25), "Load Map")) {
			WorldTiler.LoadMap("map.xml");
		}
		if (GUI.Button (new Rect (10, 60, 80, 25), "Disable / Enable Mouse Spawn")) {
			isEnabled = !isEnabled;
		}

	}

	// Update is called once per frame
	void Update () {
		/* Check for Mouse Movement  */
		if (!isEnabled)
			return;
		var mouseposition = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x,Input.mousePosition.y,0));
		if (Input.GetMouseButtonDown (0))
			WorldTiler.SpawnBlock (0, (int)Math.Round(mouseposition.x), (int)Math.Round(mouseposition.y));
		
		if (Input.GetMouseButtonDown (1))
			WorldTiler.SpawnBlock (1, (int)Math.Round (mouseposition.x), (int)Math.Round (mouseposition.y));
	}
}
