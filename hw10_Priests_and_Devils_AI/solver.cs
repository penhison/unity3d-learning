using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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