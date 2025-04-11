using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estacionamiento_V2._0
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private string connectionString = "Data Source=NEWTONDREAM\\SQLEXPRESS;Initial Catalog=Estacionamiento;Integrated Security=True;TrustServerCertificate=True";

        private void button1_Click(object sender, EventArgs e)
        {
            string numeroTarjeta = textBox1.Text;
            DateTime fechaActual = DateTime.Today;

           
            string query = @"SELECT * FROM Reservas 
                     WHERE Numero_Tarjetas = @NumeroTarjeta 
                     AND CONVERT(date, FechaReserva) = @FechaActual";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
                cmd.Parameters.AddWithValue("@FechaActual", fechaActual);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        string estatus = reader["Estatus"].ToString().Trim().ToLower();

                        if (estatus == "finalizada")
                        {
                            MessageBox.Show("Esta reserva ya ha sido finalizada. No tiene una reserva activa.");
                            reader.Close();
                            return;
                        }
                        else if (estatus == "iniciada")
                        {
                            
                            int fkRegistro = reader["FK_Registro"] != DBNull.Value ? Convert.ToInt32(reader["FK_Registro"]) : 0;
                            int fkTarjetas = reader["FK_Tarjetas"] != DBNull.Value ? Convert.ToInt32(reader["FK_Tarjetas"]) : 0;
                            int fkReservas = reader["ID_Reservas"] != DBNull.Value ? Convert.ToInt32(reader["ID_Reservas"]) : 0;

                            TimeSpan horaReserva = reader["HoraReserva"] != DBNull.Value ? (TimeSpan)reader["HoraReserva"] : TimeSpan.Zero;
                            DateTime horaEntrada = fechaActual.Add(horaReserva); 

                            reader.Close();

                            
                            string insertQuery = "INSERT INTO RegistrodeEstacionamiento " +
                                "(FK_Registro, FK_Tarjetas, FK_Reservas, HoraEntrada, HoraSalida, Pago) " +
                                "VALUES (@FK_Registro, @FK_Tarjetas, @FK_Reservas, @HoraEntrada, NULL, 0)";

                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@FK_Registro", fkRegistro);
                            insertCmd.Parameters.AddWithValue("@FK_Tarjetas", fkTarjetas);
                            insertCmd.Parameters.AddWithValue("@FK_Reservas", fkReservas);
                            insertCmd.Parameters.AddWithValue("@HoraEntrada", horaEntrada);

                            insertCmd.ExecuteNonQuery();
                            MessageBox.Show("Acceso permitido. Registro de entrada guardado.");
                        }
                        else
                        {
                            MessageBox.Show("La reserva no está activa. Estatus: " + estatus);
                            reader.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró una reserva para este número de tarjeta en la fecha de hoy.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string numeroTarjeta = textBox2.Text;
            DateTime fechaHoy = DateTime.Today;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    
                    string queryReserva = @"SELECT * FROM Reservas 
                                    WHERE Numero_Tarjetas = @NumeroTarjeta 
                                    AND Estatus = 'Iniciada' 
                                    AND CONVERT(date, FechaReserva) = @FechaHoy";

                    SqlCommand cmdReserva = new SqlCommand(queryReserva, conn);
                    cmdReserva.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
                    cmdReserva.Parameters.AddWithValue("@FechaHoy", fechaHoy);

                    SqlDataReader readerReserva = cmdReserva.ExecuteReader();

                    if (!readerReserva.HasRows)
                    {
                        MessageBox.Show("No se encontró una reserva activa para este número de tarjeta en la fecha de hoy.");
                        return;
                    }

                    readerReserva.Read();

                    int fkReserva = Convert.ToInt32(readerReserva["ID_Reservas"]);
                    int fkTarjetas = Convert.ToInt32(readerReserva["FK_Tarjetas"]);
                    readerReserva.Close();

                    
                    string queryRegistro = @"SELECT * FROM RegistrodeEstacionamiento 
                                     WHERE FK_Reservas = @FK_Reservas 
                                     AND FK_Tarjetas = @FK_Tarjetas";

                    SqlCommand cmdRegistro = new SqlCommand(queryRegistro, conn);
                    cmdRegistro.Parameters.AddWithValue("@FK_Reservas", fkReserva);
                    cmdRegistro.Parameters.AddWithValue("@FK_Tarjetas", fkTarjetas);

                    SqlDataReader readerRegistro = cmdRegistro.ExecuteReader();

                    if (!readerRegistro.HasRows)
                    {
                        MessageBox.Show("Esta reserva aún no ha ingresado al estacionamiento. No se puede realizar el cobro.");
                        return;
                    }

                    readerRegistro.Read();

                    DateTime horaEntrada = Convert.ToDateTime(readerRegistro["HoraEntrada"]);
                    readerRegistro.Close();

                    
                    string queryCredito = "SELECT Credito FROM Tarjetas WHERE Numero_Tarjeta = @NumeroTarjeta";
                    SqlCommand cmdCredito = new SqlCommand(queryCredito, conn);
                    cmdCredito.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);

                    object result = cmdCredito.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("No se encontró la tarjeta en la base de datos.");
                        return;
                    }

                    decimal creditoDisponible = Convert.ToDecimal(result);

                    
                    DateTime horaSalida = DateTime.Now;
                    TimeSpan diferencia = horaSalida - horaEntrada;
                    int horasCobradas = (int)Math.Ceiling(diferencia.TotalHours);
                    decimal pago = horasCobradas * 30;

                    if (creditoDisponible < pago)
                    {
                        MessageBox.Show($"Crédito insuficiente.\nTotal a pagar: {pago} créditos.\nCrédito disponible: {creditoDisponible} créditos.");
                        return;
                    }

                   
                    string updateRegistro = "UPDATE RegistrodeEstacionamiento " +
                        "SET HoraSalida = @HoraSalida, Pago = @Pago " +
                        "WHERE FK_Reservas = @FK_Reservas AND FK_Tarjetas = @FK_Tarjetas";

                    SqlCommand cmdUpdateRegistro = new SqlCommand(updateRegistro, conn);
                    cmdUpdateRegistro.Parameters.AddWithValue("@HoraSalida", horaSalida);
                    cmdUpdateRegistro.Parameters.AddWithValue("@Pago", pago);
                    cmdUpdateRegistro.Parameters.AddWithValue("@FK_Reservas", fkReserva);
                    cmdUpdateRegistro.Parameters.AddWithValue("@FK_Tarjetas", fkTarjetas);
                    cmdUpdateRegistro.ExecuteNonQuery();

                    
                    decimal nuevoCredito = creditoDisponible - pago;
                    string updateTarjeta = "UPDATE Tarjetas SET Credito = @NuevoCredito WHERE Numero_Tarjeta = @NumeroTarjeta";

                    SqlCommand cmdUpdateTarjeta = new SqlCommand(updateTarjeta, conn);
                    cmdUpdateTarjeta.Parameters.AddWithValue("@NuevoCredito", nuevoCredito);
                    cmdUpdateTarjeta.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
                    cmdUpdateTarjeta.ExecuteNonQuery();

                    
                    string updateEstatus = "UPDATE Reservas SET Estatus = 'Finalizada' WHERE ID_Reservas = @ID_Reservas";
                    SqlCommand cmdUpdateEstatus = new SqlCommand(updateEstatus, conn);
                    cmdUpdateEstatus.Parameters.AddWithValue("@ID_Reservas", fkReserva);
                    cmdUpdateEstatus.ExecuteNonQuery();

                    MessageBox.Show($"Salida registrada. Se descontaron {pago} créditos.\nNuevo saldo: {nuevoCredito}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            
        }
    }
}
