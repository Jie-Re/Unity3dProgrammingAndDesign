using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class VBEventHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject vb;
    public Animator ani;

    void Start()
    {
        VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        if (vbb)
        {
            Debug.Log("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        ani.SetTrigger("jump");
        Debug.Log("OnButtonPressed$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        ani.SetTrigger("idle");
        Debug.Log("Release######################################################");
    }

}