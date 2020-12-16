using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

using MazeGen;

namespace mazeGenUI {
    public partial class Form1 : Form {

        private Maze m;
        private mazeDoneData dataUpdate = null;
        private Thread pathThread;

        private int doneCells = 0;
        private float pros = 0.0f;

        public Form1() {
            InitializeComponent();
            Thread.CurrentThread.Name = "MAIN";
        }

        private void mazeDoneUpdate(object sender, mazeDoneData data) {
            dataUpdate = data;
        }
        
        private void startButton_Click(object sender, EventArgs e) {
            GC.Collect();// lazy man
            GC.WaitForPendingFinalizers();
            
            if(m?.mode != Mode.Making) {
                
                if(m != null) {
                    m.Dispose();
                }

                int seed = useSeedBox.Checked ? (int)seedBox.Value : -99;

                m = new Maze((int)numericWidth.Value,(int)numericHeigth.Value);

                m.mazeDoneEvent += mazeDoneUpdate;

                if(pathThread != null) {
                    pathThread.Abort();
                }

                Thread t = new Thread(() => {
                    m.makePath(ref doneCells, ref pros, seed);
                });
                t.Name = "MazeThread";

                progressBar1.Maximum = m.cellCount;
                progressBar1.Value = 0;
                timer1.Enabled = true;

                if(useHighPrio.Checked) {
                    t.Priority = ThreadPriority.Highest;
                }
                
                t.Start();
                pathThread = t;
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            prosentLabel.Text = $"{pros}%";

            progressBar1.Value = doneCells;

            if(m != null) {
                modeLabel.Text = $"Mode: {m.mode}";
            } else {
                modeLabel.Text = $"";
            }

            if(dataUpdate != null) {
                prosentLabel.Text = "Time used: " + dataUpdate.timeUsed.ToString();
                dataUpdate = null;
                timer1.Enabled = false;
            }
        }

        private void stopButton_Click(object sender, EventArgs e) {
            m.stop();
            m.Dispose();
            timer1.Enabled = false;
        }

        private void saveButton_Click(object sender, EventArgs e) {

            if(m == null) {
                m = new Maze((int)numericWidth.Value, (int)numericHeigth.Value);
            }

           // m.grid[500,500].DBG = true;

            DialogResult res = folderBrowserDialog1.ShowDialog();
            if(res == DialogResult.OK) {
                Bitmap pic = m.makeImage();
                string path = folderBrowserDialog1.SelectedPath + "/Maze.bmp";
                pic.Save(path, ImageFormat.Bmp);

                //pic = m.getImage(new Point(500,500), new Size(20,20));
                //path = folderBrowserDialog1.SelectedPath + "/MazeSmall.bmp";
                //pic.Save(path, ImageFormat.Bmp);


            }
        }

        private void useSeedBox_Click(object sender, EventArgs e) {
            seedBox.Enabled = useSeedBox.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if(pathThread != null) {
                pathThread.Abort();
            }
        }
    }
}
