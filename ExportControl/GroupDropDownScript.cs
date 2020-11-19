using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using UnityEngine.EventSystems;

public class GroupDropDownScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private GameObject groupContainParent ;
    private Dropdown dropdownContainer ;
    public GameObject showNowType0 ;
    public GameObject showNowType1 ;
    public GameObject showNowType2 ;

    List<int> inGroupToggleIndexList ;

    private ViewportTarget2 target;
    private ExportViewportContent expV ;
    private int groupNum ;
    private int value ;

    public Text label ;

    // public ExportViewportContent expV ;

    private void Start() {

        dropdownContainer = gameObject.GetComponentInChildren<Dropdown>() ;
        if (dropdownContainer == null)
            Debug.LogError("Dropdown Container is Null, Check the script again.") ;


        target = FindObjectOfType<ViewportTarget2>();
        if (target == null)
            Debug.LogError("target not found!");

        expV = FindObjectOfType<ExportViewportContent>() ;


        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        

    } // Start()

    private void Update() {

        if (groupContainParent == null) {

            expV.GetComponent<ExportViewportContent>().ReduceGroupCount() ;
            expV.GetComponent<ExportViewportContent>().DeleteGroupToggleLogInList(gameObject.GetComponent<GroupDropDownScript>()) ;
            Debug.Log("Delete Group num:" + groupNum) ;

            Destroy(gameObject);

        } // if

    } // Update()

    public int GetGroupType() {

        return value;

    } // NeedSaveIntoStepForGroup()

    public List<int> GetinGroupToggleListIndex(){

        return inGroupToggleIndexList;

    } // GetinGroupToggleList()


    public void SetGroupInfo( int _groupNum, GameObject _contain ) {

        groupNum = _groupNum ;
        groupContainParent = _contain ;

        SetinGroupList() ;
        label.text = "Group" + (_groupNum+1) ; // start from 0, but show 1 ;

        // SetTheOrder();
        // InvisibleFromExpV() ;
        SetChildGroupNum() ;

    } // SetGroupInfo()

    public void SetGroupNum( int _groupNum ) {
        groupNum = _groupNum;
        SetinGroupList();
        label.text = "Group" + (_groupNum + 1);
        SetChildGroupNum();
    } // 

    public void SetType( int type ) {

        value = type ;
        // dropdownContainer.value = type ;
        ChangeImageSrcLoad() ;

    } // SetType

    private void SetChildGroupNum() {

        expV = FindObjectOfType<ExportViewportContent>();
        for (int i = 0; i < groupContainParent.transform.childCount; i++) {

            expV.GetToggleByNum(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum()).SetInWhichGroup(groupNum) ;

        } // for


    }


    private void InvisibleFromExpV() {

        
        for ( int i = 0 ; i < groupContainParent.transform.childCount ; i++ ) {

            expV.GetToggleByNum(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum()).transform.parent = gameObject.transform ; 

        } // for

    } // InvisibleFromExpV()

    private void VisibleFromExpV() {
        // Redo or DecomposeGroup ;

        
        for ( int i = 0 ; i < groupContainParent.transform.childCount ; i++ ) {

            expV.GetToggleByNum(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum()).transform.parent = target.transform ;

        }

    } // VisibleFromExpV()

    private void SetTheOrder() {
        /*
        int findfirstIndex = 99 ;
        ExportViewportContent expV = FindObjectOfType<ExportViewportContent>() ;

        Debug.Log("Before - Group SiblingIndex :" + this.transform.GetSiblingIndex());

        for (int i = 0; i < groupContainParent.transform.childCount ; i++ ) {

            Debug.Log(expV.NumFindTheIndex(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum()));
            if (expV.NumFindTheIndex(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum()) < findfirstIndex)
                findfirstIndex = expV.NumFindTheIndex(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum());
            

        } // for


        
        if ( findfirstIndex == 0 )
            gameObject.transform.SetSiblingIndex( 0 ) ;
        else
            gameObject.transform.SetSiblingIndex(findfirstIndex - 1);

        Debug.Log("After - Group SiblingIndex :" +  this.transform.GetSiblingIndex() ) ;

        for (int i = 0; i < groupContainParent.transform.childCount; i++) {

            expV.NumIndexAddOne(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum());
            Debug.Log(expV.NumFindTheIndex(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum()));

        } // for
        */

    } // SetTheOrder()

    private void SetinGroupList() {

        inGroupToggleIndexList = new List<int>();

        for ( int i = 0 ; i < groupContainParent.transform.childCount ; i++ ) {
            
            inGroupToggleIndexList.Add(groupContainParent.transform.GetChild(i).GetComponent<UnitContainToggle>().GetToggleNum());
            
        } // for

    } // SetinGroupList()


    public void ChangeImageSrc() {

        if ( dropdownContainer.value == 0 ) {

            showNowType0.SetActive(true) ;
            showNowType1.SetActive(false) ;
            showNowType2.SetActive(false) ;
            
        } // if
        else if ( dropdownContainer.value == 1 ) {

            showNowType0.SetActive(false);
            showNowType1.SetActive(true);
            showNowType2.SetActive(false);
            
        } // else if
        else if ( dropdownContainer.value == 2 ) {

            showNowType0.SetActive(false);
            showNowType1.SetActive(false);
            showNowType2.SetActive(true);
            
        } // else if
        else {
            Debug.Log("GroupDropDownScript Value is Unexpected.");
        } // else

        value = dropdownContainer.value ;

    } // ChangeImageSrc()

    public void ChangeImageSrcLoad() {

        if (value == 0)
        {

            showNowType0.SetActive(true);
            showNowType1.SetActive(false);
            showNowType2.SetActive(false);

        } // if
        else if (value == 1)
        {

            showNowType0.SetActive(false);
            showNowType1.SetActive(true);
            showNowType2.SetActive(false);

        } // else if
        else if (value == 2)
        {

            showNowType0.SetActive(false);
            showNowType1.SetActive(false);
            showNowType2.SetActive(true);

        } // else if
        else
        {
            Debug.Log("GroupDropDownScript Value is Unexpected.");
        } // else


    } // ChangeImageSrc()

    public void OnPointerEnter(PointerEventData eventData) {

        groupContainParent.GetComponent<ComboParent>().ActiveAllChildren() ;

    }

    public void OnPointerExit(PointerEventData eventData) {

        groupContainParent.GetComponent<ComboParent>().InactiveAllChildren() ;

    }

}
