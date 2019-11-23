# 第八章作业 粒子系统
作业网址https://pmlpml.github.io/unity3d-learning/09-ui.html

效果展示视频：[https://www.bilibili.com/video/av76722206/](https://www.bilibili.com/video/av76722206/)

## 作业要求
1、血条（Health Bar）的预制设计。具体要求如下
- 分别使用 IMGUI 和 UGUI 实现
- 使用 UGUI，血条是游戏对象的一个子元素，任何时候需要面对主摄像机
- 分析两种实现的优缺点
- 给出预制的使用方法

## 制作过程
UGUI的制作参考课件
IMGUI的制作参考博客[https://blog.csdn.net/Runner1st/article/details/80582780](https://blog.csdn.net/Runner1st/article/details/80582780)

### IMGUI制作
用HorizontalScrollbar（水平滚动条）的宽度作为血条的显示值，代码如下：
```
using UnityEngine;

public class health_bar_IMGUI : MonoBehaviour
{
    // 当前血量
    public float health = 0.0f;
    // 增/减后血量
    private float resulthealth;

    private Rect HealthBar;
    private Rect HealthUp;
    private Rect HealthDown;

    void Start()
    {
        //血条区域
        HealthBar = new Rect(200, 50, 300, 20);
        //加血按钮区域
        HealthUp = new Rect(205, 80, 40, 20);
        //减血按钮区域
        HealthDown = new Rect(255, 80, 40, 20);
        resulthealth = health;
    }

    void OnGUI()
    {
    	print("running");
        if (GUI.Button(HealthUp, "加血"))
        {
            resulthealth = resulthealth + 0.1f > 1.0f ? 1.0f : resulthealth + 0.1f;
        }
        if (GUI.Button(HealthDown, "减血"))
        {
            resulthealth = resulthealth - 0.1f < 0.0f ? 0.0f : resulthealth - 0.1f;
        }

        //插值计算health值，以实现血条值平滑变化
        health = Mathf.Lerp(health, resulthealth, 0.05f);

        // 用水平滚动条的宽度作为血条的显示值
        GUI.HorizontalScrollbar(HealthBar, 0.0f, health, 0.0f, 1.0f);
    }
}

```

写好脚本后，在层次视图，Create -> Create Empty，重命名为health_bar_IMGUI，然后将health_bar_IMGUI.cs脚本拖到该对象，运行即可。
预制体的制作只需要将这个带有脚本的空文件拖进Project的文件目录即可。

### UGUI制作
UGUI的制作完全参考课件，详细信息见课件。
这里说一下我遇到的坑。
1. 使用脚本 `this.transform.LookAt(Camera.main.transform.position);` 报错，错误原因为Camera.main为null,空对象引用，解决办法为改变主摄像头的Inspector的Tag为MainCamera,Tag就在Inspector的摄像头名字下方。
2. Slider可以调方向，从而实现血条的从右到左是从大到小的效果。可以通过slider的Inspector视图的Direction调节。
