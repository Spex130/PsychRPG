using UnityEngine;
using System.Collections;

public class removalBlock : MonoBehaviour {

	float alpha = 1f;
	float alphaDec = .2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
		if(alpha >= 0){
			alpha-= alphaDec;
			transform.localScale += new Vector3(.1f,.1f,.1f);
		}
		else{
			//print("DESTROYED!");
			Object.Destroy(this.gameObject);
		}
	}
}
