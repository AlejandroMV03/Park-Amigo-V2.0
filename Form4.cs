using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Estacionamiento_V2._0
{
    public partial class Form4 : Form
    {
        private string numeroTelefono; // Variable para almacenar el número de teléfono
        private int codigoGenerado;    // Código de verificación
        private DateTime tiempoGeneracion; // Momento en que se generó el código
        private int tiempoRestante;

        // Cadena de conexión a la base de datos (modifícala según tu configuración)
        private string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";

        public Form4(string usuario, string fecha, string hora, string lugar)
        {
            InitializeComponent();
            labelUsuario.Text = "Usuario: " + usuario;
            labelFecha.Text = "Fecha: " + fecha;
            labelHora.Text = "Hora: " + hora;
            labelLugar.Text = "Lugar: " + lugar;

            label2.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;

            // Filtrar solo los números y eliminar caracteres no numéricos
            string numeroTelefonoCliente = new string(textBox2.Text.Where(char.IsDigit).ToArray());

            // Verificar si el número es válido
            if (string.IsNullOrEmpty(numeroTelefonoCliente))
            {
                MessageBox.Show("No ingresaste ningún número válido. Asegúrate de escribir solo números.");
                return;
            }

            if (numeroTelefonoCliente.Length < 10 || numeroTelefonoCliente.Length > 15)
            {
                MessageBox.Show("Por favor ingresa un número de teléfono válido (10-15 dígitos).");
                return;
            }

            // Generar el código de acceso
            Random random = new Random();
            codigoGenerado = random.Next(100000, 1000000); // Código de 6 dígitos
            tiempoGeneracion = DateTime.Now; // Guardar el momento en que se generó el código
            tiempoRestante = 120; // 2 minutos en segundos

            // Crear el mensaje para enviar
            string mensaje = $"Hola, tu código de acceso es: {codigoGenerado}";

            // Formatear la URL para abrir el chat con el número ingresado y enviar el mensaje
            string url = $"https://wa.me/{numeroTelefonoCliente}?text={Uri.EscapeDataString(mensaje)}";

            // Abrir la URL en el navegador para enviar el mensaje por WhatsApp Web
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

            // Limpiar el TextBox para que el usuario ingrese manualmente el código si es necesario
            textBox1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verificar si el usuario ingresó un código
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor ingresa el código de verificación.");
                return;
            }

            // Intentar convertir el texto a número entero
            if (!int.TryParse(textBox1.Text, out int codigoIngresado))
            {
                MessageBox.Show("El código ingresado no es válido. Asegúrate de escribir solo números.");
                return;
            }

            // Verificar si el código ingresado es igual al generado
            if (codigoIngresado == codigoGenerado)
            {
                // Guardar en la base de datos
                GuardarReserva();

                MessageBox.Show("Código verificado correctamente. ¡Reserva confirmada!");
            }
            else
            {
                MessageBox.Show("El código ingresado es incorrecto. Intenta nuevamente.");
            }
        }

        private void GuardarReserva()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    string query = "INSERT INTO Reservas (NombreUsuario, FechaReserva, HoraReserva, Lugar, Estatus) " +
                                   "VALUES (@NombreUsuario, @FechaReserva, @HoraReserva, @Lugar, 'Iniciada')";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@NombreUsuario", labelUsuario.Text.Replace("Usuario: ", "").Trim());
                        cmd.Parameters.AddWithValue("@FechaReserva", DateTime.Parse(labelFecha.Text.Replace("Fecha: ", "").Trim()));
                        cmd.Parameters.AddWithValue("@HoraReserva", TimeSpan.Parse(labelHora.Text.Replace("Hora: ", "").Trim()));
                        cmd.Parameters.AddWithValue("@Lugar", labelLugar.Text.Replace("Lugar: ", "").Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Reserva guardada exitosamente en la base de datos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en la base de datos: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();  
            this.Hide();
        }
    }
}