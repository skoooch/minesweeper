using System;
using System.Drawing;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public double mines = 10;
        public double spots_left = 81;
        class Spot
        {
            public bool type;
            public bool flag = false;
            public int x;
            public int y;
            public int proximity;


            public Spot(int xx, int yy, bool typee) // sets square when its created
            {
                x = xx;
                y = yy;
                type = typee;
                proximity = 9;

            }
        }
        System.Windows.Forms.Button[,] btnArray = new System.Windows.Forms.Button[9, 9]; //seperate arrays for each part of square
        System.Windows.Forms.Label[,] lblArray = new System.Windows.Forms.Label[9, 9];
        Spot[,] spots = new Spot[9, 9]; // "spots" being the "backend"
        private void Form1_Load(object sender, EventArgs e)
        {
            set_up_board();
        }
        public void set_up_board()
        {
            Random random = new Random();

            double remaining_spots = spots_left;
            for (int i = 0; i < 9; i++)
            {
                for (int p = 0; p < 9; p++)
                {
                    double prob = 0;
                    bool spot_type = false;
                    prob = mines / remaining_spots * 100; // gets probability that a bomb should be placed based on 
                    remaining_spots -= 1;               // how many bombs / spaces are left
                    int rand = random.Next(1, 100);
                    if (rand < prob)
                    {
                        spot_type = true;
                        mines -= 1;
                    }
                    set_stuff(i, p, spot_type);

                }
            }
            set_labels();
        }
        public void Button_Click(object sender, MouseEventArgs e)
        {

            Button button = sender as Button;
            int x = button.Location.X / 75;
            int y = button.Location.Y / 75;
            switch (e.Button)
            {

                case MouseButtons.Left:
                    if (spots[x, y].flag) { }
                    else
                    {
                        if (spots[x, y].type) { btnArray[x, y].Dispose(); lose(); return; }
                        if (spots[x, y].proximity == 0) { check_spot(x, y); }
                        else { btnArray[x, y].Dispose(); spots[x, y].proximity = 10; }
                        int count = 0;
                        for (int i = 0; i < 9; i++)
                        {
                            for (int p = 0; p < 9; p++)
                            {
                                if (spots[i, p].proximity != 10) { count += 1; }
                                
                            }
                        }
                        if (count == 10)
                        {
                            win();
                        }

                    }
                    break;

                case MouseButtons.Right:

                    if (spots[x, y].flag)
                    {
                        spots[x, y].flag = false;
                        btnArray[x, y].Image = null;
                    }
                    else
                    {
                        spots[x, y].flag = true;
                        btnArray[x, y].Image = System.Drawing.Image.FromFile(@"C:\Users\ewanj\Pictures\Saved Pictures\flag.png");
                    }

                    break;
            }




        }

        void set_stuff(int i, int p, bool type_spot)
        {
            Point point = new Point(i * 75, p * 75);
            Point pointt = new Point(i * 75 + 10, p * 75 + 10);
            btnArray[i, p] = new System.Windows.Forms.Button();
            lblArray[i, p] = new System.Windows.Forms.Label();
            spots[i, p] = new Spot(i, p, type_spot);
            btnArray[i, p].Width = 70;
            btnArray[i, p].Height = 70;
            btnArray[i, p].Location = point;
            btnArray[i, p].MouseDown += Button_Click;
            lblArray[i, p].Location = pointt;
            lblArray[i, p].Font = new Font("Arial", 30, FontStyle.Regular);
            lblArray[i, p].AutoSize = true;
            lblArray[i, p].BackColor = Color.Transparent;
            Controls.Add(lblArray[i, p]);
            Controls.Add(btnArray[i, p]);
            lblArray[i, p].SendToBack();
        }
        void set_labels()
        {

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int bombs = 0;
                    for (int r = x - 1; r <= x + 1; r++)
                    {
                        for (int c = y - 1; c <= y + 1; c++)
                        {
                            if (spots[x, y].type)
                            {
                                lblArray[x, y].Text = "B";
                            }
                            else
                            {
                                try
                                {
                                    if (spots[r, c].type)
                                    {
                                        bombs += 1;
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    if (spots[x, y].type) { }
                    else
                    {
                        if (bombs == 0) { lblArray[x, y].Text = ""; }
                        else { lblArray[x, y].Text = bombs.ToString(); }

                        spots[x, y].proximity = bombs;
                    }

                }
            }
        }
        void lose()
        {
            DialogResult dialogResult = MessageBox.Show("You lose, Play Again?", "yikes....", MessageBoxButtons.YesNo);
            int i = 0;
            while (i < 9)
            {
                int p = 0;
                while (p < 9)
                {
                    btnArray[i, p].Dispose();
                    lblArray[i, p].Dispose();
                    spots[i, p] = null;
                    p++;
                }
                i++;
            }
            if (dialogResult == DialogResult.Yes)
            {
                spots_left = 81;
                mines = 10;
                set_up_board();
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
            }


        }
        void win()
        {
            DialogResult dialogResult = MessageBox.Show("You Win, Play Again?", "Nice!", MessageBoxButtons.YesNo);
            int i = 0;
            while (i < 9)
            {
                int p = 0;
                while (p < 9)
                {
                    btnArray[i, p].Dispose();
                    lblArray[i, p].Dispose();
                    spots[i, p] = null;
                    p++;
                }
                i++;
            }
            if (dialogResult == DialogResult.Yes)
            {
                mines = 10;
                spots_left = 81;
                set_up_board();
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }

        void check_spot(int x, int y)
        {
            btnArray[x, y].Dispose();
            spots[x, y].proximity = 10;
            for (int r = x - 1; r <= x + 1; r++)
            {
                for (int c = y - 1; c <= y + 1; c++)
                {
                    try
                    {
                        if (spots[r, c].proximity == 0)
                        {
                            check_spot(r, c);
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (spots[r, c].proximity > 0 && spots[r, c].proximity < 9)
                        {
                            btnArray[r, c].Dispose();
                            spots[r, c].proximity = 10;


                        }
                    }
                    catch
                    {

                    }

                }
            }
        }
    }

}

