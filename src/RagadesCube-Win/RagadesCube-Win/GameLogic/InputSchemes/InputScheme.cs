using System;
using RC.Input;
using Microsoft.Xna.Framework;
using RC.Engine.Cameras;
using RagadesCube.SceneObjects;

namespace RagadesCube.GameLogic.InputSchemes
{
    public abstract class RCGLInputScheme : RCInputScheme<RCGameLogic>
    {
        private IRCGamePlayerViewer _player;

        public void AttachPlayer(IRCGamePlayerViewer player)
        {
            _player = player;
        }

        protected IRCGamePlayerViewer Player
        {
            get { return _player; }
        }

        protected void Move(Vector2 where)
        {
            RCCamera camera = Player.Camera;

            if (Vector3.Dot(Player.CubeView.WorldTrans.Up, Vector3.Up) < 0)
                where = new Vector2(where.X, -where.Y);
            
            ControlItem.MovePlayerCube(Player.Index, camera.WorldTrans.Right, where);
        }

        protected void Rotate(RCCube.RotationDirection dir)
        {
            ControlItem.RotatePlayerCube(Player.Index, dir);
        }

        protected void MoveCursor(Vector2 position)
        {
            float dotMax = float.MinValue;
            RCCube.FaceSide selectedFace = RCCube.FaceSide.Top;

            foreach (RCCube.FaceSide face in Enum.GetValues(typeof(RCCube.FaceSide)))
            {
                Vector3 source = Vector3.Zero;

                switch (face)
                {
                    case RCCube.FaceSide.Back:
                        source = Player.CubeView.WorldTrans.Forward;
                        break;
                    case RCCube.FaceSide.Bottom:
                        source = Player.CubeView.WorldTrans.Down;
                        break;
                    case RCCube.FaceSide.Front:
                        source = Player.CubeView.WorldTrans.Backward;
                        break;
                    case RCCube.FaceSide.Left:
                        source = Player.CubeView.WorldTrans.Left;
                        break;
                    case RCCube.FaceSide.Right:
                        source = Player.CubeView.WorldTrans.Right;
                        break;
                    case RCCube.FaceSide.Top:
                        source = Player.CubeView.WorldTrans.Up;
                        break;
                }

                RCCamera camera = Player.Camera;

                Vector3 orgin = camera.Viewport.Project(
                    Player.CubeView.WorldTrans.Translation,
                    camera.Projection,
                    camera.View,
                    Matrix.Identity
                );

                Vector3 result = camera.Viewport.Project(
                    source,
                    camera.Projection,
                    camera.View,
                    Matrix.Identity
                );

                result -= orgin;

                Vector2 r1 = new Vector2(result.X, -result.Y);
                r1.Normalize();

                float dr = Vector2.Dot(position, r1);

                if (dr > dotMax)
                {
                    dotMax = dr;
                    selectedFace = face;
                }
            }

            ControlItem.SelectPlayerCube(Player.Index, selectedFace);
        }
    }
}
