using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WinCore;

namespace DevelopTool
{
    public class ChessModel:SingleNotifyObject<ChessModel>
    {
        public static int ViewPosToDataPos(double t)
        {
            return (int)Math.Floor(t / cell_size + 0.5);
        }
        public static double DataPosToViewPos(int t)
        {
            return t * cell_size - chessman_size / 2;
        }
        /// <summary>
        /// 界面棋子数据结构
        /// </summary>
        public ObservableCollection<Chessman> ChessmanList
        {
            get { if (mChessmanList == null) mChessmanList = new ObservableCollection<Chessman>();return mChessmanList; }
            set { mChessmanList = value;Update("ChessmanList"); }
        }
        public ObservableCollection<Chessman> mChessmanList;
        /// <summary>
        /// 单元格大小
        /// </summary>
        public const int cell_size = 30;
        /// <summary>
        /// 棋子大小
        /// </summary>
        public const double chessman_size = 28;
        /// <summary>
        /// 15条线
        /// </summary>
        public const int cell_number = 15;//14个格子，15条线，横纵15个点
        public int[,] chessMap;
        
        public const int five_chess = 5;//5子棋

        
        const int BLACK = 2;
        const int WHITE = 1;

        public ChessPlayer[] players;
        public int turnIndex;//该谁落子
        public void Init()
        {
            players = new ChessPlayer[2];
            players[0] = new ChessPlayer() { IsBlack = true };
            players[1] = new ChessPlayer() { IsBlack = false};
            chessMap = new int[cell_number , cell_number ];
            ChessmanList = new ObservableCollection<Chessman>();
            turnIndex = 0;
            Next();
        }
        void Next()
        {
            var last = turnIndex;
            turnIndex = (turnIndex + 1) % players.Length;
            players[last].TurnToMove();
        }
        public void Remove()
        {
            for(int i=0;i<2;i++)
            {
                var it=ChessmanList.Last();
                chessMap[it.x, it.y] = 0;
                ChessmanList.Remove(it);
            }
        }
        /// <summary>
        /// 落子
        /// </summary>
        public void MoveChess(int X, int Y, bool IsBlack)
        {
            ChessmanList.Add(new Chessman() { x = X, y = Y, isBlack = IsBlack });
            chessMap[X, Y] = IsBlack?BLACK:WHITE;
            Check();
        }

        void Check()
        {
            var result = FiveChessRule.instance.GetState(ChessmanList, chessMap);
            switch (result)
            {
                case 0:
                    CustomMessageBox.Show("平局");
                    break;
                case -1:
                    Next();
                    break;
                default:
                    CustomMessageBox.Show((BLACK == result) ? "玩家胜利" : "玩家失败");
                    break;
            }
        }

    }
}
