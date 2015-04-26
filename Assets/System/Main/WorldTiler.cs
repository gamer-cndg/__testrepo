using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System;

public class WorldTiler : MonoBehaviour
{
	public string BlockPath;
	private static List<Block> Map = new List<Block>();

	public static GameObject SpawnBlock (BlockData data)
	{
		if(BlockExists(data.X, data.Y)){
			Debug.LogError("Block at " + data.X + "/" + data.Y + " already exists!");
			return null;
		}
		var pBlockPrefab = Block.GetBlock (data.ID);
		if (!pBlockPrefab) {
			Debug.LogError("No such Block ID: " + data.ID);
			return null;
		}
		var pBlock = Instantiate (pBlockPrefab, new Vector3(data.X, data.Y, 0), Quaternion.identity) as GameObject;
		if (pBlock && pBlock.GetComponent<Block> ())
			pBlock.GetComponent<Block> ().SetAttributes (data);
		else {
			Debug.LogError ("Failed to Spawn Block. Either Block or BlockScript didn't exist.");
			return null;
		}
		Map.Add (pBlock.GetComponent<Block> ());
		return pBlock;
	}

	public static GameObject SpawnBlock(int id, int x, int y)
	{
		//Create a new random BlockData..
		return SpawnBlock (new BlockData () {
			ID = id,
			X = x,
			Y = y,
			Name = "Block " + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8),
			State = 100
		});
	}

	public static bool BlockExists(int x, int y)
	{
		return Array.Find<Block>(Map.ToArray(), b => b.X == x && b.Y == y) != null;
	}

	/* Saves the Map in XML Format */ 
	public static void SaveMap(string filename) {
		XmlDocument doc = new XmlDocument ();
		XmlNode root = doc.CreateElement ("blocklist");
		foreach (Block b in Map) {
			Debug.Log ("Saving " + b.ToString());
			XmlNode block = doc.CreateElement("block");
			var id = doc.CreateElement ("id");
			id.InnerText = b.ID.ToString();
			var name = doc.CreateElement ("name");
			name.InnerText = b.Name;
			var x = doc.CreateElement ("x");
			x.InnerText = b.X.ToString();
			var y = doc.CreateElement ("y");
			y.InnerText = b.Y.ToString();
			var state = doc.CreateElement ("state");
			state.InnerText = b.State.ToString();
			block.AppendChild(id);
			block.AppendChild(name);
			block.AppendChild(x);
			block.AppendChild(y);
			block.AppendChild(state);
			root.AppendChild(block);
		}
		doc.AppendChild (root);
		doc.Save (filename);
	}

	public static void LoadMap(string path, bool deletePriorBlocks = true)
	{
		if (deletePriorBlocks) {
			GameObject.FindGameObjectsWithTag("Block").ToList().ForEach( b => { Map.Remove(b.GetComponent<Block>()); GameObject.Destroy(b.gameObject);});
		}

		var blocklist = Block.ParseBlocklistXML (path);
		Debug.Log ("Loaded " + blocklist.Count + " blocks from Mapfile.");
		foreach (var block in blocklist) 
		{
			SpawnBlock (block);
			Debug.Log ("Created : " + block.Name);
		}

	}
	
	void Start ()
	{
		LoadMap(@"C:\Users\Maxi\Desktop\test.xml");
	}
}
