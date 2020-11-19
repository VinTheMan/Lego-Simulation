using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GM : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;
    public float GridScale;
    public float GridScaleBuffer;

    Transform theRotPoint;

    public bool YesPutIiIn;

    public GameObject theDraggingObject;

    public GameObject theActiveObject;
    public List<Transform> mListFuck;
    public List<Transform> mListFucked;

    public List<Transform> AllTheDoables;

    public GameObject saveOldStates;
    public GameObject copyBuffer;

    //--------------------------------------------------------------------------------
    public Transform L1_start;
    public Transform L1_end;
    public Transform L2_start;
    public Transform L2_end;

    private Transform L1_start_temp;
    private Transform L1_end_temp;
    private Transform L2_start_temp;
    private Transform L2_end_temp;
    //--------------------------------------------------------------------------------

    void Start()
    {
        mListFuck = new List<Transform>();
        mListFucked = new List<Transform>();
        AllTheDoables = new List<Transform>();
        saveOldStates = GameObject.Find("Old Rotation");
    } // start 

    public void TurnOffOtherGM()
    {
        GetComponent<RotateGM>().enabled = false;
    } // TurnOffOtherGM

    public void deleteActive()
    {
        theDraggingObject = null;
        theActiveObject = null;
    }// deleteActive

    public void UnlockMyHeart()       
    {           ////////// 跟RotGM互相開關

        GetComponent<RotateGM>().enabled = true;
        GetComponent<RotateGM>().TurnOffOtherGM();

    } // UnlockMyHeart

    public void TargetCleanUP(Transform inp)
    {
        for (int a = 0; a < mListFuck.Count; a++)
        {
            if ((ReferenceEquals(mListFuck[a].root, inp.root) ||
                 ReferenceEquals(mListFucked[a].root, inp.root)))
            {
                mListFucked.RemoveAt(a);
                mListFuck.RemoveAt(a);
            } // if
        } // for

        mListFuck.TrimExcess();
        mListFucked.TrimExcess();

        AllTheDoables.Clear();
        AllTheDoables.TrimExcess();
    } // TargetCleanUP

    public void CtrlMovement()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("AAAAA");
            MouseScriptZ[] tempArray = (MouseScriptZ[])GameObject.FindObjectsOfType(typeof(MouseScriptZ));
            float posX = 0.0f, posY = 0.0f, posZ = 0.0f;
            int objCount = 0;
            foreach (MouseScriptZ RRR in tempArray)
            {
                RRR.transform.parent = null;
                objCount++;
                posX = posX + RRR.transform.position.x;
                posY = posY + RRR.transform.position.y;
                posZ = posZ + RRR.transform.position.z;
            } // for each

            // 群組obj的transform position中心點要設在所有選取零件的中心 不然剪下貼上的時候會有歪掉的感覺
            posX = posX / objCount;
            posY = posY / objCount;
            posZ = posZ / objCount;
            GameObject go = new GameObject();
            go.AddComponent<ComboParent>();
            go.transform.position = new Vector3(posX, posY, posZ);

            foreach (MouseScriptZ RRR in tempArray)
            {
                RRR.changeIsActive(); // 只會turn true
                // 接下來要把所有東西放到同一個obj底下(跟框選群組一樣) 
                RRR.transform.parent = go.transform;
            } // foreach

            // 然後把那個obj丟到 GM.SetActiveObj( gameObject )
            SetActiveObj(go);
        } // if
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("CCCCC");
            if (theActiveObject != null)
            {
                if (theActiveObject.GetComponent<ComboParent>() != null &&
                 theActiveObject.transform.childCount != 0)
                {
                    copyBuffer = theActiveObject;
                    GridScaleBuffer = GridScale;
                } // if
                else if (theActiveObject.GetComponent<MouseScriptZ>() != null)
                {
                    copyBuffer = theActiveObject;
                    GridScaleBuffer = GridScale;
                } // else if
            } // if

        } // else if
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("XXXXX");
            if (theActiveObject != null)
            {
                if (theActiveObject.GetComponent<ComboParent>() != null &&
                 theActiveObject.transform.childCount != 0)
                {
                    copyBuffer = null;
                    theActiveObject.gameObject.SetActive(false);
                    copyBuffer = theActiveObject;
                    deleteActive();
                    copyBuffer.gameObject.SetActive(false);
                } // if
                else if (theActiveObject.GetComponent<MouseScriptZ>() != null)
                {
                    copyBuffer = null;
                    theActiveObject.gameObject.SetActive(false);
                    copyBuffer = theActiveObject;
                    deleteActive();
                    copyBuffer.gameObject.SetActive(false);
                } // else if



            }

        } // else if
        else if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("VVVVV"); // test
            if (copyBuffer != null)
            {
                Vector3 screenPoint = Camera.main.WorldToScreenPoint(copyBuffer.transform.position);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

                if (theActiveObject != null && theActiveObject.GetComponent<ComboParent>() != null)
                {
                    theActiveObject.GetComponent<ComboParent>().InactiveAllChildren();
                } // if
                else if (theActiveObject != null && theActiveObject.GetComponent<MouseScriptZ>() != null)
                {
                    theActiveObject.GetComponent<MouseScriptZ>().isActive = false;
                    theActiveObject.GetComponent<MouseScriptZ>().mouseFu = false;
                } // else if

                SetActiveObj(Instantiate(copyBuffer, mousePos, copyBuffer.transform.rotation));
                SetDragging(theActiveObject, GridScaleBuffer);
                theActiveObject.SetActive(true);
                theActiveObject.name = copyBuffer.name;
                if (theActiveObject != null && theActiveObject.GetComponent<ComboParent>() != null)
                {
                    theActiveObject.GetComponent<ComboParent>().JustOut();
                    theActiveObject.GetComponent<ComboParent>().ActiveAllChildren();
                    theActiveObject.GetComponent<ComboParent>().ActiveAllMousFollow();

                } // if
                if (theActiveObject != null && theActiveObject.GetComponent<MouseScriptZ>() != null)
                {
                    theActiveObject.GetComponent<MouseScriptZ>().JustInit = true;
                    theActiveObject.GetComponent<MouseScriptZ>().isActive = true;
                    theActiveObject.GetComponent<MouseScriptZ>().mouseFu = true;
                } // if

            } // if
        } // else if
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            mListFuck.Clear();
            mListFucked.Clear();
            MouseScriptZ[] tempArray = (MouseScriptZ[])GameObject.FindObjectsOfType(typeof(MouseScriptZ));

            foreach (MouseScriptZ RRR in tempArray)
            {
                RRR.transform.GetComponent<MouseScriptZ>().nowGroupSelecting = true;
            } // for each

            var fooGroup = Resources.FindObjectsOfTypeAll<SelectTool>();
            if (fooGroup.Length > 0)
            {
                var foo = fooGroup[0];
                foo.transform.gameObject.SetActive(true);
            }

        } // else if

    } // CtrlMovement

    public void stickToMouse()
    { // 把物體最近點移動黏到滑鼠上

        screenPoint = Camera.main.WorldToScreenPoint(theDraggingObject.transform.position);
        Vector3 MousPos =
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        Vector3 closest = new Vector3(0, 0, 0);
        if (theDraggingObject.GetComponent<ComboParent>() != null)
        {
            Transform te = FindClosetParParToMouse(theDraggingObject.transform);
            closest = te.parent.GetComponent<MeshCollider>().ClosestPoint(MousPos);

        } // if
        else
        {
            closest = theDraggingObject.GetComponent<MeshCollider>().ClosestPoint(MousPos);
        } // else 

        Vector3 offe = MousPos - closest;
        Vector3 cur = theDraggingObject.transform.position + offe;
        theDraggingObject.transform.position = new Vector3(Mathf.Round(cur.x / GridScale) * GridScale,
                                 Mathf.Round(cur.y / GridScale) * GridScale,
                                 Mathf.Round(cur.z / GridScale) * GridScale); ;
        Debug.Log("Stick offset : " + offe); // test

    } // stickToMouse()
    public void recalculateStatus()
    {
        if (theDraggingObject.GetComponent<MouseScriptZ>() != null)
        {
            theDraggingObject.GetComponent<MouseScriptZ>().ReCalHoleOffset();
        } // if
        GetComponent<RotateGM>().setFormerRotState(ref theDraggingObject); // 跟RotGM互相幫對方set

        if (this.saveOldStates == null)
        {
            saveOldStates = GameObject.Find("Old Rotation");

            if (saveOldStates == null)
            {
                saveOldStates = new GameObject("Old Rotation");
            } // if

        } // if

        saveOldStates.transform.rotation = theDraggingObject.transform.rotation;
    } // recalculateStatus()

    void deleCleanUp(GameObject theactiveobj)
    {
        if (saveOldStates != null)
        {
            saveOldStates.transform.localEulerAngles = new Vector3(0, 0, 0);
        } // if

        if (L1_start != null && (ReferenceEquals(L1_start.root, theactiveobj.transform.root) ||
        ReferenceEquals(L2_start.root, theactiveobj.transform.root)))
        {
            L1_start = null;
            L1_end = null;
            L2_end = null;
            L2_start = null;
        } // if

        TargetCleanUP(theactiveobj.transform);

    } // deleCleanUp
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))   // 一系列的快速90度轉
        {
            QuickRot(1);

        } // if
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            QuickRot(2);

        } //else if
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            QuickRot(3);

        } //else if
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            QuickRot(4);

        } //else if

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            deleCleanUp(theActiveObject);
            if (theActiveObject != null)
            {
                if( theActiveObject.GetComponent<SelectingGameObject>() != null ) {
                    for ( int a = 0 ; a < theActiveObject.transform.childCount ; a++ ) {
                        Destroy(theActiveObject.transform.GetChild(a).gameObject);
                    } // for
                } // if
                else {
                    Destroy(theActiveObject);
                } // else
                
            } // if

            theDraggingObject = null;
            theActiveObject = null;
        } // dele
        else if (Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.M))
        {
            // Ctrl + x 剪下
            // Ctrl + a 全選
            // Ctrl + c 複製
            // Ctrl + v 貼上
            CtrlMovement();
        } // if
        else
        {
            var fooGroup = Resources.FindObjectsOfTypeAll<SelectTool>();
            if (fooGroup.Length > 0)
            {
                var foo = fooGroup[0];
                foo.transform.gameObject.SetActive(false);
            }

            MouseScriptZ[] tempArray = (MouseScriptZ[])GameObject.FindObjectsOfType(typeof(MouseScriptZ));

            foreach (MouseScriptZ RRR in tempArray)
            {
                RRR.transform.GetComponent<MouseScriptZ>().nowGroupSelecting = false;
            } // for each
        } // else


        if (theDraggingObject != null &&
         ((theDraggingObject.GetComponent<MouseScriptZ>() != null && theDraggingObject.GetComponent<MouseScriptZ>().isActive == true) ||
        (theDraggingObject.GetComponent<ComboParent>() != null && theDraggingObject.GetComponent<ComboParent>().isActive == true)))
        {
            screenPoint = Camera.main.WorldToScreenPoint(theDraggingObject.transform.position);

            offset = theDraggingObject.transform.position -
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        } // if
        else if (theDraggingObject != null && ((theDraggingObject.GetComponent<MouseScriptZ>() != null && theDraggingObject.GetComponent<MouseScriptZ>().isActive == false) ||
                                            (theDraggingObject.GetComponent<ComboParent>() != null && theDraggingObject.GetComponent<ComboParent>().isActive == false)))
        {
            mListFuck.Clear();
            mListFucked.Clear();
            AllTheDoables.Clear();
        } // else if


    } // FixedUpdate

    void RoRo(int code1, int code3, int keyDown)
    {
        theDraggingObject.transform.RotateAround(theDraggingObject.transform.position, theDraggingObject.transform.forward, 90f);
                
    } // RoRo

    Transform FindClosetParParToMouse(Transform tarTransform)
    {
        if (tarTransform != null)
        {
            Vector3 s1 = Camera.main.WorldToScreenPoint(tarTransform.transform.position);
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
             Input.mousePosition.y, s1.z));

            if (tarTransform.GetComponent<ComboParent>() != null) // 在群組底下
            {
                int childInde = 0;
                Transform ree1 = FindClosetParParToMouse(tarTransform.transform.GetChild(0));
                float dis = Vector3.Distance(ree1.position, MousePos);
                for (int a = 1; a < tarTransform.transform.childCount; a++)
                {
                    float te = 0.0f;
                    if (tarTransform.transform.GetChild(a).GetComponent<ComboParent>() != null)
                    {
                        Transform ree = FindClosetParParToMouse(tarTransform.transform.GetChild(a));
                        te = Vector3.Distance(ree.position, MousePos);
                    } // if
                    else
                    {
                        Transform ree = FindClosetParParToMouse(tarTransform.transform.GetChild(a));
                        te = Vector3.Distance(ree.position, MousePos);
                    } // else


                    if (te < dis)
                    {
                        childInde = a;
                        dis = te;
                    } // if

                } // for

                return tarTransform.transform.GetChild(childInde);
            } // if
            else if (tarTransform.GetComponent<MouseScriptZ>() != null)// 只是一個零件
            {
                int childInde = 0;
                float dis = Vector3.Distance(tarTransform.transform.GetChild(0).position, MousePos);
                for (int a = 1; a < tarTransform.transform.childCount; a++)
                {
                    float te = Vector3.Distance(tarTransform.transform.GetChild(a).position, MousePos);
                    if (te < dis)
                    {
                        childInde = a;
                        dis = te;
                    } // if

                } // for

                return tarTransform.transform.GetChild(childInde);
            } // else if
            else
            {
                return tarTransform.transform;
            } // else
        } // if
        else
        {
            return null;
        } // else

    } // FindClosetParParToMouse()

    void QuickRot(int cc) // 上下左右鍵旋轉
    {
        if (theDraggingObject != null)
        {

            // theRotPoint = FindClosetParParToMouse(theDraggingObject.transform); // not yet used 

            float angle1 = Vector3.Angle(theDraggingObject.transform.forward, Camera.main.transform.forward); // 0
            if (angle1 > 90f) { angle1 = angle1 - 180f; }
            float angle2 = Vector3.Angle(theDraggingObject.transform.up, Camera.main.transform.forward); // 1
            if (angle2 > 90f) { angle2 = angle2 - 180f; }
            float angle3 = Vector3.Angle(theDraggingObject.transform.right, Camera.main.transform.forward); // 2
            if (angle3 > 90f) { angle3 = angle3 - 180f; }

            int statee = -99;
            int statee3 = -99;

            List<float> myList = new List<float>();
            myList.Add(Mathf.Abs(angle1)); // 0
            myList.Add(Mathf.Abs(angle2)); // 1
            myList.Add(Mathf.Abs(angle3)); // 2

            statee = myList.IndexOf(myList.Min()) + 1;
            if (statee == 1 && angle1 < 0) { statee = -statee; }
            else if (statee == 2 && angle1 < 0) { statee = -statee; }
            else if (statee == 3 && angle1 < 0) { statee = -statee; }

            // //////////////////////////////////////////////////////////////////////////////////////

            angle1 = Vector3.Angle(theDraggingObject.transform.forward, Camera.main.transform.right); // 0
            if (angle1 > 90f) { angle1 = angle1 - 180f; }
            angle2 = Vector3.Angle(theDraggingObject.transform.up, Camera.main.transform.right);  // 1
            if (angle2 > 90f) { angle2 = angle2 - 180f; }
            angle3 = Vector3.Angle(theDraggingObject.transform.right, Camera.main.transform.right); // 2
            if (angle3 > 90f) { angle3 = angle3 - 180f; }

            myList.Clear();
            myList.Add(Mathf.Abs(angle1)); // 0
            myList.Add(Mathf.Abs(angle2)); // 1
            myList.Add(Mathf.Abs(angle3)); // 2

            statee3 = myList.IndexOf(myList.Min()) + 1;
            if (statee3 == 1 && angle1 < 0) { statee3 = -statee3; }
            else if (statee3 == 2 && angle1 < 0) { statee3 = -statee3; }
            else if (statee3 == 3 && angle1 < 0) { statee3 = -statee3; }

            stickToMouse();
            recalculateStatus();

        } // if

    } // up down left right

    public int mListFuckCount()
    {
        return mListFuck.Count;
    } // mListFuckCount

    public int mListFuckedCount()
    {
        return mListFucked.Count;
    } // mListFuckedCount


    public void WhenInRange(int whatList, Transform _in)
    {
        if (whatList == 1 && _in != null)
        {
            if (_in.tag.Contains("Rod"))
            {
                mListFuck.Add(_in); // Rod
            } // if
        } // if 
        else if (whatList == 2 && _in != null)
        { // 2 
            if (_in != null && _in.tag.Contains("Hole") && mListFuck.Count > mListFucked.Count)
            {
                mListFucked.Add(_in); // Holes
            } // if
        } // else if

        // Debug.Log( whatList + " : " + _in.name ) ; // test
    } // WhenInRange

    public void WhenOutOfRange(Transform _in, Transform _in2)
    { // Rods, Holes
        if (saveOldStates != null && theDraggingObject != null)
        {
            theDraggingObject.transform.rotation = saveOldStates.transform.rotation;
        }

        for (int a = 0; a < mListFuck.Count; a++)
        {
            if (ReferenceEquals(mListFuck[a], _in) && ReferenceEquals(mListFucked[a], _in2))
            {
                mListFuck.RemoveAt(a);
                mListFucked.RemoveAt(a);
                mListFucked.TrimExcess();
                mListFuck.TrimExcess();
                break;
            } // if
        } // for


        for (int a = 0; a + 1 < AllTheDoables.Count; a++)
        {
            if (GameObject.ReferenceEquals(AllTheDoables[a + 1], _in) && GameObject.ReferenceEquals(AllTheDoables[a], _in2))
            {
                AllTheDoables.RemoveAt(a + 1);
                AllTheDoables.RemoveAt(a);
                AllTheDoables.TrimExcess();
                break;
            } // if
        } // for

        L1_start = null;
        L1_end = null;
        L2_start = null;
        L2_end = null;
        L1_start_temp = null;
        L1_end_temp = null;
        L2_start_temp = null;
        L2_end_temp = null;


    } // WhenOutOfRange

    public void setFormerRotState(ref GameObject inn)
    {
        if (saveOldStates == null)
        {

            saveOldStates = GameObject.Find("Old Rotation");

            if (saveOldStates == null)
            {
                saveOldStates = new GameObject("Old Rotation");
            } // if
        } // if

        saveOldStates.transform.rotation = inn.transform.rotation;
    } // setFormerRotState

    public bool SetDragging(GameObject inp, float gridd)
    {
        this.GridScale = gridd;
        if (inp != null)
        {
            theDraggingObject = inp;
            if (saveOldStates == null)
            {
                saveOldStates = GameObject.Find("Old Rotation");

                if (saveOldStates == null)
                {
                    saveOldStates = new GameObject("Old Rotation");
                } // if
            } // if

            saveOldStates.transform.rotation = theDraggingObject.transform.rotation;
            return true;
        }
        else
        {
            return false;
        } // else 
    } // SetSetDragging

    public bool SetActiveObj(GameObject inp)
    {
        if (theActiveObject != null && inp != null)
        {
            if (theActiveObject.GetComponent<ComboParent>() != null)
            {
                theActiveObject.GetComponent<ComboParent>().InactiveAllChildren();
            }
            else if (theActiveObject.GetComponent<MouseScriptZ>() != null)
            {
                theActiveObject.GetComponent<MouseScriptZ>().InactiveAndDisableMouFu();
            } // else id
        } // if

        if (inp != null)
        {
            theActiveObject = inp;
            if (inp.GetComponent<ComboParent>() != null)
            {
                inp.GetComponent<ComboParent>().ActiveAllChildren();
            }
            else if (inp.GetComponent<MouseScriptZ>() != null)
            {
                inp.GetComponent<MouseScriptZ>().changeIsActive();
            } // else id
            return true;
        } // if
        else
        {
            return false;
        } // else 
    } // SetSetDragging


    void ForceAlign(Transform holePar, Transform rodPar, float angle, Vector3 cross)
    { // 接近時把正在拖拉的物體自動校正對準
        cross = cross.normalized;
        if (ReferenceEquals(theDraggingObject.transform, holePar.root))
        {
            Transform theRotPoint1 = theDraggingObject.transform;
            float dist = Mathf.Infinity;
            for (int a = 0; a < holePar.childCount; a++)
            {
                if (Vector3.Distance(holePar.GetChild(a).transform.position, rodPar.position) < dist &&
                 holePar.GetChild(a).tag.Contains("ParParSet1"))
                {
                    dist = Vector3.Distance(holePar.GetChild(a).transform.position, rodPar.position);
                    theRotPoint1 = holePar.GetChild(a);
                } // if
            } // for 


            theDraggingObject.transform.RotateAround(theRotPoint1.transform.position, cross, angle);
            if (IsParallelOnPlane(true, true, true) < 2)
            {
                theDraggingObject.transform.rotation = saveOldStates.transform.rotation;
                // theDraggingObject.transform.RotateAround(theDraggingObject.transform.position, cross, -angle); // y tho?
            } // if

            if (theDraggingObject.GetComponent<MouseScriptZ>() != null)
            {
                theDraggingObject.GetComponent<MouseScriptZ>().ReCalHoleOffset();
            } // if

        } // if
        else
        {
            Transform theRotPoint1 = theDraggingObject.transform;
            float dist = Mathf.Infinity;
            for (int a = 0; a < rodPar.childCount; a++)
            {
                if (Vector3.Distance(rodPar.GetChild(a).transform.position, holePar.position) < dist &&
                 rodPar.GetChild(a).tag.Contains("ParParSet1"))
                {
                    dist = Vector3.Distance(rodPar.GetChild(a).transform.position, holePar.position);
                    theRotPoint1 = rodPar.GetChild(a);
                } // if
            } // for 


            theDraggingObject.transform.RotateAround(theRotPoint1.transform.position, cross, angle);
            if (IsParallelOnPlane(true, true, true) < 2)
            {
                theDraggingObject.transform.rotation = saveOldStates.transform.rotation;
                // theDraggingObject.transform.RotateAround(theDraggingObject.transform.position, cross, -angle); // y tho? 
            } // if

            if (theDraggingObject.GetComponent<MouseScriptZ>() != null)
            {
                theDraggingObject.GetComponent<MouseScriptZ>().ReCalHoleOffset();
            } // if

        } // else         

    } // ForceAlign

    void ForceAlign2(Transform holePar, Transform rodPar, float angle, Vector3 cross)
    { // 十字軸校準
        cross = cross.normalized;
        // Debug.Log("ForceAlign2 cross vector:" + cross + "angle : " + angle);

        if (ReferenceEquals(theDraggingObject.transform, holePar.root))
        {
            Transform theRotPoint1 = theDraggingObject.transform;
            float dist = Mathf.Infinity;
            for (int a = 0; a < holePar.childCount; a++)
            {
                if (Vector3.Distance(holePar.GetChild(a).transform.position, rodPar.position) < dist &&
                 holePar.GetChild(a).tag.Contains("ParParSet1"))
                {
                    dist = Vector3.Distance(holePar.GetChild(a).transform.position, rodPar.position);
                    theRotPoint1 = holePar.GetChild(a);
                } // if
            } // for 


            theDraggingObject.transform.RotateAround(theRotPoint1.transform.position, cross, angle);
            if (IsParallelOnPlane(true, true, true) < 2)
            {
                theDraggingObject.transform.rotation = saveOldStates.transform.rotation;
                // theDraggingObject.transform.RotateAround(theDraggingObject.transform.position, cross, -angle); // y tho ? 
            } // if
            // Debug.Log("ForceAlign2 theRotPoint1 :" + theRotPoint1.name); // test

            if (theDraggingObject.GetComponent<MouseScriptZ>() != null)
            {
                theDraggingObject.GetComponent<MouseScriptZ>().ReCalHoleOffset();
            } // if
        } // if
        else
        {
            Transform theRotPoint1 = theDraggingObject.transform;
            float dist = Mathf.Infinity;
            for (int a = 0; a < rodPar.childCount; a++)
            {
                if (Vector3.Distance(rodPar.GetChild(a).transform.position, holePar.position) < dist &&
                 rodPar.GetChild(a).tag.Contains("ParParSet1"))
                {
                    dist = Vector3.Distance(rodPar.GetChild(a).transform.position, holePar.position);
                    theRotPoint1 = rodPar.GetChild(a);
                } // if
            } // for 


            theDraggingObject.transform.RotateAround(theRotPoint1.transform.position, cross, angle);
            if (IsParallelOnPlane(true, true, true) < 2)
            {
                theDraggingObject.transform.rotation = saveOldStates.transform.rotation;
                // theDraggingObject.transform.RotateAround(theRotPoint1.position, cross, -angle); // y tho ? 
            } // if


            // Debug.Log("ForceAlign2 theRotPoint1 :" + theRotPoint1.name); // test

            if (theDraggingObject.GetComponent<MouseScriptZ>() != null)
            {
                theDraggingObject.GetComponent<MouseScriptZ>().ReCalHoleOffset();
            } // if
        } // else

    } // ForceAlign2


    public int MovementRequest(Transform target)
    { // the one that has mouse on it

        bool isFuck = false;
        bool isFucked = false;
        int targetIndexInList = -1;

        // Debug.Log( target.transform.name ) ;

        if (mListFuck.Count == 0)
        {
            return 999; // 回去做貼地移動
        }
        else
        {
            for (int x = 0; x < mListFuck.Count; x++)
            {           // 找棒子碰撞名單
                if (ReferenceEquals(target, mListFuck[x].parent))
                {
                    isFuck = true;
                    targetIndexInList = x;
                    break;
                } // if
            } // for

            for (int x = 0; x < mListFucked.Count; x++)
            {               // 找洞碰撞名單
                if (ReferenceEquals(target, mListFucked[x].parent) && (targetIndexInList > x || targetIndexInList == -1))
                {
                    isFucked = true;
                    targetIndexInList = x;
                    break;
                } // if

            } // for


            // if (isFuck && isFucked)
            // {  // 找我們現在拉的是哪種東西
            //     Debug.Log("Rod and hole both true");
            // } // if
            // else if (isFuck)
            // { // 如果我們現在拉的是棒子
            //     Debug.Log(" Rod ");
            // } // else if 
            // else if (isFucked)
            // { // 如果我們現在拉的是洞
            //     Debug.Log(" Hole ");
            // } // else if 
            // else
            // {
            //     Debug.Log(" Wrong!! ");
            // } // else

            for (int i = 0; i < mListFuck.Count; i++)
            {
                YesPutIiIn = false;
                CheckTagToReachRequirement(mListFucked[i], mListFuck[i]);
                if (YesPutIiIn)
                {
                    bool hasOneIn = false;
                    for (int c = 0; c < AllTheDoables.Count; c = c + 2)
                    {
                        if (ReferenceEquals(AllTheDoables[c], mListFucked[i]) &&
                             ReferenceEquals(AllTheDoables[c + 1], mListFuck[i]))
                        {
                            hasOneIn = true;
                            break;
                        } // if
                    } // for 

                    if (!hasOneIn)
                    {
                        AllTheDoables.Add(mListFucked[i]); // even index
                        AllTheDoables.Add(mListFuck[i]);  // odd index
                    } // if

                } // if
            } // for

            for (int a = 0; a < AllTheDoables.Count; a++)
            {
                if (ReferenceEquals(target, AllTheDoables[a].root))
                {
                    return a;
                } // if
            } // for


        } // else 

        return -1;


    }  // MovementRequest

    public int MovementRequestForCombo(Transform target)
    { // the clicked comboparent

        bool isFuck = false;
        bool isFucked = false;
        int targetIndexInList = -1;

        bool oneMore = true;

        // Debug.Log( target.transform.name ) ;

        if (mListFuck.Count == 0)
        {
            return 999; // 回去做貼地移動
        }
        else
        {
            for (int x = 0; x < mListFuck.Count; x++)
            {           // 找棒子碰撞名單
                // Debug.Log(mListFuck.Count);// test
                if (ReferenceEquals(target, mListFuck[x].root))
                {
                    isFuck = true;
                    targetIndexInList = x;
                    break;
                } // if
            } // for

            for (int x = 0; x < mListFucked.Count; x++)
            {               // 找洞碰撞名單
                if (ReferenceEquals(target, mListFucked[x].root))
                {

                    if (targetIndexInList > x || targetIndexInList == -1)
                    {
                        targetIndexInList = x;
                        isFucked = true;
                    } // if

                    break;
                } // if

            } // for

            for (int i = 0; i < mListFuck.Count; i++)
            {
                YesPutIiIn = false;
                CheckTagToReachRequirement(mListFucked[i], mListFuck[i]);

                if (YesPutIiIn)
                {
                    bool hasOneIn = false;
                    for (int c = 0; c < AllTheDoables.Count; c = c + 2)
                    {
                        if (ReferenceEquals(AllTheDoables[c], mListFucked[i]) &&
                             ReferenceEquals(AllTheDoables[c + 1], mListFuck[i]))
                        {
                            hasOneIn = true;
                            break;
                        } // if
                    } // for 

                    if (!hasOneIn)
                    {
                        AllTheDoables.Add(mListFucked[i]); // even index
                        AllTheDoables.Add(mListFuck[i]);  // odd index
                    } // if

                } // if
            } // for

            for (int a = 0; a < AllTheDoables.Count; a++)
            {
                if (ReferenceEquals(target, AllTheDoables[a].root))
                {
                    return a;
                } // if
            } // for


        } // else 

        return -1;


    }  // MovementRequestForCombo


    public Transform ReLockOn(int indexx)
    {
        return AllTheDoables[indexx];
    } // ReLockOn



    string CheckTagToReachRequirement(Transform hole, Transform rod)
    {
        L1_start = null;
        L1_end = null;
        L2_start = null;
        L2_end = null;
        L2_start_temp = null;
        L2_end_temp = null;
        L1_start_temp = null;
        L1_end_temp = null;
        string reString = "";
        int correctCount = 0;

        if (hole.tag.Contains("Cross") && rod.tag.Contains("Cross"))
        {
            int assignCount = 0;

            for (int x = 0; x < hole.childCount; x++)
            { // 準備比較第一組平行(對接平行)
                Transform temp = hole.GetChild(x);
                if (temp.tag.Contains("ParParSet1"))
                {
                    assignCount++;
                    switch (assignCount)
                    {
                        case 1:
                            L1_start = temp;
                            L2_start_temp = temp;
                            break;
                        case 2:
                            L1_end = temp;
                            L2_end_temp = temp;
                            break;
                        default:
                            Debug.Log("Something went wrong in CheckTagToReachRequirement Cross ParParSet1");
                            break;

                    } // switch
                } // if
            } // for

            for (int x = 0; x < rod.childCount; x++)
            { // 準備比較第一組平行(對接平行)
                Transform temp = rod.GetChild(x);
                if (temp.tag.Contains("ParParSet1"))
                {
                    assignCount++;
                    switch (assignCount)
                    {
                        case 3:
                            L2_start = temp;
                            L1_start_temp = temp;
                            break;
                        case 4:
                            L2_end = temp;
                            L1_end_temp = temp;
                            break;
                        default:
                            Debug.Log("Something went wrong in CheckTagToReachRequirement Cross ParParSet1");
                            break;
                    } // switch
                } // if
            } // for

            float angle;
            bool AngleBig = false;
            if (ReferenceEquals(L1_end.root, theDraggingObject.transform))
            {
                angle = Vector3.Angle(L1_end.position - L1_start.position, L2_end.position - L2_start.position);
            } // if 
            else
            {
                angle = Vector3.Angle(L2_end.position - L2_start.position, L1_end.position - L1_start.position);
            } // else 
            Vector3 cross = Vector3.Cross(L1_end.position - L1_start.position, L2_end.position - L2_start.position);
            if (cross.y < 0) angle = -angle;
            if (Mathf.Abs(angle) > 45f)
            {

                AngleBig = true;

            } // if
            else
            {
                AngleBig = false;
            } // else 

            if (L2_start == null)
            {
                YesPutIiIn = false;
                return reString;
            } // if
            else if (IsParallelOnPlane(true, true, true) >= 2)
            {
                correctCount++;
                reString = "Cross";
            } // if
            else if (IsParallelOnPlane(true, true, true) < 2 && AngleBig == false)
            {
                ForceAlign(hole, rod, angle, cross);
                correctCount++;
                reString = "Cross";
            } // else if
            else
            {
                YesPutIiIn = false;
                return reString;
            } // else

            assignCount = 0;
            Debug.Log("第一組平行角度 cross : " + angle);

            for (int x = 0; x < rod.childCount; x++)
            { // 準備比較第二組平行(垂直平行)
                Transform temp = rod.GetChild(x);
                if (temp.tag.Contains("ParParSet2"))
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
                            Debug.Log("Something went wrong in CheckTagToReachRequirement Cross ParParSet2");
                            break;

                    } // switch
                } // if
            } // for

            for (int x = 0; x < hole.childCount; x++)
            { // 準備比較第二組平行(垂直平行)
                Transform temp = hole.GetChild(x);
                if (temp.tag.Contains("ParParSet2"))
                {
                    assignCount++;
                    switch (assignCount)
                    {
                        case 3:
                            L2_start = temp;
                            break;
                        case 4:
                            L2_end = temp;
                            break;
                        default:
                            Debug.Log("Something went wrong in CheckTagToReachRequirement Cross ParParSet2");
                            break;
                    } // switch
                } // if
            } // for

            if (L2_start == null)
            {
                YesPutIiIn = false;
                return reString;
            } // if
            else if (IsParallelOnPlane(true, true, true) >= 2)
            {
                correctCount++;
                reString = "Cross";
            } // if
            else
            {
                assignCount = 0;
                for (int x = 0; x < rod.childCount; x++)
                { // 準備比較第三組平行(垂直平行)
                    Transform temp = rod.GetChild(x);
                    if (temp.tag.Contains("ParParSet3"))
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
                                Debug.Log("Something went wrong in CheckTagToReachRequirement Cross ParParSet3");
                                break;
                        } // switch
                    } // if
                } // for


                if (ReferenceEquals(L1_end.root, theDraggingObject.transform))
                {
                    angle = Vector3.Angle(L1_end.position - L1_start.position, L2_end.position - L2_start.position);
                } // if 
                else
                {
                    angle = Vector3.Angle(L2_end.position - L2_start.position, L1_end.position - L1_start.position);
                } // else 

                cross = Vector3.Cross(L1_end.position - L1_start.position, L2_end.position - L2_start.position);
                if (cross.y < 0) angle = -angle;

                if (L1_start == null)
                {
                    YesPutIiIn = false;
                    return reString;
                } // if
                else if (IsParallelOnPlane(true, true, true) >= 2)
                {
                    correctCount++;
                    reString = "Cross";
                } // if
                else if (IsParallelOnPlane(true, true, true) < 2)
                {
                    ForceAlign2(hole, rod, angle, cross);
                    correctCount++;
                    reString = "Cross";
                } // else if
                else
                {
                    YesPutIiIn = false;
                    return reString;
                } // else


            } // else
        } // if
        else if (hole.tag.Contains("Round_Hole"))
        {   // 圓洞
            int assignCount = 0;
            for (int x = 0; x < hole.childCount; x++)
            { // 準備比較第一組平行(對接平行)
                Transform temp = hole.GetChild(x);
                if (temp.tag.Contains("ParParSet1"))
                {
                    assignCount++;

                    switch (assignCount)
                    {
                        case 1:
                            L1_start = temp;
                            L2_start_temp = temp;
                            break;
                        case 2:
                            L1_end = temp;
                            L2_end_temp = temp;
                            break;
                        default:
                            Debug.Log("Something went wrong in CheckTagToReachRequirement Round ParParSet1");
                            break;

                    } // switch
                } // if
            } // for

            for (int x = 0; x < rod.transform.childCount; x++)
            { // 準備比較第一組平行(對接平行)
                Transform temp = rod.transform.GetChild(x);
                if (temp.tag.Contains("ParParSet1"))
                {
                    assignCount++;
                    switch (assignCount)
                    {
                        case 3:
                            L2_start = temp;
                            L1_start_temp = temp;
                            break;
                        case 4:
                            L2_end = temp;
                            L1_end_temp = temp;
                            break;
                        default:
                            Debug.Log("Something went wrong in CheckTagToReachRequirement Round ParParSet1");
                            break;
                    } // switch
                } // if
            } // for

            float angle;
            bool AngleBig = false;
            if (ReferenceEquals(L1_end.root, theDraggingObject.transform))
            {
                angle = Vector3.Angle(L1_end.position - L1_start.position, L2_end.position - L2_start.position);
            } // if 
            else
            {
                angle = Vector3.Angle(L2_end.position - L2_start.position, L1_end.position - L1_start.position);
            } // else 

            Vector3 cross = Vector3.Cross(L1_end.position - L1_start.position, L2_end.position - L2_start.position);
            if (cross.y < 0) angle = -angle;
            if (Mathf.Abs(angle) > 45f)
            {

                AngleBig = true;

            } // if
            else
            {
                AngleBig = false;
            } // else 

            // Debug.Log("第一組平行角度 round : " + angle + AngleBig);

            if (L2_start == null)
            {
                YesPutIiIn = false;
                return reString;
            } // if
            else if (IsParallelOnPlane(true, true, true) >= 2)
            {
                correctCount++;
                reString = "Round";
            } // if
            else if (IsParallelOnPlane(true, true, true) < 2 && AngleBig == false)
            {
                ForceAlign(hole, rod, angle, cross);
                correctCount++;
                reString = "Round";
            } // else if
            else
            {
                YesPutIiIn = false;
                return reString;
            } // else 
        } // else if


        if (reString == "Cross" && correctCount >= 2)
        {
            YesPutIiIn = true;
        } // if
        else if (reString == "Round" && correctCount >= 1)
        {
            YesPutIiIn = true;
        } // else if
        else
        {
            YesPutIiIn = false;
        } // else 

        return reString;

    } // MovementRequest

    // ////////////////////////////////////  below is Paralle in 3D code //////////////////////////////////////////////////////

    int IsParallelOnPlane(bool xyPlane, bool yzPlane, bool xzPlane)
    {
        int isPara = 0;

        if (xzPlane)
        {
            //3d -> 2d
            Vector2 l1_start = new Vector2(L1_start.position.x, L1_start.position.z);
            Vector2 l1_end = new Vector2(L1_end.position.x, L1_end.position.z);

            Vector2 l2_start = new Vector2(L2_start.position.x, L2_start.position.z);
            Vector2 l2_end = new Vector2(L2_end.position.x, L2_end.position.z);

            //Direction of the lines
            Vector2 l1_dir = (l1_end - l1_start).normalized;
            Vector2 l2_dir = (l2_end - l2_start).normalized;

            //If we know the direction we can get the normal vector to each line兩條線的法線
            Vector2 l1_normal = new Vector2(-l1_dir.y, l1_dir.x);
            Vector2 l2_normal = new Vector2(-l2_dir.y, l2_dir.x);


            //Step 1: Rewrite the lines to a general form: Ax + By = k1 and Cx + Dy = k2
            //The normal vector is the A, B
            float A = l1_normal.x;
            float B = l1_normal.y;

            float C = l2_normal.x;
            float D = l2_normal.y;

            //To get k we just use one point on the line
            float k1 = (A * l1_start.x) + (B * l1_start.y);
            float k2 = (C * l2_start.x) + (D * l2_start.y);

            //Step 2: are the lines parallel? -> no solutions
            if ((l1_start == l1_end) && (l2_start != l2_end))
            {
                // Debug.Log("Its a dot for line1 On XZ Plane");
            } // if
            else if ((l1_start != l1_end) && (l2_start == l2_end))
            {
                // Debug.Log("Its a dot for line2 On XZ Plane");
            } // else if
            else if (IsParallel(l1_normal, l2_normal))
            {
                // Debug.Log("The lines are parallel on X-Z Plane!");
                isPara++;
            } // else if 

        }// if xzPlane

        //-----------------------------------------------------------------------------------------------------

        if (xyPlane)
        {
            //3d -> 2d
            Vector2 l1_start = new Vector2(L1_start.position.x, L1_start.position.y);
            Vector2 l1_end = new Vector2(L1_end.position.x, L1_end.position.y);

            Vector2 l2_start = new Vector2(L2_start.position.x, L2_start.position.y);
            Vector2 l2_end = new Vector2(L2_end.position.x, L2_end.position.y);

            //Direction of the lines
            Vector2 l1_dir = (l1_end - l1_start).normalized;
            Vector2 l2_dir = (l2_end - l2_start).normalized;

            //If we know the direction we can get the normal vector to each line
            Vector2 l1_normal = new Vector2(-l1_dir.y, l1_dir.x);
            Vector2 l2_normal = new Vector2(-l2_dir.y, l2_dir.x);


            //Step 1: Rewrite the lines to a general form: Ax + By = k1 and Cx + Dy = k2
            //The normal vector is the A, B
            float A = l1_normal.x;
            float B = l1_normal.y;

            float C = l2_normal.x;
            float D = l2_normal.y;

            //To get k we just use one point on the line
            float k1 = (A * l1_start.x) + (B * l1_start.y);
            float k2 = (C * l2_start.x) + (D * l2_start.y);

            //Step 2: are the lines parallel? -> no solutions
            if ((l1_start == l1_end) && (l2_start != l2_end))
            {
                // Debug.Log("Its a dot for line1 On XY Plane");
            } // if
            else if ((l1_start != l1_end) && (l2_start == l2_end))
            {
                // Debug.Log("Its a dot for line2 On XY Plane");
            } // else if
            else if (IsParallel(l1_normal, l2_normal))
            {
                // Debug.Log("The lines are parallel on X-Y Plane!");
                isPara++;
            }

        }// if xyPlane

        //-----------------------------------------------------------------------------------------------------

        if (yzPlane)
        {
            //3d -> 2d
            Vector2 l1_start = new Vector2(L1_start.position.y, L1_start.position.z);
            Vector2 l1_end = new Vector2(L1_end.position.y, L1_end.position.z);

            Vector2 l2_start = new Vector2(L2_start.position.x, L2_start.position.y);
            Vector2 l2_end = new Vector2(L2_end.position.x, L2_end.position.y);

            //Direction of the lines
            Vector2 l1_dir = (l1_end - l1_start).normalized;
            Vector2 l2_dir = (l2_end - l2_start).normalized;

            //If we know the direction we can get the normal vector to each line
            Vector2 l1_normal = new Vector2(-l1_dir.y, l1_dir.x);
            Vector2 l2_normal = new Vector2(-l2_dir.y, l2_dir.x);


            //Step 1: Rewrite the lines to a general form: Ax + By = k1 and Cx + Dy = k2
            //The normal vector is the A, B
            float A = l1_normal.x;
            float B = l1_normal.y;

            float C = l2_normal.x;
            float D = l2_normal.y;

            //To get k we just use one point on the line
            float k1 = (A * l1_start.x) + (B * l1_start.y);
            float k2 = (C * l2_start.x) + (D * l2_start.y);

            //Step 2: are the lines parallel? -> no solutions
            if ((l1_start == l1_end) && (l2_start != l2_end))
            {
                // Debug.Log("Its a dot for line1 On YZ Plane");
            } // if
            else if ((l1_start != l1_end) && (l2_start == l2_end))
            {
                // Debug.Log("Its a dot for line2 On YZ Plane");
            } // else if
            else if (IsParallel(l1_normal, l2_normal))
            {
                // Debug.Log("The lines are parallel on Y-Z Plane!");
                isPara++;
            } // else if

        }// if yzPlane


        return isPara;

    } //  IsParallelOnPlane()

    //Are 2 vectors parallel?
    bool IsParallel(Vector2 v1, Vector2 v2)
    {
        //2 vectors are parallel if the angle between the vectors are 0 or 180 degrees
        if (Vector2.Angle(v1, v2) == 0f || Vector2.Angle(v1, v2) == 180f)
        {
            return true;
        }

        return false;
    } // IsParallel()

    public GameObject GetActiveGroup()
    {

        if (theActiveObject.transform.childCount > 0)
            return theActiveObject;
        else
            return null;

    } // GetActiveGroup()




} // /////////////////// end class GM
