using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFactory : MonoBehaviour
{
    private GameObject coin_prefab = null;//金币
    private List<GameObject> freeCoins = new List<GameObject>();//未被获取的金币
    private float range = 10;//金币生成的坐标范围(在一个格子内）
    public Controller sceneController;

    public List<GameObject> GetCoins()
    {
        for (int i = 0; i < 9; i ++)
        {
            int item_type = Random.Range(1, 6);
            switch (item_type)
            {
                case 1:
                    coin_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Coin"));
                    coin_prefab.GetComponent<CoinData>().score = 2;
                    break;
                case 2:
                    coin_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Box"));
                    coin_prefab.GetComponent<CoinData>().score = 0;
                    break;
                case 3:
                    coin_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Bottle_green"));
                    coin_prefab.GetComponent<CoinData>().score = 5;
                    break;
                case 4:
                    coin_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Bottle_blue"));
                    coin_prefab.GetComponent<CoinData>().score = 1;
                    break;
                case 5:
                    coin_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Bottle_red"));
                    coin_prefab.GetComponent<CoinData>().score = 3;
                    break;
            }
            //coin_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/Coin"));
            float ranx = Random.Range(-range, range);
            float ranz = Random.Range(-range, range);
            float posx = ranx + (i + 1) % 3 * 20;
            float posz = ranz + ((i + 1) / 3 + 1) * 20;
            coin_prefab.transform.position = new Vector3(posx, 0, posz);
            //Debug.Log(coin_prefab.transform.position);
            freeCoins.Add(coin_prefab);
        }
        return freeCoins;
    }
}
