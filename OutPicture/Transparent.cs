using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour {

    public Material m_Normal, m_Transparent ;
    MeshRenderer MeshRenderer ;

    // Use this for initialization
    void Start () {
        m_Normal = GetComponent<MeshRenderer>().material;
        MeshRenderer = GetComponent<MeshRenderer>();
        unsetTransparent() ;
    }
	
    public void setTransparent() {

        MeshRenderer.material = m_Transparent ;

    } // setTransparent() 

    public void unsetTransparent() {

        MeshRenderer.material = m_Normal ;

    } // unsetTransparent()

}
