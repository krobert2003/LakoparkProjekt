using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LakoparkProjekt
{
    public partial class Form_Statisztika :Form
    {
        public void Form_Statisztika_Load(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                FileInfo legutolso = directory.GetFiles("statisztika_*.txt").OrderByDescending(f => f.LastWriteTime).First();
                f1.textBox1.Text = File.ReadAllText(legutolso.Name);
                f1.textBox1.Select(0, 0);
            }
            catch (IOException ex)
            {
                f1.textBox1.Text = "Statisztika fájl nem jeleníthető meg!";
                throw;
            }
        }

        private void Form_Statisztika_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
