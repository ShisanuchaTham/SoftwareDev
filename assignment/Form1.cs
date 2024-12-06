using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;

namespace Carmara
{

    public partial class Form1 : Form
    {
        private VideoCapture _capture = null;
        private Mat _flame;



        public Form1()
        {
            InitializeComponent();
            textBox_Connection.BackColor = Color.Red;
            textBox_Connection.Text = "Disconnected";
            button2.Enabled = false;
            textBox_Record.BackColor = Color.Red;
            textBox_Record.Text = "Not recording";
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                bool canCapture = _capture.Retrieve(_flame, 0);
                if (canCapture)
                {
                    imageBox1.Image = _flame;
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                textBox_Connection.BackColor = Color.Blue;
                textBox_Connection.Text = "Waiting";
                await Task.Delay(1);

                if (button1.Text == "Connect")
                {
                    _capture = new VideoCapture();
                    _capture.ImageGrabbed += ProcessFrame;

                    _flame = new Mat();

                    textBox_Connection.BackColor = Color.Green;
                    textBox_Connection.Text = "Connected";

                    button1.Text = "Disconnect";

                    button2.Enabled = true;

                }
                else if (button1.Text == "Disconnect")
                {
                    if (_capture != null)
                    {
                        _capture.Pause();
                        _capture.Dispose();
                    }
                    if (_flame != null)
                    {
                        _flame.Dispose();
                    }

                    textBox_Connection.BackColor = Color.Red;
                    textBox_Connection.Text = "Disconnected";

                    button1.Text = "Connect";

                    button2.Enabled = false;


                }

            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool recording = false;
        private void button2_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (!recording)
                {

                    _capture.Start();
                    recording = true;

                    button1.Enabled = false;

                    button2.Text = "Pause";

                    textBox_Record.BackColor = Color.Green;
                    textBox_Record.Text = "Recording";

                }
                else
                {

                    _capture.Pause();
                    recording = false;

                    button1.Enabled = true;

                    button2.Text = "Record";

                    textBox_Record.BackColor = Color.Red;
                    textBox_Record.Text = "Not recording";

                }
            }
        }
    }
}
