using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RotateGM : MonoBehaviour
{

    public Transform theRotingObject;
    public Transform previousClick;
    public GameObject arror;
    Transform theRotPoint;
    float angle = 0f;
    public GameObject saveOldStates;
    void Start()
    {
        saveOldStates = GameObject.Find("Old Rotation");
        arror = Resources.FindObjectsOfTypeAll<arrowDrg>().ElementAt(0).gameObject ;
        // arror.SetActive(false) ;
    } // start

    public void setFormerRotState(ref GameObject inn)
    {
        if (this.saveOldStates == null)
        {
            saveOldStates = GameObject.Find("Old Rotation");

            if (saveOldStates == null)
            {
                saveOldStates = new GameObject("Old Rotation");
            } // if

        } // if

        saveOldStates.transform.rotation = inn.transform.rotation;
    } // setFormerRotState

    void adjustArrorSymbol(Transform rotingObject)
    {
        Transform L1_start = null;
        Transform L1_end = null;

        int assignCount = 0;

        for (int x = 0; x < rotingObject.transform.childCount ; x++)
        { // 準備比較第一組平行(對接平行)
            Transform temp = rotingObject.transform.GetChild(x);
            if (temp.tag.Contains("ParParSet1"))
            {
                assignCount++;
                switch (assignCount)
                {
                    case 1:
                        L1_start = temp;
                        break;
                    case 2:
                        L1_end = temp;
                        break;
                    default:
                        Debug.Log("Something went wrong in adjustArrorSymbol ParParSet1");
                        break;
                } // switch
            } // if
        } // for

        
        float angle = Vector3.Angle( (L1_end.position - L1_start.position).normalized, arror.transform.forward);
     

        Vector3 cross = Vector3.Cross((L1_end.position - L1_start.position).normalized, arror.transform.forward);
        
        // Debug.Log( cross + "angle " + angle ) ; // test
        arror.transform.RotateAround(arror.transform.position, cross.normalized, angle);

    } // adjustArrorSymbol

    public void TurnOffAllArrow()
    {
        if (arror.transform.childCount != 0)
        {
            arror.transform.GetChild(0).SetParent(null);
        } // if

        arror.SetActive(false);

        // theRotingObject = null;
    } // TurnOffAllArrow

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if( arror == null ) {
                arror = Resources.FindObjectsOfTypeAll<arrowDrg>().ElementAt(0).gameObject ;
            } // if

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500))
            {
                if (ReferenceEquals(hit.transform, theRotingObject.transform))
                {
                    // dont change active
                } // if
                else if (hit.transform.tag.Contains("arrowSymbol"))
                {
                    // dont change active
                } // else if
                else
                {
                    TurnOffAllArrow();
                } // else 
            } // if
            else
            { // 沒hit到任何東西時
                TurnOffAllArrow();
                theRotingObject = null ;
            } // else

        } // if

        if (previousClick == null)
        {
            previousClick = theRotingObject;
        } // if

        if (!ReferenceEquals(previousClick, theRotingObject))
        {

            previousClick = theRotingObject;
            TurnOffAllArrow();
        } // if

        if (theRotingObject != null)
        { // 把箭頭出現
            arror.SetActive(true);
            arror.transform.position = theRotingObject.transform.position;
            adjustArrorSymbol(theRotingObject);
            theRotingObject.transform.root.SetParent(arror.transform);
        } // if

    } //FixedUpdate
    public void TurnOffOtherGM()
    {
        GetComponent<GM>().enabled = false;
    } // TurnOffOtherGM

    public void UnlockMyHeart()             // 讓我的心~~~~ Un~~~LOCK <333
    {           ////////// 跟GM互相開關

        TurnOffAllArrow();

        GetComponent<GM>().enabled = true;

        GetComponent<GM>().TurnOffOtherGM();

    } // UnlockMyHeart



    public bool SetRoting(Transform inp) // called by fucked only
    {
        if (inp != null)
        {
            theRotingObject = inp;
            if (this.saveOldStates == null)
            {
                saveOldStates = GameObject.Find("Old Rotation");

                if (saveOldStates == null)
                {
                    saveOldStates = new GameObject("Old Rotation");
                } // if

            } // if

            return true;
        }
        else
        {
            return false;
        } // else 
    } // SetRoting

} // end of class
