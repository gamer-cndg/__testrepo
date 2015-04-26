using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System;
using System.IO;

public class Block : MonoBehaviour
{
	/* Attribute eines Blocks */
	public Sprite sprite1; // Drag your first sprite here
	public Sprite sprite2; // Drag your second sprite here
	public int ID;
	public string Name;
	public int X;
	public int Y;
	public int State;
	private static GameObject[] Blocks = PreloadBlocks ();

	// Use this for initialization
	void Start ()
	{    
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	/* Übernehme Blockdaten */
	public void SetAttributes(BlockData data)
	{
		this.ID = data.ID;
		this.X = data.X; 
		this.Y = data.Y;
		this.State = data.State;
		this.Name = data.Name;
	}

	/* STATISCHE FUNKTIONEN */
	public static GameObject GetBlock(int id)
	{
		return Array.Find<GameObject> (Blocks, b => b.GetComponent<Block> ().ID == id);
	}

	private static GameObject[] PreloadBlocks ()
	{
		var blocks = new List<GameObject> ();
		string[] block_files = Directory.GetFiles (Application.dataPath + "/Resources/Blocks/", "*.prefab");
		foreach (string s in block_files) 
		{
			string block_name = Path.GetFileNameWithoutExtension (s);
			//Load that game object.. 
			var pBlock = Resources.Load("Blocks/" + block_name, typeof(GameObject)) as GameObject;
			//Get the Script Component of that block
			if(!pBlock){
				Debug.LogError("Failed to load Block: " + block_name);
				continue;
			}
			//Save it.
			blocks.Add (pBlock);
			Debug.Log ("Loaded: " + s);
		}
		return blocks.ToArray ();
	}
		
	//Liest eine XML Datei aus, gibt eine Liste von Blöcken zurück
	public static List<BlockData> ParseBlocklistXML (string path)
	{
		var blocks = new List<BlockData> ();
		XmlDocument doc = new XmlDocument ();
		doc.Load (path);
			
		foreach (XmlNode block in doc.SelectSingleNode("blocklist")) {
			try {
				blocks.Add (new BlockData () {
						ID = Convert.ToInt32(block.SelectSingleNode("id").InnerText),
						Name = block.SelectSingleNode("name").InnerText,
						X = Convert.ToInt32(block.SelectSingleNode("x").InnerText),
						Y = Convert.ToInt32(block.SelectSingleNode("y").InnerText),
						State = Convert.ToInt32(block.SelectSingleNode("state").InnerText)
					});
			} catch (Exception e) {
				Debug.Log ("ERROR Reading Block:\n" + block.InnerXml + "\n" + e.ToString ());
			}
		}
		return blocks;
	}

	/* Try to get some usefull information from this object  */
	public override String ToString()
	{
		return String.Format ("ID = {0}, Name = {1}, X = {2}, Y = {3}, State = {4}", this.ID, this.Name, this.X, this.Y, this.State);
	}

}