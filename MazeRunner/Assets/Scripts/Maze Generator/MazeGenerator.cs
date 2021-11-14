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
        private IntPair entranceChunkPos;
        private IntPair exitChunkPos;
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

            // Calculates entrance and exit chunk
            entranceChunkPos = GetParrentChunk(entrance);
            exitChunkPos = GetParrentChunk(exit);
        }

        #endregion

        #region Class utility

        public bool[,] GetChunk(IntPair chunkPosition)
        {
            // Checks if the chunkPosition is within the bounds of the chunks array
            if (WithinBounds(new IntPair(chunks.GetLength(1), chunks.GetLength(0)), chunkPosition))
            {
                // Generates the chunk if its not already generated 
                if (!chunksExistence[chunkPosition.x, chunkPosition.y])
                {
                    GenerateChunk(chunkPosition);
                }
                // Returns the chunk
                return chunks[chunkPosition.x, chunkPosition.y];
            }
            else
            {
                // There is no chunk to be returned
                return null;
            }

        }

        private void GenerateChunk(IntPair chunkPosition)
        {
            // Prevents overwriting of existing chunks
            if (chunksExistence[chunkPosition.x, chunkPosition.y])
            {
                Debug.LogWarning($"En existing chunk has been tried to be generated!");
                return;
            }
            // Data for generation
            bool entranceChunk = chunkPosition == entranceChunkPos;
            bool exitChunk = chunkPosition == exitChunkPos;
            bool[] existingAdjacency = new bool[4] { false, false, false, false }; // [up, dawn, left, right]
            foreach (int xDiff in new int[] { 1, -1 })
            {
                IntPair intPairToVal = new IntPair(chunkPosition.x + xDiff, chunkPosition.y);
                if (WithinBounds(new IntPair(chunksExistence.GetLength(1), chunksExistence.GetLength(0)), intPairToVal))
                {
                    existingAdjacency[xDiff == 1 ? 0 : 1] = chunksExistence[intPairToVal.x, intPairToVal.y];
                }
            }
            foreach (int yDiff in new int[] { -1, 1 })
            {
                IntPair intPairToVal = new IntPair(chunkPosition.x, chunkPosition.y + yDiff);
                if (WithinBounds(new IntPair(chunksExistence.GetLength(1), chunksExistence.GetLength(0)), intPairToVal))
                {
                    existingAdjacency[yDiff == -1 ? 2 : 3] = chunksExistence[intPairToVal.x, intPairToVal.y];
                }
            }
            // The chunk most be adjacent an existing chunk or contain exit or entrance position
            if (!(entranceChunk || exitChunk || existingAdjacency[0] || existingAdjacency[1] || existingAdjacency[2] || existingAdjacency[3]))
            {
                Debug.LogError($"Only chunks adjacent to an existing chunk or containing exit or entrance position can be generated !");
                return;
            }

            // Generates the chunk

            // Creates chunk
            bool[,] chunk = new bool[chunkSize, chunkSize];
            // Fills the chunk with 0 / false / empty
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    chunk[x, y] = false;
                }
            }

            // Creates the chunkMeta
            bool[,] chunkMeta = new bool[chunkSize + 2, chunkSize + 2];
            // Initiates the chunkMeta
            for (int x = 0; x < chunkSize + 2; x++)
            {
                for (int y = 0; y < chunkSize + 2; y++)
                {
                    chunkMeta[x, y] = false;
                }
            }
            for (int i = 0; i < chunkSize; i++)
            {
                if (existingAdjacency[0]) // up
                {
                    chunkMeta[i+1, chunkSize + 1] = chunks[chunkPosition.x, chunkPosition.y + 1][i, 0]; 
                }
                if (existingAdjacency[1]) // dawn
                {
                    chunkMeta[i+1, 0] = chunks[chunkPosition.x, chunkPosition.y - 1][i, chunkSize - 1];
                }
                if (existingAdjacency[2]) // left
                {
                    chunkMeta[0, i+1] = chunks[chunkPosition.x - 1, chunkPosition.y][chunkSize - 1, i];
                }
                if (existingAdjacency[3]) // right
                {
                    chunkMeta[chunkSize + 1, i + 1] = chunks[chunkPosition.x + 1, chunkPosition.y][0, i];
                }
            }

            // If the chunk contains a entranceChunk point the generation will begins from there

            // If the chunk contains a exitChunk point there will be at least one path to it

            // Assigns the generated chunk to the chunk array
            chunks[chunkPosition.x, chunkPosition.y] = chunk;
        }

        // Return the chunk coordinate that contains the child point
        public IntPair GetParrentChunk(IntPair childPoint)
        {
            // Gets the chunk position
            int chunkX = Mathf.FloorToInt(childPoint.x / chunkSize);
            int chunkY = Mathf.FloorToInt(childPoint.y / chunkSize);

            // Returns the chunk coordinate
            return new IntPair(chunkX, chunkY);
        }

        // Returns true if the point is in the bound of dimensions
        public bool WithinBounds(IntPair dimensions, IntPair point)
        {
            return !(point.x < 0 || point.y < 0 || point.x >= dimensions.x || point.y >= dimensions.y);
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

        // ToString overwrite
        public override string ToString()
        {
            return $"{x}, {y}";
        }

        // GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Equals
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        // Operations
        public static bool operator ==(IntPair a, IntPair b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(IntPair a, IntPair b)
        {
            return a.x != b.x || a.y != b.y;
        }
    }
}

