using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[ExecuteInEditMode]

public class VEGstep : MonoBehaviour
{
    public VisualEffect VFX;
    public bool STEP=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(STEP)
        {
            VFX.SendEvent("ONSTOP");
        }
    }
}
