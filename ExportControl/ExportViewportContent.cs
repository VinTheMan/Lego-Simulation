using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class ExportViewportContent : MonoBehaviour {

    private ViewportTarget c_target ;
    public Text showText;
    List<ExportToggleScript> listToggle;

    private int indexOfStep;
    private List<ListOfStep> listOfStep;
    private CameraGetPrtSc camera ;
    private bool picing ;
    private int picingIndex ;

    private List<GroupDropDownScript> listOfGroup ;
    private int groupCount ;

    private CameraViewChange cameraViewChangeCall ;

    private bool readyToSave ;

    private bool optionApplyDone ;
    private ViewControler viewControler ; // must call in Apply ;

    private NetworkControl networkControl ;

    private void Awake() {

        c_target = FindObjectOfType<ViewportTarget>() ;

        camera = FindObjectOfType<CameraGetPrtSc>();
        if (camera == null)
            Debug.Log("cameraGetPrtSc In ExportViewport not exist.");

        listToggle = new List<ExportToggleScript>();
        indexOfStep = 0;
        listOfStep = new List<ListOfStep>();
        updateText() ;

        listOfGroup = new List<GroupDropDownScript>() ;

        cameraViewChangeCall = FindObjectOfType<CameraViewChange>() ;
        
        groupCount = 0 ;
        readyToSave = true ;

        viewControler = FindObjectOfType<ViewControler>().gameObject.GetComponent<ViewControler>() ;

        networkControl = FindObjectOfType<NetworkControl>() ;

    } // Awake() 



    public void LoadStepFromUnitM(SaveDataStep _save ) {

        for ( int i = 0 ; i < _save.content.Length; i++ ) {

            for ( int j = 0 ; j < listToggle.Count ; j++ )
                listToggle[j].TurnOnToggle(false); // turn all toggle off ;


            ListOfStep tempStep = new ListOfStep();
            tempStep.contents = new List<int>();
            
            for ( int nowStep = _save.step[i] ; i < _save.content.Length ; i++ ) {

                listToggle[_save.content[i]].TurnOnToggle(true);
                Debug.Log("Turn On" + _save.content[i]);
                if ( i + 1 < _save.content.Length && nowStep != _save.step[i + 1]) {
                    break ; 
                } // if

            } // for
            SaveStep() ;
            
            Debug.Log("LoadOneStep");
            ClickNextStep();

        }


    } // LoadStepFromUnitM()

    public List<ListOfStep> GetAllStepForSave() {

        return listOfStep ;

    } // 

    public void PicAll() {

        picing = true ;
        indexOfStep = 0 ;
        picingIndex = 0 ;

    } // PicAll()

    private void LateUpdate() {
       
        if ( picing ) {

            
            if ( camera.ListenWaitingBool() ) {
                SetObjTrueForViewWithIndex(picingIndex);
                camera.ScreenShotCall();
                Debug.Log("Pic" + picingIndex);
                picingIndex++ ;
            } // if 

        
        } // if

        if ( picingIndex == listOfStep.Count ) {
            picing = false ;
        } // if

    } // update()

    public void SetObjTrueForViewWithIndex( int stepNum ) {

        viewControler.Btn_StartViewStep(stepNum) ;

        /*
        // old
        for ( int i = 0 ; i < listToggle.Count ; i++ )
            listToggle[i].SetExporting(false);

        for ( int i = 0; i < listOfStep[stepNum].contents.Count ; i++ ) {

            for ( int j = 0 ; j < listToggle.Count; j++ ) {

                if (j == listOfStep[stepNum].contents[i]) {

                    cameraViewChangeCall.MoveCameraToTheNearestPoint(listToggle[j].returnGameObject().transform);
                    listToggle[j].SetExporting(true);
                    
                }
               

            } // for


        } // for
        */

    } // SetObjTrueForViewWithIndex

    private void updateText() {
        showText.text = (indexOfStep + 1).ToString();
        Debug.Log( "Now editing " + (indexOfStep+1) + "steps" );
    } // updateText() ;

    public void ClickNextStepForView() {

        if (indexOfStep < listOfStep.Count) {
            indexOfStep = indexOfStep + 1;
            updateText();
        } // if
        StepToggleUpdateForView();

    } // ClickNextStep() 

    public void ClickPreStepForView() {

        if (indexOfStep > 0) {
            indexOfStep = indexOfStep - 1;
            updateText();
        } // if
        StepToggleUpdateForView();

    } // ClickPreStep()

    public void ClickNextStep() {

        if (indexOfStep < listOfStep.Count) {
            indexOfStep = indexOfStep + 1 ;
            updateText() ;
        } // if
        StepToggleUpdate() ;

    } // ClickNextStep() 

    public void ClickPreStep() {

        if (indexOfStep > 0 ) {
            indexOfStep = indexOfStep - 1;
            updateText();
        } // if
        StepToggleUpdate() ;

    } // ClickPreStep()

    private void StepToggleUpdate() {

        if ( indexOfStep >= 0 && indexOfStep < listOfStep.Count ) {

            for ( int i = 0 ; i < listToggle.Count ; i++ ) {

                listToggle[i].TurnOnToggle(false) ;

            } // for

            for ( int i = 0 ; i < listOfStep[indexOfStep].contents.Count ; i++ ) {

                listToggle[listOfStep[indexOfStep].contents[i]].TurnOnToggle(true) ;

            } // for

        } // if

    } // StpToggleUpdate()

    private void StepToggleUpdateForView()
    {

        if (indexOfStep >= 0 && indexOfStep < listOfStep.Count)
        {

            for (int i = 0; i < listToggle.Count; i++)
            {

                listToggle[i].returnGameObject().GetComponent<Transparent>().setTransparent() ;

            } // for

            for (int i = 0; i < listOfStep[indexOfStep].contents.Count; i++)
            {
                listToggle[listOfStep[indexOfStep].contents[i]].returnGameObject().GetComponent<Transparent>().unsetTransparent();
                // listToggle[listOfStep[indexOfStep].contents[i]].TurnOnToggle(true);

            } // for

        } // if

    } // StpToggleUpdate()

    public void ClickSaveStep() {
        
        bool groupHaveJob = false ;

        for ( int i = 0 ; i < listOfGroup.Count ; i++ ) {

            if ( listOfGroup[i].GetGroupType() < 3 ) {


                if (listOfGroup[i].GetGroupType() == 0)
                    TypeGroup_0(listOfGroup[i].GetinGroupToggleListIndex());
                else if (listOfGroup[i].GetGroupType() == 1)
                    TypeGroup_1(listOfGroup[i].GetinGroupToggleListIndex());
                else if (listOfGroup[i].GetGroupType() == 2)
                    TypeGroup_2(listOfGroup[i].GetinGroupToggleListIndex());
                
                else
                    Debug.LogError("Unexpected SaveStep in grouptype") ;

                groupHaveJob = true ;

            } // if

        } // for

        

        if (!groupHaveJob) {

            SaveStep() ;

        } // if

        DebugLogInIndex() ;

    } // ClickSaveStep() 

    private void SaveStep() {

        Debug.Log("SaveOneStep");
        ListOfStep tempStep = new ListOfStep();
        tempStep.contents = new List<int>();

        for (int i = 0; i < listToggle.Count; i++)
        {

            if (listToggle[i].CheckToggleIsOn())
            {

                tempStep.contents.Add(i);

            } // if

        } // for

        if (listOfStep.Count <= indexOfStep)
        {

            listOfStep.Insert(indexOfStep, tempStep);
        } // if
        else
        {
            // this step already exist, instead origin step in the list ;
            listOfStep.RemoveAt(indexOfStep);
            listOfStep.Insert(indexOfStep, tempStep);
        } // else

        readyToSave = true ;
        ClickNextStep();

    }

    public void DebugLogInIndex() {

        for ( int i = 0 ; i < listOfStep.Count; i++ ) {

            for ( int j = 0 ; j < listOfStep[i].contents.Count ; j++ ) {

                Debug.Log(i + "-" + listOfStep[i].contents[j]) ;

            } // for-j

        } // for-i

    } // DebugLogInIndex

    public void AddToggleLog( GameObject lego ) {

        Object obj = Resources.Load("toggleLog");
        GameObject newToggle = Instantiate(obj) as GameObject ;
        newToggle.name = "toggleLog_" + lego.name + ":" + listToggle.Count + "th" ;
        ExportToggleScript toggle = newToggle.GetComponent<ExportToggleScript>() ;

        toggle.GameObjectContain( lego ) ;
        toggle.NumSet(listToggle.Count) ;
        
        listToggle.Add(toggle) ;

    } // AddToggleLog()

    public void DeleteToggleLogInList( ExportToggleScript toggle) {

        for ( int i = 0; i < listToggle.Count ; i++ ) {

            if ( listToggle[i] == toggle ) {

                listToggle.RemoveAt(i) ;
                Debug.Log("Remove toggle" + i + "Sucess.") ;
                break ;

            } // if 

        } // for

        // for debug & reset
        for (int i = 0; i < listToggle.Count; i++) {
            Debug.Log(i) ;
            listToggle[i].NumSet(i) ;

        } // for


    } // DeleteToggleLogInList()

    public void AddGroupDropdown( GameObject _group ) {

        Object obj = Resources.Load("GroupDropdown") ;
        GameObject newGroupDropdown = Instantiate(obj) as GameObject ;
        newGroupDropdown.name = "groupDropdown_Group:" + groupCount + "th" ;
        GroupDropDownScript gdds = newGroupDropdown.GetComponent<GroupDropDownScript>() ;

        gdds.SetGroupInfo(groupCount, _group);
        groupCount++ ;

        listOfGroup.Add(gdds) ;

    } // AddGroupDropdown()

    public void DeleteGroupToggleLogInList( GroupDropDownScript toggle ) {

        for (int i = 0; i < listOfGroup.Count; i++) {

            if (listOfGroup[i] == toggle) {

                listOfGroup.RemoveAt(i);
                Debug.Log("Remove group toggle" + i + "Sucess.");
                break;

            } // if 

        } // for

        // for debug
        for (int i = 0; i < listOfGroup.Count; i++) {
            Debug.Log(i);
            listOfGroup[i].SetGroupNum(i);
        } // for

    } // DeleteToggleLogInList()

    public void AddGroupDropdownWithType(GameObject _group, int typeValue) {

        Object obj = Resources.Load("GroupDropdown");
        GameObject newGroupDropdown = Instantiate(obj) as GameObject;
        newGroupDropdown.name = "groupDropdown_Group:" + groupCount + "th";
        GroupDropDownScript gdds = newGroupDropdown.GetComponent<GroupDropDownScript>();

        gdds.SetGroupInfo(groupCount, _group);
        gdds.SetType(typeValue);
        groupCount++;

        listOfGroup.Add(gdds);

    } // AddGroupDropdown()

    public List<int> GetTheStepContent( int _step ) {
        return listOfStep[_step].contents ;
    } // CallToggleGroup() 
    public List<int> GetTheStepTransContent( int _step ) {
        return listOfStep[_step].transparentContents ;
    }

    public int GetListOfStepMax() {
        return listOfStep.Count ;
    } // GetListOfStepMax()
    public List<ExportToggleScript> GetAllToggle() {
        return listToggle ;
    }


    public int NumFindTheIndex( int _num ) {

        return listToggle[_num].transform.GetSiblingIndex() ;

    }

    public void NumIndexAddOne( int _num ) {

        listToggle[_num].transform.SetSiblingIndex(listToggle[_num].transform.GetSiblingIndex() + 1);

    }

    public ExportToggleScript GetToggleByNum( int _Num ) {

        return listToggle[_Num] ;

    } 

    public void ReduceGroupCount() {
        groupCount = groupCount - 1 ;
    } // 

    public void ResetLayout() {
        

        for (int i = 0; i < listToggle.Count; i++) {

            for ( int j = i + 1; j < listToggle.Count; j++ ) {

                if ( listToggle[i].transform.localPosition.y < listToggle[j].transform.localPosition.y ) {
                    ExportToggleScript temp = listToggle[i] ;
                    listToggle[i] = listToggle[j] ;
                    listToggle[j] = temp ;

                } // if

            } // for

        } // for

        for ( int i = 0; i < listToggle.Count; i++ ) {

            listToggle[i].transform.SetAsLastSibling(); 

        } // for

        // Debug for
        for ( int i = 0 ; i < listToggle.Count ; i++ ) {
            Debug.Log( i + "th-" + listToggle[i].transform.GetSiblingIndex() + " " + listToggle[i].name ) ;
        }


        c_target.gameObject.SetActive(false);
        c_target.gameObject.SetActive(true);


    } // ResetLayout()

    public void OptionApply() {

        listOfStep = new List<ListOfStep>() ;

        List<int> skiplist = new List<int>() ; // if someone have been done in groupAll-type, it need skip ;

        for (int i = 0; i < listToggle.Count; i++) {

            // first, check skip list ;
            bool needskip = false ;
            for ( int sk = 0; sk < skiplist.Count ; sk++ ) {
                if (i == skiplist[sk])
                    needskip = true ; 
            } // for

            // 
            if (!needskip) {
                ListOfStep newStep = new ListOfStep();
                newStep.contents = new List<int>();
                newStep.transparentContents = new List<int>();

                newStep.contents.Add(i);
                 
                if (listOfGroup[listToggle[i].GetGroupNum()].GetGroupType() == 0) {
                    Debug.Log("Work0,single");
                    for (int j = 0; j < listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex().Count; j++) {

                        if (listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j] < i)
                            newStep.transparentContents.Add(listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j]);

                    } // for

                } // if
                else if (listOfGroup[listToggle[i].GetGroupNum()].GetGroupType() == 1) {
                    Debug.Log("Work1,groupAll");
                    for (int j = 0; j < listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex().Count; j++) {

                        newStep.contents.Add(listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j]);
                        skiplist.Add(listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j]);

                    } // for

                } // else if 
                else if (listOfGroup[listToggle[i].GetGroupNum()].GetGroupType() == 2) {
                    Debug.Log("Work2,mix");
                    for (int j = 0; j < listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex().Count; j++) {

                        if (listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j] < i) {
                            newStep.contents.Add(listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j]);
                            //newStep.transparentContents.Add(listOfGroup[listToggle[i].GetGroupNum()].GetinGroupToggleListIndex()[j]);
                        }

                    } // for

                } // else if 

                listOfStep.Add(newStep);
            }

        } // for

        ListOfStep finalStep = new ListOfStep();
        finalStep.contents = new List<int>();
        finalStep.transparentContents = new List<int>() ;
        for ( int i = 0; i < listToggle.Count; i++ ) {
            finalStep.contents.Add(i) ; 
        } // for
        listOfStep.Add(finalStep) ;

        DebugLogInIndex() ;
        viewControler.StartFromExpVCall() ;

    } // OptionApply()





    private void TypeGroup_2(List<int> _g)
    {
        Debug.Log("T2 mix");
        for (int j = 0; j < _g.Count; j++)
        {

            listToggle[_g[j]].TurnOnToggle(false);

        } // for

        for (int i = 0; i < _g.Count; i++)
        {

            if (readyToSave)
            {
                readyToSave = false;
                listToggle[_g[i]].TurnOnToggle(true);
                SaveStep();

            } // if
            else
                i--;

        } // for

    } // Type0Group()

    private void TypeGroup_1(List<int> _g)
    {
        Debug.Log("T1 all");

        for (int j = 0; j < listToggle.Count; j++) {

            listToggle[j].TurnOnToggle(false);

        } // for
        for (int i = 0; i < _g.Count; i++) {


            listToggle[_g[i]].TurnOnToggle(true);

        } // for

        SaveStep();


    } // Type1Group

    private void TypeGroup_0(List<int> _g) {
        Debug.Log("T0 solo");
        for (int i = 0; i < _g.Count; i++) {

            if (readyToSave) {

                for (int j = 0; j < listToggle.Count; j++) {

                    listToggle[j].TurnOnToggle(false);

                } // for

                readyToSave = false;
                listToggle[_g[i]].TurnOnToggle(true);
                SaveStep();

            } // if
            else
                i--;

        } // for

    } // Type0Group()

    public void OnAll() {

        for (int j = 0; j < listToggle.Count; j++)
        {

            listToggle[j].TurnOnToggle(true);

        } // for

    }

    public GroupDropDownScript GetGroupDropDownByNum( int _i ) {
        return listOfGroup[_i] ;
    }


}
