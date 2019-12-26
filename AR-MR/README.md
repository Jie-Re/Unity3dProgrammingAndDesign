# Unity实现AR功能
- 在[Vuforia官网](https://developer.vuforia.com/)注册账号
- 登录
- 新建一个Unity项目`MyFirstAR`
- 在Unity2019.2以后的版本中，无需导入Vuforia扩展包，只要激活即可
	- `File`->`Build Settings...`->`Android`
	- 若之前安装Unity时未勾选Android开发环境，则此时需要额外下载Android支持模块安装程序
		![AndroidDownload.PNG](https://img-blog.csdnimg.cn/20191226194844398.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
	- 下载好后，退出正在运行的Unity程序，运行安装程序（`UnitySetup-Android-Support-for-Editor-2019.2.3f1.exe`），等待即可
	- 安装成功后，重新打开项目`MyFirstAR`，继续前面所述的步骤，这时我们就可以进行相关设置了
	![VuforiaSettings.PNG](https://img-blog.csdnimg.cn/20191226200900384.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
	- `Player Settings`->`XR Settings`->勾选`Vuforia Augmented Reality Supported`后面的复选框
- 在Vuforia开发管理界面创建证书，用于获取License Key。Vuforia在Unity中需要相应的Key对SDK进行配置，否则无法使用
![License.PNG](https://img-blog.csdnimg.cn/20191220191502169.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 创建目标数据库,用于对所有Target及其特征数据进行管理和保存
- Vuforia要求将特定识别的目标提前上传至数据库进行特征提取。目标有多种类型，此处选择image，以对单幅图像进行识别
![TargetDB.PNG](https://img-blog.csdnimg.cn/20191220191648647.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 以unity package形式从Target Manger页面下载目标数据库并导入项目
![downloadDB.PNG](https://img-blog.csdnimg.cn/20191220191836398.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
![DownloadChoice.PNG](https://img-blog.csdnimg.cn/20191220191847674.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 导入下载的目标扩展包：`Assets`->`Import Package`->`Custom Package...`，选择下载的待识别图数据库包，`打开`->`Import`
![ImportPackage.PNG](https://img-blog.csdnimg.cn/2019122620135597.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
![ImportDBPackage.PNG](https://img-blog.csdnimg.cn/20191220192015148.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
可以看到此时`Assets`的目录结构为：
![AssetsHierachy.PNG](https://img-blog.csdnimg.cn/20191226201610617.PNG)
- 删除场景中原有的摄像机，并创建AR摄像机：`GameObject`->`Vuforia Engine`->`AR Camera`，若弹出一个提示框一定要选择`Accept`，否则后面`AR Camera`的脚本将不能加载
- 配置Vuforia
	- 在`ARCamera`的`Inspector`面板中点击`Open Vuforia Engine configuration`，打开Vuforia的配置面板
![ARCameraConfiguration.PNG](https://img-blog.csdnimg.cn/20191226203017353.PNG)
	- 将前面申请的许可证密钥添加到`APP License Key`后面的输入框中
	![AddLicenseKey.PNG](https://img-blog.csdnimg.cn/20191226203430563.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
	- 勾选`Disable model extraction from database`和`Enable video background`
	![configuration.PNG](https://img-blog.csdnimg.cn/20191226203712366.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 在场景中添加目标识别图：`GameObject`->`Vuforia engine`->`Image`，在`Hierachy`面板中将多一个`ImageTarget`对象
- 将准备好的龙模型挂载到`ImageTarget`下作为子对象，调整大小
![ImageTarget.png](https://img-blog.csdnimg.cn/20191226210659369.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 将`dragon`图片存至手机，运行项目，让手机上的`dragon`图片置于电脑摄像机前，即可看见效果如下图
	![result.PNG](https://img-blog.csdnimg.cn/20191226211504351.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
	详细演示视频可[前往B站观看](https://www.bilibili.com/video/av80775730/)
# 虚拟按键小游戏
## 虚拟按键添加方式
- 在`ImageTarget`的`Inspector`面板中，打开`advance`，点击`Add Virtual Button`一下即可在`ImageTarget`下添加一个虚拟按键
	![VirtualButton.PNG](https://img-blog.csdnimg.cn/20191226215229851.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
	注意：虚拟按键无法旋转，只能更改长度，可以通过`name`给虚拟按键添加一个标识，从而在代码中访问到此键

## 游戏设计
由于时间原因，本次小游戏设计为一个简单的跳高游戏：按下虚拟按钮，使小绵羊跳起来。
其中，虚拟按键事件处理器实现部分代码如下所示：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class VBEventHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject vb;
    public Animator ani;

    void Start()
    {
        VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        if (vbb)
        {
        	...
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        ani.SetTrigger("jump");
		...
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        ani.SetTrigger("idle");
        ...
    }

}
```
[完整项目传送门]()
详细演示视频可[前往B站查看]()
# 参考资料
[3D Game Programming & Design AR/MR 技术](https://pmlpml.github.io/unity3d-learning/12-AR-and-MR#2ar-sdk-%E4%B8%8E%E5%BA%94%E7%94%A8)
