using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoompiData : MonoBehaviour
{
    public int sign;                      //标志私生饭在哪一块区域
    public bool follow_player = false;    //是否跟随
    public int wall_sign = -1;            //当前王一博所在区域标志
    public GameObject player;             //跟踪对象
    public Vector3 start_position;        //当前私生饭初始位置
}