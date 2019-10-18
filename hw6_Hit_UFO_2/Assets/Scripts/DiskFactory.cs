using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    public GameObject disk_prefab = null;                 //飞碟预制体
    private List<DiskData> used = new List<DiskData>();   //正在被使用的飞碟列表
    private List<DiskData> free = new List<DiskData>();   //空闲的飞碟列表
    private Vector3[] scales = new Vector3[] {
            new Vector3(3f,1.5f,3f),
            new Vector3(2f,1f,2f),
            new Vector3(1.6f,0.8f,1.6f),
            new Vector3(1f,0.5f,1f),
            new Vector3(0.5f,0.25f,0.5f)
    };
    private float[] threshold = {0.5f, 0.7f, 0.8f, 0.9f, 1.0f};


    public GameObject GetDisk(int round)
    {
        float start_y = -10f;                             //刚实例化时的飞碟的竖直位置
        disk_prefab = null;

        //寻找相同tag的空闲飞碟

        if(free.Count > 0)
        {
            disk_prefab = free[free.Count-1].gameObject;
            free.Remove(free[free.Count-1]);
        }
        //如果空闲列表中没有，则重新实例化飞碟
        if(disk_prefab == null)
        {
            disk_prefab = Instantiate(Resources.Load<GameObject>("Prefabs/disk"), new Vector3(0, start_y, 0), Quaternion.identity);
            //给新实例化的飞碟赋予其他属性
        }
        float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
        int score = getScore();
        // disk_prefab.GetComponent<Renderer>().material.color = disk_prefab.GetComponent<DiskData>().color;
        disk_prefab.GetComponent<Renderer>().material.color = new Vector4(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1);
        disk_prefab.GetComponent<DiskData>().direction = new Vector3(ran_x, Random.Range(-10f, 10f), 0);
        disk_prefab.GetComponent<DiskData>().score = score;
        disk_prefab.GetComponent<DiskData>().scale = scales[score-1];
        disk_prefab.transform.localScale = scales[score-1];
        //添加到使用列表中
        used.Add(disk_prefab.GetComponent<DiskData>());
        return disk_prefab;
    }

    private int getScore() {
        float ran = Random.Range(0f, 1f);
        int i = 0;
        for (i = 0; i < 5; i++) {
            if (ran <= threshold[i]) break;
        }
        return i+1;
    }

    //回收飞碟
    public void FreeDisk(GameObject disk)
    {
        for(int i = 0;i < used.Count; i++)
        {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }
}
