namespace LittlePet
{
    partial class PetForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ImageBox = new LittlePet.InterpolationPictureBox();
            this.RandomActTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageBox
            // 
            this.ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageBox.Image = global::LittlePet.Properties.Resources.pixy;
            this.ImageBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.ImageBox.Location = new System.Drawing.Point(0, 0);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(272, 265);
            this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ImageBox.TabIndex = 0;
            this.ImageBox.TabStop = false;
            this.ImageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseDown);
            this.ImageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
            this.ImageBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseUp);
            // 
            // RandomActTimer
            // 
            this.RandomActTimer.Enabled = true;
            this.RandomActTimer.Interval = 20000;
            this.RandomActTimer.Tick += new System.EventHandler(this.RandomActTimer_Tick);
            // 
            // PetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(272, 265);
            this.Controls.Add(this.ImageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PetForm";
            this.Text = "Pixy";
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private InterpolationPictureBox ImageBox;
        private System.Windows.Forms.Timer RandomActTimer;
    }
}

