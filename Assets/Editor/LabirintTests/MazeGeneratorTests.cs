
using NUnit.Framework;
using System;
using UnityEngine;

namespace Assets.Editor.LabirintTests
{
    public class MazeGeneratorTests
    {
        public static int CountChecks = 50;

        public class StructureTests
        {

        }

        public class PointsTests
        {

            [Test]
            public void _0_Check_Main_Enter_And_Finish_Point()
            {
                bool flag = true;
                for (int i = 0; i < CountChecks; i++)
                {
                    Vector2Int size = MazeGenerator2D.GenerateMazeRandomSize(i);

                    MazeGenerator2D mazeGenerator = new MazeGenerator2D(0, MazeAreaType.Main, size.x, size.y, Vector2Int.zero);
                    Maze maze = mazeGenerator.Generate();

                    if (maze.StartPosition == -Vector2Int.one || maze.StartPosition.x >= size.x || maze.StartPosition.y >= size.y)
                    {
                        flag = false;
                        break;
                    }

                    if (maze.FinishPosition == -Vector2Int.one || maze.FinishPosition.x >= size.x || maze.FinishPosition.y >= size.y)
                    {
                        flag = false;
                        break;
                    }


                }

                Assert.IsTrue(flag);
            }

            [Test]
            public void _1_Check_Secondary_Enter_And_Finish_Point()
            {
                bool flag = true;
                for (int i = 0; i < CountChecks; i++)
                {
                    Vector2Int size = MazeGenerator2D.GenerateMazeRandomSize(i);

                    foreach (MazeAreaType type in (MazeAreaType[])Enum.GetValues(typeof(MazeAreaType)))
                    {
                        if (type == MazeAreaType.Main) continue;

                        MazeGenerator2D mazeGenerator = new MazeGenerator2D(0, type, size.x, size.y, Vector2Int.zero);
                        Maze maze = mazeGenerator.Generate();

                        if (maze.StartPosition != -Vector2Int.one)
                        {
                            break;
                        }

                        if (maze.FinishPosition != -Vector2Int.one)
                        {
                            flag = false;
                            break;
                        }
                    }


                }

                Assert.IsTrue(flag);
            }

        }

    }
}
