using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Estacionamiento_V2._0
{
    public partial class Form4 : Form
    {
        private Timer timer;
        private int tiempoRestante = 120;  
        private int codigoGenerado; 
        
        private DateTime tiempoGeneracion;
        private string fechaReserva;
        private string horaReserva;
        private string lugarReserva;
        private string accountSid = "AC522e92d95e9904f9cfc973922529735c";
        private string twilioAuthToken = "b335e0e850816e64046f7d8657cb555a";
        private string twilioNumber = "+19472186624";


        
        private string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";

        public Form4(string usuario, string fecha, string hora, string lugar)
        {
            InitializeComponent();
            labelUsuario.Text = "Usuario: " + usuario;
            labelFecha.Text = "Fecha: " + fecha;
            labelHora.Text = "Hora: " + hora;
            labelLugar.Text = "Lugar: " + lugar;
            timer1 = new Timer();
            timer1.Interval = 1000; 
            timer1.Tick += timer1_Tick;
            label2.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;

            string numeroTelefonoCliente = textBox2.Text.Trim(); 

            if (string.IsNullOrEmpty(numeroTelefonoCliente) || numeroTelefonoCliente.Length < 10)
            {
                MessageBox.Show("Por favor ingresa un número de teléfono válido.");
                return;
            }

            
            Random random = new Random();
            codigoGenerado = random.Next(100000, 1000000); 
            tiempoGeneracion = DateTime.Now; 

            
            textBox1.Text = codigoGenerado.ToString();

            
            tiempoRestante = 120; 
            label4.Text = "Tiempo restante : 2:00";
            timer1.Start(); 

           
            EnviarCodigoPorSMS(numeroTelefonoCliente, codigoGenerado);
        }

        private void EnviarCodigoPorSMS(string numeroDestino, int codigo)
        {
            try
            {
                
                TwilioClient.Init(accountSid, twilioAuthToken);

                
                var mensaje = MessageResource.Create(
                    from: new PhoneNumber(twilioNumber), 
                    to: new PhoneNumber("+52" + numeroDestino), 
                    body: $"Hola, tu código de acceso es: {codigo}"
                );

                MessageBox.Show("Mensaje enviado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }
        private void GenerarNuevoCodigo()
        {
            Random random = new Random();
            codigoGenerado = random.Next(100000, 1000000); 
            tiempoGeneracion = DateTime.Now;
            textBox1.Text = codigoGenerado.ToString();
            tiempoRestante = 120; 
            label4.Text = "Tiempo restante: 2:00";
            timer1.Start();

            string numeroTelefonoCliente = textBox1.Text.Trim();
            string mensaje = $"Hola, tu nuevo código de acceso es: {codigoGenerado}";
            string url = $"https://wa.me/{numeroTelefonoCliente}?text={Uri.EscapeDataString(mensaje)}";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor ingresa el código de verificación.");
                return;
            }

            
            if (!int.TryParse(textBox1.Text, out int codigoIngresado))
            {
                MessageBox.Show("El código ingresado no es válido. Asegúrate de escribir solo números.");
                return;
            }

            
            if (codigoIngresado == codigoGenerado)
            {
                
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

                    string nombreUsuario = labelUsuario.Text.Replace("Usuario: ", "").Trim();

                    // Obtener ID del usuario
                    string consultaUsuario = "SELECT ID_Registro FROM [Registro_Usuario] WHERE TRIM(Nombre_de_usuario) = @NombreUsuario";
                    int idUsuario = -1;

                    using (SqlCommand cmdUsuario = new SqlCommand(consultaUsuario, conexion))
                    {
                        cmdUsuario.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        object resultadoUsuario = cmdUsuario.ExecuteScalar();

                        if (resultadoUsuario != null)
                        {
                            idUsuario = Convert.ToInt32(resultadoUsuario);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un usuario registrado con este nombre.");
                            return;
                        }
                    }

                    // Verificar si el usuario ya tiene una reserva activa (Iniciada o En progreso)
                    string consultaReservasActivas = "SELECT COUNT(*) FROM Reservas WHERE FK_Registro = @FK_Registro AND Estatus IN ('Iniciada', 'En progreso')";
                    using (SqlCommand cmdVerificar = new SqlCommand(consultaReservasActivas, conexion))
                    {
                        cmdVerificar.Parameters.AddWithValue("@FK_Registro", idUsuario);
                        int reservasActivas = (int)cmdVerificar.ExecuteScalar();

                        if (reservasActivas > 0)
                        {
                            MessageBox.Show("Ya tienes una reserva activa. No puedes hacer otra hasta que se finalice la actual.");
                            return;
                        }
                    }

                    // Obtener ID de la tarjeta y el Número de tarjeta
                    string consultaTarjeta = "SELECT ID_Tarjetas, Numero_Tarjeta FROM Tarjetas WHERE FK_Registro = @FK_Registro";
                    int idTarjeta = -1;
                    string numeroTarjeta = "";

                    using (SqlCommand cmdTarjeta = new SqlCommand(consultaTarjeta, conexion))
                    {
                        cmdTarjeta.Parameters.AddWithValue("@FK_Registro", idUsuario);
                        using (SqlDataReader reader = cmdTarjeta.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                idTarjeta = Convert.ToInt32(reader["ID_Tarjetas"]);
                                numeroTarjeta = reader["Numero_Tarjeta"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró una tarjeta asociada con este usuario.");
                                return;
                            }
                        }
                    }

                    // Insertar la reserva
                    string query = "INSERT INTO Reservas (FK_Registro, FK_Tarjetas, NombreUsuario, FechaReserva, HoraReserva, Lugar, Estatus, Numero_Tarjetas) " +
                                   "VALUES (@FK_Registro, @FK_Tarjetas, @NombreUsuario, @FechaReserva, @HoraReserva, @Lugar, 'Iniciada', @Numero_Tarjetas)";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@FK_Registro", idUsuario);
                        cmd.Parameters.AddWithValue("@FK_Tarjetas", idTarjeta);
                        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        cmd.Parameters.AddWithValue("@FechaReserva", DateTime.Parse(labelFecha.Text.Replace("Fecha: ", "").Trim()));
                        cmd.Parameters.AddWithValue("@HoraReserva", TimeSpan.Parse(labelHora.Text.Replace("Hora: ", "").Trim()));
                        cmd.Parameters.AddWithValue("@Lugar", labelLugar.Text.Replace("Lugar: ", "").Trim());
                        cmd.Parameters.AddWithValue("@Numero_Tarjetas", numeroTarjeta);

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante--;
                int minutos = tiempoRestante / 60;
                int segundos = tiempoRestante % 60;
                label4.Text = $"Tiempo restante: {minutos:D2}:{segundos:D2}";
            }
            else
            {
                textBox1.Clear();
                label4.Text = "Código expirado";
                timer1.Stop();
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}