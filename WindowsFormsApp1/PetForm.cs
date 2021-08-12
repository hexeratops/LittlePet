using LittlePet.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittlePet
{
    /// <summary>
    /// The main form for the pet character.
    /// </summary>
    public partial class PetForm : Form
    {
        bool ClickHeld = false;
        bool IsPetted = false;
        bool Sleeping = false;
        Point DrawLocation = new Point(0, 0);
        Random Rand = new Random();
        ImageLoader ImgLoader;



        /// <summary>
        /// Constructor.
        /// </summary>
        public PetForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            
            // Set the transparency stuff and keep the window on top.
            TransparencyKey = Color.White;
            InteropWorkarounds.SetWindowPos(Handle, InteropWorkarounds.HWND_TOPMOST, 0, 0, 0, 0, InteropWorkarounds.TOPMOST_FLAGS);

            // Start the base animation.
            ImgLoader = new ImageLoader(ImageBox);
            ImgLoader.Start(TimeSpan.FromMilliseconds(800), CharacterMode.IdleStance1);
        }



        /// <summary>
        /// Pixy says something.
        /// </summary>
        /// <param name="statement">The thing to say</param>
        private void Speak(string statement)
        {
            SpeachForm f = new SpeachForm(statement);
            var loc = Location;
            loc.Y -= f.Height;

            double middleA = (Width / 2.0) - 10; // Offset for the tail
            double middleB = f.Width / 2.0;

            loc.X = Location.X - (int)(middleB - middleA);
            f.Location = loc;
            f.Show();
        }



        /// <summary>
        /// Moves Pixy to a given location.
        /// </summary>
        /// <param name="loc">The location to move the character.</param>
        private void MoveCharacter(Point loc)
        {
            Location = loc;
            Rectangle bounds = Screen.FromControl(this).Bounds;
            ImgLoader.CharacterMoved(bounds, loc);
        }




        private void Wander()
        {

        }



        private void PerformRandomAct()
        {
        }



        private void ImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Move somewhere.
                ClickHeld = true;
                DrawLocation = new Point(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Left)
            {
                // EXECUTE KIND PETTING/CURSOR SCRATCHING
                IsPetted = true;
                ImgLoader.Start(TimeSpan.FromMilliseconds(250), CharacterMode.Happy);
            }
        }



        private void ImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            ClickHeld = false;

            if (IsPetted)
            {
                IsPetted = false;
                ImgLoader.Start(TimeSpan.FromMilliseconds(800), CharacterMode.IdleStance1);
            }
#if TALK_DEMO
            Speak("Sooo I made some pixel art, and then I made this because I wanted to make use of it. It's just too adorable to just sit in my pictures doing nothing.");
#endif
        }



        private void ImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(ClickHeld)
            {
                // Update the location
                Point loc = this.Location;
                loc.X += e.X - DrawLocation.X;
                loc.Y += e.Y - DrawLocation.Y;
                MoveCharacter(loc);
            }
        }



        private void RandomActTimer_Tick(object sender, EventArgs e)
        {
            long idleTime = InteropWorkarounds.GetLastInputTime();
            if ((idleTime / 60.0) >= 20.0)
            {
                ImgLoader.Stop();
                ImgLoader.SetState(CharacterMode.SleepMode, 0);
                Sleeping = true;
            }
            else if (Sleeping)
            {
                ImgLoader.Start(TimeSpan.FromMilliseconds(800), CharacterMode.IdleStance1);
                Sleeping = false;
            }
            else
            {
                PerformRandomAct();
            }
        }
    }
}
