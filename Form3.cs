using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using Twilio.Types;
namespace Estacionamiento_V2._0
{
    public partial class Form3 : Form
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





        public Form3()
        {
            InitializeComponent();
            textBoxCodigo.Visible = false;
            labelCodigo.Visible = false;
            textBoxTelefono.Visible = false;
            lbltelefono.Visible = false;

        }
        private void InicializarComponentes()

        {
            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick_1;


            textBoxCodigo.Visible = true;
            btnValidarSolicitud.Visible = false;
            lbltelefono.Visible = true;
            textBoxTelefono.Visible = true;
           


        }
        private string nombreUsuarioActual;

        public Form3(string usuario)
        {
            InitializeComponent();
            InicializarComponentes();
            label1.Text = "Bienvenido: " + usuario;

            labelEdad.Visible = false;
            textBoxEdad.Visible = false;
            Validar.Visible = false;
            labelCodigo.Visible = true;
            textBoxCodigo.Visible = true;
            btnValidarSolicitud.Visible = false;

            labelFechaHora.Visible = true;
            dateTimePickerReserva.Visible = false;
            labelLugar.Visible = true;
            comboBoxLugar.Visible = false;
            btnEnviarCodigo.Visible = false;
            listView1.Visible = false;

            comboBoxLugar.Items.AddRange(new string[]
            {
                "Lugar 1", "Lugar 2", "Lugar 3", "Lugar 4", "Lugar 5",
                "Lugar 6", "Lugar 7", "Lugar 8", "Lugar 9", "Lugar 10"
            });
        }



        private void btnValidarCURP_Click(object sender, EventArgs e)
        {
            string numeroTelefonoCliente = textBoxTelefono.Text.Trim();

            if (string.IsNullOrEmpty(numeroTelefonoCliente) || numeroTelefonoCliente.Length < 10)
            {
                MessageBox.Show("Por favor ingresa un número de teléfono válido.");
                return;
            }

            Random random = new Random();
            codigoGenerado = random.Next(100000, 1000000);
            tiempoGeneracion = DateTime.Now;

            //textBoxCodigo.Text = codigoGenerado.ToString();

            tiempoRestante = 120;
            lbltime.Text = "Tiempo restante : 2:00";
            timer1.Start();

            EnviarCodigoPorSMS(numeroTelefonoCliente, codigoGenerado);

            
            string nombreUsuarioActual = label1.Text.Replace("Bienvenido: ", "").Trim();

            
            using (SqlConnection conexion = new SqlConnection("Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conexion.Open();
                    string insertQuery = "INSERT INTO Solicitudes (NombreUsuario, FechaSolicitud) VALUES (@NombreUsuario, @FechaSolicitud)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conexion))
                    {
                        
                        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuarioActual);
                        cmd.Parameters.AddWithValue("@FechaSolicitud", DateTime.Now);

                        
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Solicitud registrada correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar la solicitud: " + ex.Message);
                }
            }
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

        private int CalcularEdadDesdeCURP(string curp)
        {

            string fechaNacimiento = curp.Substring(4, 6);
            int año = Convert.ToInt32(fechaNacimiento.Substring(0, 2));
            int mes = Convert.ToInt32(fechaNacimiento.Substring(2, 2));
            int dia = Convert.ToInt32(fechaNacimiento.Substring(4, 2));


            año += (año >= 30) ? 1900 : 2000;

            DateTime fechaNac = new DateTime(año, mes, dia);
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNac.Year;

            if (hoy < fechaNac.AddYears(edad))
            {
                edad--;
            }

            return edad;
        }

        private void btnValidarSolicitud_Click(object sender, EventArgs e)
        {

            if (DateTime.Now.Subtract(tiempoGeneracion).TotalSeconds > 120)
            {
                MessageBox.Show("El código ha expirado. Se generará uno nuevo.");
                GenerarNuevoCodigo();
                return;
            }


            if (int.TryParse(textBoxCodigo.Text, out int codigoIngresado) && codigoIngresado == codigoGenerado)
            {
                MessageBox.Show("¡Código confirmado exitosamente!");
            }
            else
            {
                MessageBox.Show("Código incorrecto. Intenta nuevamente.");
            }
        }
        private void GenerarNuevoCodigo()
        {
            Random random = new Random();
            codigoGenerado = random.Next(100000, 1000000);
            tiempoGeneracion = DateTime.Now;
            //textBoxCodigo.Text = codigoGenerado.ToString();
            tiempoRestante = 120;
            lbltime.Text = "Tiempo restante: 2:00";
            timer1.Start();

            string numeroTelefonoCliente = textBoxTelefono.Text.Trim();
            string mensaje = $"Hola, tu nuevo código de acceso es: {codigoGenerado}";
            string url = $"https://wa.me/{numeroTelefonoCliente}?text={Uri.EscapeDataString(mensaje)}";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            labelEdad.Visible = true;
            textBoxEdad.Visible = true;
            Validar.Visible = true;
            labelCodigo.Visible = true;
            textBoxCodigo.Visible = true;
            btnValidarSolicitud.Visible = true;
            textBoxTelefono.Visible = true;
            lbltelefono.Visible = true;
            labelFechaHora.Visible = false;
            dateTimePickerReserva.Visible = false;
            labelLugar.Visible = false;
            comboBoxLugar.Visible = false;
            btnEnviarCodigo.Visible = false;
            listView1.Visible = false;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            labelFechaHora.Visible = true;
            dateTimePickerReserva.Visible = true;
            labelLugar.Visible = true;
            comboBoxLugar.Visible = true;
            btnEnviarCodigo.Visible = true;
            labelEdad.Visible = false;
            textBoxEdad.Visible = false;
            Validar.Visible = false;
            labelCodigo.Visible = false;
            textBoxCodigo.Visible = false;
            btnValidarSolicitud.Visible = false;
            textBoxTelefono.Visible = false;
            lbltelefono.Visible = false;
            listView1.Visible = false;
        }

        private void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            fechaReserva = dateTimePickerReserva.Value.ToString("yyyy-MM-dd");
            horaReserva = dateTimePickerReserva.Value.ToString("HH:mm");
            lugarReserva = comboBoxLugar.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(lugarReserva))
            {
                MessageBox.Show("Por favor, selecciona un lugar de estacionamiento.");
                return;
            }

            MessageBox.Show($"Reserva guardada:\nFecha: {fechaReserva}\nHora: {horaReserva}\nLugar: {lugarReserva}");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fechaReserva) || string.IsNullOrEmpty(horaReserva) || string.IsNullOrEmpty(lugarReserva))
            {
                MessageBox.Show("Debes tener antes una reserva ya ralizada.");
                return;
            }

            string usuario = label1.Text.Replace("Bienvenido: ", "").Trim();
            Form4 form4 = new Form4(usuario, fechaReserva, horaReserva, lugarReserva);
            form4.Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante--;
                int minutos = tiempoRestante / 60;
                int segundos = tiempoRestante % 60;
                lbltime.Text = $"Tiempo restante: {minutos:D2}:{segundos:D2}";
            }
            else
            {
                textBoxCodigo.Clear();
                lbltime.Text = "Código expirado";
                timer1.Stop();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBoxCodigo.Visible = false;
            labelCodigo.Visible = false;
            textBoxTelefono.Visible = false;
            lbltelefono.Visible = false;
            dateTimePickerReserva.Visible = false;
            comboBoxLugar.Visible = false;
            btnEnviarCodigo.Visible = false;
            labelFechaHora.Visible = false;
            labelLugar.Visible = false;
            listView1.Visible = false;

            dateTimePickerReserva.MinDate = DateTime.Today; 
            dateTimePickerReserva.MaxDate = DateTime.Today.AddMonths(3);

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            textBoxCodigo.Visible = false;
            labelCodigo.Visible = false;
            textBoxTelefono.Visible = false;
            lbltelefono.Visible = false;
            dateTimePickerReserva.Visible = false;
            comboBoxLugar.Visible = false;
            btnEnviarCodigo.Visible = false;
            labelFechaHora.Visible = false;
            labelLugar.Visible = false;
            listView1.Visible = false;
            dateTimePickerReserva.MinDate = DateTime.Today; 
            dateTimePickerReserva.MaxDate = DateTime.Today.AddMonths(3);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ConfigurarListView1()
        {
            listView1.View = View.Details;
            listView1.Columns.Clear();
            listView1.Columns.Add("ID_Reservas", 80);
            listView1.Columns.Add("NombreUsuario", 120);
            listView1.Columns.Add("FechaReserva", 120);
            listView1.Columns.Add("Lugar", 100);
            listView1.Columns.Add("Estatus", 100);
            listView1.Columns.Add("Numero_Tarjetas", 120);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            listView1.Visible = true;
            ConfigurarListView1(); 

            listView1.Items.Clear(); 

            string usuario = label1.Text.Replace("Bienvenido: ", "").Trim();

            using (SqlConnection conexion = new SqlConnection("Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True"))
            {
                string query = "SELECT ID_Reservas, NombreUsuario, FechaReserva, Lugar, Estatus, Numero_Tarjetas FROM Reservas WHERE NombreUsuario = @usuario";

                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["ID_Reservas"].ToString());
                                item.SubItems.Add(reader["NombreUsuario"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["FechaReserva"]).ToString("yyyy-MM-dd HH:mm"));
                                item.SubItems.Add(reader["Lugar"].ToString());
                                item.SubItems.Add(reader["Estatus"].ToString());
                                item.SubItems.Add(reader["Numero_Tarjetas"].ToString());

                                listView1.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
           
        }

       
    }
    
}