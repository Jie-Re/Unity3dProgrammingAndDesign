using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneActionManager : SSActionManager  //本游戏管理器
{
    private SSMoveToAction moveBoatToEndOrStart;     //移动船到结束岸，移动船到开始岸
    private SequenceAction moveRoleToLandorBoat;     //移动角色到陆地，移动角色到船上

    public Controller sceneController;

    protected void Start()
    {
        sceneController = (Controller)SSDirector.GetInstance().CurrentSceneController;
        sceneController.actionManager = this;
    }
    public void moveBoat(GameObject boat, Vector3 target, float speed)
    {
        moveBoatToEndOrStart = SSMoveToAction.GetSSAction(target, speed);
        this.RunAction(boat, moveBoatToEndOrStart, this);
    }

    public void moveRole(GameObject role, Vector3 middle_pos, Vector3 end_pos, float speed)
    {
        SSAction action1 = SSMoveToAction.GetSSAction(middle_pos, speed);
        SSAction action2 = SSMoveToAction.GetSSAction(end_pos, speed);
        moveRoleToLandorBoat = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });
        this.RunAction(role, moveRoleToLandorBoat, this);
    }
}
