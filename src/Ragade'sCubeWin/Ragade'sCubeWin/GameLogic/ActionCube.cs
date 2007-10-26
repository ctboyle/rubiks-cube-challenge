using System;
using Microsoft.Xna.Framework;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Animation.Controllers;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GameLogic.Scenes;
using System.Collections.Generic;

namespace RagadesCubeWin.GameLogic
{
    public interface IRCCubeViewer
    {
        Matrix LocalTrans { get; }
        Matrix WorldTrans { get; }
        bool IsSolved { get; }
    }

    public class RCActionCube : GameComponent, IRCCubeViewer
    {
        private RCCube _myCube;
        private RCCubeController _controller;
        private RCCubeCursor _cursor;
        private Vector2 _moveVector;
        private Vector3 _axisVector;

        public RCActionCube(Game game)
            : base(game)
        {
            _moveVector = new Vector2();
            _myCube = new RCCube(3, 3, 3);
            _controller = new RCCubeController();
            _cursor = new RCCubeCursor(_myCube);

            _myCube.AttachController(_controller);
            _myCube.AddChild(_cursor);

            Game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            _myCube.LocalTrans = Matrix.CreateRotationY(_moveVector.Y)
                * Matrix.CreateFromAxisAngle(_axisVector, _moveVector.X);
        }

        public void AttachToScene(RCCubeSceneCreator scene)
        {
            scene.AttachCube(_myCube);
        }

        public void Select(RCCube.FaceSide selectedFace)
        {
            _cursor.SelectedFace = selectedFace;
        }

        public void Move(Vector3 axis, Vector2 where)
        {
            _moveVector += where;
            _axisVector = axis;
        }

        public void Rotate(RCCube.RotationDirection rotationDir)
        {
            RCCube.FaceSide faceSide = _cursor.SelectedFace;
            _controller.RotateFace(faceSide, rotationDir);
        }

        public Matrix LocalTrans
        {
            get { return _myCube.LocalTrans; }
        }
 
        public Matrix WorldTrans
        {
            get { return _myCube.WorldTrans; }
        }

        public bool IsSolved
        {
            get
            {
                foreach (RCCube.FaceSide face in Enum.GetValues(typeof(RCCube.FaceSide)))
                {
                    List<RCFacelet> facelets = _myCube.GetFaceletsOnFace(face);

                    if(facelets.Count > 1)
                    {
                        Color c = default(Color);

                        foreach (RCFacelet facelet in facelets)
                        {
                            if (facelet == null)
                                continue;
                            else
                            {
                                if (c == default(Color))
                                    c = facelet.Color;

                                if (facelet.Color != c)
                                    return false;
                            }
                        }
                    }
                }

                return true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Game.Components.Remove(this);

            base.Dispose(disposing);
        }
    }
}
