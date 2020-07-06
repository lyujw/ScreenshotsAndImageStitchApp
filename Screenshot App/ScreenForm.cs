using System;
using System.Drawing;
using System.Windows.Forms;

namespace Screenshot_App {
    public partial class ScreenForm : Form {
        public ScreenForm() {
            InitializeComponent();
        }


        // Define variables

        // Used to record the coordinates pressed by the mouse
        // used to determine the starting point of drawing
        private Point StartPoint;

        // Indicates whether the screenshot is complete
        private bool IsFinished = false;

        // Indicates the start of the screenshot
        private bool IsStart = false;

        // Save original image
        private Bitmap originPic;

        // Rectangle to save screenshot
        private Rectangle CatchBox;

        /// <summary>
        /// Form initialization operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenForm_Load(object sender, EventArgs e) {
            // Set the control style to double buffering
            // which can effectively reduce the problem of picture flicker
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
            // Change mouse style
            this.Cursor = Cursors.Cross;
            // Save full screen picture
            originPic = new Bitmap(this.BackgroundImage);
        }

        /// <summary>
        /// Right-click to end the screenshot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cutter_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Mouse down event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cutter_MouseDown(object sender, MouseEventArgs e) {
            // Press the left mouse button to start drawing, which is a screenshot
            if (e.Button == MouseButtons.Left) {
                // If the capture does not start
                if (!IsStart) {
                    IsStart = true;
                    // Save the mouse down coordinates at this time
                    StartPoint = new Point(e.X, e.Y);
                }
            }
        }


        /// <summary>
        /// Mouse movement event handler, that is
        /// the process of the user changing the size of the screenshot
        ///  This method is the core method of the screenshot function
        ///  which is to draw a screenshot
        /// </summary>
        /// <param name="sender"></param>6
        /// <param name="e"></param>
        private void Cutter_MouseMove(object sender, MouseEventArgs e) {
            // Make sure the screenshot starts
            if (IsStart) {
                // Create a new picture object and make it the same as the screen picture
                Bitmap copyBmp = (Bitmap)originPic.Clone();

                // Get the coordinates of the mouse press
                Point newPoint = new Point(StartPoint.X, StartPoint.Y);

                // create graphics and pen
                Graphics g = Graphics.FromImage(copyBmp);
                Pen p = new Pen(Color.Red, 2);

                // Get the length and width of a rectangle
                int width = Math.Abs(e.X - StartPoint.X);
                int height = Math.Abs(e.Y - StartPoint.Y);
                if (e.X < StartPoint.X) {
                    newPoint.X = e.X;
                }
                if (e.Y < StartPoint.Y) {
                    newPoint.Y = e.Y;
                }

                CatchBox = new Rectangle(newPoint, new Size(width, height));

                // Draw rectangle on graphics
                g.DrawRectangle(p, CatchBox);

                // Free the current graphics
                g.Dispose();
                p.Dispose();
                // Create a new graphics from the current form
                Graphics g1 = this.CreateGraphics();

                // Draw the picture you just painted onto the screenshot form
                // Double buffer
                g1.DrawImage(copyBmp, new Point(0, 0));
                g1.Dispose();
                // Release copy pictures to prevent memory from being consumed
                copyBmp.Dispose();
            }
        }

        /// <summary>
        /// Left mouse button up event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cutter_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                // If the screenshot has started
                // the left mouse button pops up to set the screenshot completed
                if (IsStart) {
                    IsStart = false;
                    IsFinished = true;
                }
            }
        }

        /// <summary>
        /// Mouse double-click event
        /// if the mouse is inside the rectangle
        /// save the picture in the rectangle to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cutter_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && IsFinished) {
                // Create a new blank picture the same size as the rectangle
                Bitmap CatchedBmp = new Bitmap(CatchBox.Width, CatchBox.Height);

                Graphics g = Graphics.FromImage(CatchedBmp);

                // Draw the specified part of originBmp to the blank image according to the specified size
                // CatchRectangle specifies the specified part in originBmp
                // The second parameter specifies the position and size of the blank image drawn
                // After drawing, CatchedBmp is no longer a blank picture, but has the same content as the intercepted picture
                g.DrawImage(originPic, new Rectangle(0, 0, CatchBox.Width, CatchBox.Height), CatchBox, GraphicsUnit.Pixel);

                // Save the picture to the clipboard
                Clipboard.SetImage(CatchedBmp);
                g.Dispose();
                IsFinished = false;
                this.BackgroundImage = originPic;
                CatchedBmp.Dispose();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
