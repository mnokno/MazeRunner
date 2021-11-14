using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeRunner.Generator
{
    public class MazeGenerator
    {
        #region Class variables

        private bool[,][,] chunks;
        private bool[,] chunksExistence;
        private IntPair mazeDimensions;
        private IntPair entrance;
        private IntPair exit;
        private int chunkSize;

        #endregion

        #region Class constructor

        /// <summary>
        /// Provides an interface to define a maze and get chunks
        /// </summary>
        /// <param name="mazeDimensions"> chunkSize most be a multiple of mazeDimensions </param>
        /// /// <param name="chunkSize"> chunkSize most be a multiple of mazeDimensions </param>
        public MazeGenerator(IntPair mazeDimensions, int chunkSize) 
        {
            // Validates inputs
            if (mazeDimensions.x % chunkSize != 0 || mazeDimensions.y % chunkSize != 0)
            {
                throw new System.Exception("Dimension Exception: chunkSize most a multiple of mazeDimensions.x and mazeDimensions.y !");
            }
            if (mazeDimensions.x < 8 || mazeDimensions.y < 8 || chunkSize < 8)
            {
                throw new System.Exception("Dimension Exception: mazeDimensions.x, mazeDimensions.y and chunkSize most be greater or equal to 8 !");
            }

            // Initiates variables
            this.mazeDimensions = mazeDimensions;
            this.chunkSize = chunkSize;
            chunks = new bool[mazeDimensions.x / chunkSize, mazeDimensions.y / chunkSize][,];
            chunksExistence = new bool[mazeDimensions.x / chunkSize, mazeDimensions.y / chunkSize];
            for (int x = 0; x < mazeDimensions.x / chunkSize; x++)
            {
                for (int y = 0; y < mazeDimensions.y / chunkSize; y++)
                {
                    chunksExistence[x, y] = false;
                }
            }

            // Choses entrance and exit at random
            entrance = Random.Range(0, 2) == 0 ? new IntPair(Random.Range(0, mazeDimensions.x), 0) : new IntPair(0, Random.Range(0, mazeDimensions.y));
            do
            {
                exit = Random.Range(0, 2) == 0 ? new IntPair(Random.Range(0, mazeDimensions.x), 0) : new IntPair(0, Random.Range(0, mazeDimensions.y));
            }
            while (Mathf.Abs(entrance.x - exit.x) + Mathf.Abs(entrance.x - exit.x) < chunkSize);
        }

        #endregion

        #region Class utility

        public bool[,] GetChunk(IntPair chunkPosition)
        {
            // Returns the chunk
            return null;
        }

        private bool[,] GenerateChunk(IntPair chunkPosition)
        {
            // Return the chunk
            return null;
        }

        // Return the chunk coordinate that contains the child point
        private IntPair GetParrentChunk(IntPair childPoint)
        {
            // Returns the chunk coordinate
            return new IntPair(-1, -1);
        }

        #endregion
    }

    public struct IntPair
    {
        // Struct variables
        public int x;
        public int y;

        // Struct constructor
        public IntPair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

