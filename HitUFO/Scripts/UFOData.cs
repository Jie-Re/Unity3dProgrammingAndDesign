using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOData : MonoBehaviour
{
    public int score = 1;                               //射击此飞碟得分
    public Color color = Color.green;                   //飞碟颜色
    public Vector3 direction;                           //飞碟初始的位置
    public Vector3 scale = new Vector3(1, 0.25f, 1);   //飞碟大小
}
