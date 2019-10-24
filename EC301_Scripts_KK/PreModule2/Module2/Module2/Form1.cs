using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double result = new double();

            switch (comboBox1.Text)
            {
                case "+":
                    result = Convert.ToDouble(textBox1.Text) + Convert.ToDouble(textBox2.Text);
                    break;

                case "-":
                    result = Convert.ToDouble(textBox1.Text) - Convert.ToDouble(textBox2.Text);
                    break;

                case "*":
                    result = Convert.ToDouble(textBox1.Text) * Convert.ToDouble(textBox2.Text);
                    break;

                case "/":
                    result = Convert.ToDouble(textBox1.Text) / Convert.ToDouble(textBox2.Text);
                    break;

            }

            textBox3.Text = Convert.ToString(result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Write code here to determine if it's a palindrome
            string inpString = textBox4.Text.Replace(" ", "");
            char[] inp = inpString.ToCharArray();
            Array.Reverse(inp);
            string revString = new string(inp);
            if (inpString == revString)
                textBox5.Text = "yes";
            else
                textBox5.Text = "no";
        }
    }
}
