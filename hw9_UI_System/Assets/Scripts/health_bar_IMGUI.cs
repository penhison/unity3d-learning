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
