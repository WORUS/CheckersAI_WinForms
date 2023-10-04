using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace CheckersAI
{
    public partial class Form1 : Form
    {
        public PictureBox pb = new PictureBox();
        static readonly public int cellCount = 64;
        public Image img;

        public Form1()
        {
            Program.f1 = this;
            //this.BackgroundImageLayout = ImageLayout.Stretch;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        img = new Bitmap(imageList1.Images[0],70,70);

            
            
        }



        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
           // MessageBox.Show("Ты опять ошибся с кнопками");
        }

        public void ClickOnPBTest(object sender, EventArgs e) {
            PictureBox pressedBox = sender as PictureBox;
            MessageBox.Show(pressedBox.Location.X.ToString() + "  " + pressedBox.Location.Y.ToString());

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
           // MessageBox.Show("Что-то нажато");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream resourceStream =
                assembly.GetManifestResourceStream(@"CheckersAI.NewGame.wav");
            SoundPlayer player = new SoundPlayer(resourceStream);
            player.Play();
            Program.f2.RefreshField();
            Program.f2.Controls.Clear();
            Program.f2.RefreshMapImage();
        }

       
    }
}
