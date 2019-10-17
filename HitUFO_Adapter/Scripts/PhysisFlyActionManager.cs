using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysisFlyActionManager : SSActionManager
{
    private PhysisFlyAction fly;//飞碟飞行的动作
    //public Controller sceneController;//当前场景的场景控制器

    protected void Start()
    {
        //sceneController = (Controller)SSDirector.GetInstance().CurrentSceneController;
        //sceneController.action_manager = this;
    }

    //飞碟飞行
    public void UFOFly(GameObject disk, float angle, float power)
    {
        fly = PhysisFlyAction.GetSSAction(disk.GetComponent<UFOData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }
}
