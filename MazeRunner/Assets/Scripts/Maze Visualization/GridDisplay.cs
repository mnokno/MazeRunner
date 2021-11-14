using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace MazeRunner.Visualization
{
    /// <summary>
    /// Only support square grids
    /// </summary>
    public class GridDisplay : MonoBehaviour
    {
        #region Class variables

        // Grid display sprites (the sprites should have resolution of 64)
        public Sprite spriteTrue;
        public Sprite spriteFalse;

        // Grid data storage
        private bool[,] gridData;
        // Gird display sprite data
        private GameObject[,] gridDisplayData;

        // Dimensions
        private int height;
        private int width;

        // Flags
        public bool existing { get; private set; } = false;

        #endregion

        #region Utility

        public void GenerateGrid(bool[,] gridData)
        {
            // If the gird is already generated it has to be destroyed
            if (existing)
            {
                DestroyGrid();
            }

            // Extracts the dimensions
            height = gridData.GetLength(0);
            width = gridData.GetLength(1);

            // Initiates the arrays
            this.gridData = gridData;
            gridDisplayData = new GameObject[height, width];

            // Generates the visual maze
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject go = new GameObject(); // Creates new game object
                    go.transform.SetParent(this.transform); // Assignees parent
                    SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>(); // Adds sprite renderer component
                    spriteRenderer.sprite = gridData[y, x] ? spriteTrue : spriteFalse; // Assignees sprite
                    go.transform.localPosition = GetLocalPosition(x, y); // Gets the target local position
                    go.transform.localScale = new Vector3((1f / width) * (100f / 64f), (1f / height) * (100f / 64f), 0); // Sets local scale
                    gridDisplayData[y, x] = go; // Saves reference to the game object
                }
            }
        }

        public void DestroyGrid()
        {
            // Destroys all sprites contained in this grid
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Destroy(gridDisplayData[y, x]); // Destroys the game object
                }
            }
        }

        private Vector3 GetLocalPosition(int x, int y)
        {
            // Creates the local variables
            float desX;
            float desY;

            if (width % 2 == 0) // even
            {
                // Calculates the x destination
                desX = (-(width / 2) + 0.5f + x) * (1f / width);
                // Calculates the y destination
                desY = -(-(height / 2) + 0.5f + y) * (1f / height);
            }
            else // odd
            {
                // Calculates the x destination
                desX = (-((width - 1) / 2) + x) * (1f / width);
                // Calculates the y destination
                desY = -(-((height - 1) / 2) + y) * (1f / height);
            }

            // Returns the calculated position
            return new Vector3(desX, desY, 0);
        }
        #endregion
    }
} 

