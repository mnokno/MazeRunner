using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeRunner.Visualization;
using MazeRunner.Generator;

namespace MazeRunner
{
    public class GameManager : MonoBehaviour
    {
        // Test Variable
        public bool[,] testGridA = new bool[,] { { false, false, false, false, true, true},
                                                { false, false, false, false, true, false },
                                                { false, false, true,  true, true, false },
                                                { false, true,  true,  false, false, false },
                                                { true, true,  false,  false, false, false },
                                                { true,  false,  false, false, false, false } };

        public bool[,] testGridB = new bool[,] { { false, false, false, false, true},
                                                { false, false, false, false, true },
                                                { false, false, true,  true, true},
                                                { false, true,  true,  false, false },
                                                { true, true,  false,  false, false } };


        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<GridDisplay>().GenerateGrid(testGridA);
            MazeGenerator maze = new MazeGenerator(new IntPair(100, 100), 10);
        }

        // Update is called once per frame
        void Update()
        {

        }

    }

}
