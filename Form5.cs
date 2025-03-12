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
using System.Data.SqlClient;  // Usar SqlClient para SQL Server

namespace Estacionamiento_V2._0
{
    public partial class Form5 : Form
    {
        // Cadena de conexión para SQL Server
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
                    string query = "SELECT [ID], [ID_Usuario], [NumeroTarjeta], [Credito] FROM Tarjetas WHERE [NumeroTarjeta] = @numero";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@numero", numeroTarjeta);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        // Mostrar los resultados en el DataGridView2
                        dataGridView2.DataSource = tabla;

                        // Permitir la edición de celdas
                        dataGridView2.ReadOnly = false;
                        foreach (DataGridViewColumn col in dataGridView2.Columns)
                        {
                            col.ReadOnly = false;
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
            using (SqlConnection conexion = new SqlConnection(conexionString)) // Cambiado a SqlConnection
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT id, nombre, numero, email FROM tarjetas WHERE id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conexion)) // Cambiado a SqlCommand
                    {
                        cmd.Parameters.AddWithValue("@id", idCliente);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd); // Cambiado a SqlDataAdapter
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
                    string query = "SELECT IdReserva, NombreUsuario, FechaReserva, HoraReserva, Lugar, Estatus FROM Reservas";
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
            try
            {
                // Guardar cambios en el estado
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    conexion.Open();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Verificar si el estatus está marcado como "Finalizado"
                        if (row.Cells["Estatus"].Value != null && row.Cells["Estatus"].Value.ToString() == "Finalizada")
                        {
                            // Obtener el ID de la reserva (usando IdReserva como nombre de columna)
                            int idReserva = Convert.ToInt32(row.Cells["IdReserva"].Value);

                            // Eliminar la reserva finalizada
                            string deleteQuery = "DELETE FROM Reservas WHERE IdReserva = @IdReserva";
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conexion))
                            {
                                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // Recargar las reservaciones actualizadas
                CargarReservacionesEnCurso(); // Aquí se usa el método adecuado
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar las reservaciones finalizadas: " + ex.Message);
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
                        // Verificar si la celda de Crédito ha sido modificada
                        if (row.Cells["Credito"].Value != null)
                        {
                            decimal creditoNuevo;
                            bool esCreditoValido = Decimal.TryParse(row.Cells["Credito"].Value.ToString(), out creditoNuevo);

                            if (esCreditoValido)
                            {
                                int idTarjeta = Convert.ToInt32(row.Cells["ID"].Value); // Usamos la columna ID

                                // Actualizar el valor del crédito en la base de datos
                                string updateQuery = "UPDATE Tarjetas SET [Credito] = @credito WHERE [ID] = @id";
                                using (SqlCommand cmd = new SqlCommand(updateQuery, conexion))
                                {
                                    cmd.Parameters.AddWithValue("@credito", creditoNuevo);
                                    cmd.Parameters.AddWithValue("@id", idTarjeta);
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
            // Verificar si el campo de búsqueda no está vacío
            if (string.IsNullOrEmpty(txtbuscarusuario.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre de usuario para buscar.");
                return;
            }

            string nombreUsuario = txtbuscarusuario.Text.Trim();

            // Realizar la búsqueda en la base de datos
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    // Query para buscar al usuario por nombre de usuario
                    string query = "SELECT [Nombre], [Apellido], [Nombre_de_usuario], [Contraseña], [Numero_de_celular] FROM [Registro-usuario] WHERE [Nombre_de_usuario] = @nombreUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        // Verificar si se encontraron resultados
                        if (tabla.Rows.Count > 0)
                        {
                            // Mostrar los resultados en el DataGridView3
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
                        if (row.Cells["ID"].Value != null)
                        {
                            int idUsuario;
                            if (!int.TryParse(row.Cells["ID"].Value.ToString(), out idUsuario))
                            {
                                continue; // Si no se puede convertir, salta a la siguiente fila
                            }

                            string nuevoNombre = row.Cells["Nombre"].Value?.ToString() ?? "";
                            string nuevoApellido = row.Cells["Apellido"].Value?.ToString() ?? "";
                            string nuevoUsuario = row.Cells["Nombre_de_usuario"].Value?.ToString() ?? "";
                            string nuevaContraseña = row.Cells["Contraseña"].Value?.ToString() ?? "";
                            string nuevoCelular = row.Cells["Numero_de_celular"].Value?.ToString() ?? "";

                            string updateQuery = "UPDATE [Registro-usuario] SET " +
                                                 "[Nombre] = @nuevoNombre, " +
                                                 "[Apellido] = @nuevoApellido, " +
                                                 "[Nombre_de_usuario] = @nuevoUsuario, " +
                                                 "[Contraseña] = @nuevaContraseña, " +
                                                 "[Numero_de_celular] = @nuevoCelular " +
                                                 "WHERE [ID] = @idUsuario";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conexion))
                            {
                                cmd.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                                cmd.Parameters.AddWithValue("@nuevoApellido", nuevoApellido);
                                cmd.Parameters.AddWithValue("@nuevoUsuario", nuevoUsuario);
                                cmd.Parameters.AddWithValue("@nuevaContraseña", nuevaContraseña);
                                cmd.Parameters.AddWithValue("@nuevoCelular", nuevoCelular);
                                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                                cmd.ExecuteNonQuery();
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
                    string query = "SELECT [ID], [Nombre], [Apellido], [Nombre_de_usuario], [Contraseña], [Numero_de_celular] FROM [Registro-usuario] WHERE [Nombre_de_usuario] = @nombreUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        // Asignar los datos al DataGridView
                        dataGridView3.DataSource = tabla;

                        // Asegurar que la columna 'ID' está visible
                        if (dataGridView3.Columns.Contains("ID"))
                        {
                            dataGridView3.Columns["ID"].Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al recargar los datos: " + ex.Message);
                }
            }
        }
    }
}