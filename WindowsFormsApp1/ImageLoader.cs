using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittlePet
{
    /// <summary>
    /// This class is responsible for pretty much all the 
    /// low-level animation management.
    /// </summary>
    public class ImageLoader
    {
        InterpolationPictureBox PictureBox;
        Timer Refresher = new Timer();
        int FC = 0;                                 // Frame counter
        int State = CharacterMode.IdleStance1;


        /// <summary>
        /// This container associates pixels with character modes. The order
        /// of the image resources indicates how they are played.
        /// </summary>
        Dictionary<int, List<Image>> Keyframes = new Dictionary<int, List<Image>>()
        {
            {
                CharacterMode.IdleStance1, new List<Image>()
                {
                    Properties.Resources.pixy,
                    Properties.Resources.pixy_idle2
                }
            },
            {
                CharacterMode.SleepMode, new List<Image>()
                {
                    Properties.Resources.pixy_sleep
                }
            },
            {
                CharacterMode.Walk, new List<Image>()
                {
                    Properties.Resources.pixy_left_step,
                    Properties.Resources.pixy,
                    Properties.Resources.pixy_right_step,
                    Properties.Resources.pixy,
                }
            },
            {
                CharacterMode.Happy, new List<Image>()
                {
                    Properties.Resources.pixy_happy_1,
                    Properties.Resources.pixy_happy_2
                }
            }
        };
        
        /// <summary>
        /// Dictates the direction the image resources are drawn in.
        /// </summary>
        public DrawDirection DrawDir { get; private set; } = DrawDirection.Right;



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pictureBox">The image box to manipulate.</param>
        public ImageLoader(InterpolationPictureBox pictureBox)
        {
            PictureBox = pictureBox;
            Refresher.Tick += Refresher_Tick;
            Refresher.Interval = 60;
        }



        /// <summary>
        /// Starts an animation cycle. Select an animation speed and
        /// the CharacterMode and the animation will advance every
        /// tick of the animSpeed parameter.
        /// </summary>
        /// <param name="animSpeed">The time between animation frames.</param>
        /// <param name="state">The animation to play.</param>
        public void Start(TimeSpan animSpeed, int state)
        {
            if(Refresher.Enabled)
            {
                Refresher.Stop();
            }

            State = state;
            PictureBox.Image = Keyframes[state][0]; // Load up the first sprite
            Refresher.Interval = (int)animSpeed.TotalMilliseconds;
            Refresher.Start();
        }



        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void Stop()
        {
            Refresher.Stop();
        }



        /// <summary>
        /// Allows external code to set a specific frame.
        /// All animation will be halted.
        /// </summary>
        /// <param name="state">The character mode to use.</param>
        /// <param name="frame">The frame of the character mode to display.</param>
        public void SetState(int state, int frame)
        {
            if (Refresher.Enabled)
            {
                Refresher.Stop();
            }

            State = state;
            PictureBox.Image = Keyframes[state][frame];
        }



        /// <summary>
        /// A handler to manage when the character is moved. In this case, my requirements
        /// are that the character should always look away from the edge of the screen.
        /// This block of code performs that.
        /// </summary>
        /// <param name="screenBounds">The boundary of the screen.</param>
        /// <param name="loc">The location of the character.</param>
        public void CharacterMoved(Rectangle screenBounds, Point loc)
        {
            double halfScreen = (screenBounds.Width / 2);

            if ((DrawDir == DrawDirection.Right && loc.X >= halfScreen) || (DrawDir == DrawDirection.Left && loc.X < halfScreen))
            {
                foreach(var imageList in Keyframes.Values)
                {
                    foreach(var img in imageList)
                    {
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                }

                DrawDir = (DrawDir == DrawDirection.Right) ? DrawDirection.Left : DrawDirection.Right;
            }
        }


        
        /// <summary>
        /// Advances the current state of animation to the next frame.
        /// </summary>
        public void AdvanceState()
        {
            FC = (FC + 1) % Keyframes[State].Count;   // Calculate the next frame position and wrap around at the end.
            PictureBox.Image = Keyframes[State][FC];  // Set the image.
        }



        /// <summary>
        /// The timer tick behaviour, which is to advance to the next
        /// frame every tick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresher_Tick(object sender, EventArgs e)
        {
            AdvanceState();
        }
    }
}
