using LittlePet.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittlePet
{
    /// <summary>
    /// This form is a talk bubble for Pixy.
    /// </summary>
    public partial class SpeachForm : Form
    {
        Task SpeachTask;
        System.Timers.Timer FadeOutTimer;
        string FullSentence;
        SoundPlayer player;



        /// <summary>
        /// Constructs the form with the speach bubble.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        public SpeachForm(string msg)
        {
            InitializeComponent();
            TransparencyKey = Color.Magenta;
            InteropWorkarounds.SetWindowPos(Handle, InteropWorkarounds.HWND_TOPMOST, 0, 0, 0, 0, InteropWorkarounds.TOPMOST_FLAGS);

            // Dynamically size the thing based on the message being displayed.
            var expectedSize = GraphicsHelper.MeasureString(msg, label1.Font);
            int lines = (int)(expectedSize.Width / Width) + 1;
            int width = Width;
            if(expectedSize.Width < Width)
            {
                width = (int)(expectedSize.Width + 20);
            }
            Size = new Size(width + Padding.Left + Padding.Right, (int)(expectedSize.Height * lines) + Padding.Top + Padding.Bottom);

            FadeOutTimer = new System.Timers.Timer();
            FadeOutTimer.Interval = 50;
            FadeOutTimer.Elapsed += FadeOutTimer_Elapsed;

            player = new SoundPlayer(Resources.pixy_sounds);

            label1.Text = "";
            FullSentence = msg;
        }



        /// <summary>
        /// Callback event that happens after the form is shown. This starts
        /// the text appearance animation and queues up the fade-out event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeachForm_Shown(object sender, EventArgs e)
        {
            SpeachTask = Task.Factory.StartNew(() => { Vocalize(FullSentence); });
            SpeachTask.ContinueWith((x) =>
            {
                FadeOutTimer.Start();
            });
        }



        /// <summary>
        /// This method makes text appear in the talk bubble one character at a time.
        /// </summary>
        /// <param name="phrase"></param>
        private void Vocalize(string phrase)
        {
            HashSet<char> pauseChars = new HashSet<char>() { '.', ';', '!', '?' };

            for(int i = 0; i < phrase.Length; i++)
            {
                label1.SafeInvoke(() => { label1.Text += phrase[i]; });

                if (pauseChars.Contains(phrase[i]) && ((phrase.Length - 1) == i || !pauseChars.Contains(phrase[i + 1])))
                {
                    // On a punctuation character, pause.
                    player.Play();
                    Thread.Sleep(500);
                }
                else
                {
                    // On a regular character, keep talking
                    player.Play();
                    Thread.Sleep(60);
                }
            }
        }



        /// <summary>
        /// This performs a simple fadeout effect based on a timer. It fades out the
        /// opacity by 1/25 every tick. Once the opacity hits 0, it closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FadeOutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.SafeInvoke(() =>
            {
                Opacity -= 0.04;
                if (Opacity <= 0)
                {
                    FadeOutTimer.Stop();
                    Close();
                }
            });
        }



        /// <summary>
        /// Sets the interpolation mode to nearest neighbour.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            base.OnPaintBackground(e);
        }



        /// <summary>
        /// Experimentation code lifted from stackoverflow somewhere. Kind of nice to have,
        /// but I prefer my handrolled but monotonous text.
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="msDuration"></param>
        /// <param name="volume"></param>
        public static void PlayTone(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        {
            using (var mStrm = new MemoryStream())
            {
                using (var writer = new BinaryWriter(mStrm))
                {
                    const double tau = 2 * Math.PI;
                    const int formatChunkSize = 16;
                    const int headerSize = 8;
                    const short formatType = 1;
                    const short tracks = 1;
                    const int samplesPerSecond = 44100;
                    const short bitsPerSample = 16;
                    const short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                    const int bytesPerSecond = samplesPerSecond * frameSize;
                    const int waveSize = 4;
                    var samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
                    int dataChunkSize = samples * frameSize;
                    int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;

                    writer.Write(0x46464952);
                    writer.Write(fileSize);
                    writer.Write(0x45564157);
                    writer.Write(0x20746D66);
                    writer.Write(formatChunkSize);
                    writer.Write(formatType);
                    writer.Write(tracks);
                    writer.Write(samplesPerSecond);
                    writer.Write(bytesPerSecond);
                    writer.Write(frameSize);
                    writer.Write(bitsPerSample);
                    writer.Write(0x61746164);
                    writer.Write(dataChunkSize);

                    double theta = frequency * tau / samplesPerSecond;
                    double amp = volume >> 2;
                    for (int step = 0; step < samples; step++)
                    {
                        writer.Write((short)(amp * Math.Sin(theta * step)));
                    }

                    mStrm.Seek(0, SeekOrigin.Begin);
                    using (var player = new System.Media.SoundPlayer(mStrm))
                    {
                        player.PlaySync();
                    }
                }
            }
        }
    }
}
