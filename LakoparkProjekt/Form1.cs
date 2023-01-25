using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LakoparkProjekt
{
    public partial class Form1 : Form
    {
        HappyLiving happyLiving = new HappyLiving(@"..\..\lakoparkok.txt");
        readonly int buttonSize = 50;
        int aktPark = 0;
        List<Image> szintek = new List<Image>();
        Form form_statisztika = new Form_Statisztika();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            szintek.Add(Image.FromFile(@"..\\..\\Kepek\kereszt.jpg"));  
            szintek.Add(Image.FromFile(@"..\\..\\Kepek\haz1.jpg"));     
            szintek.Add(Image.FromFile(@"..\\..\\Kepek\haz2.jpg"));     
            szintek.Add(Image.FromFile(@"..\\..\\Kepek\haz3.jpg"));     
            PanelUpdate();

        }

        void PanelUpdate()
        {
            this.Text = happyLiving.Lakoparkok[aktPark].Nev + " lakópark";
            if (aktPark == 0)
            {
                button1.Enabled = false;
                button2.Hide();
            }
            else if (aktPark == happyLiving.Lakoparkok.Count - 1)
            {
                button2.Enabled = false;
                button2.Hide();
            }
            else
            {
                button1.Enabled = true;
                button1.Show();
                button2.Enabled = true;
                button2.Show();

            }
            pictureBox1.BackgroundImage = happyLiving.Lakoparkok[aktPark].Nevado;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Clear();
            for (int i = 0; i < happyLiving.Lakoparkok[aktPark].Hazak.GetLength(1); i++)
            {
                for (int j = 0; j < happyLiving.Lakoparkok[aktPark].Hazak.GetLength(0); j++)
                {
                 
                    Button g = new Button();
                    g.BackgroundImage = szintek[happyLiving.Lakoparkok[aktPark].Hazak[j, i]];
                    g.BackgroundImageLayout = ImageLayout.Stretch;
                    g.SetBounds(i * buttonSize, j * buttonSize, buttonSize, buttonSize);
               
                    int utca = j;
                    int haz = i;
                    g.Click += (o, e) =>
                    {
                        happyLiving.Lakoparkok[aktPark].UjSzint(utca, haz);
                        PanelUpdate();
                    };
                    panel2.Controls.Add(g);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aktPark--;
            PanelUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aktPark++;
            PanelUpdate();
        }

        private void button_Mentes_Click(object sender, EventArgs e)
        {
            if (happyLiving.Mentes())
            {
                MessageBox.Show("Sikeres Mentés");
            }
            else
            {
                MessageBox.Show("Adatok mentése nem sikerült!");
            }
        }

        private void button_Statisztika_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("statisztika_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"))
                {
                    sw.WriteLine("Statisztika");
                 
                    foreach (Lakopark item in happyLiving.Lakoparkok)
                    {
                        item.beepitettsegiAranytSzamol();
                        item.teljesBeepitettsegetVizsgal();
                    }
                   
                    sw.WriteLine();
                    bool nincsTeljesenBeepitett = true;
                    foreach (Lakopark item in happyLiving.Lakoparkok)
                    {
                        if (item.VanTeljesenBeepitettUtca)
                        {
                            sw.WriteLine($"A {item.Nev} lakópark {item.ElsoTeljesenBeepitettUtca}. utcája teljesen beépített");
                            nincsTeljesenBeepitett = false;
                            break;
                        }
                    }
                    if (nincsTeljesenBeepitett)
                    {
                     
                        sw.WriteLine("A HappyLiving cég tulajdonában nincs teljesen beépített utca");
                    }
   
                    sw.WriteLine();
                    Lakopark legjobbanBeepitett = happyLiving.Lakoparkok.OrderBy(s => s.BeepitettsegiArany).Last();
                    sw.WriteLine($"\nA legjobban beépített a {legjobbanBeepitett.Nev} lakópark {legjobbanBeepitett.BeepitettsegiArany * 100:N1} % beépítettséggel.");

               
                    sw.WriteLine();
                    sw.WriteLine($"\nA HappyLiving cégnek az összes bevétele {happyLiving.Lakoparkok.Sum(a => a.ertekesitesiOsszeg()):N0} Ft");
                }
                form_statisztika.ShowDialog();

            }
            catch (IOException ex)
            {
                MessageBox.Show("A statisztikai adatok mentése sikertelen!\n\n" + ex.Message);
                return;
            }
        }

       
    }
}