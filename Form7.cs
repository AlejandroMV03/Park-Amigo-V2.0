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
    public partial class Form7 : Form
    {
        private string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
           
            CargarSolicitudes();
        }

        private void CargarSolicitudes()
        {
            
            string query = "SELECT ID_Solicitudes, NombreUsuario, FechaSolicitud FROM Solicitudes";

            
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    
                    conexion.Open();

                    
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);

                   
                    DataTable dtSolicitudes = new DataTable();

                    
                    adapter.Fill(dtSolicitudes);

                   
                    dataGridView1.DataSource = dtSolicitudes;
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Error al cargar las solicitudes: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }
    }
}