using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class FixedPointCastFloatTest : TrueSyncBehaviour
{
    public override void OnSyncedStart()
    {
        FP a = 3.7897495f;

        for (int i = 0; i < 100; i++)
        {
            a = Mathf.Sin(a.AsFloat() + a.AsFloat());

            Debug.Log("a: " + a);
        }
    }

}
