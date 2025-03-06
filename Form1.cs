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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usua = textBox1.Text;

            if (string.IsNullOrEmpty(usua))
            {
                MessageBox.Show("Por Favor, Ingresa un nombre de usuario");
                return;
            }

            string contra = textBox2.Text;

            if (string.IsNullOrEmpty(contra))
            {
                MessageBox.Show("Por Favor, Ingresa una contraseña");
                return;
            }

            string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT COUNT(*) FROM [Registro-usuario] WHERE Nombre_de_usuario = @Nombre AND Contraseña = @Contra";
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

                    }

                    string userExistsQuery = "SELECT COUNT(*) FROM [Registro-usuario] WHERE Nombre_de_usuario = @Nombre";
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();

            // Mostrar Formulario3
            form3.Show();

            // Ocultar Formulario1 (opcional)
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();

            // Mostrar Formulario3
            form5.Show();

            // Ocultar Formulario1 (opcional)
            this.Hide();
        }
    }
    }
