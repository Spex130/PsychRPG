using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {

	public LayerMask touchInputMask;

	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;

	private RaycastHit hit;

	//public colorBlock selected;
	public GameObject selected;

	// Update is called once per frame
	void Update () {



		if(Input.touchCount>0){

			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo(touchesOld);
			touchList.Clear();



			//Here is where we check each individual touch and see what it is doing.
		foreach(Touch touch in Input.touches){

			Ray ray = GetComponent<Camera>().ScreenPointToRay (touch.position);
			


				if(Physics.Raycast(ray, out hit, touchInputMask)){//If we send out a ray and it hits something in the touchInputMask...

					//GameObject recipient = hit.transform.gameObject;
						//touchList.Add (recipient);



					if (touch.phase == TouchPhase.Began){

						if(!(selected != null)){
							//selected = hit.transform.gameObject.GetComponent<colorBlock>();
							selected = hit.transform.gameObject;
                            selected.SendMessage("pickupBlock", SendMessageOptions.DontRequireReceiver);
						}

					}

					if (touch.phase == TouchPhase.Ended){
						//recipient.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);

						//selected.transform.position = selected.gameObject.GetComponent<colorBlock>().gridPos;
						selected = null;
						print("released");
						selected.SendMessage ("releaseBlock", SendMessageOptions.DontRequireReceiver);


						
					}

					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
						//recipient.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
						
					}


					if (touch.phase == TouchPhase.Canceled){
						//recipient.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
						print("released");
						selected = null;
						selected.SendMessage ("releaseBlock", SendMessageOptions.DontRequireReceiver);

					}

				}
				else if(touch.phase == TouchPhase.Ended){
					selected = null;
				}

			}

			/*
				foreach(GameObject g in touchesOld){
					//These are the things that are no longer held down.
					if(!touchList.Contains(g)){
						g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				}*/
		}


		//If we have something selected, move it.
		if(selected != null){
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			selected.transform.position = new Vector3(curPosition.x, curPosition.y, selected.transform.position.z);
		}

	}

	void printTouchLoc(){
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		

		print(curPosition);

	}

}


