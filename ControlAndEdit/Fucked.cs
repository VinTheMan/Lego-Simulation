using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fucked : MonoBehaviour
{

    public GameObject GM;
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM");
        Transform temp = this.transform.parent;

        for (; this.transform.root != temp; temp = temp.transform.parent)
        { // ignore all collider on same 零件(群組) // 一層層往上處理
            if (temp.transform.GetComponent<Collider>() != null)
            { // parent的collider先忽略
                Physics.IgnoreCollision(this.GetComponent<Collider>(), temp.transform.GetComponent<Collider>());
            } //if

            for (int x = 0; x < temp.parent.childCount; x++)
            { // 同一輩的collider全數忽略
                for (int y = 0; y < temp.parent.GetChild(x).childCount; y++)
                { // 同一輩的collider全數忽略
                    if (temp.parent.GetChild(x).GetChild(y).transform.GetComponent<Collider>() != null)
                    {
                        Physics.IgnoreCollision(this.GetComponent<Collider>(), temp.parent.GetChild(x).GetChild(y).transform.GetComponent<Collider>());
                    } // if
                } // for
            } // for

        } // for

        if (temp.transform.GetComponent<Collider>() != null)
        { // 最後如果root上有collider 也要忽略
            Physics.IgnoreCollision(this.GetComponent<Collider>(), temp.transform.GetComponent<Collider>());
        } // if

    } // start

    // Update is called once per frame
    void Update()
    {
        if (GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GM");
        } // if

        if (GM.GetComponent<GM>().enabled == false)
        { // 要旋轉時 換layer
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = true;
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        } // if
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = true;
            this.GetComponent<BoxCollider>().enabled = false;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        } // else

    } // update

    void OnMouseDown() // for dragging 坐標軸定位 // 只能也只會點在Box collider上
    {
        if (GM.GetComponent<RotateGM>().enabled == true)
        {
            GM.GetComponent<RotateGM>().SetRoting(this.transform.parent); // 以這個parparent為軸心
            // Debug.Log("????????" ) ; //test
        } // if
    } // OnMouseDown

    void OnTriggerEnter(Collider other)
    {
        if (GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GM");
        } // if

        if (GM.GetComponent<GM>().enabled == true)
        {
            if (this.transform.tag.Contains("Hole") && other.transform.tag.Contains("Hole"))
            {
                // 忽略洞撞洞
            } // if
            else if (this.transform.tag.Contains("Rod") && other.transform.tag.Contains("Rod"))
            {
                // 忽略棒撞棒
            } // else if
            else
            {
                GM.GetComponent<GM>().WhenInRange(1, other.transform.parent);  // Rods  // 都是parparent
                GM.GetComponent<GM>().WhenInRange(2, this.transform.parent);  //  Holes // 都是parparent
                                                                              // 都是回傳自己的parparent
            } // else
        } // if



    } // OnTrigger

    void OnTriggerExit(Collider other)
    {
        if (GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GM");
        } // if

        if (this.transform.tag.Contains("Hole") && other.transform.tag.Contains("Hole"))
        {
            // 忽略洞撞洞
        } // if
        else if (this.transform.tag.Contains("Rod") && other.transform.tag.Contains("Rod"))
        {
            // 忽略棒撞棒
        } // else if
        else
        {
            GM.GetComponent<GM>().WhenOutOfRange(other.transform.parent, this.transform.parent); // Rods, Holes
        } // else
    } // OnTriggerExit
} // end of class
