using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


[ExecuteInEditMode]
public class camImpulse : MonoBehaviour
{
    Unity.Cinemachine.CinemachineCollisionImpulseSource impulse;
    public bool CreatImpluse;
    void Start()
    {
        impulse = GetComponent<CinemachineCollisionImpulseSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CreatImpluse)
        {
            impulse.GenerateImpulse();
        }
        
    }
}
