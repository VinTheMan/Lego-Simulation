using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
private Vector3 screenPoint;
 private Vector3 offset;
 private Vector3 offsetWithChild;
 private bool YesPutIiIn ; 
 public bool isActive ; // 生成時 跟著游標走
 public float mouse_magenet_dist = 3f ;
 Transform other ; 
 bool isClose ; 


 void OnMouseDown()
 {
	 
     screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
     offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	 offsetWithChild = gameObject.transform.position - gameObject.transform.parent.position ;
     isActive = false ;
 
 }

 void OnMouseDrag()
 {
	Vector3 curScreenPoint ; 
    Vector3 curPosition ; 
	
	curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
    curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint)+offset-offsetWithChild;

    this.transform.parent.position = curPosition;
 }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate()
 	{
    
        

 	} // FixedUpdate
}
