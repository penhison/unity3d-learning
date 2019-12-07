# Priests and Devils AI

游戏视频[传送门](https://www.bilibili.com/video/av78398101/)

牧师与魔鬼AI
将代码搬运至hw3原始的牧师与魔鬼中Assets文件夹中并覆盖即可运行。

## AI算法
广度优先搜索找到最短路径后回溯即可。

### AI建模

#### 1.状态定义 
牧师与魔鬼的游戏状态可用三元组（p, d, side）即起始岸的牧师数目p，起始岸的魔鬼数目d和船的位置（0为起始岸，1为终点岸）
AI的搜索空间是一个4\*4\*2的三元组空间。
AI的起始状态是任意一个合法的状态，目标状态是（0, 0, 1）
状态合法性我定义为p==0 || p == 3 || p == d，这里只判定了状态能不能通过游戏失败的判定，没有考虑状态能否到达，如（0，0，0）状态是一个合法状态，但不是一个游戏可到达的状态。由于游戏机制保证了起始状态是合法且可到达的，所以这个判定满足游戏要求。

#### 2. 状态转移
船一次只能移动1或2个人，状态转移时side变成1-side,即1变0，0变1，表示船移动到了对岸，二元组(p,d)的变换数目(dp,dd)是{(1,0),(0,1),(1,1),(0,2),(2,0)}，当side==0时(p,d) -> (p-dp,d-dd), side==1时 (p,d) -> (p+dp,d+dd)


### AI实现
实现AI只要使用广度优先搜索，最短路径后回溯，具体过程为：
1. 定义状态
```
public struct SolverState {
	public int p;
	public int d;
	public int side;
	public SolverState(int _p, int _d, int _s) {
		p = _p;
		d = _d;
		side = _s;
	}
}
```
2. 使用队列做广度优先搜索，同时使用一个数组记录搜索过程的父节点，找到目标节点后通过父节点数组回溯。
实现时写成一个函数，传入起始状态，返回起始状态的下一个状态，具体代码为：
```
public class Solve {
	public static bool isLegalState(SolverState s) {
		return (s.p >= 0 && s.p <= 3) && (s.d >= 0 && s.d <= 3) && (s.p == 0 || s.p == 3 || s.p == s.d);
	}

	public static SolverState solve(SolverState start) {
		SolverState target_state = new SolverState(0, 0, 1);
		if (start.Equals(target_state)) return start;

		SolverState empty_state = new SolverState(0, 0, 0);
		int[] dp = {0, 1, 1, 2, 0};
		int[] dd = {1, 0, 1, 0, 2};
		SolverState[,,] parent = new SolverState[4,4,2];
		int i, j, k;
		for (i = 0; i < 4; i++) {
			for (j = 0; j < 4; j++) {
				for (k = 0; k < 2; k++) {
					parent[i,j,k] = empty_state;
				}
			}
		}
		Queue q = new Queue();
		q.Enqueue(start);
		parent[start.p, start.d, start.side] = start;
		SolverState cur_state;
		SolverState next_state;
		while (q.Count > 0) {
			cur_state = (SolverState) q.Dequeue();
			k = cur_state.side == 0 ? -1 : 1;
			for (i = 0; i < 5; i++) {
				next_state = new SolverState(cur_state.p + k * dp[i], cur_state.d + k * dd[i], 1 - cur_state.side);
				if (!isLegalState(next_state) || !(parent[next_state.p, next_state.d, next_state.side].Equals(empty_state))) continue;
				if (next_state.Equals(target_state)) {
					parent[next_state.p, next_state.d, next_state.side] = cur_state;
					while (!(parent[next_state.p, next_state.d, next_state.side].Equals(start))) {
						next_state = parent[next_state.p, next_state.d, next_state.side];
					}
					return next_state;
				}
				q.Enqueue(next_state);
				parent[next_state.p, next_state.d, next_state.side] = cur_state;
			}
		}
		return empty_state;
	}
}
```

## 游戏中使用AI
只要在UserGUI里画多一个button,Controlor里提供一个AI的函数,当这个按钮按下后，由AI控制游戏移动一步即可。
AI函数如下(Controlor新增)：
```
    public void AI() {
        int side = boat.GetBoatSign() == 1 ? 0 : 1;
        LandModel land = side == 0 ? start_land : end_land;
        int p = (start_land.GetRoleNum())[0] + (1-side) * (boat.GetRoleNumber())[0];
        int d = (start_land.GetRoleNum())[1] + (1-side) * (boat.GetRoleNumber())[1];
        SolverState cur_state = new SolverState(p, d, side);
        SolverState next_state = Solve.solve(cur_state);
        int move_p = next_state.p - p > 0 ? next_state.p - p : p - next_state.p;
        int move_d = next_state.d - d > 0 ? next_state.d - d : d - next_state.d;
        while (move_p < boat.GetRoleNumber()[0]) {
            MoveRole(boat.GetRoleByType(0));
            return;
        }
        while (move_d < boat.GetRoleNumber()[1]) {
            MoveRole(boat.GetRoleByType(1));
            return;
        }
        while (move_p > boat.GetRoleNumber()[0]) {
            MoveRole(land.GetRoleByType(0));
            return;
        }
        while (move_d > boat.GetRoleNumber()[1]) {
            MoveRole(land.GetRoleByType(1));
            return;
        }
        MoveBoat();
    }
```
添加的button如下（UserGUI新增)：
```
        if (GUI.Button(new Rect(700, 10, 60, 30), "AI next", button_style))
        {
            action.AI();
        }
```

具体实现看代码。