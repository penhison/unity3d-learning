# 第八章作业 粒子系统
作业网址https://pmlpml.github.io/unity3d-learning/08-particle-system

效果展示视频：[https://www.bilibili.com/video/av75122834/](https://www.bilibili.com/video/av75122834/)

## 作业要求
1、简单粒子制作

- 按参考资源要求，制作一个粒子系统，[参考资源](https://pmlpml.github.io/unity3d-learning/08-particle-system)

- 使用 3.3 节介绍，用代码控制使之在不同场景下效果不一样

本次作业是3选1作业，我选择了第一个作业。作业完全按照参考资源实现。

## 制作过程
基本制作过程为：
1. 创建新的空物体，加入粒子系统的组件。
2. 设置粒子系统组件为光晕效果的主体。
3. 空物体添加部件粒子系统，设置光晕，增强光晕效果
4. 空物体添加子空物体，空白物体设置部件粒子系统，设置光芒，完成粒子飞散效果。

添加控制脚本，实现放大缩小效果：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrl : MonoBehaviour
{
    ParticleSystem particleSystem;
    float time;
    ParticleSystem exhaust;

    void Start()
    {
        time = 0;
        particleSystem = GetComponent<ParticleSystem>();
    }

    [System.Obsolete]
    void Update()
    {
    }

    [System.Obsolete]
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 70, 30), "放大"))
        {
            particleSystem.startSize = particleSystem.startSize + 0.5f;
        }

        if (GUI.Button(new Rect(100, 200, 70, 30), "缩小"))
        {
            particleSystem.startSize = particleSystem.startSize - 0.5f;
        }

    }

}
```
将控制脚本拖到承载粒子系统的空物体就可以了。