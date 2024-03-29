using System;
using Microsoft.Xna.Framework;
using RagadesCube.SceneObjects;
using RagadesCube.Controllers;
using Microsoft.Xna.Framework.Graphics;
using RagadesCube.Scenes;
using System.Collections.Generic;
using RC.Engine.Cameras;
using RC.Engine.Animation;


namespace RagadesCube.GameLogic
{
    public interface IRCCubeViewer
    {
        int MoveCount { get; }
        Matrix LocalTrans { get; }
        Matrix WorldTrans { get; }
        bool IsSolved { get; }
    }

    public class RCActionCube : GameComponent, IRCCubeViewer
    {
        public delegate void ActionCubeEventHandler(RCActionCube cube);
        public event ActionCubeEventHandler RotateAnimationComplete;

        private bool _isMoving;
        private bool _isOrienting;
        private int _moveCount;
        private RCCube _myCube;
        private RCCubeController _faceRotateController;
        private RCKeyFrameController<RCCube> _orientController;
        private RCCubeCursor _cursor;
        private Vector2 _moveVector;
        private Vector2 _moveVectorDelta;
        private Vector3 _xAxis;
        private Vector3 _yAxis;
        private RCCube.FaceSide _upFace;
        private Matrix _currentBase;
 

        public RCActionCube(Game game)
            : base(game)
        {
            _moveCount = 0;
            _moveVector = new Vector2();
            _moveVectorDelta = new Vector2();

            _myCube = new RCCube(3, 3, 3);
            _faceRotateController = new RCCubeController();
            _orientController = new RCKeyFrameController<RCCube>(); 
            _cursor = new RCCubeCursor(_myCube);
            _faceRotateController.AttachToObject(_myCube);
            _orientController.AttachToObject(_myCube);
            _myCube.AddChild(_cursor);

            _upFace = RCCube.FaceSide.Top;
            _currentBase = Matrix.Identity;

            _faceRotateController.OnComplete += OnAnimationComplete;
            _orientController.OnComplete += OnOrientComplete;
            Game.Components.Add(this);
        }

        public int MoveCount
        {
            get { return _moveCount; }
        }

        public int Length
        {
            get { return _myCube.Length; }
        }

        public int Width
        {
            get { return _myCube.Width; }
        }

        public int Height
        {
            get { return _myCube.Height; }
        }

        public void ResetMoveCount()
        {
            _moveCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _cursor.IsVisible = !_faceRotateController.IsAnimating;
       
            if (_isMoving)
            {
                _moveVector += _moveVectorDelta * (float)gameTime.ElapsedGameTime.TotalSeconds;

                Matrix yRot = Matrix.CreateFromAxisAngle(
                    _xAxis, 
                    _moveVector.X
                    );
                Matrix xRot = Matrix.CreateFromAxisAngle(
                    Vector3.Up,
                    _moveVector.Y
                    );
                Matrix totalRot = xRot * yRot;

                _myCube.LocalTrans = _currentBase * totalRot;
                _isMoving = false;
            }
        }

        public RCCubeScene CreateScene(Viewport vp)
        {
            RCCubeScene scene = new RCCubeScene(vp, _myCube);
            return scene;
        }

        public void Select(RCCube.FaceSide selectedFace)
        {
            _cursor.SelectedFace = selectedFace;
        }

        public void Orient()
        {
            if (!_isOrienting)
            {
                Vector3 selectedFaceNormal = _myCube.GetWorldFaceNormal(_cursor.SelectedFace);
                Vector3 worldRotateAxis = Vector3.Cross(selectedFaceNormal, Vector3.Up);

                _upFace = _cursor.SelectedFace;


                // Using the cross product definition I am able to find the angle between the vectors.
                // Math is simpler as I am using 2 source unit vectors.
                float angleToRotate = (float)Math.Asin(worldRotateAxis.Length());

                if (float.IsNaN(angleToRotate))
                {
                    angleToRotate = MathHelper.PiOver2;
                }

                float roationDir = (float)Vector3.Dot(selectedFaceNormal, Vector3.Up);
                if (roationDir > 0)
                {
                    angleToRotate = MathHelper.Pi - angleToRotate;
                }
                else if (roationDir == 1.0f)
                {
                    angleToRotate = MathHelper.Pi;
                    worldRotateAxis = Vector3.UnitX;

                }
                else if (roationDir == -1.0f)
                {
                    angleToRotate = 0.0f;
                    return;
                }


                worldRotateAxis.Normalize();

                Matrix cubeDestination = LocalTrans * Matrix.CreateFromAxisAngle(
                    worldRotateAxis,
                    -angleToRotate
                    );


                _orientController.RotationMode = RCKeyFrameController<RCCube>.InterpolationMode.SmoothStep;
                _orientController.BeginAnimation(_myCube.LocalTrans, cubeDestination, 1.0f);
                _isOrienting = true;

            }
        }

        public void Move(Vector3 xAxis, Vector3 yAxis, Vector2 where)
        {
            if (IsMoving) return;
            
            _xAxis = xAxis;
            _yAxis = yAxis;
            _moveVectorDelta = where;
            _isMoving = true;
        }

        public void Rotate(Vector3 viewVector, RCCube.RotationDirection rotationDir)
        {
            if (IsRotating) return;

            RCCube.FaceSide faceSide = _cursor.SelectedFace;
            Vector3 faceNormal = _myCube.GetWorldFaceNormal(faceSide);

            if (Vector3.Dot(viewVector, faceNormal) < 0)
            {
                if (rotationDir == RCCube.RotationDirection.Clockwise)
                    rotationDir = RCCube.RotationDirection.CounterClockwise;
                else
                    rotationDir = RCCube.RotationDirection.Clockwise;
            }

            Rotate(rotationDir);
        }

        public void Rotate(RCCube.RotationDirection rotationDir)
        {
            if (IsRotating) return;
            _faceRotateController.RotateFace(_cursor.SelectedFace, rotationDir);
            _moveCount++;
        }

        public Matrix LocalTrans
        {
            get { return _myCube.LocalTrans; }
        }
 
        public Matrix WorldTrans
        {
            get { return _myCube.WorldTrans; }
        }

        public bool IsMoving
        {
            get { return _isMoving; }
        }

        public bool IsRotating
        {
            get { return _faceRotateController.IsAnimating; }
        }

        public bool IsSolved
        {
            get
            {
#if !XBOX

                foreach(RCCube.FaceSide face in Enum.GetValues(typeof(RCCube.FaceSide)))

#else

                foreach (RCCube.FaceSide face in EnumHelper.GetValues(typeof(RCCube.FaceSide)))

#endif

                {
                    List<RCFacelet> facelets = _myCube.GetFaceletsOnFace(face);
                    string colorName = "";

                    if (facelets.Count == 0) return false;

                    foreach (RCFacelet facelet in facelets)
                    {
                        if(colorName == "")
                            colorName = facelet.Color.ToString();
                        else if(colorName != facelet.Color.ToString())
                            return false;
                    }
                }

                return true;
            }
        }

        protected void OnAnimationComplete(object sender, EventArgs e)
        {
            if (RotateAnimationComplete != null)
            {
                RotateAnimationComplete(this);
            }
        }

        protected void OnOrientComplete()
        {
            _currentBase = _myCube.LocalTrans;
            _moveVector = Vector2.Zero;
            _isOrienting = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Game.Components.Remove(this);

            base.Dispose(disposing);
        }
    }
}
