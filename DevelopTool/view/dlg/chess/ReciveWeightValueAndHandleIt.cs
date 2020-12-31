
using System.Windows;

public class ReciveWeightValueAndHandleIt
{

    //use to store the value that the calculate weight value method returened 
    assStructArray[,] peoAssistantStructArray = new assStructArray[15, 15];
    assStructArray[,] macAssistantStructArray = new assStructArray[15, 15];

    //use to mark the max index of the carray
    const int chessMaxCount = 15;


    public void initializeVariable(int[,] array)
    {

        PrimaryLevelCalculateWeightValue primaryLevelcalculateMachineChessWeight = new PrimaryLevelCalculateWeightValue();

        //tranfer array to initialize variable
        primaryLevelcalculateMachineChessWeight.initializeScanChessTable(array);

        //calculate weight value
        primaryLevelcalculateMachineChessWeight.calculateWeightValue();

        //store people's weight value and point value
        peoAssistantStructArray = primaryLevelcalculateMachineChessWeight.peopleAssistantStructArray;

        //store people's weight value and point value
        macAssistantStructArray = primaryLevelcalculateMachineChessWeight.machineAssistantStructArray;

    }//end method initializeVariable




    #region return point by compare weight value in primary Level

    public Point primaryWeightValueCalculate()
    {
        int i, j, k;
        int flagValue = 0;

        Point point = new Point();

        point.X = point.Y = chessMaxCount;

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    // if machine find it can play five chess on a line ,it will win the game
                    if ((macAssistantStructArray[i, j].directionWeightValue[k] > 20) && (macAssistantStructArray[i, j].directionWeightValue[k] <= 25))
                    {
                        point.X = i;
                        point.Y = j;
                        return point;
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find people will have five chess on a line ,it will block it 
                    if ((peoAssistantStructArray[i, j].directionWeightValue[k] > 20) && (peoAssistantStructArray[i, j].directionWeightValue[k] <= 25))
                    {
                        point.X = i;
                        point.Y = j;
                        return point;
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find it has four chess on a line , it will win the game
                    if ((macAssistantStructArray[i, j].directionWeightValue[k] == 20))
                    {
                        point.X = i;
                        point.Y = j;
                        return point;
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find people has four chess on a line , it will block it
                    if ((peoAssistantStructArray[i, j].directionWeightValue[k] == 20))
                    {
                        point.X = i;
                        point.Y = j;
                        return point;
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find it has two"three chess on a line ", it will win the game
                    if ((macAssistantStructArray[i, j].directionWeightValue[k] >= 15))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            return point;
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }


        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find people has two"three chess on a line ", it will block it
                    if ((peoAssistantStructArray[i, j].directionWeightValue[k] >= 15))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            return point;
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find it has the condition"33->9"
                    if (macAssistantStructArray[i, j].directionWeightValue[k] >= 15)
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            if ((macAssistantStructArray[i, j].directionWeightValue[k] > 15) || (macAssistantStructArray[(int)point.X, (int)point.Y].directionWeightValue[k] > 15))
                            {
                                return point;
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find people has the condition "33->9"
                    if (peoAssistantStructArray[i, j].directionWeightValue[k] >= 15)
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            if ((peoAssistantStructArray[i, j].directionWeightValue[k] > 15) || (peoAssistantStructArray[(int)point.X, (int)point.Y].directionWeightValue[k] > 15))
                            {
                                return point;
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find it has two "33"
                    if ((macAssistantStructArray[i, j].directionWeightValue[k] >= 15))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            return point;
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find it people the condition"33->9)
                    if ((peoAssistantStructArray[i, j].directionWeightValue[k] >= 15))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            return point;
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find people has two "22"
                    if ((macAssistantStructArray[i, j].directionWeightValue[k] >= 10))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            return point;
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //if machine find people has two "22"
                    if ((peoAssistantStructArray[i, j].directionWeightValue[k] >= 10))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            return point;
                        }
                        else
                        {
                            point.X = i;
                            point.Y = j;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    //fill people's single chess
                    if ((peoAssistantStructArray[i, j].directionWeightValue[k] == 4))
                    {
                        if ((point.X == i) && (point.Y == j))
                        {
                            point.X = i;
                            point.Y = j;
                            return point;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                for (k = 0; k < 4; k++)
                {
                    if (peoAssistantStructArray[i, j].directionWeightValue[k] >= flagValue)
                    {   // machine use the max weght value to fill X and Y
                        flagValue = peoAssistantStructArray[i, j].directionWeightValue[k];
                        point.X = i;
                        point.Y = j;
                    }
                    else
                    {
                    }
                }
            }
        }

        return point;

    }//end method primaryWeightValueCalculate

    #endregion return point by compare weight value in primary Level


}

