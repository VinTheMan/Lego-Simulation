using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboParent : MonoBehaviour
{

    public float GridScale; // 接child的
    public Transform groundPos;

    public Vector3 lockOnAxis;

    public Vector3 screenPoint; // from child
    public Vector3 offset;
    public Vector3 offsetToLine;

    public Vector3 MousClikedPos; // 按下的child零件要傳過來
    private bool YesPutIiIn;
    public bool isActive; // 現在亮起的零件
    public bool nowGroupSelecting;

    public const float mouse_magenet_dist = 0.3f;

    public Transform L_start = null;
    public Transform L_end = null;

    Transform other;
    public Transform mySelf;
    public GameObject GM;

    Vector3 holeOffset;

    public bool mouseFollow;
    void Start()
    {
        if (GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GM");
        } // if

        YesPutIiIn = false;
        nowGroupSelecting = false;
        screenPoint = new Vector3(-999, -999, -999);
        offset = new Vector3(-999, -999, -999);
        holeOffset = new Vector3(-999, -999, -999);
    } // start

    // Update is called once per frame
    void Update()
    {
        if (isActive && mouseFollow)
        {
            MouseFollow();
        } // if
    } // update

    public void infoFromChild(float inGridScale, Transform inGroundPos,
     Vector3 inScreenPoint, Vector3 MousClikedPos)
    {
        this.GridScale = 0.1f; //inGridScale;
        this.groundPos = inGroundPos;
        this.screenPoint = inScreenPoint;
        this.MousClikedPos = MousClikedPos;
        this.offset = this.transform.position - MousClikedPos;



    } // infoFromChild

    public Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        lineDir.Normalize();   //this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);
        return linePnt + lineDir * d;
    } // NearestPointOnLine

    public void ActiveAllChildren()
    {
        this.isActive = true;
        this.mouseFollow = false;
        for (int a = 0; a < this.transform.childCount; a++)
        {
            if (this.transform.GetChild(a).GetComponent<MouseScriptZ>() != null)
            {
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().isActive = true;
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().mouseFu = false;
                this.transform.GetChild(a).GetComponent<Outline>().enabled = true;
            } // if
            else if (this.transform.GetChild(a).GetComponent<ComboParent>() != null)
            {
                this.transform.GetChild(a).GetComponent<ComboParent>().ActiveAllChildren();
            } // else if
        } // for
    } // ActiveAllChildren

    public void ActiveAllMousFollow()
    {
        this.isActive = true;
        this.mouseFollow = true;
        for (int a = 0; a < this.transform.childCount; a++)
        {
            if (this.transform.GetChild(a).GetComponent<MouseScriptZ>() != null)
            {
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().mouseFu = true;
                this.transform.GetChild(a).GetComponent<Outline>().enabled = true;
            } // if

            if (this.transform.GetChild(a).GetComponent<ComboParent>() != null)
            {
                this.transform.GetChild(a).GetComponent<ComboParent>().ActiveAllMousFollow();
            } // if
        } // for
    } // ActiveAllMousFollow

    public void JustOut()
    {
        this.isActive = true;
        this.mouseFollow = true;
        for (int a = 0; a < this.transform.childCount; a++)
        {
            if (this.transform.GetChild(a).GetComponent<MouseScriptZ>() != null)
            {
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().JustInit = true;
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().isActive = true;
                this.transform.GetChild(a).GetComponent<Outline>().enabled = true;
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().mouseFu = true;
            } // if
            else if (this.transform.GetChild(a).GetComponent<ComboParent>() != null)
            {
                this.transform.GetChild(a).GetComponent<ComboParent>().JustOut();
            } // else if
        } // for
    } // JustOut

    public void InactiveAllChildren()
    {
        this.isActive = false;
        this.mouseFollow = false;
        for (int a = 0; a < this.transform.childCount; a++)
        {
            if (this.transform.GetChild(a).GetComponent<MouseScriptZ>() != null)
            {
                this.transform.GetChild(a).GetComponent<MouseScriptZ>().InactiveAndDisableMouFu();
            } // if
            else if (this.transform.GetChild(a).GetComponent<ComboParent>() != null)
            {
                this.transform.GetChild(a).GetComponent<ComboParent>().InactiveAllChildren();
            } // else if
        } // for
    } // InactiveAllChildren

    void MouseFollow()
    {
        Vector3 curScreenPoint;
        Vector3 curPosition;
        int reCode = -1;

        reCode = GM.GetComponent<GM>().MovementRequestForCombo(this.transform);

        // Debug.Log(reCode); // test

        var collider = GetComponent<Collider>();
        Vector3 screenPointNow = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Vector3 nowMouPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPointNow.z));

        float mouseToObj = Vector3.Distance(this.transform.position - offset, nowMouPoint);
        // Debug.Log( mouse_magenet_dist + "+++" + mouseToObj );

        if (reCode == 999 || mouseToObj >= mouse_magenet_dist)
        { // 會進來 有return 999

            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            // curPosition.y = groundPos.position.y; // 貼地移動
            if (ReferenceEquals(this.transform.root, this.transform))
            {
                this.transform.position = new Vector3(Mathf.Round(curPosition.x / GridScale) * GridScale,
                                 Mathf.Round(curPosition.y / GridScale) * GridScale,
                                 Mathf.Round(curPosition.z / GridScale) * GridScale);
            }

        } // if
        else if (reCode == -1)
        { // GM list not found return -1 
            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            // curPosition.y = groundPos.position.y; // 貼地移動
            if (ReferenceEquals(this.transform.root, this.transform))
            {
                this.transform.position = new Vector3(Mathf.Round(curPosition.x / GridScale) * GridScale,
                                 Mathf.Round(curPosition.y / GridScale) * GridScale,
                                 Mathf.Round(curPosition.z / GridScale) * GridScale);
            }

        } // else if
        else // 可以連接
        {
            // Debug.Log(this.name + " RETURN INDEX: " + reCode); // reCode是這個物件在ALLDOABLE裡面的位置

            mySelf = GM.GetComponent<GM>().ReLockOn(reCode);

            if (reCode % 2 == 0)
            { // if index is even the one we need to lock on( other ) is a hole 
                other = GM.GetComponent<GM>().ReLockOn(reCode + 1);
            } // if
            else
            { // otherwise other is a rod
                other = GM.GetComponent<GM>().ReLockOn(reCode - 1);
            } // else 

            int assignCount = 0;
            L_start = null;
            L_end = null;

            for (int x = 0; x < other.childCount; x++)
            {
                Transform temp = other.GetChild(x);
                if (temp.tag.Contains("ParParSet1"))
                {
                    assignCount++;
                    switch (assignCount)
                    {
                        case 1:
                            L_start = temp;
                            break;
                        case 2:
                            L_end = temp;
                            break;
                        default:
                            Debug.Log("Lock-On ParParSet are wrong");
                            break;
                    } // switch
                } // if
            } // for

            //linePnt - point the line passes through
            //lineDir - unit vector in direction of line, either direction works
            //pnt - the point to find nearest on line for
            Vector3 nextOn = new Vector3();
            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            Vector3 a = NearestPointOnLine(L_start.position, (L_end.position - L_start.position), mySelf.position);
            Vector3 b = NearestPointOnLine(L_start.position, (L_end.position - L_start.position), MousClikedPos);
            Vector3 c = NearestPointOnLine(L_start.position, (L_end.position - L_start.position), this.transform.position);
            Vector3 d = NearestPointOnLine(L_start.position, (L_end.position - L_start.position), curPosition);
            offsetToLine = mySelf.position - a;
            offset = this.transform.position - MousClikedPos;

            if (offsetToLine == new Vector3(0, 0, 0))
            {
                // nextOn = NearestPointOnLine(this.transform.position, (L_end.position - L_start.position), curPosition+offset);
            } // if
            else
            {
                nextOn = this.transform.position + offsetToLine;
                offsetToLine = new Vector3(0, 0, 0);
            } // else






            Debug.Log(mySelf.name + " : " + mySelf.position + " cc :" + offsetToLine); // test

            this.transform.position = new Vector3(Mathf.Round(nextOn.x / GridScale) * GridScale,
                                                 Mathf.Round(nextOn.y / GridScale) * GridScale,
                                                 Mathf.Round(nextOn.z / GridScale) * GridScale);



        } // else

    } // MouseFollow


} // ComboParent
