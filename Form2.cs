using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Estacionamiento_V2._0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO [Registro_Usuario] (Nombre, Apellido, Nombre_de_usuario, Contraseña, Numero_de_celular) VALUES (@Nombre, @Apellido, @Nombre_de_usuario, @Contraseña, @Numero_de_celular)";
               
                using (SqlCommand cmd = new SqlCommand(query, cn)) 
                {
                    cmd.Parameters.AddWithValue("@Nombre", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Apellido", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Nombre_de_usuario", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Contraseña", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Numero_de_celular", textBox5.Text);

                    try
                    {
                        cn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registro guardado exitosamente.");

                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            textBox5.Text = "";

                            textBox1.Focus();

                            Form1 form1 = new Form1();
                            form1.Show();
                            this.Hide(); 
                        }
                        else
                        {
                            MessageBox.Show("Error al guardar el registro.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }

                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
