using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.SceneObjects;

namespace RagadesCubeWin.GameLogic
{
    public interface IRCCubeShuffer
    {
        void Shuffle(RCActionCube[] cubes);
        bool IsShuffling { get; }
    }

    class DefaultCubeShuffler : IRCCubeShuffer
    {
        private Random _rand;
        private RCActionCube[] _cubes;

        public DefaultCubeShuffler()
        {
            _rand = new Random();
        }

        public bool IsShuffling
        {
            get 
            {
                try
                {
                    foreach (RCActionCube cube in _cubes)
                        if (cube.IsRotating)
                            return true;
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Shuffle(RCActionCube[] cubes)
        {
            if (IsShuffling)
                throw new Exception("Cannot shuffle cube because busy.");

            _cubes = cubes;

            int rotationCount = Enum.GetValues(typeof(RCCube.RotationDirection)).Length;
            int faceSideCount = Enum.GetValues(typeof(RCCube.FaceSide)).Length;

            int faceSide = _rand.Next(0, faceSideCount);
            int rotate = _rand.Next(0, rotationCount);

            foreach (RCActionCube cube in cubes)
            {
                cube.Select((RCCube.FaceSide)faceSide);
                cube.Rotate((RCCube.RotationDirection)rotate);
            }
        }
    }
}
