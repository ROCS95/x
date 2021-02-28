using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonolithConect
{
    public partial class IpSetUp : Form
    {
        public IpSetUp()
        {
            InitializeComponent();
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Aceptar_Click(object sender, EventArgs e)
        {
            String AllowedChars = @"^\d+$";
            if (isValidIP(AllowedChars))
            {
                using (StreamWriter ip = new StreamWriter(Path.Combine("ip.txt")))
                {
                    ip.WriteLine(IP1.Text+"."+ IP2.Text + "." + IP3.Text + "." + IP4.Text);
                }
                if (Port.Text.ToString() != "" && Regex.IsMatch(Port.Text, AllowedChars))
                {
                    using (StreamWriter port = new StreamWriter(Path.Combine("port.txt")))
                    {
                        port.WriteLine(Port.Text.ToString());
                    }
                }
                else
                {
                    using (StreamWriter port = new StreamWriter(Path.Combine("port.txt")))
                    {
                        port.WriteLine("8080");
                    }
                }
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("El formato de la IP es incorrecto o el campo esta en blanco");
            }
        }

        private bool isValidIP(string allowedChars)
        {
            bool valid = false;
            if (IP1.Text != "" && IP2.Text != "" && IP3.Text != "" && IP4.Text != "")
            {
                if (Regex.IsMatch(IP1.Text, allowedChars)&& Regex.IsMatch(IP2.Text, allowedChars)
                    && Regex.IsMatch(IP3.Text, allowedChars)&& Regex.IsMatch(IP4.Text, allowedChars))
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
            return valid;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("El formato de la IP es 0.0.0.0, no puede estar en blanco. \nSi el campo de Puerto esta en blanco o es invalido el puerto sera 8080.");
        }

        private void IP1_TextChanged(object sender, EventArgs e)
        {
            //Get text from textbox
            string text = ((TextBox)sender).Text;


            //Do what ever it is you want to do to edit the text
            if (text.Length == 3)
            {
                IP2.Focus();
            }

        }

        private void IP2_TextChanged(object sender, EventArgs e)
        {
            //Get text from textbox
            string text = ((TextBox)sender).Text;


            //Do what ever it is you want to do to edit the text
            if (text.Length == 3)
            {
                IP3.Focus();
            }
            else if (text.Length == 0)
            {
                IP1.Focus();
            }
        }

        private void IP3_TextChanged(object sender, EventArgs e)
        {
            //Get text from textbox
            string text = ((TextBox)sender).Text;


            //Do what ever it is you want to do to edit the text
            if (text.Length == 3)
            {
                IP4.Focus();
            }
            else if (text.Length == 0)
            {
                IP2.Focus();
            }
        }

        private void IP4_TextChanged(object sender, EventArgs e)
        {
            //Get text from textbox
            string text = ((TextBox)sender).Text;


            //Do what ever it is you want to do to edit the text
            if (text.Length == 0)
            {
                IP3.Focus();
            }
        }
    }
}
