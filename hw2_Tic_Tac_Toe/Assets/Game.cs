using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    int[,] board = new int[3,3];
    string[] piece = {"", "O", "X"};

    int mode = 0;
    string[] mode_name = {"Player VS AI", "AI VS Player", "Player VS Player", "AI VS AI"};
    string[,] end_name = { { "", "Player WIN", "AI WIN", "DRAW" },
                           { "", "AI WIN", "Player WIN", "DRAW" },
                           { "", "Player1 WIN", "Player2 WIN", "DRAW" },
                           { "", "AI1 WIN", "AI2 WIN", "DRAW" }};

    int turn = 1;
    string[,] turn_name = {{"click start to play", "Player turn", "AI turn"},
                           {"click start to play", "AI turn", "Player turn"},
                           {"click start to play", "Player1 turn", "Player2 turn"},
                           {"click start to play", "AI1 turn", "AI2 turn"}
                            };

    void reset() {
        turn = 1;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                board[i,j] = 0;
            }
        }
    }

    int checkWin()
    { 
        for (int i = 0; i < 3; ++ i)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != 0) return board[i, 0];
        }
        for (int j = 0; j < 3; ++j)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[0, j] != 0) return board[0, j];
        }
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[1, 1] != 0) return board[1, 1];
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[1, 1] != 0) return board[1, 1];
        
        for (int i = 0; i < 3; ++ i)
        {
            for (int j = 0; j < 3; ++ j)
            {
                if (board[i,j] == 0) return 0;
            }
        }

        return 3;
    }

    bool isAITurn()
    {
        if ((mode == 0 && turn == 2) || (mode == 1 && turn == 1) || mode == 3) return true;
        return false;
    }

    void AI()
    {
        if (checkWin() != 0) return;
        int count = 0;
        for (int i = 0; i < 3; ++ i)
        {
            for (int j = 0; j < 3; ++ j)
            {
                if (board[i, j] == 0) count++; 
            }
        }
        int locate = Random.Range(0, count);
        //Debug.Log(locate);
        count = 0;
        for (int i = 0; i < 3; ++ i)
        {
            for (int j = 0; j < 3; ++ j)
            {
                if (board[i,j] == 0)
                {
                    if (count == locate)
                    {
                        board[i, j] = turn;
                        turn = 3 - turn;
                        return;
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 130, 25), "reset"))
        {
            reset();
        }
        if (GUI.Button(new Rect(100, 130, 130, 25), mode_name[mode]))
        {
            reset();
            mode = (mode + 1) % 4;
        }


        if (checkWin() != 0)
        {
            if(GUI.Button(new Rect(300, 100, 130, 100), end_name[mode, checkWin()] + "\n\nPress to play again"))
            {
                reset();
            }
        }
        else
        {
            if (isAITurn())
            {
                AI();
            }
            GUI.Box(new Rect(275, 70, 200, 200), turn_name[mode, turn]);
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (board[i, j] != 0)
                    {
                        GUI.Button(new Rect(300 + i * 50, 100 + j * 50, 50, 50), piece[board[i, j]]);
                    }
                    else
                    {
                        if (GUI.Button(new Rect(300 + i * 50, 100 + j * 50, 50, 50), piece[board[i, j]]))
                        {
                            board[i, j] = turn;
                            turn = 3 - turn;
                        }
                    }
                }
            }
        }
    }
}
