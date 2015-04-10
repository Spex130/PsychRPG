using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class startTouch : MonoBehaviour {

	public LayerMask touchInputMask;
	private RaycastHit hit;
	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;

	//Maintains the touch input system. Everything having to do with that loop is here, more or less.
	void touchCheck(){
		
	
			
			if(Input.touchCount>0){
				
				touchesOld = new GameObject[touchList.Count];
				touchList.CopyTo(touchesOld);
				touchList.Clear();
				
				
				
				//Here is where we check each individual touch and see what it is doing.
				foreach(Touch touch in Input.touches){
					
					Ray ray = camera.ScreenPointToRay (touch.position);
					
					
					
					if(Physics.Raycast(ray, out hit, touchInputMask)){//If we send out a ray and it hits something in the touchInputMask...
						
						
						
						//GameObject recipient = hit.transform.gameObject;
						//touchList.Add (recipient);
						
						
						
						if (touch.phase == TouchPhase.Began){
							Application.LoadLevel("TestScene");
							print("TOUCHBEGIN");
						}
						
						else if (touch.phase == TouchPhase.Ended){
							
						}
						
						else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){

						}
						
						
						else if (touch.phase == TouchPhase.Canceled){

						}

						else if(touch.phase == TouchPhase.Ended){

						}
						
						
						
						
					}
					
				}
				
			}

			
		}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		touchCheck();
	}
}
