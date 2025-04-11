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
using System.Data.SqlClient;  

namespace Estacionamiento_V2._0
{
    public partial class Form5 : Form
    {
         //Conecta la base Mex
        private string conexionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";

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
            button4.Visible = false;
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
            button1.Visible = false;
            button2.Visible = false;
            dataGridView3.Visible = false;
            button3.Visible = false;
            dataGridView2.Visible = false;
            txtbuscarusuario.Visible=false;
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
            dataGridView1.Visible = false;
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
            btnlimpiar.Visible = false;
            dataGridView2.Visible = true;
            button1.Visible = true;
            dataGridView3.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button4.Visible = true;

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
            button1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
        }


        private void btnbuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Ingrese un número de tarjeta.");
                return;
            }

            string numeroTarjeta = txtId.Text.Trim();

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT [ID_Tarjetas], [FK_Registro], [Numero_Tarjeta], [Credito] FROM Tarjetas WHERE [Numero_Tarjeta] = @numero";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@numero", numeroTarjeta);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        
                        if (tabla.Rows.Count == 0)
                        {
                            MessageBox.Show("Esta tarjeta no pertenece al estacionamiento.", "Tarjeta no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView2.DataSource = null; 
                        }
                        else
                        {
                            dataGridView2.DataSource = tabla;

                            
                            dataGridView2.ReadOnly = false;
                            foreach (DataGridViewColumn col in dataGridView2.Columns)
                            {
                                col.ReadOnly = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar tarjeta: " + ex.Message);
                }
            }
        }

        private void BuscarTarjeta(int idCliente)
        {
            using (SqlConnection conexion = new SqlConnection(conexionString)) 
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT id, nombre, numero, email FROM tarjetas WHERE id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", idCliente);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd); 
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
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT ID_Reservas AS ID_Reservas, NombreUsuario, FechaReserva, HoraReserva, Lugar, Estatus FROM Reservas";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
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
            button4.Visible = false;
            btnreservacionesencurso.Visible = false;
            lblId.Visible = false;
            txtId.Visible = false;
            dataGridView1.Visible = true;
            btnbuscar.Visible = false;
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
            btnlimpiar.Visible = false;
            dataGridView3.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button1.Visible = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            lblreservaciones.Visible = false;
            button4.Visible = false;
            btnreservacionesencurso.Visible = false;
            lblId.Visible = false;
            txtId.Visible = false;
            dataGridView1.Visible = false;
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
            Form6 form6 = new Form6();
            form6.Show();
            
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
            
        }

        private void bntlimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    conexion.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells["Estatus"].Value != null)
                        {
                            string nuevoEstatus = row.Cells["Estatus"].Value.ToString();
                            int idReserva = Convert.ToInt32(row.Cells["ID_Reservas"].Value);

                            string updateQuery = "UPDATE Reservas SET Estatus = @estatus WHERE ID_Reservas = @idReserva";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conexion))
                            {
                                cmd.Parameters.AddWithValue("@estatus", nuevoEstatus);
                                cmd.Parameters.AddWithValue("@idReserva", idReserva);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                CargarReservacionesEnCurso(); 
                MessageBox.Show("Los estatus se han actualizado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los estatus: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    conexion.Open();

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        
                        if (row.Cells["Credito"].Value != null)
                        {
                            decimal creditoNuevo;
                            bool esCreditoValido = Decimal.TryParse(row.Cells["Credito"].Value.ToString(), out creditoNuevo);

                            if (esCreditoValido)
                            {
                                int idTarjeta = Convert.ToInt32(row.Cells["ID_Tarjetas"].Value); 

                                
                                string updateQuery = "UPDATE Tarjetas SET [Credito] = @credito WHERE [ID_Tarjetas] = @id_Tarjetas";
                                using (SqlCommand cmd = new SqlCommand(updateQuery, conexion))
                                {
                                    cmd.Parameters.AddWithValue("@credito", creditoNuevo);
                                    cmd.Parameters.AddWithValue("@id_Tarjetas", idTarjeta);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                MessageBox.Show("El valor del crédito no es válido.");
                                return;
                            }
                        }
                    }

                    MessageBox.Show("Los cambios se han guardado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbuscarusuario.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre de usuario para buscar.");
                return;
            }

            string nombreUsuario = txtbuscarusuario.Text.Trim();

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    
                    string query = @"
                SELECT 
                    RU.ID_Registro,
                    RU.Nombre,
                    RU.Apellido,
                    RU.Nombre_de_usuario,
                    RU.Contraseña,
                    RU.Numero_de_celular,
                    T.Numero_Tarjeta
                FROM Registro_Usuario RU
                LEFT JOIN Tarjetas T ON RU.ID_Registro = T.FK_Registro
                WHERE RU.Nombre_de_usuario = @nombreUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        if (tabla.Rows.Count > 0)
                        {
                            dataGridView3.DataSource = tabla;
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron usuarios con ese nombre de usuario.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar el usuario: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    conexion.Open();

                    if (dataGridView3.Rows.Count == 0)
                    {
                        MessageBox.Show("No hay datos para actualizar.");
                        return;
                    }

                    foreach (DataGridViewRow row in dataGridView3.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells["ID_Registro"].Value != null)
                        {
                            int idRegistro = Convert.ToInt32(row.Cells["ID_Registro"].Value);

                            string nuevoNombre = row.Cells["Nombre"].Value?.ToString() ?? "";
                            string nuevoApellido = row.Cells["Apellido"].Value?.ToString() ?? "";
                            string nuevoUsuario = row.Cells["Nombre_de_usuario"].Value?.ToString() ?? "";
                            string nuevaContraseña = row.Cells["Contraseña"].Value?.ToString() ?? "";
                            string nuevoCelular = row.Cells["Numero_de_celular"].Value?.ToString() ?? "";

                            
                            string nuevoNumeroTarjeta = row.Cells["Numero_Tarjeta"].Value?.ToString() ?? "";

                            
                            string updateQuery = "UPDATE [Registro_Usuario] SET " +
                                                 "[Nombre] = @nuevoNombre, " +
                                                 "[Apellido] = @nuevoApellido, " +
                                                 "[Nombre_de_usuario] = @nuevoUsuario, " +
                                                 "[Contraseña] = @nuevaContraseña, " +
                                                 "[Numero_de_celular] = @nuevoCelular " +
                                                 "WHERE [ID_Registro] = @idRegistro";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conexion))
                            {
                                cmd.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                                cmd.Parameters.AddWithValue("@nuevoApellido", nuevoApellido);
                                cmd.Parameters.AddWithValue("@nuevoUsuario", nuevoUsuario);
                                cmd.Parameters.AddWithValue("@nuevaContraseña", nuevaContraseña);
                                cmd.Parameters.AddWithValue("@nuevoCelular", nuevoCelular);
                                cmd.Parameters.AddWithValue("@idRegistro", idRegistro);

                                cmd.ExecuteNonQuery();
                            }

                            if (!string.IsNullOrEmpty(nuevoNumeroTarjeta))
                            {
                                
                                string checkTarjetaQuery = "SELECT COUNT(*) FROM Tarjetas WHERE FK_Registro = @idRegistro";
                                using (SqlCommand checkCmd = new SqlCommand(checkTarjetaQuery, conexion))
                                {
                                    checkCmd.Parameters.AddWithValue("@idRegistro", idRegistro);
                                    int count = (int)checkCmd.ExecuteScalar();

                                    if (count > 0)
                                    {
                                        
                                        string updateTarjetaQuery = "UPDATE Tarjetas SET Numero_Tarjeta = @numeroTarjeta WHERE FK_Registro = @idRegistro";
                                        using (SqlCommand updateTarjetaCmd = new SqlCommand(updateTarjetaQuery, conexion))
                                        {
                                            updateTarjetaCmd.Parameters.AddWithValue("@numeroTarjeta", nuevoNumeroTarjeta);
                                            updateTarjetaCmd.Parameters.AddWithValue("@idRegistro", idRegistro);
                                            updateTarjetaCmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        
                                        string insertTarjetaQuery = "INSERT INTO Tarjetas (FK_Registro, Numero_Tarjeta, Credito) VALUES (@idRegistro, @numeroTarjeta, 0)";
                                        using (SqlCommand insertTarjetaCmd = new SqlCommand(insertTarjetaQuery, conexion))
                                        {
                                            insertTarjetaCmd.Parameters.AddWithValue("@idRegistro", idRegistro);
                                            insertTarjetaCmd.Parameters.AddWithValue("@numeroTarjeta", nuevoNumeroTarjeta);
                                            insertTarjetaCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    CargarDatosUsuarios();
                    MessageBox.Show("Los cambios se han guardado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message);
            }
        }

            private void CargarDatosUsuarios()
        {
            string nombreUsuario = txtbuscarusuario.Text.Trim();

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT [ID_Registro], [Nombre], [Apellido], [Nombre_de_usuario], [Contraseña], [Numero_de_celular] FROM [Registro_Usuario] WHERE [Nombre_de_usuario] = @nombreUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        
                        dataGridView3.DataSource = tabla;

                        
                        if (dataGridView3.Columns.Contains("ID_Registro"))
                        {
                            dataGridView3.Columns["ID_Registro"].Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al recargar los datos: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            
        }
    }
}
//Aldair Flores
//Alejandro Mex
//UNID 