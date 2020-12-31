public struct assStructArray
{
    public int pointValue;
    public int[] directionWeightValue;
}


public class PrimaryLevelCalculateWeightValue
{


    #region initialize globle variable

    const int chessMaxCount = 15;

    public assStructArray[,] peopleAssistantStructArray = new assStructArray[15, 15];
    public assStructArray[,] machineAssistantStructArray = new assStructArray[15, 15];

    public int[,] cheArray = new int[15, 15];

    #endregion initialize globle variable

    #region scan horizon weight weightValue

    public void scanPeopleHorizonWeightValue(int X, int Y)
    {

        int i = X;
        int j = Y;

        if (cheArray[X, Y] == 0)
        {
            //Scan space position weight value
            while (true)
            {
                // scan left position
                peopleAssistantStructArray[X, Y].directionWeightValue[0] += 5;
                i--;

                // over boundary line
                if (i < 0)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }
                //no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                //not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }

                //not own or opponent
                else
                {

                }
            }//end top while
            i = X;
            j = Y;
            while (true)
            {
                // scan right position
                i++;

                // over boundary line
                if (i >= chessMaxCount)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }

                // no chess
                else if (cheArray[X, Y] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }

                // own
                else
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[0] += 5;
                }
            }

        }

    }

    #endregion scan horizon weight weightValue

    #region scan vertical weight weightValue

    //Scan vertical weight value
    public void scanPeopleVerticalWeightValue(int X, int Y)
    {
        int i = X;
        int j = Y;


        // Scan null weight value
        if (cheArray[X, Y] == 0)
        {

            // Scan up weight value
            while (true)
            {
                peopleAssistantStructArray[X, Y].directionWeightValue[1] += 5;
                j--;

                // over boundary line
                if (j < 0)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                //not own or opponent
                else
                {
                }
            }

            i = X;
            j = Y;

            // scan down weight value
            while (true)
            {
                j++;

                // over boundary line
                if (j >= chessMaxCount)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                //not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                // own
                else
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[1] += 5;
                }
            }

        }
        else
        {
        }
    }


    #endregion scan vertical weight weightValue

    #region scan '\' weight weightValue

    // scan '\' table
    public void scanPeopleLeftUpWeightValue(int X, int Y)
    {
        int i = X;
        int j = Y;

        // Scan null weight value
        if (cheArray[X, Y] == 0)
        {
            // Scan up weight value
            while (true)
            {
                i--;
                j--;
                peopleAssistantStructArray[X, Y].directionWeightValue[2] += 5;

                // over boundary line
                if ((i < 0) || (j < 0))
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }


                else
                {
                }
            }
            i = X;
            j = Y;

            // scan down weight value
            while (true)
            {
                i++;
                j++;

                // over boundary line
                if ((i >= chessMaxCount) || (j >= chessMaxCount))
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }

                // own
                else
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[2] += 5;
                }
            }

        }
        else
        {
        }
    }


    #endregion scan '\' weight weightValue

    #region scan '/' weight weightValue

    // scan '\' table
    public void scanPeopleRightUpWeightValue(int X, int Y)
    {
        int i = X;
        int j = Y;

        // Scan space weight value
        if (cheArray[X, Y] == 0)
        {
            // scan left weight value
            while (true)
            {
                peopleAssistantStructArray[X, Y].directionWeightValue[3] += 5;
                i--;
                j++;

                // over boundary line
                if ((i < 0) || (j >= chessMaxCount))
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }


                else
                {
                }
            }
            i = X;
            j = Y;

            // scan right weight value
            while (true)
            {
                i++;
                j--;

                // over boundary line
                if ((i >= chessMaxCount) || j < 0)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[X, Y] != 1)
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }

                // own
                else
                {
                    peopleAssistantStructArray[X, Y].directionWeightValue[3] += 5;
                }
            }

        }

        else
        {
        }
    }

    #endregion scan '/' weight weightValue

    #region scan machine horizon weight weightValue

    // scan machine Horizon weight value
    public void scanMachineHorizonWeightValue(int X, int Y)
    {
        int i = X;
        int j = Y;

        // Scan space weight value
        if (cheArray[X, Y] == 0)
        {

            // scan left weight value
            while (true)
            {
                machineAssistantStructArray[X, Y].directionWeightValue[0] += 5;
                i--;

                // over boundary line
                if (i < 0)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }


                else
                {
                }
            }
            i = X;
            j = Y;

            // scan right weight value
            while (true)
            {
                i++;

                // over boundary line
                if (i >= chessMaxCount)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[0]--;
                    break;
                }


                else
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[0] += 5;
                }
            }
        }
        else
        {
        }
    }

    #endregion scan machine horizon weight weightValue

    #region scan machine vertical weight weightValue

    public void scanMachineVerticalWeightValue(int X, int Y)
    {
        int i = X;
        int j = Y;

        // Scan null weight value
        if (cheArray[X, Y] == 0)
        {

            // Scan up weight value
            while (true)
            {
                machineAssistantStructArray[X, Y].directionWeightValue[1] += 5;
                j--;

                // over boundary line
                if (j < 0)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }


                else
                {
                }
            }
            i = X;
            j = Y;

            // scan down weight value
            while (true)
            {
                j++;

                // over boundary line
                if (j >= chessMaxCount)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[1]--;
                    break;
                }

                // own
                else
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[1] += 5;
                }
            }

        }
        else
        {
        }
    }

    #endregion scan machine vertical weight weightValue

    #region scan machine '\' weight weightValue

    public void scanMachineLeftUpWeightValue(int X, int Y)
    {
        int i = X;
        int j = Y;


        // Scan null weight value
        if (cheArray[X, Y] == 0)
        {

            // Scan up weight value
            while (true)
            {
                machineAssistantStructArray[X, Y].directionWeightValue[2] += 5;
                i--;
                j--;

                // over boundary line
                if ((i < 0) || (j < 0))
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }


                else
                {
                }
            }
            i = X;
            j = Y;

            // scan down weight value
            while (true)
            {
                i++;
                j++;

                // over boundary line
                if ((i >= chessMaxCount) || (j >= chessMaxCount))
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[2]--;
                    break;
                }

                // own
                else
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[2] += 5;
                }
            }

        }
        else
        {
        }
    }

    #endregion scan machine '/' weight weightValue

    #region scan machine '/' weight weightValue

    // scan '\' table
    public void scanMachineRightUpWeightValue(int X, int Y)
    {

        int i = X;
        int j = Y;


        // Scan null weight value
        if (cheArray[X, Y] == 0)
        {

            // scan left weight value
            while (true)
            {
                machineAssistantStructArray[X, Y].directionWeightValue[3] += 5;
                i--;
                j++;

                // over boundary line
                if ((i < 0) || (j >= chessMaxCount))
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                // own
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }


                else
                {
                }
            }
            i = X;
            j = Y;

            // scan right weight value
            while (true)
            {
                i++;
                j--;

                // over boundary line
                if ((i >= chessMaxCount) || j < 0)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }

                // no chess
                else if (cheArray[i, j] == 0)
                {
                    break;
                }

                //not opponent
                else if (cheArray[i, j] != 2)
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[3]--;
                    break;
                }

                // own
                else
                {
                    machineAssistantStructArray[X, Y].directionWeightValue[3] += 5;
                }
            }

        }
        else
        {
        }
    }

    #endregion scan machine '\' weight weightValue     

    public void initializeScanChessTable(int[,] chArray)
    {

        int i, j;

        //initialize globle array(in this class) 
        cheArray = chArray;

        for (i = 0; i < chessMaxCount; i++)
        {
            for (j = 0; j < chessMaxCount; j++)
            {
                peopleAssistantStructArray[i, j].pointValue = chArray[i, j];
                peopleAssistantStructArray[i, j].directionWeightValue = new int[4] { 0, 0, 0, 0 };
                machineAssistantStructArray[i, j].pointValue = chArray[i, j];
                machineAssistantStructArray[i, j].directionWeightValue = new int[4] { 0, 0, 0, 0 };
            }//end inner for
        }//end outer for
    }//end method initializeScanChessTable

    public void calculateWeightValue()
    {

        int m, n;

        for (m = 0; m < chessMaxCount; m++)
        {
            for (n = 0; n < chessMaxCount; n++)
            {
                scanPeopleHorizonWeightValue(m, n);
                scanPeopleVerticalWeightValue(m, n);
                scanPeopleLeftUpWeightValue(m, n);
                scanPeopleRightUpWeightValue(m, n);

                scanMachineHorizonWeightValue(m, n);
                scanMachineVerticalWeightValue(m, n);
                scanMachineLeftUpWeightValue(m, n);
                scanMachineRightUpWeightValue(m, n);
            }//end inner for
        }//end outer for
    }//end method calculateWeightValue

}//end this class