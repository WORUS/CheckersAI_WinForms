using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersAI
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            MainMenu();

        }
        public void MainMenu() {


            PictureBox pBoxMenu = new PictureBox();
            pBoxMenu.Location = new Point(0, 0);
            pBoxMenu.Image = Properties.Resources.mainmenu;
            pBoxMenu.Dock = DockStyle.Fill;
            Button btnStart = new Button();
            Button btnSetngs = new Button();
            btnStart.Location = new Point(200, 85);
            btnSetngs.Location = new Point(200, 170);
            btnStart.Width = 200;
            btnStart.Height = 60;
            btnSetngs.Width = 200;
            btnSetngs.Height = 60;
            btnStart.Text = "Начать игру";
            btnSetngs.Text = "Настройки";
            btnSetngs.Click += new EventHandler(butStngsClick);
            btnStart.Click += new EventHandler(btnStartClick);
            this.Controls.Add(btnSetngs);
            this.Controls.Add(btnStart);
            this.Controls.Add(pBoxMenu);

        }
        public System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

        public void btnStartClick(object sender, EventArgs e) {
            System.IO.Stream resourceStream =
                   assembly.GetManifestResourceStream(@"CheckersAI.NewGame.wav");
            SoundPlayer player = new SoundPlayer(resourceStream);
            player.Play();
            Form a = new Form2();
            a.Show();
        }

        public void ClickBackMenu(object sender, EventArgs e){

            System.IO.Stream resourceStream =
                   assembly.GetManifestResourceStream(@"CheckersAI.mmsound.wav");
            SoundPlayer player = new SoundPlayer(resourceStream);
            player.Play();
            this.Controls.Clear();
            MainMenu();
        }
        public void butStngsClick(object sender, EventArgs e)
        {
            System.IO.Stream resourceStream =
                  assembly.GetManifestResourceStream(@"CheckersAI.mmsound.wav");
            SoundPlayer player = new SoundPlayer(resourceStream);
            player.Play();
            this.Controls.Clear();
            TrackBar trb = new TrackBar();
            Button butMenu = new Button();
            Label lbl = new Label();
            lbl.Text = "Выберите сложность";
            lbl.Width = 200;
            lbl.Location = new Point(245, 130);
            butMenu.Dock = DockStyle.Bottom;
            butMenu.Text = "Вернуться в главное меню";
            butMenu.Height = 40;
            trb.Maximum = 1;
            trb.Location = new Point(250,150);
            butMenu.Click += new EventHandler(ClickBackMenu);
            this.Controls.Add(trb);
            this.Controls.Add(butMenu); 
            this.Controls.Add(lbl);


        }
    }
}
