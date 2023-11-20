using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Life_game
{
    public partial class Form1 : Form
    {
        #region Varriables
        private int currentGen = 0;
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;

        private bool validateMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Gray;
        }

        #region Screening
        private void StartGame()
        {
            if (timer1.Enabled)
                return;

            currentGen = 0;
            Text = $"Generation {currentGen}";

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int x = 0; x<cols; x++)
            {
                for (int y = 0; y<rows; y++)
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void NextGen()
        {
            graphics.Clear(Color.Black);

            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighbourCount = CountNeighbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighbourCount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neighbourCount < 2 || neighbourCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];

                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.DarkKhaki, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                        
                }
            }
            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation {++currentGen}";
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for(int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;

                    var isSelfChecking = col == x && row == y;
                    var hasLife = field[col, row];
                    if (hasLife && !isSelfChecking)
                        count++;
                }
            }
            return count;
        }
        
        private void StopGame()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }
        #endregion

        #region Start-Stop
        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGen();
        }
        private void bPause_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        #endregion

        #region MouseMove
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //new cells
            if(e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = validateMousePosition(x, y);
                if (validationPassed)
                {
                    field[x, y] = true;
                    graphics.FillRectangle(Brushes.DarkKhaki, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
                
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = validateMousePosition(x, y);
                field[x, y] = false;
                graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution - 1, resolution - 1);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // Увеличение масштаба (приближение)
                pictureBox1.Width += 10;
                pictureBox1.Height += 10;
            }
            else if (e.Delta < 0)
            {
                // Уменьшение масштаба (отдаление)
                if (pictureBox1.Width > 10 && pictureBox1.Height > 10)
                {
                    pictureBox1.Width -= 10;
                    pictureBox1.Height -= 10;
                }
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {currentGen}";
        }
    }
}