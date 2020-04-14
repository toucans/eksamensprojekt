using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Databank_Eksamens_Projekt
{
    public partial class FaceDetection : Form
    {
        public string usernameFromLogin;
        public FaceDetection(string username)
        {
            usernameFromLogin = username;
            InitializeComponent();
        }
        // makes 2 commands that are used from Aforge 
        FilterInfoCollection filter;
        VideoCaptureDevice device;

        // this is a special file that makes it posible to find a face.
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml"); // This file is copy from this link "https://github.com/opencv/opencv/tree/master/data/haarcascades"

        // ---combobox --- 
        private void FaceDetection_Load(object sender, EventArgs e)
        {
            // Makes a filter for det cammera devices that the user can use
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filter)
            {
                comboBoxDevice.Items.Add(device.Name);
            }
            comboBoxDevice.SelectedIndex = 0;
            device = new VideoCaptureDevice();
        }

        // button to start the camera 
        private void buttonDetect_Click(object sender, EventArgs e)
        {
            device = new VideoCaptureDevice(filter[comboBoxDevice.SelectedIndex].MonikerString);
            device.NewFrame += Device_NewFrame;
            device.Start();
        }

        // bitmap frame that makes the rektangles
        private void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bitmap);
            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.2, 1);

            // here it makes the rektangel foreach object in the image
            foreach (Rectangle rectangle in rectangles)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Red, 1))
                    {
                        graphics.DrawRectangle(pen, rectangle);
                    }

                }
            }
            // and in the end put it in the piceturebox
            pic.Image = bitmap;
        }

        private void Close(object sender, FormClosedEventArgs e)
        {
            if (device.IsRunning)
            {
                device.Stop();
            }
        }
    }
}
