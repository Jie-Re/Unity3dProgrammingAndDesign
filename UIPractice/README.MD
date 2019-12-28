[博客传送门](https://blog.csdn.net/xxiangyusb/article/details/103740611)
# UGUI基础
## 画布
### 基础概念
- 画布(`Canvas`)：是绘图区域，同时是UI元素的容器。容器中UI元素及其子UI元素都将绘制在其上。拥有`Canvas`组件的游戏对象都有一个画布，它空间中的子对象，如果是UI元素将渲染在画布上。
- UI元素采用**像素单位**表示位置和尺寸
- UI元素的显示顺序：画布种的UI元素*按照它们在层次结构中出现的顺序绘制*。如果两个UI元素重叠，后面的元素将出现在较早的元素之上。因此，**最后一个孩子显示在最上面**。
	要更改这种显示顺序，可以通过直接拖动它们在`Hierarchy`视图中的位置，也可通过`Transform`组件的方法在脚本中控制，如`SetAsFirstSiling`，`SetAsLastSibling`，`SetSiblingIndex`
- 渲染模式(`Render Mode`)：画布组件有渲染模式设置，可用于使其在屏幕空间(`Screen Space`）或世界空间(`World Space`)中渲染
	- 屏幕空间 - 叠加(Screen Space - Overlay)
		将UI元素放置在场景顶部渲染的屏幕，画布会自动更改大小匹配屏幕。Canvas默认中心点为屏幕中心！
	- 屏幕空间 - 相机(Screen Space - Camera)
		画布放在制定的渲染摄像机前，如100的位置，画布会自动匹配为屏幕分辨率。Canvas默认中心点为屏幕中心！
	- 世界空间(World Space)
		画布行为与场景中的其他任何对象一样，UI元素将放置在其他对象的前面或后面渲染。**画布大小和位置任意设置**。

### 测试渲染模式
- 创建一个`Button`对象：`GameObject`->`UI`->`Button`
- 创建一个`Cube`对象
- 检查、设置以下对象属性
	- `Main Camera`的`Position=(0,1,-10)`
	- `Canvas::Button`的`(PosX, PosY, PosZ) = (0,0,0)`
	- `Cube`的`Position=(0,0.5,0)`
- 使用鼠标中间滚轮在场景视图中缩小，直到看到整个画布，中间有一个Button，如图所示：
![ButtonInCanvas.PNG](https://img-blog.csdnimg.cn/20191228200651654.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Canvas`对象的`Inspector`面板中`Canvas`组件
	- 设置`Render Mode`为`Screen Space - Overlay`
	- 结果：在`Game`视图中，可以看到**Button在Cube前面**
	![ScreenOverlay.PNG](https://img-blog.csdnimg.cn/20191228201818964.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Canvas`对象的`Inspector`面板中`Canvas`组件
	- 设置`Render Mode`为`Screen Space - Camera`
	- 设置`Render Camera`为`Main Camera`（拖过去赋值）
	![ScreenSpaceCamera.png](https://img-blog.csdnimg.cn/20191228201416690.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)

	结果：在`Game`视图中可以看到**Button在Cube后面**
	![ScreenSpaceCameraGame.png](https://img-blog.csdnimg.cn/2019122820161474.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Main Camera`：
	- 在`Camera`组件面板中修改`Filed of View`
	- 结果1：在`Game`视图中，`Cube`大小发生变化，而`Button`保持不变
	![RenderTestGame.gif](https://img-blog.csdnimg.cn/20191228202604894.gif)
	- 结果2：在`Scene`视图中，画布随着视口自动改变大小
	![RenderTestScene.gif](https://img-blog.csdnimg.cn/2019122820264210.gif)
## UI布局基础
### 基本概念
每个UI元素都被表示为一个矩形，为了相对于Canvas和其他UI元素实现定位，Unity在Transform基础上定义了`Rect Transform`(矩形变换)支持矩形元素在2/3D场景中变换。
- 矩形变换
	- 位置和尺寸应使用像素，以匹配素材
	- **Pivot/旋转点/轴心**：旋转点在场景视图中显示为**蓝色圆圈**，用规范化坐标表示位置。修改`Rotation`属性，矩形围绕此点旋转。
	- **Anchors/锚点**：锚点在场景视图中显示为**四个小三角形手柄**（四页花）。每个叶子位置对应矩形的四个顶点。当锚点随父对象变换时，矩形的顶点与对应的锚点**相对位置必须保持不变**

### 锚点练习
- 将场景另存为`Scene2`
- 将场景视图设置为2D模式
	![Scene2D.png](https://img-blog.csdnimg.cn/20191228203337452.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 使用鼠标中间滚轮在场景视图中缩小，直到看到整个画布，中间有一个Button
- 将`Canvas`的`Render Mode`设为`World Space`，以便改变父对象（画布）的大小
	- 测试结果1：UI元素锚定到父级的中心。该元素保持到中心的固定偏移量
	![AnchorTest1.gif](https://img-blog.csdnimg.cn/20191228205019465.gif)
	- 测试结果2：UI元素锚定在父级的右下角，该元素保持到右下角的固定偏移量
	![AnchorTest2.gif](https://img-blog.csdnimg.cn/20191228205433880.gif)
	- 测试结果3：左侧角落的UI元素锚定在父级的左下角，右侧角落锚定在右下角，则元素的角落保持固定的偏移到它们各自的锚点
	![AnchorTest3.gif](https://img-blog.csdnimg.cn/20191228221936688.gif)
- 实现父子元素的等比缩放的方式：选择`Canvas`，直接在场景视图中对`Canvas`进行缩放即可

## UI组件与元素
### 基本概念
UI部件都是用Script开发的自定义组件。包括在UI、Layout和Rendering等分类中。
- 可视化组件：包括
	- `Text`：显示文本的文本区域。可以设置字体样式、大小等文本功能
	- `Image`：显示图片的区域。可以设置GUI精灵、色彩
	- `Raw Image`：原始图像采用纹理，进行UV矩形贴图
	- `Mask`：不是一个可见的UI控件。它将子元素限制(即“掩蔽”)为父元素的形状。如果孩子比父控件大，那么只有适合父节点Mask的部分是可见的
	- `Effects`：应用各种简单的效果，例如简单的投影或轮廓
	更多详细说明可参考[官方文档](https://docs.unity3d.com/Manual/UIVisualComponents.html)
- UI交互元素：UI交互元素是GameObject，它拥有UI交互组件、UI可视化组件及相关组件的组合，以及一些UI子元素构成，以方便用户在设计场景中创建交互界面
	- `Button`
	- `Toggle`
	- `Toggle Group`
	- `Slider`
	- `Scrollbar`
	- `Dropdown`
	- `Input Field`
	- `Scroll Rect`(Scroll View)
	更多详细说明可以参考[官方文档](https://docs.unity3d.com/Manual/UIInteractionComponents.html)

### Mask练习
- 将场景另存为`Scene3`，将除`Main Camera`和`Directional Light`以外的对象删掉
- 将如下所示的图片拖入`Assets/Texture`目录中，作为纹理图像
![yibo.jpg](https://img-blog.csdnimg.cn/2019122822252936.jpg?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 创建`Panel`对象：`GameObject`->`UI`->`Panel`
- 在`Panel`下添加`Raw Image`对象：右键`Panel`->`UI`->`Raw Image`
- 将前面的纹理图像拖入`Raw Image`的`Textrue`插槽中
	![RawImageTexture.png](https://img-blog.csdnimg.cn/20191228223311987.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 将场景视图设置为`2D`，并缩放至能看到整个画布
	![Scene2Dyibo.PNG](https://img-blog.csdnimg.cn/20191228223531545.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Panel`对象
	- 在`Rect Transform`组件选择`Anchor Presets`为`(middle, center)`
	![middleCenterAnchorPreset.png](https://img-blog.csdnimg.cn/2019122822381054.png)
	- 用`Rect Tool`，将`Panel`与图片的大小调为一致
	![RecTool](https://img-blog.csdnimg.cn/20191228224146515.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
	- 添加`Mask`组件
	- 选择`Image`组件，选择`Source Image`为`Knob`
- 在`Game`视图中，可看到如下效果图：
	![GameKnob.PNG](https://img-blog.csdnimg.cn/20191228224543934.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
### 动画练习
- 添加`Animation`编辑视图：`Window`->`Animation`->`Animation`
- 选择`Raw Image`
- 在视图`Animation`中，点击`create`按钮，则系统将在该对象上创建动画组件、动画控制器、动画文件，将文件保存为`tets1.anim`
	- `Add Property`->`Rect Transform`->`Archored Position`->`+`
- 选择`Scene`视图，移动`Raw Image`到`Panel`左边
	![left.PNG](https://img-blog.csdnimg.cn/20191229002132659.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Animation`视图，（记得先开启录制）在`0s`位置添加关键帧，将播放位置标线放置在最后
- 选择`Scene`视图，移动`Raw Image`到`Panel`右边
	![PanelRawImage.PNG](https://img-blog.csdnimg.cn/20191229001838528.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Animation`视图，在最后位置添加关键帧
- 运行动画：
	![Animation.gif](https://img-blog.csdnimg.cn/20191229004949291.gif)
### 富文本测试
为了显示格式复杂的文字，Unity提供了**类似HTML**标签，控制字体、字号、颜色。
- 在`Canvas`下添加`Text`元素
- 在`Text`组件中输入：`We are <color=green>green</color> with envy`
- 结果：
	![RichText.PNG](https://img-blog.csdnimg.cn/20191229003950102.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
### 简单血条
给动画人物`Ethan`添加`Health Bar`
- 将场景另存为`Scene4`，将除`Main Camera`和`Directional Light`以外的对象删掉
- 在`Assets Store`中下载并导入`Standard Assets`
- 添加一个`Plane`对象：`GameObject`->`3D Object`->`Plane`
- 将目录`Assets/Standard Assets/Characters/ThirdPersonCharacter/Prefabs`下的`ThirdPersonController`预制拖放入场景中，重命名为`Ethan`
- 检查以下属性：
	- `Plane`的`Transform`的`Position=(0,0,0)`
	- `Ethan`的`Transform`的`Position=(0,0,0)`
	- `Main Camera`的`Transform`的`Position=(0,1,-10)`
- 为`Ethan`添加画布子对象：右键`Ethan`->`UI`->`Canvas`
- 添加滑条子对象作为血条：右键`Ethan`的子对象`Canvas`->`UI`->`Slider`
- 选择`Ethan`的`Canvas`，在`Inspector`视图中，
	- 设置`Canvas`组件`Render Mode`为`World Space`
	- 设置`Rect Transform`组件`(PosX, PosY, Width, Height)`为`(0,2,160,20)`，此外`Scale`为`(0.01,0.01,1)`
- 运行效果：
	![SliderTest.gif](https://img-blog.csdnimg.cn/20191229012223959.gif)
- 展开`Slider`
	- 选择`Handle Slider Area`，在`Inspector`中反勾选该对象，在层级视图中它将变灰
	- 选择`Background`，在`Inspector`中反勾选该对象，在层级视图中它将变灰
	- 选择`Fill Area`的`Fill`，修改`Image`组件的`Color`为红色
	![SliderConfig.png](https://img-blog.csdnimg.cn/20191229010629415.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 选择`Slider`的`Slider`组件
	- 设置`Max Value`为`100`
	- 设置`Value`为`75`
	![SliderComponentConfig.png](https://img-blog.csdnimg.cn/20191229010908478.png)
- 给`Canvas`挂载脚本`LookAtCamera.cs`
	```csharp
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public class LookAtCamera : MonoBehaviour
	{
	    // Update is called once per frame
	    void Update()
	    {
	        this.transform.LookAt(Camera.main.transform.position);
	    }
	}
	```
- 运行效果：
	![SliderLookAtCamera.gif](https://img-blog.csdnimg.cn/20191229013123600.gif)
# 血条(Health Bar)的预制设计
## UGUI实现
- 用UGUI制作血条的方式见前面**简单血条**部分
- 将`Ethan`的子对象`Canvas`拖入`Assets/Prefabs`制成预制，重命名为`UGUIHealthBar`
## IMGUI实现
- 场景另存为`Scene6`，去除多余的对象，保留`Main Camera`，`Directional Light`和`Plane`
- 创建一个空对象：`GameObject`->`Creat Empty`，并重命名为`IMGUIHealthBar`
- 创建脚本`IMGUIHealthBar.cs`
	- 借助`HorizontalScrollbar`(水平滚动条)实现
		```csharp
		// 用水平滚动条的宽度作为血条的显示值
		GUI.color = Color.red;
		GUI.HorizontalScrollbar(HealthBar, 0, health, 0.0f, maxHealth);
		```
	- 使用`Mathf.Lerp`插值计算血量，以实现血条值平滑变化，而非突变
		```csharp
		if (GUI.Button(HealthUp, "+"))
		{
		    resulthealth = resulthealth + 0.1f > 1.0f ? 1.0f : resulthealth + 0.1f;
		}
		if (GUI.Button(HealthDown, "-"))
		{
		    resulthealth = resulthealth - 0.1f < 0.0f ? 0.0f : resulthealth - 0.1f;
		}
		//插值计算health值，以实现血条值平滑变化
		health = Mathf.Lerp(health, resulthealth, 0.05f);
		```
	- 让滚动条跟随父对象运动
	获取父对象的位置：
	

		```csharp
		private Transform father;
		...
		void Start()
		{
		    father = this.transform.parent.transform;
		    ...
		}
		```
		根据父对象的位置设置滚动条的位置：
	

		```csharp
		//血条区域
		HealthBar = new Rect(Screen.width / 2 + father.position.x * father.localScale.x * 100 - father.localScale.x * 10, 
		    Screen.height / 2 + (father.position.z - father.localScale.y * 7) * father.localScale.z * 10, 
		    father.localScale.x * 100, father.localScale.z * 10);
		//加血按钮区域  
		HealthUp = new Rect(Screen.width / 2 + father.position.x * father.localScale.x * 100 - father.localScale.x * 30
		    , Screen.height / 2 + (father.position.z - father.localScale.y * 7) * father.localScale.z * 10
		    , father.localScale.x * 20, father.localScale.z * 10);
		//减血按钮区域
		HealthDown = new Rect(Screen.width / 2 + father.position.x * father.localScale.x * 100 + father.localScale.x * 90, 
		    Screen.height / 2 + (father.position.z - father.localScale.y * 7) * father.localScale.z * 10, 
		    father.localScale.x * 20, father.localScale.z * 10);
		```

- 将脚本`IMGUIHealthBar.cs`和前面已经写好的`LookAtCamera.cs`挂载到`IMGUIHealthBar`下
- 将`IMGUIHealthBar`拖入`Assets/Prefabs`制成预制
- 运行效果：
	![IMGUIHealthBar.gif](https://img-blog.csdnimg.cn/20191229030117708.gif)

## 两种实现的比较
||UGUI|IMGUI|
|--|--|--|
|优点|1. 所见即所得（WYSIWYG）设计工具，设计师也能参与程序开发<br>2.支 持多模式、多摄像机渲染<br>3. UI 元素与游戏场景融为一体的交互<br>4. 面向对象的编程|1. 符合游戏编程传统<br>2. 在修改模型，渲染模型这样的经典游戏循环编程模式中，在渲染阶段之后，绘制 UI 界面无可挑剔<br>3. 这样的编程既避免了 UI 元素保持在屏幕最前端，又有最佳的执行效率，一切控制掌握在程序员手中|
|缺点|1. 没有 UIWrap 来循环 scrollview 内容<br>2. 暂时没有Tween组件|1. 传统代码驱动的 UI 面临效率低下<br>2. 难以调试|
## 使用方式
创建一个`ThirdPersonController`对象，将预制`UGUIHealthBar`或`IMGUIHealthBar`拖到层级面板中，使之成为`ThirdPersonController`对象的子对象即可
# 其他参考资料
[潘老师的课程网站 第九章 UI系统](https://pmlpml.github.io/unity3d-learning/09-ui.html)
[Unity3D学习笔记（四）分别使用IMGUI和UGUI实现血条的预制设计](https://blog.csdn.net/Runner1st/article/details/80582780)
