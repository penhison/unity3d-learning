using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public FlyActionManager action_manager;
    public DiskFactory disk_factory;
    public UserGUI user_gui;
    public ScoreRecorder score_recorder;
    public int trail = 0;

    private Queue<GameObject> disk_queue = new Queue<GameObject>();          //游戏场景中的飞碟队列
    private List<GameObject> disk_notshot = new List<GameObject>();          //没有被打中的飞碟队列
    private int round = 1;                                                   //回合
    private float speed = 2f;                                                //发射一个飞碟的时间间隔
    private bool playing_game = false;                                       //游戏中
    private bool game_over = false;                                          //游戏结束
    private bool game_start = false;                                         //游戏开始

    void Start ()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        disk_factory = Singleton<DiskFactory>.Instance;
        score_recorder = Singleton<ScoreRecorder>.Instance;
        action_manager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
    }

	void Update ()
    {
        if(game_start)
        {
            //游戏结束，取消定时发送飞碟
            if (game_over)
            {
                CancelInvoke("LoadResources");
                CancelInvoke("NewRound");
            }
            //设定一个定时器，发送飞碟，游戏开始
            if (!playing_game)
            {
                trail = 0;
                user_gui.NewRoundStart();
                Invoke("NewRound", 1f);
                playing_game = true;
            }
            //发送飞碟
            SendDisk();
            //回合升级
            if (trail >= 10)
            {
                round++;
                //缩小飞碟发送间隔
                speed = speed * 0.9f;
                CancelInvoke("LoadResources");
                playing_game = false;
            }
        }
    }

    public void LoadResources()
    {
        disk_queue.Enqueue(disk_factory.GetDisk(round));
    }

    private void SendDisk()
    {
        float position_x = 16;
        if (disk_queue.Count != 0)
        {
            GameObject disk = disk_queue.Dequeue();
            disk_notshot.Add(disk);
            disk.SetActive(true);
            //设置被隐藏了或是新建的飞碟的位置
            float ran_y = Random.Range(1f, 4f);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<DiskData>().direction = new Vector3(ran_x, ran_y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * position_x, ran_y, 0);
            disk.transform.position = position;
            //设置飞碟初始所受的力和角度
            float power = Random.Range(10f, 15f);
            float angle = Random.Range(15f, 28f);
            action_manager.UFOFly(disk,angle,power);
        }

        for (int i = 0; i < disk_notshot.Count; i++)
        {
            GameObject temp = disk_notshot[i];
            //飞碟飞出摄像机视野也没被打中
            if (temp.transform.position.y < -10 && temp.gameObject.activeSelf == true)
            {
                trail++;
                disk_factory.FreeDisk(disk_notshot[i]);
                disk_notshot.Remove(disk_notshot[i]);
                //玩家血量-1
                user_gui.ReduceBlood();
            }
        }
    }

    public void NewRound() {
        CancelInvoke("NewRound");
        user_gui.NewRound();
        InvokeRepeating("LoadResources", 1f, speed);
    }

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool not_hit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //射线打中物体
            if (hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                //射中的物体要在没有打中的飞碟列表中
                for (int j = 0; j < disk_notshot.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == disk_notshot[j].gameObject.GetInstanceID())
                    {
                        not_hit = true;
                    }
                }
                if(!not_hit)
                {
                    return;
                }
                trail++;
                disk_notshot.Remove(hit.collider.gameObject);
                //记分员记录分数
                score_recorder.Record(hit.collider.gameObject);
                //显示爆炸粒子效果
                Transform explode = hit.collider.gameObject.transform.GetChild(0);
                explode.GetComponent<ParticleSystem>().Play();
                //等0.1秒后执行回收飞碟
                StartCoroutine(WaitingParticle(0.08f, hit, disk_factory, hit.collider.gameObject));
                break;
            }
        }
    }
    //获得分数
    public int GetScore()
    {
        return score_recorder.score;
    }
    //重新开始
    public void ReStart()
    {
        game_over = false;
        playing_game = false;
        score_recorder.score = 0;
        round = 1;
        speed = 2f;
        trail = 0;
    }
    //设定游戏结束
    public void GameOver()
    {
        game_over = true;
    }
    //暂停几秒后回收飞碟
    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, DiskFactory disk_factory, GameObject obj)
    {
        yield return new WaitForSeconds(wait_time);
        //等待之后执行的动作
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.FreeDisk(obj);
    }
    public void BeginGame()
    {
        game_start = true;
    }
}
