using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.LabirintTests
{
    internal class LevelGeneratorTests
    {
        public int CountChecks = 50;

        [Test]
        public void _0_Exist_MainLabirint()
        {
            bool flag = true;
            for (int i = 0; i < CountChecks; i++)
            {
                Vector2Int size = MazeGenerator2D.GenerateMazeRandomSize(i);

                LevelGenerator2D levelGenerator = new(size.x, size.y);
                Level level = levelGenerator.GenerateLevel();
                if (level.MainMaze == null)
                {
                    flag = false;
                    break;
                }

            }

            Assert.IsTrue(flag);
        }

        [Test]
        public void _1_Exist_SecondaryLabirints()
        {
            bool flag = true;
            for (int i = 1; i < CountChecks; i++)
            {
                Vector2Int size = MazeGenerator2D.GenerateMazeRandomSize(i);

                LevelGenerator2D levelGenerator = new(size.x, size.y);
                levelGenerator.GenerateSecondaryMazes = true;

                Level level = levelGenerator.GenerateLevel();
                if (level.SecondaryMazes.Count == 0)
                {
                    flag = false;
                    break;
                }
            }

            Assert.IsTrue(flag);
        }

        [Test]
        public void _2_Check_Main_Portal_Links()
        {

            bool flag = true;
            for (int i = 0; i < CountChecks; i++)
            {
                Vector2Int size = MazeGenerator2D.GenerateMazeRandomSize(i);

                LevelGenerator2D levelGenerator = new(size.x, size.y);
                levelGenerator.GenerateSecondaryMazes = true;

                Level level = levelGenerator.GenerateLevel();

                foreach (PortalIn portalIn in level.MainMaze.PortalsIn)
                {
                    if (level.GetMazeByName(portalIn.FromMazeName) == null)
                    {
                        //Debug.Log(portalIn.FromMazeName);
                        flag = false;
                        break;
                    }
                }

                foreach (PortalOut portalOut in level.MainMaze.PortalsOut)
                {
                    if (level.GetMazeByName(portalOut.ToMazeName) == null)
                    {
                        //Debug.Log(portalOut.ToMazeName);
                        flag = false;
                        break;
                    }
                }

                if (!flag)
                    break;

            }

            Assert.IsTrue(flag);

        }

        [Test]
        public void _3_Check_All_Portal_Links()
        {

            bool flag = true;
            for (int i = 0; i < CountChecks; i++)
            {
                Vector2Int size = MazeGenerator2D.GenerateMazeRandomSize(i);

                LevelGenerator2D levelGenerator = new(size.x, size.y);
                levelGenerator.GenerateSecondaryMazes = true;

                Level level = levelGenerator.GenerateLevel();

                foreach (Maze maze in level.GetAllMazes())
                {

                    foreach (PortalIn portalIn in maze.PortalsIn)
                    {
                        if (level.GetMazeByName(portalIn.FromMazeName) == null)
                        {
                            flag = false;
                            break;
                        }
                    }

                    foreach (PortalOut portalOut in maze.PortalsOut)
                    {
                        if (level.GetMazeByName(portalOut.ToMazeName) == null)
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (!flag)
                        break;
                }

            }

            Assert.IsTrue(flag);
        }

    }
}
