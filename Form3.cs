using System;
using System.Windows.Forms;

namespace Estacionamiento_V2._0
{
    public partial class Form3 : Form
    {
        private string codigoGenerado;
        private string fechaReserva;
        private string horaReserva;
        private string lugarReserva;

        public Form3(string usuario)
        {
            InitializeComponent();
            label1.Text = "Bienvenido: " + usuario;

            // Inicialización de visibilidad de los elementos
            labelEdad.Visible = false;
            textBoxEdad.Visible = false;
            Validar.Visible = false;
            labelCodigo.Visible = false;
            textBoxCodigo.Visible = false;
            btnValidarSolicitud.Visible = false;
            label3.Visible = false;
            labelFechaHora.Visible = false;
            dateTimePickerReserva.Visible = false;
            labelLugar.Visible = false;
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
            string curp = textBoxEdad.Text.Trim();

            if (curp.Length != 18)
            {
                MessageBox.Show("La CURP debe tener 18 caracteres.");
                return;
            }

            int edad = CalcularEdadDesdeCURP(curp);
            if (edad < 18)
            {
                MessageBox.Show("Debes tener al menos 18 años para solicitar la tarjeta.");
                return;
            }

            // Si es mayor de edad, mostrar el campo del código de confirmación
            MessageBox.Show("CURP validada. Se ha enviado un código de confirmación.");

            // Simulación de código enviado por WhatsApp
            Random rand = new Random();
            codigoGenerado = rand.Next(1000, 9999).ToString();
            MessageBox.Show($"Código enviado (simulado): {codigoGenerado}");

            labelCodigo.Visible = true;
            textBoxCodigo.Visible = true;
            btnValidarSolicitud.Visible = true;
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
            if (string.IsNullOrEmpty(textBoxCodigo.Text))
            {
                MessageBox.Show("Por favor, ingresa el código de confirmación.");
                return;
            }

            if (textBoxCodigo.Text == codigoGenerado)
            {
                MessageBox.Show("Trámite realizado con éxito.");
            }
            else
            {
                MessageBox.Show("Código de confirmación incorrecto.");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            labelEdad.Visible = true;
            textBoxEdad.Visible = true;
            Validar.Visible = true;
            labelCodigo.Visible = false;
            textBoxCodigo.Visible = false;
            btnValidarSolicitud.Visible = false;
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
        }

        private void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            if (comboBoxLugar.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un lugar de estacionamiento antes de confirmar.");
                return;
            }

            fechaReserva = dateTimePickerReserva.Value.ToString("yyyy-MM-dd");
            horaReserva = dateTimePickerReserva.Value.ToString("HH:mm");
            lugarReserva = comboBoxLugar.SelectedItem.ToString();

            MessageBox.Show($"Reserva guardada:\nFecha: {fechaReserva}\nHora: {horaReserva}\nLugar: {lugarReserva}");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fechaReserva) || string.IsNullOrEmpty(horaReserva) || string.IsNullOrEmpty(lugarReserva))
            {
                MessageBox.Show("Debes confirmar tu reserva antes de continuar.");
                return;
            }

            string usuario = label1.Text.Replace("Bienvenido: ", "").Trim();

            // Enviar datos correctos a Form4
            Form4 form4 = new Form4(usuario, fechaReserva, horaReserva, lugarReserva);
            form4.Show();
        }
    }
}

