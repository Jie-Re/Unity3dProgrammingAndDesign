# 智能巡逻兵
## 游戏设计要求
- 创建一个地图和若干巡逻兵（使用动画）
- 每个巡逻兵走一个3-5个边的凸多边形，位置数据是相对地址。即每次确定下一个目标位置，用自己当前位置为原点计算
- 巡逻兵碰撞到障碍物，则会自动选下一个点为目标
- 巡逻兵在设定范围内感知到玩家 ，会自动追击玩家
- 失去玩家目标后，继续巡逻
- 计分：玩家每次甩掉一个巡逻兵计一分，与巡逻兵碰撞游戏结束

## 程序设计
- 使用MVC架构（前面的实验博客中已有详细说明，此处不再赘述）
- 使用订阅与发布模式传消息：`CoinCollide`和`SoompiCollide`将“碰撞”消息传递给`GameEventManager`和`Controller`，由`GameEventManager`和`Controller`通知其他模型
- 工厂模式产生巡逻兵和物品

[代码传送门](https://github.com/Jie-Re/Unity3dProgrammingAndDesign/tree/master/EscapeSoompi)
## 场景制作
### 地图绘制
- 下载并导入资源`Wispy Sky`和`Winter Zone Mini`
- Skybox设置参考[我之前的博客](https://blog.csdn.net/xxiangyusb/article/details/101618889)
- 创建地形Terrain：菜单栏GameObject->3D Object->Terrain
- 在Inspector面板上设置Terrain的Material为`SnowAndRock_1`
![fig](https://img-blog.csdnimg.cn/20191103095857144.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
### Animation&Animator
#### 巡逻兵动画制作TODO
- 下载并导入资源`ToonyTinyPeople`
- 创建Animator：菜单栏Window->Animation->Animator
- 具体动画制作可参考[博客](https://blog.csdn.net/ChinarCSDN/article/details/81437311)
- 我的Animator最终制作结果为：
![Soompi](https://img-blog.csdnimg.cn/20191103231625782.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
#### 玩家动画制作
- 下载并导入资源`ToonyTinyPeople`
- 我的Animator最终制作结果为：
![wyb](https://img-blog.csdnimg.cn/20191103231644563.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
#### 金币动画制作
- 下载并导入资源`RPG Pack`
- 可直接使用资源库中提供的Animator
### 摄像机跟随CameraFollow
- 创建`CameraFollow.cs`脚本：

	```csharp
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public class CameraFollow : MonoBehaviour
	{
	    public GameObject follow;            //跟随的物体
	    public float smothing = 5f;          //相机跟随的速度
	    Vector3 offset;                      //相机与物体相对偏移位置
	
	    void Start()
	    {
	        offset = transform.position - follow.transform.position;
	    }
	
	    void FixedUpdate()
	    {
	        Vector3 target = follow.transform.position + offset;
	        //摄像机自身位置到目标位置平滑过渡
	        transform.position = Vector3.Lerp(transform.position, target, smothing * Time.deltaTime);
	    }
	
	}
	```

- 将该脚本挂载在主摄像机下
- **将`Main Camera`拖动赋值给`Controller`的`main_camera`属性**
### 玩家移动JoyStick
与之前HitUFO中的`JoyStick`实现方式不同，这里`w`和`s`键控制的移动改为`Z轴`上的移动，`a`和`d`键控制的移动改为游戏对象的旋转

```csharp

```
# 游戏演示视频
[前往b站观看游戏演示视频](https://www.bilibili.com/video/av74460388/)
# 参考资料
[发布订阅模式与观察者模式](https://segmentfault.com/a/1190000018706349)
[Unity动画机制 Animator与Animator Controller教程](https://blog.csdn.net/ChinarCSDN/article/details/81437311)
