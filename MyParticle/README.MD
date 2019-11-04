# 汽车尾气模拟
## 情景简介
众所周知，汽车在将燃料转化为动力时会排放废气。我们可以使用粒子系统来给一辆车增加一个排气口以作为车辆的装饰，从而使其更接近现实。
## 粒子的时间线
- 汽车尾气将以很快的速度从排气管道中冒出来
- 汽车尾气与大气一接触，速度会迅速减慢
- 汽车尾气与大气接触后，会扩散开来，变得越来越微弱
- 由于汽车尾气是热的，当它通过周围较冷的空气时，它也会略微上升

对于一粒废气烟而言：
- 必须从不大于管道宽度的地方开始
- 在其短暂的使用寿命内，尺寸会大大增加
- 在其短暂的使用寿命内，尺寸会大大增加
- 与空气混合时，通常会开始部分透明，然后逐渐完全透明
- 在动力学方面，粒子会很快发射出来，但随后会很快变慢，还会稍稍向上提升。

## 粒子制作
- 圆柱形排气管
	- 在`Shape`模块中，选择`Cone`，并将它的`Angle`属性设置为`0`，在这种情况下圆锥体cone实际上是一个圆柱形管道
	- 管道的半径`Radius`取决于车辆的大小，通常可以利用场景视图中提供的Gizmo网格线框来匹配两者的大小
		半径实际上决定了有关所选特性的许多设置，例如粒子的大小（particle size）和发射速率（emission rate）
	- 在本例中，我们假设车辆遵循Unity的的标准尺寸惯例，即以1米为1个世界单位（one world unit），因此将半径设置为`0.05`左右（也就是5cm左右）

- Smoke材料（Material）
	- 在资源商店（Asset Store）中下载并导入资源`Standard Assets`
	- 将其中的`ParticleSystems拖到Assets目录下
	- 将ParticleSystems/Prefabs下的Smoke拖入场景视图中（或者打开其预制进行查看），可以看到其Render模块中的Material已经设置为了`ParticleSmokeBlack`

- 粒子系统属性配置
	- `Start Lifetime`：`2.5`
		一般对于汽车尾气来说，默认的5秒生存周期过长，因此可以打开粒子系统模块（该模块名称与GameObject同名，本例中为Smoke）对Start Lifetime进行设置。在本模块中还将进行以下配置
	- `Simulation Space`：`Wolrd`
		通过使用世界模拟空间，即使车辆移动，烟雾也可以继续萦绕在它产生的地方。
	- `Gravity Modifier`：`-0.01`（也可以是其他绝对值较小的负数）
		负重力效应使烟雾颗粒上升，从而模拟它们相对于周围大气是热气体的情景
	- `Start Rotation`：将其设置为`Random Between Two Constants`（在右侧的下拉菜单栏中选择），然后将这两个常数值设置为`0`和`360`
		如此设置将可以使烟雾粒子在发射时随机旋转，形成随机的、不成形的烟雾轨迹的效果，从而更逼真

接下来配置烟雾向外翻滚和消散的效果：
- 粒子`Color over Lifetime`模块
	- 单击渐变条最右端的上标定（用于设置alpha值），将透明度alpha值设置为`0`，由此场景中的烟雾粒子将具有逐渐消失为0的效果（实际上预制中已配置好了）
		![ColorOverLifetime](https://img-blog.csdnimg.cn/20191104154530784.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70#pic_center)
		事实上，根据发动机的清洁程度，我们往往希望在汽车启动时降低渐变的alpha值。此外，浓重的黑烟往往表明燃烧不干净，效率低下

- 粒子`Size Over Lifetime`模块
	- 打开模块，选择曲线并滑动左端的曲线控制柄，使粒子以其完整大小（full size）的一小部分开始，从而使粒子具有逸出时增大的效果
		![SizeOverLifetime.PNG](https://img-blog.csdnimg.cn/20191104155449657.PNG)
	- `Size`的选择取决于排气管的尺寸（稍大于排气管尺寸的Size可以使效果更逼真）
	- 可以在场景视图中进行模拟并调试，以获得烟雾外观的良好视觉效果。如果烟雾的扩散程度不足以产生所需的效果，则此时可以增大`Particle System`模块中的`Start Size`

最后，配置当烟散开时减慢速度的效果：
- 粒子`Force Over Lifetim`模块
	- `Space`：`Local`
	- `Z`：一个负值，如：`-0.75`（系统沿对象局部空间的正Z方向发射粒子的情况下，否则则设置正值）

## 借助脚本模拟不同场景效果
- 汽车启动
- 汽车运行
- 汽车停下
- 汽车故障

思路：如果脚本中有一个表示发动机转速或车辆速度的变量，则只需将该值乘以一个常数，然后将结果赋给particlesystem的emissionrate属性。

为此，我设计了一个简单的小游戏以表示各个不同的场景
- 道路`road`：
	- 下载并导入资源`KajamansRoads`，自行选用其Free/Prefabs目录下的一个道路
	- 合理布置于场景中
- 汽车`Car`：
	- 下载并导入资源`Retro Cartoon Car Cicada`
	- 合理布置于场景中
	- 挂载`JoyStick.cs`代码实现虚拟轴控制汽车移动
		```csharp
		//JoyStick.cs
		using System.Collections;
		using System.Collections.Generic;
		using UnityEngine;
		
		public class JoyStick : MonoBehaviour
		{
		    private float speedX = 150F;
		    private float speedZ = 15F;
		
		    // Update is called once per frame
		    void Update()
		    {
		        float translationZ = Input.GetAxis("Vertical") * speedZ;
		        float translationX = Input.GetAxis("Horizontal") * speedX;
		        translationZ *= Time.deltaTime;
		        translationX *= Time.deltaTime;
		        transform.Translate(0, 0, translationZ);
		        transform.Rotate(0, translationX, 0);
		    }
		}
		```
	- 设置主相机跟随，将`CameraFollow.cs`代码挂载在`Main Camera`对象下，并将`Car`游戏对象拖动赋值给脚本中的`follow`
		```csharp
		//CameraFollow.cs
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
		        if (follow != null)
		        {
		            Vector3 target = follow.transform.position + offset;
		            //摄像机自身位置到目标位置平滑过渡
		            transform.position = Vector3.Lerp(transform.position, target, smothing * Time.deltaTime);
		        }
		    }
		}
		```

	- 挂载`SmokeEmission.cs`脚本，实现烟雾粒子发射随不同场景变化效果
		```csharp
		//SmokeEmission.cs
		//设置烟雾粒子生成地的坐标随汽车坐标移动变换
		GameObject smoke = null;
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1)
		{
		    smoke = Instantiate(Resources.Load<GameObject>("Prefabs/Smoke"), new Vector3(this.gameObject.transform.position.x - 0.635f,
		        this.transform.position.y + 0.17f, this.transform.position.z - 1.6f), Quaternion.identity);
		    float tranlationZ = Input.GetAxis("Vertical") * speedZ;
		    smoke.GetComponent<ParticleSystem>().emissionRate = engineCoeffient * tranlationZ;
		}
		
		...
		
		//汽车发生故障（与障碍物发生碰撞）后烟雾粒子效果变化以及故障恢复
		float engineCoeffient = 2.5f;
	    bool break_down = false;
	    int recover_time = 500;
	    if (Vector3.Distance(this.gameObject.transform.position, barrier_pos) < 1.5f)
        {
            //car is break down
            break_down = true;
            engineCoeffient = 10;
        }
	    if (break_down)
        {
            if (recover_time == 0)
            {
                break_down = false;
                recover_time = 500;
                engineCoeffient = 2.5f;
            }
            else
            {
                recover_time--;
            }
        }
		```

- 障碍物`Barrier`：
	- 下载并导入资源`IndustrialEnvironment`，自行选用TrafficBarriers/Prefabs目录下的一个障碍物作为预制，置于目录Assets/Resources/Prefabs下
	- 汽车每向前移动100个单位就生成一个障碍物
		```csharp
		//SmokeEmission.cs
		float old_pos_z;
		void Start()
		{
		    old_pos_z = this.gameObject.transform.position.z;
		}
		
		void Update() {
			...
		
			shift += (this.gameObject.transform.position.z - old_pos_z);
			        old_pos_z = this.gameObject.transform.position.z;
			
			if (shift >= 100)
			{
			    barrier_pos = new Vector3(this.gameObject.transform.position.x-1,
			        this.gameObject.transform.position.y, this.gameObject.transform.position.z + 30);
			    barrier = Instantiate(Resources.Load<GameObject>("Prefabs/Barrier"), barrier_pos, Quaternion.identity);
			    shift = 0;
			}
			
			...
		}
		```

[博客传送门](https://blog.csdn.net/xxiangyusb/article/details/102892598)
# 演示视频
[前往b站观看效果演示视频]()
# 参考资料
[潘老师的课程网站](https://pmlpml.github.io/unity3d-learning/08-particle-system)  
[Unity官方汽车模拟教程](https://docs.unity3d.com/Manual/PartSysExhaust.html)
