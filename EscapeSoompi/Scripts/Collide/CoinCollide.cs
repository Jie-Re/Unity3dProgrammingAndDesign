using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollide : MonoBehaviour
{
    Controller sceneController;
    private void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentSceneController as Controller;
    }

    private void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, sceneController.wyb.transform.position) < 2 && this.gameObject.activeSelf)
        {
            //王一博营业挣到金币
            sceneController.AddScore(this.gameObject.GetComponent<CoinData>().score);
            this.gameObject.SetActive(false);
            Singleton<GameEventManager>.Instance.ReduceCoinNum();
        }
    }
}
