using System;
using System.Collections.Generic;
using System.Text;
using RC.Gui.Primitives;
using RC.Gui.Fonts;
using Microsoft.Xna.Framework.Graphics;

using RC.Engine.SoundManagement;

namespace RC.Gui.Controls.Control_Subclasses
{
    /// <summary>
    /// Two buttons beneath an image.  A button is clicked on to change the image.  Each image has a 
    /// uniquely identifying string 'key' that the code usese to identify which image the user has selected.
    /// </summary>
    public class RCSpinner : RCControl
    {
        #region    ------------------------------Private Data Members

        /// <summary>
        /// The Previous button; decrements the RCSpinner's list.
        /// </summary>
        private RCButton prevButton = null;

        /// <summary>
        /// The Next button; increments the RCSpinner's list.
        /// </summary>
        private RCButton nextButton = null;

        /// <summary>
        /// The file name of the image over the Previous button.
        /// </summary>
        private string _prevButtonBaseFileNameMinusExtension = "EmergingArrowImageLeft";

        /// <summary>
        /// The file name of the image over the Next button.
        /// </summary>
        private string _nextButtonBaseFileNameMinusExtension = "EmergingArrowImageRight";

        /// <summary>
        /// The file name of the image over the Previous button when it is in focus.
        /// </summary>
        private string _prevButtonFocusedFileNameMinusExtension = "EmergingArrowImageLeftSelected";

        /// <summary>
        /// The file name of the image over the Next button when it is in focus.
        /// </summary>
        private string _nextButtonFocusedFileNameMinusExtension = "EmergingArrowImageRightSelected";

        /// <summary>
        /// The file name of the image over the Next button while it is being pressed.
        /// </summary>
        private string _nextButtonActivatedFileNameMinusExtension = "EmergingArrowImageRightActivating";
        
        /// <summary>
        /// The file name of the image over the Previous button while it is being pressed.
        /// </summary>
        private string _prevButtonActivatedFileNameMinusExtension = "EmergingArrowImageLeftActivating";

        /// <summary>
        /// The background image for the spinning image.
        /// </summary>
        private RCQuad imageBeingSpun;

        /// <summary>
        /// The file name of the image presently showing on the RCSpinner.
        /// </summary>
        private string _imageBeingSpunFileNameMinusExtension = "nothing";

        /// <summary>
        /// A list of all of the images.  Indices should coordinate with texts, imageFiles, and keys.
        /// </summary>
        private List<RCQuad> images = new List<RCQuad>();

        /// <summary>
        /// A list of all of the images' file names.  Indices should coordinate with texts, keys, and images.
        /// </summary>
        private List<string> imageFiles = new List<string>();

        /// <summary>
        /// A list of all of the RCText objects.  Indices should coordinate with keys, imageFiles, and images.
        /// </summary>
        private List<RCText> texts = new List<RCText>();

        /// <summary>
        /// A list of all of the keys.  Indices should coordinate with texts, imageFiles, and images.
        /// </summary>
        private List<string> keys = new List<string>();

        /// <summary>
        /// The index of the current string.  This index is used to coordinate between
        /// the string-key, the RCText object, the image, and the image's file name.
        /// </summary>
        private int curIndex = 0;
        

        #endregion ------------------------------Private Data Members


        #region    ------------------------------Events and their delegates


        /// <summary>
        /// The delegate for RCSpinner events with no parameters.
        /// </summary>
        public delegate void RCSpinnerEvent();

        /// <summary>
        /// Thrown when an action button has been pressed over the spinner but is then cancelled.
        /// </summary>
        public event RCSpinnerEvent Declined;


        /// <summary>
        /// Thrown when an action button has been pressed and released over the spinner. 
        /// </summary>
        public event RCSpinnerEvent Accepted;



        #endregion ------------------------------Events and their delegates


        #region    ------------------------------Public Read-Only Properties

        /// <summary>
        /// Returns the current key.
        /// </summary>
        public string currentKey
        {
            get { return keys[curIndex]; }
        }

        #endregion ------------------------------Public Read-Only Properties



        /// <summary>
        /// Creates a new RCSpinner.  Note that the image being spun is dimensioned at the full width and 1/2 of the height of the
        /// RCSpinner, and the two buttons are 1/4 the width and 1/5 the height of the RCSpinner.  The buttons are located 3 pixels
        /// beneath the image being spun, and are centered beneath the image being spun with the size of one button between them.
        /// </summary>
        /// <param name="width">The width of the entire spinner, in world coordinates.</param>
        /// <param name="height">The height of the entire spinner, in world coordinates.</param>
        /// <param name="screenWidth">The width of the entire spinner, in pixels.</param>
        /// <param name="screenHeight">The height of the entire spinner, in pixels.</param>
        /// <param name="Font">The font used for the internal buttons.  Not nullable, but doesn't really matter at this point.</param>
        public RCSpinner(
            float width,
            float height,
            int screenWidth,
            int screenHeight,
            BitmapFont Font
        )
            : base(
            width,
            height,
            screenWidth,
            screenHeight
        )
        {
            nextButton = new RCButton(width/5, height/4, screenWidth/5, screenHeight/4, Font);
            prevButton = new RCButton(width/5, height/4, screenWidth/5, screenHeight/4, Font);

            imageBeingSpun = new RCQuad(width, height / 2, screenWidth, screenHeight / 2);

            nextButton.baseImageFileName = _nextButtonBaseFileNameMinusExtension;
            nextButton.focusedImageFileName = _nextButtonFocusedFileNameMinusExtension;
            nextButton.activatingImageFileName = _nextButtonActivatedFileNameMinusExtension;
            
            prevButton.baseImageFileName = _prevButtonBaseFileNameMinusExtension;
            prevButton.focusedImageFileName = _prevButtonFocusedFileNameMinusExtension;
            prevButton.activatingImageFileName = _prevButtonActivatedFileNameMinusExtension;

            nextButton.AfterPressedAndReleased += spinUp;
            prevButton.AfterPressedAndReleased += spinDown;
            


            AddChild(nextButton, (int)(3 * width / 5), imageBeingSpun.ScreenHeight + 3, 0);
            AddChild(prevButton, (int)(width / 5), imageBeingSpun.ScreenHeight + 3, 0);
            AddChild(imageBeingSpun, (int)(width/8), 0, 0);

        }

        

        #region    ------------------------------Public spinner manipulations 

        #region    ----------------------------------addSpinItem (+3 overrides)

        /// <summary>
        /// Adds a new item to the list that the spinner will seek to.
        /// </summary>
        /// <param name="key">The key that external code will use to refer to the new data.</param>
        /// <param name="text">The text string of at least zero-length that will be output to represent the new data.</param>
        /// <param name="imageFileName">The file name of the image (.png) that will be displayed to represent the new data.  Pass "nothing" to use no image.</param>
        /// <param name="Font">The Font that <paramref name="text"/> shall be output in.  Is NOT nullable.</param>
        [notFullyImplemented("Validate new item")]
        public void addSpinItem(string key, string text, string imageFileName, BitmapFont Font)
        {
            keys.Add(key);
            addImageToListByFileName(key, imageFileName);
            addTextToListByString(key, text, Font);
        }

        /// <summary>
        /// Adds a new item to the list that the spinner will seek to.
        /// </summary>
        /// <param name="key">The key that external code will use to refer to the new data.</param>
        /// <param name="text">The text string of at least zero-length that will be output to represent the new data.</param>
        /// <param name="image">The (nullable) RCImage item that will be displayed to represent the new data.</param>
        /// <param name="Font">The Font that <paramref name="text"/> shall be output in.  Is NOT nullable.</param>
        [notFullyImplemented("Validate new item")]
        public void addSpinItem(string key, string text, RCQuad image, BitmapFont Font)
        {
            images.Add(image);
            addTextToListByString(key, text, Font);
        }

        /// <summary>
        /// Adds a new item to the list that the spinner will seek to.
        /// </summary>
        /// <param name="key">The key that external code will use to refer to the new data.</param>
        /// <param name="textObject">The (nullable) RCText item that will be displayed to represent the new data.</param>
        /// <param name="image">The (nullable) RCImage item that will be displayed to represent the new data.</param>
        [notFullyImplemented("Validate new item")]
        public void addSpinItem(string key, RCText textObject, RCQuad image)
        {
            keys.Add(key);
            images.Add(image);
            texts.Add(textObject);
        }

        /// <summary>
        /// Adds a new item to the list that the spinner will seek to.
        /// </summary>
        /// <param name="key">The key that external code will use to refer to the new data.</param>
        /// <param name="textObject">The (nullable) RCText item that will be displayed to represent the new data.</param>
        /// <param name="imageFileName">The file name of the image (.png) that will be displayed to represent the new data.  Pass "nothing" to use no image.</param>
        [notFullyImplemented("Validate new item")]
        public void addSpinItem(string key, RCText textObject, string imageFileName)
        {
            keys.Add(key);
            addImageToListByFileName(key, imageFileName);
            texts.Add(textObject);
        }

        #endregion ----------------------------------addSpinItem (+3 overrides)


        /// <summary>
        /// Spins to the next item.
        /// </summary>
        public void spinUp()
        {
            // will need to think of a way to avoid doing this
            //  but will play sound when toggling spinners
            SoundManager.PlayCue("blip");

            if (curIndex >= keys.Count - 1)
            { SpinToIndex(0); }
            else
            { SpinToIndex(curIndex + 1); }
        }

        /// <summary>
        /// Spins to the previous item.
        /// </summary>
        public void spinDown()
        {
            // will need to think of a way to avoid doing this
            //  but will play sound when toggling spinners
            SoundManager.PlayCue("blip");

            if (curIndex == 0)
            { SpinToIndex(keys.Count - 1); }
            else
            { SpinToIndex(curIndex - 1); }
        }

        /// <summary>
        /// Spins directly to the image or text associated with <paramref name="stringToSpinTo"/>
        /// </summary>
        /// <param name="stringToSpinTo">The image or text to spin directly to.</param>
        /// <returns>Returns true if the item in question was found, and false if it was not.</returns>
        public bool spinTo(string stringToSpinTo)
        {
            for (int indexToCheck = 0; indexToCheck < keys.Count; indexToCheck++)
            {
                if (stringToSpinTo == keys[indexToCheck])
                {
                    SpinToIndex(indexToCheck);
                    return true;
                }
            }
            return false;
        }

        
        
        #endregion ------------------------------Public spinner manipulations


        #region    ------------------------------Overrides

        public override void LoadGraphicsContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            imageBeingSpun.Image = content.Load<Texture2D>("Content\\Textures\\" + _imageBeingSpunFileNameMinusExtension);
            

            for (int imageIndex = 0; imageIndex < images.Count; imageIndex++)
            {
                images[imageIndex].Image = content.Load<Texture2D>("Content\\Textures\\" + imageFiles[imageIndex]);
            }
                base.LoadGraphicsContent(graphics, content);
        }


        public override bool OnEvent(GUIEvent guiEvent)
        {
            bool handledByMe = false;

            if (guiEvent is GUISelectEvent)
            {
                GUISelectEvent selectEvent = (GUISelectEvent)(guiEvent);

                switch (selectEvent.SelectEvent)
                {
                    case GUISelectEvent.GUISelectType.Accept:
                        ThrowLocalEvent(Accepted);
                        break;
                    case GUISelectEvent.GUISelectType.Decline:
                        ThrowLocalEvent(Declined);
                        break;
                }

                handledByMe = true;
            }

            bool handledByBase = base.OnEvent(guiEvent);
            return (handledByBase || handledByMe);
        }

        #endregion ------------------------------Overrides


        #region    ------------------------------Private Helper Functions


        /// <summary>
        /// Helper function to create a new RCText object and put it in the RCSpinner's list, given its key, text, and font.
        /// </summary>
        /// <param name="key">The key to associate the RCText object with.</param>
        /// <param name="text">The text that the RCText object shall display.</param>
        /// <param name="Font">The font that the RCText object shall use for displaying <paramref name="text"/>.</param>
        private void addTextToListByString(string key, string text, BitmapFont Font)
        {
            RCText newText = new RCText(Font, ScreenWidth, ScreenHeight / 2, ScreenWidth, ScreenHeight / 2);
            newText.Text = text;
            texts.Add(newText);
            //            AddChild(newText, 0, 0, 0);
        }

        /// <summary>
        /// Helper function to create aa new RCQuad object and put it in the RCSpinner's list, given its key and image file name.
        /// </summary>
        /// <param name="key">The key to associate the RCQuad object with.</param>
        /// <param name="imageFileName">The file name of the image that the RCQuad shall displlay.</param>
        private void addImageToListByFileName(string key, string imageFileName)
        {
            RCQuad newGraphic = new RCQuad(ScreenWidth, ScreenHeight / 2, ScreenWidth, ScreenHeight / 2);
            imageFiles.Add(imageFileName);
            images.Add(newGraphic);
            //            AddChild(newGraphic, 0,0, 0);
        }

        /// <summary>
        /// Helper function to shift to a specific index.
        /// </summary>
        /// <param name="indexToSpinTo">The index to shift to.</param>
        private void SpinToIndex(int indexToSpinTo)
        {
            RemoveChild(images[curIndex]);
            RemoveChild(texts[curIndex]);
            curIndex = indexToSpinTo;
            AddChild(images[curIndex], 0, 0, 0);
            AddChild(texts[curIndex], 0, 0, 0);
        }

        #endregion ------------------------------Private Helper Functions


        /// <summary>
        /// Attempts to handle the specified local event, and returns if there is a handler set.
        /// </summary>
        /// <param name="eventToThrow">The event to attempt to throw.</param>
        /// <returns>Whether or not <paramref name="eventToThrow"/> was handled (true if it did, false if it did not).</returns>
        public bool ThrowLocalEvent(RCSpinnerEvent eventToThrow)
        {
            if (eventToThrow == null)
            { return false; }
            else
            {
                eventToThrow();
                return true;
            }
        }



        
    }
}
