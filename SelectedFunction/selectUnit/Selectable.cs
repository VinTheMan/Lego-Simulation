using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour
{
    public Material Normal, Selected;
    MeshRenderer MeshRenderer;
    float R;
    SelectingGameObject selGroup;
    TransformSetParent m_parent;
    UnitManager unitManager;
    private bool InGroup;
    public GameObject GM;

    // Use this for initialization
    void Start()
    {
        selGroup = FindObjectOfType<SelectingGameObject>();
        Normal = GetComponent<MeshRenderer>().material;
        R = UnityEngine.Random.Range(30, 120);
        MeshRenderer = GetComponent<MeshRenderer>();
        SetUnSelected();
        InGroup = false;
        if (GM == null) GM = GameObject.FindGameObjectWithTag("GM");
        unitManager = FindObjectOfType<UnitManager>();
    } // Start()

    void Update()
    {
        if (this.transform.root.name.Contains("Selecting"))
        {

        }
        else
        {
            MeshRenderer.material = Normal;
        } // else

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.root == this.transform.root)
                {
                    // dont change active
                } // if
                else
                {
                    if (this.transform.root.name.Contains("Selecting"))
                    {
                        SetUnSelected();
                        this.transform.parent = null;

                    } // if
                } // else 
            } // if
            else if (IsMouseOnUI() == false)
            {
                if (this.transform.root.name.Contains("Selecting"))
                {
                    SetUnSelected();
                    this.transform.parent = null;

                } // if 
            } // else
        } // if
    } // update


    bool IsMouseOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void SetSelected()
    {
        if (this.transform.root.GetComponent<ComboParent>() != null)
            this.transform.root.GetComponent<ComboParent>().ActiveAllChildren();
    } // SetSelected()
    public void SetUnSelected()
    {
        if (this.transform.root.GetComponent<ComboParent>() != null)
            this.transform.root.GetComponent<ComboParent>().InactiveAllChildren();
    } // SetUnSelected()

    void OnTriggerEnter(Collider other)
    {
        if (other != null && other.transform.parent != null && other.transform.parent.GetComponent<SelectTool>() != null && !InGroup)
        {
            transform.SetParent(selGroup.transform);
            SetSelected();
            GM.GetComponent<GM>().SetActiveObj(selGroup.gameObject);
        } // if
        else {
            // 零件不能組但是碰在一起了
        } // else
    } // OnTriggerEnter
    void OnTriggerExit(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.GetComponent<SelectTool>())
        {
            SetUnSelected();
            if (gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<SelectingGameObject>() != null)
                gameObject.transform.parent = null;
        } // if
    } // OnTriggerExit()

    public void SetIsInGroup(bool _isGroup)
    {
        InGroup = _isGroup;
    } // SetIsInGroup

}

