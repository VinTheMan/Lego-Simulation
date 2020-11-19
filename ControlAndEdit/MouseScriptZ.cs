using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;


public class MouseScriptZ : MonoBehaviour
{
    public float GridScale;
    public Transform groundPos;// 貼地移動時 會由旋轉script更動 ?????????

    public Vector3 lockOnAxis;
    public Vector3 MousClikedPos; // 傳給comboParent用

    public Vector3 screenPoint;
    public Vector3 offset;
    public Vector3 offsetWithRoot;
    private bool YesPutIiIn;
    public bool isActive; // 現在亮起的零件
    public bool mouseFu; // 要拉一下才能跟著動
    public bool nowGroupSelecting;
    public bool JustInit = false;
    int frames; // 用數字增加拖曳按壓時間

    public const float mouse_magenet_dist = 0.3f;

    Transform other;
    Transform mySelf;
    public GameObject GM;
    Vector3 holeOffset;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM");
        // isActive = false;
        YesPutIiIn = false;
        if (this.transform.name.Contains("22"))
        {
            GridScale = 0.1f;
        } // if
        else
        {
            GridScale = 0.1f;
        } // else

        mouseFu = false;
        nowGroupSelecting = false;
        frames = 0;
        holeOffset = new Vector3(-999, -999, -999);

        // GetComponent<Outline>().enabled = false;

        groundPos = GM.transform;  // ?? ground maybe ???

        if (JustInit)
        {
            isActive = true;
            GetComponent<Outline>().enabled = true;
            mouseFu = true;
            JustInit = false;
        } // if


    } // start

    public void ReCalHoleOffset()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position -
           Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        MousClikedPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        offsetWithRoot = this.transform.position - this.transform.root.position;

        if (this.transform.root.GetComponent<ComboParent>() != null)
        {
            this.transform.root.GetComponent<ComboParent>().infoFromChild(GridScale, groundPos,
                             screenPoint, MousClikedPos);
        } // if

    } // ReCalHoleOffset

    public void TurnOffOtherMouseScript()
    {
        GetComponent<MouseScriptRot>().isActive = false;
        GetComponent<MouseScriptRot>().enabled = false;
    } //TurnOffOtherMouseScript

    bool IsMouseOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void Update()
    {

        if (nowGroupSelecting)
        {
            mouseFu = false;
            frames = 0;
        } // if

        if (GM.GetComponent<RotateGM>().enabled == true)
        {
            GetComponent<MouseScriptRot>().enabled = true;
            if (isActive)
            {
                GetComponent<MouseScriptRot>().isActive = true;
            }
            else
            {
                GetComponent<MouseScriptRot>().isActive = false;
            } // if else

            GetComponent<MouseScriptRot>().TurnOffOtherMouseScript();
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        } // if

        if (Input.GetMouseButtonDown(0) && isActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (mouseFu && IsMouseOnUI() == false)
            {
                isActive = false;
                GM.GetComponent<GM>().deleteActive();
                GetComponent<Outline>().enabled = false;
                if (this.transform.root.GetComponent<ComboParent>() != null)
                {
                    this.transform.root.GetComponent<ComboParent>().InactiveAllChildren();
                } // if
                mouseFu = false;
                GM.GetComponent<GM>().theDraggingObject = null;
                frames = 0;
            } // if
            else if (Physics.Raycast(ray, out hit) && IsMouseOnUI() == false)
            {
                if (ReferenceEquals(hit.transform.root, this.transform.root))
                {
                    // dont change active
                } // if
                else
                {
                    isActive = false;
                    GetComponent<Outline>().enabled = false;

                    GM.GetComponent<GM>().deleteActive();
                    if (this.transform.root.GetComponent<ComboParent>() != null)
                    {
                        this.transform.root.GetComponent<ComboParent>().InactiveAllChildren();
                    } // if
                    mouseFu = false;

                    GM.GetComponent<GM>().theDraggingObject = null;
                    frames = 0;
                } // else 
            } // else if
            else if (IsMouseOnUI() == true)
            {
                mouseFu = false;
                GM.GetComponent<GM>().theDraggingObject = null;
                frames = 0;
            } // else if
            else
            {
                isActive = false;
                GetComponent<Outline>().enabled = false;
                GM.GetComponent<GM>().deleteActive();
                if (this.transform.root.GetComponent<ComboParent>() != null)
                {
                    this.transform.root.GetComponent<ComboParent>().InactiveAllChildren();
                } // if
                mouseFu = false;

                GM.GetComponent<GM>().theDraggingObject = null;
                frames = 0;
            } // else

        } // if

        if (isActive == true && mouseFu == true)
        {
            if (this.transform.root.GetComponent<ComboParent>() != null)
            {

                // this.transform.root.GetComponent<ComboParent>().infoFromChild(GridScale, groundPos,
                //      screenPoint, MousClikedPos);

            } // if 
            else
            {
                MouseFollow();
            } // else
        } // if
        else
        {
            // do nothing 
        } // else
    } // Update

    //linePnt - point the line passes through
    //lineDir - unit vector in direction of line, either direction works
    //pnt - the point to find nearest on line for
    public Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        lineDir.Normalize();   //this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);
        return linePnt + lineDir * d;
    } // NearestPointOnLine

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position -
           Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        MousClikedPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        offsetWithRoot = this.transform.position - this.transform.root.position;

        if (!isActive)
        {
            InvokeRepeating("changeIsActive", 0.1f, 0f); // 延後時間一點點再觸發才不會亂跳
            GM.GetComponent<GM>().SetActiveObj(this.transform.root.gameObject);

        } // if

        if (this.transform.root.GetComponent<ComboParent>() != null)
        {

            this.transform.root.GetComponent<ComboParent>().infoFromChild(GridScale, groundPos,
                 screenPoint, MousClikedPos);

        } // if 

    } // OnMouseDown()

    void OnMouseDrag()
    {
        frames++;

        if (frames > 15 && isActive)
        {
            mouseFu = true;
            if (this.transform.root.GetComponent<ComboParent>() != null)
            {
                this.transform.root.GetComponent<ComboParent>().mouseFollow = true;
            } // if

            GM.GetComponent<GM>().SetDragging(this.transform.root.gameObject, this.GridScale );
        } // if 

    } // OnMouseDrag

    public void changeIsActive()
    {
        this.isActive = true;
        this.GetComponent<Outline>().enabled = true;
        if (this.transform.root.GetComponent<ComboParent>() != null)
        {
            this.transform.root.GetComponent<ComboParent>().ActiveAllChildren();
        } // if
    }// Change!

    public void changeIsActiveSingle()
    {
        this.isActive = true;
        this.GetComponent<Outline>().enabled = true;
    }// Change!

    public void InactiveAndDisableMouFu()
    {
        this.isActive = false;
        this.GetComponent<Outline>().enabled = false;
        this.mouseFu = false;
        this.frames = 0;
        if (this.transform.root.GetComponent<ComboParent>() != null)
        {
            this.transform.root.GetComponent<ComboParent>().mouseFollow = false;
        } // if

    } // InactiveAndDisableMouFu

    public void InactiveAndDisableMouFuSingle()
    {
        this.isActive = false;
        this.GetComponent<Outline>().enabled = false;
        this.mouseFu = false;
        this.frames = 0;
    } // InactiveAndDisableMouFu

    void MoveOnGround()
    {
        Vector3 curScreenPoint;
        Vector3 curPosition;
    } // MoveOnGround
    void MouseFollow()
    {
        Vector3 curScreenPoint;
        Vector3 curPosition;

        int reCode = GM.GetComponent<GM>().MovementRequest(this.transform);

        // Debug.Log(this.transform); // test

        var collider = GetComponent<Collider>();
        Vector3 screenPointNow = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Vector3 nowMouPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPointNow.z));
        float mouseToObj = Vector3.Distance(this.transform.position - offset, nowMouPoint);
        // Debug.Log( mouse_magenet_dist + "+++" + mouseToObj ); // test

        if (reCode == 999 || mouseToObj >= mouse_magenet_dist)
        { // 會進來 有return 999

            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset + offsetWithRoot;
            // curPosition.y = groundPos.position.y; // 貼地移動
            if (ReferenceEquals(this.transform.root, this.transform))
            {
                this.transform.position = new Vector3(Mathf.Round(curPosition.x / GridScale) * GridScale,
                                 Mathf.Round(curPosition.y / GridScale) * GridScale,
                                 Mathf.Round(curPosition.z / GridScale) * GridScale);
            }
            else
            { // 在群組底下
                Debug.Log(" should not be here! Under combo parent but single reCode 999"); // test
            } // else

        } // if
        else if (reCode == -1)
        { // GM list not found return -1 
            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset + offsetWithRoot;
            // curPosition.y = groundPos.position.y; // 貼地移動
            if (ReferenceEquals(this.transform.root, this.transform))
            {
                this.transform.position = new Vector3(Mathf.Round(curPosition.x / GridScale) * GridScale,
                                 Mathf.Round(curPosition.y / GridScale) * GridScale,
                                 Mathf.Round(curPosition.z / GridScale) * GridScale);
            }
            else
            { // 在群組底下
                Debug.Log(" should not be here! Under combo parent but single reCode -1"); // test
            } // else

        } // else if
        else // 可以連接
        {
            // Debug.Log(this.name + " RETURN INDEX: " + reCode); // reCode是這個物件在ALLDOABLE裡面的位置

            mySelf = GM.GetComponent<GM>().ReLockOn(reCode);
            if (this.transform.childCount == 0)
            {
                Debug.Log(" this :::::" + this.name + "::::: item is missing its parparent collider ");
                return;
            } // if

            if (reCode % 2 == 0)
            { // if index is even the one we need to lock on( other ) is a hole 
                other = GM.GetComponent<GM>().ReLockOn(reCode + 1);
            } // if
            else
            { // otherwise other is a rod
                other = GM.GetComponent<GM>().ReLockOn(reCode - 1);
            } // else 

            int assignCount = 0;
            Transform L_start = null;
            Transform L_end = null;

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
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            // Debug.Log(holeOffset); // test
            if (L_start != null)
            {
                nextOn = NearestPointOnLine(L_start.position, (L_end.position - L_start.position), curPosition) - mySelf.localPosition;
            } // if


            if (ReferenceEquals(this.transform, this.transform.root))
            {
                this.transform.position = new Vector3(Mathf.Round(nextOn.x / GridScale) * GridScale,
                                                 Mathf.Round(nextOn.y / GridScale) * GridScale,
                                                 Mathf.Round(nextOn.z / GridScale) * GridScale);


                YesPutIiIn = true;
                // Debug.Log(this.transform.name + " YES ");
            } // if
            else
            { // 在群組底下
                Debug.Log(" should not be here! Under combo parent but single reCode sth correct"); // test
            } // else 



        } // else

    } // MouseFollow

} // end class