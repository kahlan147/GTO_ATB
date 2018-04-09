/*
 * Arthur Cousseau, 2017
 * https://www.linkedin.com/in/arthurcousseau/
 * Please share this if you enjoy it! :)
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Piece_Data", menuName = "Eldoir/Piece")]
public class PieceData : ScriptableObject
{
    private const int defaultGridSize = 3;

    [Range(1, 20)]
    public int gridSize = defaultGridSize;

    public CellRow[] cells = new CellRow[defaultGridSize];


    public int[,] GetCells()
    {
        int[,] ret = new int[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                ret[i, j] = cells[i].row[j];
            }
        }

        return ret;
    }

    /// <summary>
    /// Just an example, you can remove this.
    /// </summary>
    public int GetCountActiveCells()
    {
        int count = 0;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                //if (cells[i].row[j]) count++;
            }
        }

        return count;
    }


    [System.Serializable]
    public class CellRow
    {
        public int[] row = new int[defaultGridSize];
    }
}
