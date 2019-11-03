using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoompiCollide : MonoBehaviour
{
    public int sign = 0;
    private float distance_limit;//王一博被私生饭发现的距离上限
    Controller sceneController;
    private void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentSceneController as Controller;
        Debug.Log(distance_limit);
    }

    private void Update()
    {
        distance_limit = 15;
        float distance = Vector3.Distance(this.gameObject.transform.position, sceneController.wyb.transform.position);

        if (distance <= 5)
        {
            //王一博被私生饭怼脸拍
            sceneController.wyb.GetComponent<Animator>().SetTrigger("death");
            this.gameObject.GetComponent<Animator>().SetTrigger("attack");
            Singleton<GameEventManager>.Instance.PlayerGameover();
        }
        else if (distance < distance_limit)
        //else if (distanceX < distance_limit || distanceZ < distance_limit)
        {
            //王一博进入私生饭蹲点范围
            sceneController.wall_sign = this.gameObject.GetComponent<SoompiData>().sign;//标记王一博所属区域
            this.gameObject.GetComponent<SoompiData>().follow_player = true;
            this.gameObject.GetComponent<SoompiData>().player = sceneController.wyb;
        }
        else
        {
            //王一博不在私生饭蹲点范围
            this.gameObject.GetComponent<SoompiData>().follow_player = false;
            this.gameObject.GetComponent<SoompiData>().player = null;
        }
    }
}
