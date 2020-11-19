using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseUnit : MonoBehaviour {
    public static ChooseUnit _instance;//做成單例
    private void Awake()
    {
        _instance = this;
    }


    private Transform curSelectedOne;//單選變數

    /// <summary>
    /// 顯示選擇圈
    /// </summary>
    /// <param name="go"></param>
    void Active(Transform go)
    {
        go.gameObject.SetActive(true);
    }
    /// <summary>
    /// 隱藏選擇圈
    /// </summary>
    /// <param name="go"></param>
    void DeActive(Transform go)
    {
        go.gameObject.SetActive(false);
    }
    /// <summary>
    /// 檢測點選事件
    /// </summary>
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            ShiftMultiSelect();//呼叫shift連選
        }
        else if (Input.GetMouseButtonDown(0))
        {
            SelectOne();//單選
        }
    }
    /// <summary>
    /// 單擊滑鼠左鍵點選一個單位
    /// </summary>
    void SelectOne()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;//發射射線檢測與物體的碰撞
        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.Log("2");
            if (hitInfo.collider.gameObject.tag == "testUnit")//物體必須有Unit標籤才能被選中
            {
                Transform circle = hitInfo.collider.transform.Find("selectedsprite");//選擇圈臨時變數
                if (curSelectedOne != null)//如果當前選擇單位
                {
                    if (circle != curSelectedOne)//如果當前選擇單位！=前一個單位
                    {
                        DeActive(curSelectedOne);//關閉前一個單位的顯示
                        Active(circle);//顯示當前單位
                        curSelectedOne = circle;
                    }
                }
                else
                {
                    curSelectedOne = circle;
                    Active(circle);
                }
                //Debug.Log("3");
                //Debug.Log(circle.name.ToString());
            }
        }
    }

    /// <summary>
    /// shift+滑鼠點選加選單位
    /// </summary>
    void ShiftMultiSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.tag == "testUnit")
            {
                hitInfo.collider.GetComponent<WasSelected>().wasSelected = !hitInfo.collider.GetComponent<WasSelected>().wasSelected;//對被選擇單位的wasSelected變數取反
                if (hitInfo.collider.GetComponent<WasSelected>().wasSelected == true)//若是true則顯示，else關閉顯示；
                {
                    Transform circle = hitInfo.collider.transform.Find("selectedsprite");
                    Active(circle);
                }
                else
                {
                    Transform circle = hitInfo.collider.transform.Find("selectedsprite");
                    DeActive(circle);
                }
            }

        }
    }

}
