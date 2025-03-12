using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace Estacionamiento_V2._0
{
    public partial class Form3 : Form
    {


        private Timer timer;
        private int tiempoRestante = 120;  // 2 minutos en segundos
        private int codigoGenerado; // Código generado
        private DateTime tiempoGeneracion;
        private string fechaReserva;
        private string horaReserva;
        private string lugarReserva;
      
        private string numeroAdmi = "529831682479"; // Número del administrador (reemplaza con el número real)




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
            timer1.Interval = 1000; // 1 segundo
         

            textBoxCodigo.Visible = true;
            btnValidarSolicitud.Visible = false;
            lbltelefono.Visible = true;
            textBoxTelefono.Visible = true;

          
        }
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

            comboBoxLugar.Items.AddRange(new string[]
            {
                "Lugar 1", "Lugar 2", "Lugar 3", "Lugar 4", "Lugar 5",
                "Lugar 6", "Lugar 7", "Lugar 8", "Lugar 9", "Lugar 10"
            });
        }

      

        private void btnValidarCURP_Click(object sender, EventArgs e)
        {

            string numeroTelefonoCliente = textBoxTelefono.Text.Trim(); // Número del cliente

            if (string.IsNullOrEmpty(numeroTelefonoCliente) || numeroTelefonoCliente.Length < 10)
            {
                MessageBox.Show("Por favor ingresa un número de teléfono válido.");
                return;
            }
    
            // Generar el código de acceso
            Random random = new Random();
            codigoGenerado = random.Next(100000, 1000000); // Código de 6 dígitos
            tiempoGeneracion = DateTime.Now; // Guardar el momento en que se generó el código

            // Mostrar el código en el TextBox (aunque no es necesario aquí si solo se usa para WhatsApp)
            textBoxCodigo.Text = codigoGenerado.ToString();

            // Mostrar el tiempo restante (2 minutos)
            tiempoRestante = 120; // 2 minutos en segundos
            lbltime.Text = "Tiempo restante : 2:00";
            timer1.Start(); // Iniciar el temporizador para la cuenta regresiva

            string numeroAdmi = "529831682479"; // Debe estar en formato internacional (con código de país)

            // Crear el mensaje para enviar
            string mensaje = $"Hola , tu código de acceso es: {codigoGenerado}";

            // Formatear la URL para abrir el chat con el número administrador y enviar el mensaje
            string url = $"https://wa.me/{numeroTelefonoCliente}?text={Uri.EscapeDataString(mensaje)}";

            // Abrir la URL en el navegador para enviar el mensaje por WhatsApp Web
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

            // Limpiar el TextBox para que el usuario ingrese manualmente el código si es necesario
            textBoxCodigo.Clear();
        }
       


      

        private int CalcularEdadDesdeCURP(string curp)
        {
            // Extraer fecha de nacimiento de la CURP (posición 4 a 9: AAMMDD)
            string fechaNacimiento = curp.Substring(4, 6);
            int año = Convert.ToInt32(fechaNacimiento.Substring(0, 2));
            int mes = Convert.ToInt32(fechaNacimiento.Substring(2, 2));
            int dia = Convert.ToInt32(fechaNacimiento.Substring(4, 2));

            // Ajustar el año para considerar siglos
            año += (año >= 30) ? 1900 : 2000;

            DateTime fechaNac = new DateTime(año, mes, dia);
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNac.Year;

            if (hoy < fechaNac.AddYears(edad))
            {
                edad--; // Ajustar si aún no ha cumplido años este año
            }

            return edad;
        }

        private void btnValidarSolicitud_Click(object sender, EventArgs e)
        {
            // Verificar si el código ya expiró
            if (DateTime.Now.Subtract(tiempoGeneracion).TotalSeconds > 120)
            {
                MessageBox.Show("El código ha expirado. Se generará uno nuevo.");
                GenerarNuevoCodigo();
                return;
            }

            // Validar el código ingresado
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
            codigoGenerado = random.Next(100000, 1000000); // Nuevo código de 6 dígitos
            tiempoGeneracion = DateTime.Now; // Registrar el nuevo tiempo de generación
            textBoxCodigo.Text = codigoGenerado.ToString();
            tiempoRestante = 120; // Reiniciar el temporizador a 2 minutos
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
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();  // Muestra Form1
            this.Close();
        }
    }

}
