using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoompiActionManager : SSActionManager
{
    private SoompiAction go_soompi;

    public void SoompiMove(GameObject soompi)
    {
        go_soompi = SoompiAction.GetSSAction(soompi.transform.position);
        this.RunAction(soompi, go_soompi, this);
    }

    //停止所有动作
    public void DestroyAllAction()
    {
        DestroyAll();
    }
}
