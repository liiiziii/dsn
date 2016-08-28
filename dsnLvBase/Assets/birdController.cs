using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class birdController : MonoBehaviour {
	[SerializeField]
	List<BlockInformation> blocks = new List<BlockInformation> ();
	public GameObject birdPrefab;
	List<GameObject> birds = new List<GameObject>();
	//List<toShadows> birdShadows = new List<toShadows>();
	List<Vector3> birdsPosO = new List<Vector3>(); 
	int meetSPCharIndex = 0;//bear

	void Start () {
		for (int i = 0; i < blocks.Count; i++) {//create birds
			Vector3 pos = blocks [i].transform.position + new Vector3(0, 1.5f, 0);
			GameObject b = Instantiate (birdPrefab as GameObject);
			b.transform.position = pos;
			birdsPosO.Add (b.transform.position);
			birds.Add (b);
			//birdShadows.Add (b.GetComponent<toShadows> ());
		}
	
	}

	void Update () {
		for (int i = 0; i < blocks.Count; i++) {
			birdMeetsBearZY (blocks [i], birds [i],birdsPosO[i]);
			}
	
	}

	void birdMeetsBearZY(BlockInformation bk, GameObject bd, Vector3 bp){
		Vector3 nextPos = bp;
		Color nextColor = Color.white;
		if (bk.SPCharBeTouched [meetSPCharIndex].y == 1) {//if bearZY touches the block, play bird flying away
			nextPos = bp + new Vector3 (0, 10, 0);
			nextColor = new Color (1, 1, 1, 0);
		}
		birdMoves (bd, nextPos,nextColor );
	}

	void birdMoves(GameObject b, Vector3 p, Color c){
		b.transform.position = Vector3.MoveTowards (b.transform.position, p, Time.deltaTime * 10);
		b.GetComponent<Renderer> ().material.color = Color.Lerp (b.GetComponent<Renderer> ().material.color, c, Time.deltaTime * 10);

//		foreach (Transform trans in bs.myShadows) {
//			trans.gameObject.SetActive (a);
//		}
	}
}
