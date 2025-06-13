using System.Collections.Generic;
using UnityEngine;

public class SmartAIStrategy : IAIStrategy
{
    public int GetMove(Board board)
    {
        var boardArray = board.GetBoard();
        //检查是否能取胜
        for (int i = 0; i < 9; i++)
        {
            if(boardArray[i%3, i/3] == 0)
            {
                //尝试落子
                boardArray[i%3, i/3] = 2; // AI 落子
                if (board.CheckWinner() == 2)
                {
                    boardArray[i%3, i/3] = 0;
                    return i; // 返回这个位置
                }
                boardArray[i%3, i/3] = 0; // 撤销落子
            }
        }
        //检查是否要防守
        for (int i = 0; i < 9; i++)
        {
            if (boardArray[i % 3, i / 3] == 0)
            {
                boardArray[i % 3, i / 3] = 1;
                if (board.CheckWinner() == 1)
                {
                    boardArray[i%3, i/3] = 0;
                    return i;
                }
                boardArray[i % 3, i / 3] = 0;
            }
        }
        //随机寻找空位
        List<int> available = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (boardArray[i % 3, i / 3] == 0)
            {
                available.Add(i);
            }
        }
        if (available.Count > 0)
        {
            int randomIndex = Random.Range(0, available.Count);
            return available[randomIndex];
        }

        return -1;
    }
}