using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportTarget2 : MonoBehaviour {

    public GameObject scrollview ;  

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetToFirst() {

        scrollview.transform.SetAsFirstSibling();

    } // SetToFirst()

}
