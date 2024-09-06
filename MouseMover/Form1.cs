using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MouseMover
{
    public partial class Form1 : Form
    {
        //Import user32.dll to use Set and Get CursorPos Methods for moving mouse
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        public enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000, //continuous monitoring
            ES_DISPLAY_REQUIRED = 0x00000002, // Prevent the display from turning off
            ES_SYSTEM_REQUIRED = 0x00000001, // Prevent sleep mode
            ES_AWAYMODE_REQUIRED = 0x00000040, // Prevent idle-to-sleep mode
        }

        bool stop=false;
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);
        private bool _isRunning = false;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                stop = true;
                _isRunning = false;
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
                ExitButton.Enabled = true;
                // run the loop in a background task
                _cancellationTokenSource.Cancel(stop);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        { 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                stop = false;
                _isRunning = true;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                ExitButton.Enabled = true;
                // run the loop in a background task
                _cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => Loop(_cancellationTokenSource.Token));
            }
        }
        private void Loop(CancellationToken cancellationToken)
        {       buttonStop.Enabled = true;
                int originalX = Cursor.Position.X;
                int originalY = Cursor.Position.Y;

                //SetCursorPos(newX, newY);
                SetCursorPos(originalX + 10, originalY + 10);

                Thread.Sleep(1000);
                SetCursorPos(originalX, originalY);
            while (stop == false;)
                {
                    //prevent sleep mode
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
                    //Wait for 10 seconds
                    Thread.Sleep(10000);

                    originalX = Cursor.Position.X;
                    originalY = Cursor.Position.Y;

                    //SetCursorPos(newX, newY);
                    SetCursorPos(originalX + 10, originalY + 10);


                    Thread.Sleep(5000);
                    SetCursorPos(originalX, originalY);
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                }
              if (stop == false)
            {
                _cancellationTokenSource.Cancel();
            }
        } 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_isRunning)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}