using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estacionamiento_V2._0
{
    public partial class Form3 : Form
    {
        private string codigoReservaGenerado;
        public Form3(string usuario)
        {
            InitializeComponent();
            label1.Text = "Bienvenido: " + usuario;


            labelEdad.Visible = false;
            textBoxEdad.Visible = false;
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

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnValidarSolicitud_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxEdad.Text) || string.IsNullOrEmpty(textBoxCodigo.Text))
            {
                MessageBox.Show("Por favor, llena todos los campos.");
                return;
            }

            int edad;
            if (!int.TryParse(textBoxEdad.Text, out edad) || edad < 18)
            {
                MessageBox.Show("Debes tener al menos 18 años para solicitar la tarjeta.");
                return;
            }
            //WHATSAPP EMPRESA "PENDEINTE NUEMERO DEL ADMINISTRADOR"
            if (textBoxCodigo.Text == "1234") 
            {
                MessageBox.Show("Solicitud validada con éxito. Tarjeta aprobada.");
            }
            else
            {
                MessageBox.Show("Código de confirmación incorrecto.");
            }
        }

        private void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            if (comboBoxLugar.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un lugar de estacionamiento.");
                return;
            }

            string fechaHora = dateTimePickerReserva.Value.ToString("yyyy-MM-dd HH:mm");
            string lugarSeleccionado = comboBoxLugar.SelectedItem.ToString();

            
            Random rand = new Random();
            codigoReservaGenerado = rand.Next(1000, 9999).ToString();

            MessageBox.Show($"Código de confirmación: {codigoReservaGenerado}\nFecha y Hora: {fechaHora}\nLugar: {lugarSeleccionado}");

            //Configurar para enviar el ci¿ódigo a whatsapp
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            labelEdad.Visible = true;
            textBoxEdad.Visible = true;
            labelCodigo.Visible = true;
            textBoxCodigo.Visible = true;
            btnValidarSolicitud.Visible = true;
            label3.Visible = true;
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
                labelCodigo.Visible = false;
                textBoxCodigo.Visible = false;
                btnValidarSolicitud.Visible = false;
            
        }
    }

}
