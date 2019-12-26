using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class VirtualButtonEventHandler : MonoBehaviour,IVirtualButtonEventHandler
{
    public GameObject vb;

    void Start()
    {
        Debug.Log("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        if (vbb)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!");
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnVirtualButtonPressed()
    {

    }


    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed");

    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("Release");
    }

}