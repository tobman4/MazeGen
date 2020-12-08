using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace MazeGen {
    public class Maze : IDisposable {

        public event EventHandler<mazeDoneData> mazeDoneEvent;

        private Cell[,] grid;
        private int width, height;

        public float prosentDone = 0;
        public int cellCount;
        public int cellsDone = 0;

        public Mode mode {
            get;
            private set;
        }

        public Maze(int width, int height) {

            mode = Mode.NotMade;

            grid = new Cell[width, height];

            this.width = width;
            this.height = height;

            cellCount = width * height;

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    grid[i, j] = new Cell(i, j);
                }
            }
        }

        private List<Cell> getFrends(int x, int y) {
            List<Cell> o = new List<Cell>();
            try {
                o.Add(grid[x + 1, y]);
            } catch { }
            try {
                o.Add(grid[x - 1, y]);
            } catch { }
            try {
                o.Add(grid[x, y + 1]);
            } catch { }
            try {
                o.Add(grid[x, y - 1]);
            } catch { }

            return o;
        }

        public void makePath(ref int doneCells,ref float prosent, int seed = -99) {
            DateTime startTime = DateTime.Now;
            
            mode = Mode.Making;
            prosentDone = 0;
            doneCells = 0;

            Random r;
            if(seed != -99) {
                r = new Random(seed);
            } else {
                r = new Random();
            }

            List<Cell> stack = new List<Cell> { grid[0,0] };
            //List<Cell> done = new List<Cell>();

            bool flag = false;
            while(!flag) {

                if(mode == Mode.Stoped) {
                    return;
                } else if(mode == Mode.Paused && Thread.CurrentThread.Name != "MAIN") {
                    ThreadPriority prePause = Thread.CurrentThread.Priority;
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    do {
                        Thread.Sleep(500);
                    } while (mode == Mode.Paused);
                    Thread.CurrentThread.Priority = prePause;
                }

                prosent = ((float)doneCells / (float)cellCount)*100;

                Cell curr = stack[stack.Count-1];
                if(!curr.isDone) {
                    curr.isDone = true;
                    doneCells++;
                }
                
                List<Cell> frends = getFrends(curr.x,curr.y);

                for (int i = frends.Count; i > 0; i--) {
                    if(frends[i-1].isDone) {
                        frends.Remove(frends[i-1]);
                    }
                }

                if (frends.Count == 0) {
                    
                    stack.RemoveAt(stack.Count-1);
                    if (stack.Count == 0) {
                        flag = true;
                    }
                } else {


                    int index = r.Next(frends.Count);

                    Cell next = frends[index];
                    if(curr.x < next.x) {
                        curr.setWall(wall.Rigth,false);
                        next.setWall(wall.Left,false);
                    } else if(curr.y < next.y) {
                        curr.setWall(wall.Down, false);
                        next.setWall(wall.Top, false);
                    } else if(curr.y > next.y) {
                        curr.setWall(wall.Top, false);
                        next.setWall(wall.Down, false);
                    } else if(curr.x > next.x) {
                        curr.setWall(wall.Left, false);
                        next.setWall(wall.Rigth, false);
                    }

                    if (frends.Count == 1) {
                        stack.Remove(curr);
                    }

                    stack.Add(next);
                }
            }

            mazeDoneData data = new mazeDoneData();
            data.seed = seed;
            data.timeUsed = DateTime.Now - startTime;

            mazeDoneEvent.Invoke(this,data);


            mode = Mode.Done;
        }

        public void stop() {
            if(mode == Mode.Making) {
                mode = Mode.Stoped;
            }
        }

        public Bitmap makeImage() {
            bool wasPaused = mode == Mode.Making;
            if(wasPaused) {
                mode = Mode.Paused;
                Thread.Sleep(500); // give it time to get into the pause loop
            }

            Bitmap o = new Bitmap(width*2,height*2);
            for(int i = 0; i < o.Width; i++) {
                for(int j = 0; j < o.Height; j++) {
                    if(i%2 == 0 && j%2 == 0) {

                        Color cellColor = (grid[i / 2, j / 2].DBG) ? Color.Pink : Color.White;
                        o.SetPixel(i,j, cellColor);
                        
                        bool wall_rigth = grid[i / 2, j / 2].getWall(wall.Rigth);
                        bool wall_down = grid[i / 2, j / 2].getWall(wall.Down);
                        if(i < o.Width) {
                            o.SetPixel(i+1,j,(wall_rigth) ? Color.Black : Color.White);
                        }
                        if (j < o.Height) {
                            o.SetPixel(i, j+1, (wall_down) ? Color.Black : Color.White);
                        }
                    }
                }
            }
            if(wasPaused) {
                mode = Mode.Making;
            }
            return o;
        }

        public void Dispose() {

            for(int i = 0; i < width; i++) {
                for(int j = 0; j < height; j++) {
                    grid[i, j].Dispose();
                }
            }
        }
    }
}
