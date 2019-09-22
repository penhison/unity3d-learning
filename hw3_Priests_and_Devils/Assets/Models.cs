﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame
{

    public interface ISceneController                      //加载场景
    {
        void LoadResources();
    }
    public interface IUserAction                           //用户互动会发生的事件
    {
        void MoveBoat();                                   //移动船
        void Restart();                                    //重新开始
        void MoveRole(RoleModel role);                     //移动角色
        int Check();                                       //检测游戏结束
    }

    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
        public ISceneController CurrentScenceController { get; set; }
        public static SSDirector GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }

    public class LandModel
    {
        GameObject land;                                //陆地对象
        Vector3[] positions;                            //保存每个角色放在陆地上的位置
        int land_sign;                                  //到达陆地标志为-1，开始陆地标志为1
        RoleModel[] roles = new RoleModel[6];           //陆地上有的角色

        public LandModel(string land_mark)
        {
            positions = new Vector3[] {new Vector3(14,0,0), new Vector3(12,0,0), new Vector3(10,0,0),
                new Vector3(16,0,0), new Vector3(18,0,0), new Vector3(20,0,0)};
            if (land_mark == "start")
            {
                land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), new Vector3(20, -5, 0), Quaternion.identity) as GameObject;
                land_sign = 1;
            }
            else
            {
                land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), new Vector3(-20, -5, 0), Quaternion.identity) as GameObject;
                land_sign = -1;
            }
        }

        public int GetEmptyNumber()                      //得到陆地上哪一个位置是空的
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                    return i;
            }
            return -1;
        }

        public int GetLandSign() { return land_sign; }

        public Vector3 GetEmptyPosition()               //得到陆地上空位置
        {
            Vector3 pos = positions[GetEmptyNumber()];
            pos.x = land_sign * pos.x;                  //因为两个陆地是x坐标对称
            return pos;
        }

        public void AddRole(RoleModel role)
        {
            roles[GetEmptyNumber()] = role;
        }

        public RoleModel DeleteRoleByName(string role_name)      //离开陆地
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null && roles[i].GetName() == role_name)
                {
                    RoleModel role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }

        public int[] GetRoleNum()
        {
            int[] count = { 0, 0 };                    //count[0]是牧师数，count[1]是魔鬼数
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null)
                {
                    if (roles[i].GetSign() == 0)
                        count[0]++;
                    else
                        count[1]++;
                }
            }
            return count;
        }

        public void Reset()
        {
            roles = new RoleModel[6];
        }
    }

    public class BoatModel
    {
        GameObject boat;
        Vector3[] start_empty_pos;                                    //船在开始陆地的空位位置
        Vector3[] end_empty_pos;                                      //船在结束陆地的空位位置
        Move move;
        Click click;
        int boat_sign = 1;                                                     //船在开始还是结束陆地
        RoleModel[] roles = new RoleModel[2];                                  //船上的角色

        public BoatModel()
        {
            boat = Object.Instantiate(Resources.Load("Boat", typeof(GameObject)), new Vector3(7, -4, 0), Quaternion.identity) as GameObject;
            boat.name = "boat";
            move = boat.AddComponent(typeof(Move)) as Move;
            click = boat.AddComponent(typeof(Click)) as Click;
            click.SetBoat(this);
            start_empty_pos = new Vector3[] { new Vector3(8,-2,0), new Vector3(6,-2,0) };
            end_empty_pos = new Vector3[] { new Vector3(-8,-2,0), new Vector3(-6,-2,0) };
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null)
                    return false;
            }
            return true;
        }

        public void BoatMove()
        {
            if (boat_sign == -1)
            {
                move.MovePosition(new Vector3(7,-4,0));
                boat_sign = 1;
            }
            else
            {
                move.MovePosition(new Vector3(-7,-4,0));
                boat_sign = -1;
            }
        }

        public int GetBoatSign(){ return boat_sign;}

        public RoleModel DeleteRoleByName(string role_name)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null && roles[i].GetName() == role_name)
                {
                    RoleModel role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }

        public int GetEmptyNumber()
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 GetEmptyPosition()
        {
            Vector3 pos;
            if (boat_sign == -1)
                pos = end_empty_pos[GetEmptyNumber()];
            else
                pos = start_empty_pos[GetEmptyNumber()];
            return pos;
        }

        public void AddRole(RoleModel role)
        {
            roles[GetEmptyNumber()] = role;
        }

        public GameObject GetBoat(){ return boat; }

        public void Reset()
        {
            if (boat_sign == -1)
                BoatMove();
            roles = new RoleModel[2];
        }

        public int[] GetRoleNumber()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                    continue;
                if (roles[i].GetSign() == 0)
                    count[0]++;
                else
                    count[1]++;
            }
            return count;
        }
    }

    public class RoleModel
    {
        GameObject role;
        int role_sign;             //0为牧师，1为恶魔
        Click click;
        bool on_boat;              //是否在船上
        Move move;
        PlayAnimation play_ani;
        LandModel land_model = (SSDirector.GetInstance().CurrentScenceController as Controllor).start_land;

        public RoleModel(string role_name)
        {
            if (role_name == "priest")
            {
                role = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_sign = 0;
            }
            else
            {
                role = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_sign = 1;
            }
            move = role.AddComponent(typeof(Move)) as Move;
            click = role.AddComponent(typeof(Click)) as Click;
            play_ani = role.AddComponent(typeof(PlayAnimation)) as PlayAnimation;
            click.SetRole(this);
        }

        public int GetSign() { return role_sign;}
        public LandModel GetLandModel(){return land_model;}
        public string GetName() { return role.name; }
        public bool IsOnBoat() { return on_boat; }
        public void SetName(string name) { role.name = name; }
        public void SetPosition(Vector3 pos) { role.transform.position = pos; }

        public void PlayGameOver()
        {
            play_ani.Play();
        }

        public void PlayIdle()
        {
            play_ani.NotPlay();
        }

        public void Move(Vector3 vec)
        {
            move.MovePosition(vec);
        }

        public void GoLand(LandModel land)
        {
            role.transform.parent = null;
            land_model = land;
            on_boat = false;
        }

        public void GoBoat(BoatModel boat)
        {
            role.transform.parent = boat.GetBoat().transform;
            land_model = null;
            on_boat = true;
        }

        public void Reset()
        {
            land_model = (SSDirector.GetInstance().CurrentScenceController as Controllor).start_land;
            GoLand(land_model);
            SetPosition(land_model.GetEmptyPosition());
            land_model.AddRole(this);
        }
    }

    public class Move : MonoBehaviour
    {
        float move_speed = 250;                   //移动速度
        int move_sign = 0;                        //0是不动，1水平移动，2竖直移动
        Vector3 end_pos;
        Vector3 middle_pos;

        void Update()
        {
            if (move_sign == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, middle_pos, move_speed * Time.deltaTime);
                if (transform.position == middle_pos)
                    move_sign = 2;
            }
            else if (move_sign == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, end_pos, move_speed * Time.deltaTime);
                if (transform.position == end_pos)
                    move_sign = 0;
            }
        }
        public void MovePosition(Vector3 position)
        {
            end_pos = position;
            if (position.y == transform.position.y)         //船只会水平移动
            {
                move_sign = 2;
            }
            else if (position.y < transform.position.y)      //角色从陆地到船
            {
                middle_pos = new Vector3(position.x, transform.position.y, position.z);
            }
            else                                          //角色从船到陆地
            {
                middle_pos = new Vector3(transform.position.x, position.y, position.z);
            }
            move_sign = 1;
        }
    }

    public class Click : MonoBehaviour
    {
        IUserAction action;
        RoleModel role = null;
        BoatModel boat = null;
        public void SetRole(RoleModel role)
        {
            this.role = role;
        }
        public void SetBoat(BoatModel boat)
        {
            this.boat = boat;
        }
        void Start()
        {
            action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        }
        void OnMouseDown()
        {
            if (boat == null && role == null) return;
            if (boat != null)
                action.MoveBoat();
            else if(role != null)
                action.MoveRole(role);
        }
    }

    public class PlayAnimation : MonoBehaviour
    {
        public void Play()                                  //播放gameover状态的动画
        {
            Animator anim = this.GetComponent<Animator>();
            anim.SetBool("isgameover", true);
        }
        public void NotPlay()                              //播放Idle状态的动画
        {
            Animator anim = this.GetComponent<Animator>();
            anim.SetBool("isgameover", false);
        }
    }
}


