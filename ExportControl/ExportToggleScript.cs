using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using UnityEngine.EventSystems;

public class ExportToggleScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private GameObject containGameObject ;
    private ExportViewportContent viewportContent ;
    private ViewportTarget target ;
    private bool setupReady ;
    private Toggle toggle ;
    private Text childLabel ;
    private int logNum ;
    private int groupNum ;
    
    public GameObject returnGameObject()
    {
        return containGameObject ;
    }

    private void Awake() {
        viewportContent = FindObjectOfType<ExportViewportContent>();
        if (viewportContent == null)
            Debug.LogError("viewportContent not found!");

        target = FindObjectOfType<ViewportTarget>();
        if (target == null)
            Debug.LogError("ViewportTarget not found!");

        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        setupReady = false ;
        toggle = GetComponent<Toggle>() ;
        childLabel = GetComponentInChildren<Text>();
        
        

    }

    public void GameObjectContain( GameObject _gameObject ) {

        containGameObject = _gameObject ;
        childLabel.text = _gameObject.name ;
        setupReady = true ;

    }
    public void NumSet( int _num ) {

        logNum = _num ;
        containGameObject.GetComponent<UnitContainToggle>().SetToggleConnectToUnit(logNum) ;

    }

    public int GetNum(){
        return logNum ; 
    }

    private void Update() {
        
        if ( setupReady && containGameObject == null ) {
      
            viewportContent.DeleteToggleLogInList(gameObject.GetComponent<ExportToggleScript>());
            Destroy(gameObject);
            setupReady = false ;
        }
        if ( toggle.isOn && containGameObject != null ) {
            containGameObject.SetActive(true) ;
        }
        else if ( !toggle.isOn && containGameObject != null ) {
            containGameObject.SetActive(false);
        }


    } // Update() 

    public bool CheckToggleIsOn() {
        return toggle.isOn ;
    } // CheckToggleIsOn()

    public void TurnOnToggle( bool _b ) {
        toggle.isOn = _b ;
    } // TurnOnToggle()

    public void SetExporting( bool tf ) {
        toggle.isOn = tf ;
    } // SetExporting() 

    public void OnPointerEnter(PointerEventData eventData) {

        containGameObject.GetComponent<MouseScriptZ>().changeIsActiveSingle();

    }

    public void OnPointerExit(PointerEventData eventData) {

        containGameObject.GetComponent<MouseScriptZ>().InactiveAndDisableMouFuSingle();

    }

    public void SetInWhichGroup(int _i) {

        groupNum = _i ;
        for ( int i = 0 ; i < childLabel.text.Length; i++ ) {

            if (childLabel.text.ToCharArray()[i] == '(') {
                string strtemp ;

                strtemp = childLabel.text.Substring(0, i) ;

                childLabel.text = strtemp ;
                break ;
            } // if
                
        } // for

        childLabel.text = childLabel.text + "( " + (groupNum+1) + " )" ; 

    } 

    public int GetGroupNum() {
        return groupNum ;
    } 

}
