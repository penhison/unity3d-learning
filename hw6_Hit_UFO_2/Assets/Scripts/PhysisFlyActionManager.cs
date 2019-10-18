using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysisFlyActionManager : SSActionManager
{

    public PhysisUFOFlyAction fly;                            //飞碟飞行的动作

    protected void Start()
    {
    }
    //飞碟飞行
    public void UFOFly(GameObject disk, float angle, float power)
    {
        fly = PhysisUFOFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }
}
