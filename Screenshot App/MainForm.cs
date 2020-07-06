using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Office.Interop.Word;

namespace Screenshot_App {
    /// <summary>
    /// Capture pictures as needed
    /// Combine several pictures with overlapping parts into one picture
    /// Date: May 2020
    /// </summary>
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        static ScreenForm cutter = null;

        // Create an array to store the address of the picture
        string[] FilePathArray = new string[4];

        String PackagePath = null;

        /// <summary>
        /// Capture picture
        /// </summary>
        /// <returns> Pictures captured by users</returns>
        public Bitmap GetAndReturnPic() {
            // Create a picture the same size as the monitor
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            cutter = new ScreenForm();
            cutter.BackgroundImage = bmp;
            if (cutter.ShowDialog() == DialogResult.OK) {
                IDataObject iData = Clipboard.GetDataObject();
                DataFormats.Format format = DataFormats.GetFormat(DataFormats.Bitmap);
                if (iData.GetDataPresent(DataFormats.Bitmap)) {
                    //ShowPicBox.Paste(format);
                    //Clipboard.Clear();
                }
            }
            return bmp;
        }

        /// <summary>
        /// Select a word document
        /// </summary>
        /// <returns> Address of the selected word document</returns>
        public static String OpenWord() {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.DefaultExt = ".docx";
            fileDialog.Filter = "Word文档(*.docx)|*.docx";
            String wordPath = null;
            if (fileDialog.ShowDialog() == DialogResult.OK) {
                wordPath = fileDialog.FileName.ToString();
            }
            return wordPath;
        }

        /// <summary>
        /// Save the captured image locally
        /// </summary>
        /// <param name="order"> Is the default name of the file before or after</param>
        /// <param name="position"> The location of the captured image</param>
        /// <returns></returns>
        public String SavePic(String order,String position) {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Filter = "Bmp 图片|*.bmp";
            savedialog.FilterIndex = 0;
            savedialog.RestoreDirectory = true;// Save dialog remember last opened directory
            savedialog.CheckPathExists = true;// check directories
            String picFileName;
            if (position == "") {
                picFileName = "-" + order;
            } else {
                picFileName = "-" + order + "-" + position;
            }
            savedialog.FileName = picFileName; ;// Set default filename
            String localFilePath = null;
            if (savedialog.ShowDialog() == DialogResult.OK) {
                Image bmp = Clipboard.GetImage();
                bmp.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);// Image is the picture to save
                MessageBox.Show(this, "Picture saved successfully！", "Information tips");
                localFilePath = savedialog.FileName.ToString();
                //PackagePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));
            }
            Clipboard.Clear();
            return localFilePath;
        }

        /// <summary>
        /// Crop bmp pictures
        /// </summary>
        /// <param name="b"> Original bmp picture</param>
        /// <param name="StartX"> Starting X coordinate</param>
        /// <param name="StartY"> Starting Y coordinate</param>
        /// <param name="iWidth"> The width to be intercepted</param>
        /// <param name="iHeight"> The height to be intercepted</param>
        /// <returns> Cropped bmp picture </returns>
        public static Bitmap CutPic(Bitmap b, int StartX, int StartY, int iWidth, int iHeight) {
            if (b == null) {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            if (StartX >= w || StartY >= h) {
                return null;
            }

            if (StartX + iWidth > w) {
                iWidth = w - StartX;
            }

            if (StartY + iHeight > h) {
                iHeight = h - StartY;
            }

            try {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new System.Drawing.Rectangle(0, 0, iWidth, iHeight), new System.Drawing.Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();

                return bmpOut;
            } catch {
                return null;
            }
        }

        /// <summary>
        /// Find the coordinates of the black border around the bmp picture
        /// </summary>
        /// <param name="oribitmap"></param>
        /// <returns></returns>
        public int[] FindBlack2(Bitmap oribitmap) {
            int[] position = new int[4];
            int width = oribitmap.Width;
            int height = oribitmap.Height;
            for (int i = 0; i < height; i++) {
                Color curColor1 = oribitmap.GetPixel(width / 2, i);
                if (curColor1.R <= 99 && curColor1.G <= 99 && curColor1.B <= 99 ) {
                    position[0] = i;
                    break;
                }
            }
            for (int j = 0; j < width; j++) {
                Color curColor2 = oribitmap.GetPixel(j, height / 2);
                if (curColor2.R <= 99 && curColor2.G <= 99 && curColor2.B <= 99 ) {
                    position[1] = j;
                    break;
                }
            }
            for (int m = height - 1; m > height / 2; m--) {
                Color curColor3 = oribitmap.GetPixel(width / 2, m);
                if (curColor3.R <= 99 && curColor3.G <= 99 && curColor3.B <= 99 ) {
                    position[2] = height - m;
                    break;
                }
            }
            for (int n = width - 1; n > width / 2; n--) {
                Color curColor4 = oribitmap.GetPixel(n, height / 2);
                if (curColor4.R <= 99 && curColor4.G <= 99 && curColor4.B <= 99 ) {
                    position[3] = width - n;
                    break;
                }
            }
            return position;
        }

        /// <summary>
        /// Find the coordinates of the black border around the bmp picture
        /// </summary>
        /// <param name="oribitmap"> Bmp picture with black frame </param>
        /// <returns> Int array with four numbers </returns>
        public int[] FindBlack(Bitmap oribitmap) {
            int[] position = new int[4];
            int width = oribitmap.Width;
            int height = oribitmap.Height;
            for (int i = 0; i < height; i++) {
                Color curColor1 = oribitmap.GetPixel(width / 2, i);
                if (curColor1.R <= 99 && curColor1.G <= 99 && curColor1.B <= 99 && curColor1.R>1 && curColor1.G > 1 && curColor1.B > 1) {
                    position[0] = i;
                    break;
                }
            }
            for (int j = 0; j < width; j++) {
                Color curColor2 = oribitmap.GetPixel(j, height / 2);
                if (curColor2.R <= 99 && curColor2.G <= 99 && curColor2.B <= 99 && curColor2.R > 1 && curColor2.G > 1 && curColor2.B > 1) {
                    position[1] = j;
                    break;
                }
            }
            for (int m = height - 1; m > height / 2; m--) {
                Color curColor3 = oribitmap.GetPixel(width / 2, m);
                if (curColor3.R <= 99 && curColor3.G <= 99 && curColor3.B <= 99 && curColor3.R > 1 && curColor3.G > 1 && curColor3.B > 1) {
                    position[2] = height - m;
                    break;
                }
            }
            for (int n = width - 1; n > width / 2; n--) {
                Color curColor4 = oribitmap.GetPixel(n, height / 2);
                if (curColor4.R <= 99 && curColor4.G <= 99 && curColor4.B <= 99 && curColor4.R > 1 && curColor4.G > 1 && curColor4.B > 1) {
                    position[3] = width - n;
                    break;
                }
            }
            return position;
        }

        /// <summary>
        /// Change the black frame to white according to the position of the black frame
        /// </summary>
        /// <param name="bmp"> Bmp picture to be changed</param>
        /// <param name="position"> Black frame coordinates</param>
        /// <returns> Remove the black frame bmp picture </returns>
        public static Bitmap BlackToWhite(Bitmap bmp, int[] position) {
            for (int i = 0; i < position[0] + 3; i++) {
                for (int j = 0; j < bmp.Width; j++) {
                    bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
            }

            for (int i = 0; i < position[1] + 3; i++) {
                for (int j = 0; j < bmp.Height; j++) {
                    bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            }

            for (int i = bmp.Height - position[2] - 3; i < bmp.Height; i++) {
                for (int j = 0; j < bmp.Width; j++) {
                    bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
            }

            for (int i = bmp.Width - position[3] - 3; i < bmp.Width; i++) {
                for (int j = 0; j < bmp.Height; j++) {
                    bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            }

            return bmp;
        }

        /// <summary>
        /// Insert bmp picture into word document
        /// </summary>
        /// <param name="bmp_path"> bmp picture address </param>
        /// <param name="path"> word document address </param>
        public static void AddBmpToWord(String bmp_path,String path) {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Document doc = app.Documents.Add(path);
            app = doc.Application;
            doc.ActiveWindow.Visible = true;
            Selection sel = app.Selection;
            sel.InlineShapes.AddPicture(bmp_path);
            doc.SaveAs(path);
            app.Quit();
            MessageBox.Show("Picture inserted successfully！", "Information tips");
        }

        /// <summary>
        /// The first case of composite pictures
        /// </summary>
        public void SplicingPicSample1() {
            Bitmap before_bmp = new Bitmap(FilePathArray[0]);
            int[] position = FindBlack2(before_bmp);
            //Bitmap after_bmp = CutPic(before_bmp, position[1] + 5, position[0] + 5, (before_bmp.Width - position[1] - position[3]-10), (before_bmp.Height - position[0] - position[2]-10));
            Bitmap after_bmp = BlackToWhite(before_bmp, position);
            Clipboard.SetImage(after_bmp);
            String bmp_path = SavePic("after", "");
            Clipboard.Clear();
            String WordPath = OpenWord();
            AddBmpToWord(bmp_path, WordPath);
            
        }

        /// <summary>
        /// The second case of composite pictures
        /// </summary>
        public void SplicingPicSample2() {
            String img_top_path= FilePathArray[1];
            String img_down_path= FilePathArray[0];
           
            Image<Bgr, byte> img_top = new Image<Bgr, byte>(img_top_path);
            Image<Bgr, byte> img_down = new Image<Bgr, byte>(img_down_path);
            Stitcher stitcher = new Stitcher(true);
            Mat img_whole = new Mat();
            stitcher.Stitch(new VectorOfMat(new Mat[] { img_top.Mat, img_down.Mat }), img_whole);
            Bitmap whole_bmp=img_whole.Bitmap;
            //Bitmap cut_bmp=CutPic(whole_bmp,Convert.ToInt32(whole_bmp.Width*0.02), Convert.ToInt32(whole_bmp.Height*0.02), Convert.ToInt32(whole_bmp.Width*0.94), Convert.ToInt32(whole_bmp.Height*0.96));
            //int[] position = FindBlack(cut_bmp);
            int[] position = FindBlack(whole_bmp);


            //Bitmap after_bmp = BlackToWhite(cut_bmp, position);
            Bitmap after_bmp = BlackToWhite(whole_bmp, position);
            Clipboard.SetImage(after_bmp);
            String bmp_path = SavePic("after", "");
            Clipboard.Clear();
            String WordPath = OpenWord();
            AddBmpToWord(bmp_path, WordPath);
        }

        public void SplicingPicSample3() {
            String img_left_path = null;
            String img_right_path = null;
            if (FilePathArray[0].IndexOf("left") != -1) {
                img_left_path = FilePathArray[0];
                img_right_path = FilePathArray[1];
            }
            if (FilePathArray[1].IndexOf("right") != -1) {
                img_left_path = FilePathArray[0];
                img_right_path = FilePathArray[1];
            }
            Image<Bgr, byte> img_left = new Image<Bgr, byte>(img_left_path);
            Image<Bgr, byte> img_right = new Image<Bgr, byte>(img_right_path);
            Stitcher stitcher = new Stitcher(true);
            Mat img_whole = new Mat();
            stitcher.Stitch(new VectorOfMat(new Mat[] { img_left.Mat, img_right.Mat }), img_whole);
            Bitmap whole_bmp = img_whole.Bitmap;
            Bitmap cut_bmp = CutPic(whole_bmp, Convert.ToInt32(whole_bmp.Width * 0.05), Convert.ToInt32(whole_bmp.Height * 0.05), Convert.ToInt32(whole_bmp.Width * 0.92), Convert.ToInt32(whole_bmp.Height * 0.92));
            //Bitmap no_black_bmp = RemoveBlackEdge(cut_bmp);
            //Clipboard.SetImage(no_black_bmp);
            int[] position = FindBlack(cut_bmp);
            Bitmap after_bmp = CutPic(cut_bmp, position[0] + 10, position[1] + 6, (cut_bmp.Width - position[1] - position[3] - 15), (cut_bmp.Height - position[0] - position[2] - 15));
            Clipboard.SetImage(after_bmp);
            SavePic("after", "");
            Clipboard.Clear();
        }

        /// <summary>
        /// The third case of composite pictures
        /// </summary>
        public void SplicingPicSample4() {
            String img_topleft_path = FilePathArray[0];
            String img_topright_path = FilePathArray[1];
            String img_downleft_path = FilePathArray[2];
            String img_downright_path = FilePathArray[3];
            Image<Bgr, byte> img_topleft = new Image<Bgr, byte>(img_topleft_path);
            Image<Bgr, byte> img_topright = new Image<Bgr, byte>(img_topright_path);
            Image<Bgr, byte> img_downleft = new Image<Bgr, byte>(img_downleft_path);
            Image<Bgr, byte> img_downright = new Image<Bgr, byte>(img_downright_path);
            Stitcher stitcher = new Stitcher(true);
            Mat img_whole = new Mat();
            stitcher.Stitch(new VectorOfMat(new Mat[] { img_topleft.Mat, img_topright.Mat, img_downleft.Mat, img_downright.Mat }), img_whole);
            Bitmap whole_bmp = img_whole.Bitmap;
            //Bitmap cut_bmp = CutPic(whole_bmp, Convert.ToInt32(whole_bmp.Width * 0.03), Convert.ToInt32(whole_bmp.Height * 0.03), Convert.ToInt32(whole_bmp.Width * 0.94), Convert.ToInt32(whole_bmp.Height * 0.94));
            int[] position = FindBlack(whole_bmp);
            //Bitmap after_bmp = CutPic(cut_bmp, position[1] + 5, position[0] + 5, (cut_bmp.Width - position[1] - position[3] - 10), (cut_bmp.Height - position[0] - position[2] - 10));
            Bitmap after_bmp = BlackToWhite(whole_bmp, position);
            Clipboard.SetImage(after_bmp);
            String bmp_path = SavePic("after", "");
            Clipboard.Clear();
            String WordPath = OpenWord();
            AddBmpToWord(bmp_path, WordPath);
        }

        /// <summary>
        /// Take a whole picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPicButton1_Click(object sender, EventArgs e) {
            //ShowPicBox.Clear();
            Bitmap bmp_whole = GetAndReturnPic();
            SavePic("before","");
        }

        /// <summary>
        /// Take pictures up and down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPicButton2_Click(object sender, EventArgs e) {
            //ShowPicBox.Clear();
            Bitmap bmp_top = GetAndReturnPic();
            SavePic("before", "top");
            Thread.Sleep(1000);
            Bitmap bmp_dowm = GetAndReturnPic();
            SavePic("before", "down");
        }

        /// <summary>
        /// Take pictures left and right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPicButton3_Click(object sender, EventArgs e) {
            //ShowPicBox.Clear();
            Bitmap bmp_left = GetAndReturnPic();
            SavePic("before", "left");
            Thread.Sleep(1000);
            Bitmap bmp_right = GetAndReturnPic();
            SavePic("before", "right");
        }

        /// <summary>
        /// Take pictures topleft,topright,downleft and downright
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPicButton4_Click(object sender, EventArgs e) {
            //ShowPicBox.Clear();
            Bitmap bmp_topleft = GetAndReturnPic();
            SavePic("before", "topleft");
            Thread.Sleep(1000);
            Bitmap bmp_topright = GetAndReturnPic();
            SavePic("before", "topright");
            Thread.Sleep(1000);
            Bitmap bmp_downleft = GetAndReturnPic();
            SavePic("before", "downleft");
            Thread.Sleep(1000);
            Bitmap bmp_downright = GetAndReturnPic();
            SavePic("before", "downright");
        }

        /// <summary>
        /// Select the picture to be stitched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplicingButton_Click(object sender, EventArgs e) {
            Array.Clear(FilePathArray, 0, FilePathArray.Length);
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true; // Can select multiple files
            fileDialog.DefaultExt = ".bmp";
            fileDialog.Filter = "Bmp 图片|*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK) {
                for(int i = 0; i < fileDialog.FileNames.Length; i++) {
                    FilePathArray[i] = Path.GetFullPath(fileDialog.FileNames[i]);
                }
            }

            if (fileDialog.FileNames.Length == 1) {
                SplicingPicSample1();
            }else if (fileDialog.FileNames.Length == 2) {
                SplicingPicSample2();
                /*if((fileDialog.FileNames[0].IndexOf("top")!=-1) || (fileDialog.FileNames[0].IndexOf("down") != -1)) {
                    SplicingPicSample2();
                } 
                if((fileDialog.FileNames[0].IndexOf("left") != -1) || (fileDialog.FileNames[0].IndexOf("right") != -1)){
                    SplicingPicSample3();
                }*/
            } else if (fileDialog.FileNames.Length == 4) {
                SplicingPicSample4();
            } else {
                MessageBox.Show("Error, please select again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Menu Bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manualToolStripMenuItem_Click(object sender, EventArgs e) {
            String msg = "The first button is to capture the whole picture, " +
                        "the second button is to capture the picture up and down, " +
                        "the third button is to capture the picture left and right, " +
                        "the fourth button is to capture the picture up and down, left and right " +
                        "The orientation suffix will be automatically generated, " +
                        "please make sure that the captured images have overlapping parts. " +
                        "After clicking the synthesis button, please select the picture to be synthesized " +
                        "(multiple pictures can be selected at the same time). " +
                        "After a few seconds, the picture will pop up after the picture is successfully synthesized. " +
                        "Also, the orientation suffix of the picture will be automatically generated and the picture " +
                        "will be saved successfully After that, a file dialog will continue to pop up, " +
                        "select a word document, and the synthesized picture will be automatically inserted into the word document.";
            MessageBox.Show(msg);
        }
    }
}
