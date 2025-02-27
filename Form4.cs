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
    public partial class Form4 : Form
    {
        public Form4(string usuario, string fecha, string hora, string lugar)
        {
            InitializeComponent();

            // Asignar valores a los labels
            labelUsuario.Text = "Usuario: " + usuario;
            labelFecha.Text = "Fecha: " + fecha;
            labelHora.Text = "Hora de entrada: " + hora;
            labelLugar.Text = "Lugar seleccionado: " + lugar;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labelUsuario_Click(object sender, EventArgs e)
        {

        }
    }
}
