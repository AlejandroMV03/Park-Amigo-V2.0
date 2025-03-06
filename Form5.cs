using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Estacionamiento_V2._0
{
    public partial class Form5 : Form
    {
        private string conexionString = "localhost ;"; // conexion localhost //
        public int TarifaPorHora { get; private set; } = 30;
        public string MetodoPago { get; private set; } = "Efectivo";
        public bool PermitirEntradaSinSaldo { get; private set; } = false;

        public Form5()
        {
            InitializeComponent();
            numericUpDown1.Value = TarifaPorHora;
            comboBox1.Items.AddRange(new string[] { "Efectivo", "Tarjeta", "NFC", "Transferencia" });
            comboBox1.SelectedItem = MetodoPago;
            checkBox1.Checked = PermitirEntradaSinSaldo;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            lblreservaciones.Visible = true;
            btnreservacionesencurso.Visible = true;
            lblId.Visible = false;
            txtId.Visible = false;
            dataGridView1.Visible = true;
            btnlimpiar.Visible = true;
            btnbuscar.Visible = false;
            comboBox1.Visible = false;
            checkBox1.Visible = false;
            lbltarifa.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            btnconfig.Visible = false;
            label4.Visible = false;
            linkLabel1.Visible = false;


        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();


            form1.Show();


            this.Hide();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            lblId.Visible = true;
            txtId.Visible = true;
            dataGridView1.Visible = true;
            lblreservaciones.Visible = false;
            btnreservacionesencurso.Visible = false;
            btnbuscar.Visible = true;
            comboBox1.Visible = false;
            checkBox1.Visible = false;
            lbltarifa.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            btnconfig.Visible = false;
            numericUpDown1.Visible = false;
            label4.Visible = false;
            txtbuscarusuario.Visible = false;
            linkLabel1.Visible = false;
            btnlimpiar.Visible = true;
        }


        private void Form5_Load(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            lblreservaciones.Visible = false;
            lblId.Visible = false;
            txtId.Visible = false;
            lblreservaciones.Visible = false;
            btnreservacionesencurso.Visible = false;
            btnbuscar.Visible = false;
            comboBox1.Visible = false;
            checkBox1.Visible = false;
            lbltarifa.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            btnconfig.Visible = false;
            numericUpDown1.Visible = false;
            label4.Visible = false;
            txtbuscarusuario.Visible = false;
            linkLabel1.Visible = false;
            btnlimpiar.Visible = false;


        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int idTarjeta))
            {
                BuscarTarjeta(idTarjeta);
            }
            else
            {
                MessageBox.Show("Ingrese un ID válido.");
            }
            string nombreCliente = txtbuscarusuario.Text.Trim();

            if (string.IsNullOrEmpty(nombreCliente))
            {
                MessageBox.Show("Ingrese un nombre para buscar.");
                return;
            }

            using (MySqlConnection conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT id, nombre, numero, email FROM clientes WHERE nombre LIKE @nombre";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", "%" + nombreCliente + "%");
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView1.DataSource = tabla;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        


        private void BuscarTarjeta(int idCliente)
        {
            using (MySqlConnection conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT id, nombre, numero, email FROM tarjetas WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", idCliente);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView1.DataSource = tabla;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void btnreservacionesencurso_Click(object sender, EventArgs e)
        {
            CargarReservacionesEnCurso();
        }

        private void CargarReservacionesEnCurso()
        {
            using (MySqlConnection conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open(); //cinexion base dadoz //
                    string query = @"
                        SELECT r.id AS 'ID Reservación', c.nombre AS 'Cliente', c.numero AS 'Teléfono',
                               c.email AS 'Email', r.fecha_ingreso AS 'Fecha de Ingreso', r.espacio AS 'Espacio'
                        FROM reservaciones r
                        JOIN clientes c ON r.id_cliente = c.id
                        WHERE r.fecha_salida IS NULL";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dataGridView1.DataSource = tabla;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar las reservaciones: " + ex.Message);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            lblreservaciones.Visible = false;
            btnreservacionesencurso.Visible = false;
            lblId.Visible = false;
            txtId.Visible = false;
            dataGridView1.Visible = true;
            btnbuscar.Visible = true;
            comboBox1.Visible = false;
            checkBox1.Visible = false;
            lbltarifa.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            btnconfig.Visible = false;
            numericUpDown1.Visible = false;
            label4.Visible = true;
            txtbuscarusuario.Visible = true;
            linkLabel1.Visible = true;
            btnlimpiar.Visible = true;


        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            lblreservaciones.Visible = false;
            btnreservacionesencurso.Visible = false;
            lblId.Visible = false;
            txtId.Visible = false;
            dataGridView1.Visible = false;
            btnbuscar.Visible = false;
            comboBox1.Visible = true;
            checkBox1.Visible = true;
            lbltarifa.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            btnconfig.Visible = true;
            numericUpDown1.Visible = true;
            label4.Visible = false;
            txtbuscarusuario.Visible = false;
            linkLabel1.Visible = false;
            btnlimpiar.Visible = false;
        }

        private void btnconfig_Click(object sender, EventArgs e)
        {
            TarifaPorHora = (int)numericUpDown1.Value;
            MetodoPago = comboBox1.SelectedItem.ToString();
            PermitirEntradaSinSaldo = checkBox1.Checked;

            MessageBox.Show("Configuración aplicada correctamente.", "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void bntlimpiar_Click(object sender, EventArgs e)
        {
            
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

         
            txtId.Clear();
            txtbuscarusuario.Clear();
          
        }

        
        }
    }




            
        