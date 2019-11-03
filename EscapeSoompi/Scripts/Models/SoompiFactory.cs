using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoompiFactory : MonoBehaviour
{
    private GameObject soompi_prefab = null;//私生饭预制体
    private List<GameObject> on_soompies = new List<GameObject>();//正在蹲点的私生饭
    private Vector3[] vec = new Vector3[9];//保存每个私生饭的初始位置
    private int map_size = 40;
    private int pos = 15;

    public Controller sceneController;
    
    public List<GameObject> GetSoompies()
    {
        int index = 0;
        //生成不同私生饭的初始位置
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j ++)
            {
                vec[index] = new Vector3(pos + map_size * j, 9, pos + map_size * i + 20);
                index++;
            }
        }
        for (int i = 0; i < 9; i++)
        {
            soompi_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Soompi"));
            soompi_prefab.transform.position = vec[i];
            soompi_prefab.GetComponent<SoompiData>().sign = i + 1;
            soompi_prefab.GetComponent<SoompiData>().start_position = vec[i];
            //Debug.Log(soompi_prefab.GetComponent<SoompiData>().start_position);
            on_soompies.Add(soompi_prefab);
        }
        return on_soompies;
    }

    public void StopSoompi()
    {
        //切换所有私生饭的动画
        for (int i = 0; i < on_soompies.Count; i++)
        {
            //on_soompies[i].GetComponent<Animator>().SetBool("walk", true);
            on_soompies[i].GetComponent<Animator>().SetBool("run", false);
        }
    }

}
