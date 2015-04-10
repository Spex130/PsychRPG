using UnityEngine;
using System.Collections;

public class faderScript : MonoBehaviour {

	public GameObject text;
	public Color color = new Color(255,255,255, 255);
	public int reverser = 1;
	float mover = .5f;
	public int moveInt = 200;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y + reverser, gameObject.transform.position.z);

		if(moveInt > 220 || moveInt < 50){
			reverser*= -1;
		}

		moveInt -= 1 * reverser;
		renderer.material.color = new Color(255,255,255, moveInt);
	}
}
