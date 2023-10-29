
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;

namespace Assets.Editor.LabirintTests
{
    public class MazeGeneratorTests
    {
        private static int MinWidth = 2;
        private static int MinHeight = 2;
        private static int MaxWidth = 100;
        private static int MaxHeight = 100;

        private static int MinMainWidth = 21;
        private static int MinMainHeight = 21;
        private static int MaxMainWidth = 99;
        private static int MaxMainHeight = 99;

        public static List<Vector2Int> GetTestMazeSizeVariations(MazeAreaType areaType)
        {
            List<Vector2Int> sizes = new List<Vector2Int>();
            if (areaType == MazeAreaType.Field)
            {
                sizes.Add(new Vector2Int());
            }
            else if (areaType == MazeAreaType.Room) { }


            else if (areaType == MazeAreaType.Corridor) {
                sizes.Add(new Vector2Int(MinWidth, MaxHeight));
                sizes.Add(new Vector2Int(MaxWidth, MinHeight));
            }
            else
            {
                sizes.Add(new Vector2Int(MinMainWidth, MinMainHeight));
                sizes.Add(new Vector2Int(MaxMainWidth, MaxMainHeight));
                sizes.Add(new Vector2Int(MinMainWidth, MaxMainHeight));
                sizes.Add(new Vector2Int(MaxMainWidth, MinMainHeight));
            }

            return sizes;
        }

        public class StructureTests
        {

        }

        public class PointsTests
        {

            [Test]
            public void _0_Check_Main_Enter_And_Finish_Point()
            {
                bool flag = true;
                foreach (Vector2Int size in GetTestMazeSizeVariations(MazeAreaType.Main))
                {

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
                foreach (Vector2Int size in GetTestMazeSizeVariations(MazeAreaType.Main))
                {

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
