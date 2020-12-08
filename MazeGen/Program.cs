using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;

namespace MazeGen {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Making 1000*1000 maze");
            
            Maze m = new Maze(100,100);

            float pros = 0.0F;
            int doneCells = 0;
            int lastDone = 0;

            Thread pathThread = new Thread(() => { m.makePath(ref doneCells, ref pros); });
            pathThread.Priority = ThreadPriority.Highest;
            pathThread.Start();

            int lastCount = -1;
            while (m.mode != Mode.Done) {
                Thread.Sleep(1000);
                Console.WriteLine($"({DateTime.Now}){doneCells}/{m.cellCount}({pros}%)");
            }
            
            Bitmap pic = m.makeImage();

            string path = @"pic.bmp";
            pic.Save(path,ImageFormat.Bmp);
        }
    }
}
