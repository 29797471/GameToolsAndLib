using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/// <summary>
/// 五子棋规则
/// </summary>
public class FiveChessRule : Singleton<FiveChessRule>
{
    public bool CanDown(int X, int Y, int[,] chessMap)
    {
        return chessMap[X, Y] == 0;
    }

    /// <summary>
    /// 1白5连 2.黑5连 0.无
    /// </summary>
    int CalcHasFiveLine(int startX, int startY, int deltaX, int deltaY,int turnX,int turnY, int[,] chessMap)
    {
        int state = 0;//棋子颜色
        int number = 0;//横排连续棋子数
        int currentState = 0;//0.无子 1.白 2.黑
        for(int fromX=startX,fromY=startY; fromX>=0 && fromX < ChessModel.cell_number && fromY >= 0 && fromY < ChessModel.cell_number; fromX+= turnX, fromY+= turnY)
        {
            int x = fromX;//横坐标
            int y = fromY;//纵坐标
            while (x < ChessModel.cell_number && x>=0 && y < ChessModel.cell_number && y>=0)
            {
                currentState = chessMap[x, y];
                if (state != currentState)
                {
                    state = currentState;
                    number = 1;
                }
                else
                {
                    number++;
                    if (number >= ChessModel.five_chess && state != 0)
                    {
                        return currentState;
                    }
                }
                x += deltaX;
                y += deltaY;
            }
        }
        
        return 0;
    }
    /// <summary>
    /// 1白棋胜 2.黑棋胜 0.平局 -1.未分胜负
    /// </summary>
    public int GetState(ObservableCollection<Chessman> list, int[,] chessMap)
    {
        if (list.Count == ChessModel.cell_number * ChessModel.cell_number)
        {
            return 0;
        }
        else
        {
            int currentState = 0;
            //检查水平
            currentState = CalcHasFiveLine(0, 0, 1, 0,0,1, chessMap);
            if (currentState != 0) return currentState;
            //检查垂直
            currentState = CalcHasFiveLine(0, 0, 0, 1,1,0, chessMap);
            if (currentState != 0) return currentState;

            //检查\
            currentState = CalcHasFiveLine(0, 0, 1, 1, 1, 0, chessMap);
            if (currentState != 0) return currentState;
            currentState = CalcHasFiveLine(0, 0, 1, 1, 0, 1, chessMap);
            if (currentState != 0) return currentState;
            //检查/
            currentState = CalcHasFiveLine(0, ChessModel.cell_number-1, 1, -1, 1, 0, chessMap);
            if (currentState != 0) return currentState;
            //检查/
            currentState = CalcHasFiveLine(0, ChessModel.cell_number - 1, 1, -1, 0, -1, chessMap);
            if (currentState != 0) return currentState;
            return -1;
        }
    }
}