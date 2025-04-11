using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Estacionamiento_V2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
            button3.Visible = false;
            linkLabel1.Visible = false;
            textBox1.Text = "Ingrese su usuario";
            textBox1.ForeColor = Color.Black;
            textBox1.Enter += textBox_Enter;
            textBox1.Leave += textBox_Leave;
            label4.Visible = false;
            label5.Visible = false;
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                if (txt.Text == "Ingrese su usuario")
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;

                }
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    if (txt == textBox1)
                    {
                        txt.Text = "Ingrese su usuario";
                    }
                    txt.ForeColor = Color.White;
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usua = textBox1.Text.Trim();
            string contra = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(usua))
            {
                MessageBox.Show("Por favor, ingresa un nombre de usuario o si eres Administrador podras ingresar tu Numero de Tarjeta.");
                return;
            }

            string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                
                string tarjetaQuery = "SELECT COUNT(*) FROM Administrador WHERE Tarjeta = @Tarjeta";
                using (SqlCommand tarjetaCmd = new SqlCommand(tarjetaQuery, cn))
                {
                    tarjetaCmd.Parameters.AddWithValue("@Tarjeta", usua);

                    int tarjetaCount = (int)tarjetaCmd.ExecuteScalar();
                    if (tarjetaCount > 0)
                    {
                        MessageBox.Show("Inicio de sesión con tarjeta exitoso.");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox1.Focus();

                        Form5 form5 = new Form5();
                        form5.Show();
                        this.Hide();
                        return;
                    }
                }

                
                if (!string.IsNullOrEmpty(contra))
                {
                    string adminQuery = "SELECT COUNT(*) FROM Administrador WHERE NombreDeAdministrador = @Nombre AND Contraseña = @Contra";
                    using (SqlCommand adminCmd = new SqlCommand(adminQuery, cn))
                    {
                        adminCmd.Parameters.AddWithValue("@Nombre", usua);
                        adminCmd.Parameters.AddWithValue("@Contra", contra);

                        int adminCount = (int)adminCmd.ExecuteScalar();
                        if (adminCount > 0)
                        {
                            MessageBox.Show("Inicio de sesión de administrador exitoso.");
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox1.Focus();

                            Form5 form5 = new Form5();
                            form5.Show();
                            this.Hide();
                            return;
                        }
                    }
                }

                
                if (!string.IsNullOrEmpty(contra))
                {
                    string query = "SELECT COUNT(*) FROM [Registro_Usuario] WHERE Nombre_de_usuario = @Nombre AND Contraseña = @Contra";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", usua);
                        cmd.Parameters.AddWithValue("@Contra", contra);

                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Inicio de sesión exitoso.");
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox1.Focus();

                            Form3 form3 = new Form3(usua);
                            form3.Show();
                            this.Hide();
                            return;
                        }
                        else
                        {
                            string userExistsQuery = "SELECT COUNT(*) FROM [Registro_Usuario] WHERE Nombre_de_usuario = @Nombre";
                            using (SqlCommand userCmd = new SqlCommand(userExistsQuery, cn))
                            {
                                userCmd.Parameters.AddWithValue("@Nombre", usua);
                                int userExists = (int)userCmd.ExecuteScalar();

                                if (userExists > 0)
                                {
                                    MessageBox.Show("Contraseña incorrecta.");
                                }
                                else
                                {
                                    MessageBox.Show("Usuario no registrado.");
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa una contraseña.");
                }
            }
        }
    
            

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Form3 form3 = new Form3();

           
            form3.Show();

            
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();

            
            form5.Show();

            
            this.Hide();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
