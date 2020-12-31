using DevelopTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinCore;

public class ChessPlayer
{
    public bool IsBlack;
    bool turn;//轮到自己
    public void TurnToMove()
    {
        turn = true;

        if(IsBlack==false)
        {
            //AI
            AIMove();
        }
    }

    public void AIMove()
    {
        var reciveWeightValueAndHandleIt = new ReciveWeightValueAndHandleIt();
        reciveWeightValueAndHandleIt.initializeVariable(ChessModel.instance.chessMap);

        var point = reciveWeightValueAndHandleIt.primaryWeightValueCalculate();
        MoveChess((int)point.X, (int)point.Y);
    }


    /// <summary>
    /// 落子
    /// </summary>
    public void MoveChess(int X, int Y)
    {
        if (turn == false) return;
        if (FiveChessRule.instance.CanDown(X, Y, ChessModel.instance.chessMap) == false) return;
        ChessModel.instance.MoveChess(X, Y, IsBlack);
    }
}