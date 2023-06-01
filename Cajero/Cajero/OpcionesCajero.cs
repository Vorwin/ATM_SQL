using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cajero
{
    public partial class OpcionesCajero : Form
    {
        private string usuario;
        private int codigo;
        private string contraseña;
        private int retiroDiario;
        private DateTime ultimaFechaRetiro;
        Regex reg_Monto = new Regex(@"\d*");

        //Jalo los datos de usuario, codigo y contraseña del form1 para poder acceder a ese en la base de datos
        public OpcionesCajero(string usuario, int codigo, string contraseña)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.codigo = codigo;
            this.contraseña = contraseña;
            tabControl1.ItemSize = new Size(0, 1);
            this.retiroDiario = 0;
            this.ultimaFechaRetiro = DateTime.MinValue;
        }

        ClientesDatosDataContext datos = new ClientesDatosDataContext();

        private void Botones(object sender, EventArgs e)
        {
            Button aux = (Button)sender;

            switch (aux.Text)
            {
                case "Salir": this.Close(); Form1 principal = new Form1(); principal.Show(); break;
                case "Retiro":tabControl1.SelectTab(2);break;
                case "Deposito":tabControl1.SelectTab(1);break;
                case "Consulta":consulta();tabControl1.SelectTab(3);break;
                case "<<":/*tabControl1.SelectTab(0);*/tabControl1.SelectTab(4); break;
            }
        }

        private void consulta()
        {
            //Los convierto a minusculas para generalizarlos
            string usuarioLower = usuario.ToLower();
            string contraseñaLower = contraseña.ToLower();

            //comparo los datos de la base y del form1
            var cliente = datos.inforClientesATM.SingleOrDefault(x =>
                x.usuario.ToLower() == usuarioLower &&
                x.codigo == codigo &&
                x.contra.ToLower() == contraseñaLower);

            if (cliente != null)
            {
                //obtengo el monto actual, el long? ni perra idea pero solo long me tiraba error, me decia que metiera el "?" ahuevo
                long? monto = cliente.monto;
                txt_montoActual.Text = monto.ToString();
            }
            else{ MessageBox.Show("Error en la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            /*tabControl1.SelectTab(4);*/
        }

        private void Depositar(object sender, EventArgs e)
        {
            int nuevo_deposito = 0;

            if (reg_Monto.IsMatch(txt_monto.Text))
            {
                try
                {
                    nuevo_deposito = Convert.ToInt32(txt_monto.Text);
                    String usuarioLower = usuario.ToLower();
                    String contraseñaLower = contraseña.ToLower();
                    var cliente = datos.inforClientesATM.SingleOrDefault(x =>
                    x.usuario.ToLower() == usuarioLower &&
                    x.codigo == codigo &&
                    x.contra.ToLower() == contraseñaLower);

                    cliente.monto += nuevo_deposito;
                    datos.SubmitChanges();
                    txt_monto.Text = "";
                    MessageBox.Show("Deposito realizado", "Deposito Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch {MessageBox.Show("Error ingrese solo números", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else { MessageBox.Show("Error ingrese solo números", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            /*tabControl1.SelectTab(4);*/
        }

        private void Retiro(object sender, EventArgs e)
        {
            Button aux = (Button)sender;

            int cont = 0;
            String usuarioLower = usuario.ToLower();
            String contraseñaLower = contraseña.ToLower();

            try
            {
                var cliente = datos.inforClientesATM.SingleOrDefault(x =>
                    x.usuario.ToLower() == usuarioLower &&
                    x.codigo == codigo &&
                    x.contra.ToLower() == contraseñaLower);

                int montoRetiro = 0;

                switch (aux.Text)
                {
                    case "Q. 50": montoRetiro = 50; break;
                    case "Q. 100": montoRetiro = 100; break;
                    case "Q. 200": montoRetiro = 200; break;
                    case "Q. 400": montoRetiro = 400; break;
                    case "Q. 600": montoRetiro = 600; break;
                    case "Q. 1000": montoRetiro = 1000; break;
                }

                // Lee desde el csv las fechas para validar el limite diario
                string rutaArchivoConfig = "configuracion.csv";
                int retiroDiarioGuardado = 0;
                DateTime ultimaFechaRetiroGuardada = DateTime.MinValue;

                if (File.Exists(rutaArchivoConfig))
                {
                    string[] lineas = File.ReadAllLines(rutaArchivoConfig);
                    if (lineas.Length >= 2)
                    {
                        string[] datosUsuario = lineas[1].Split(',');
                        if (datosUsuario[0] == usuario)
                        {
                            int.TryParse(datosUsuario[1], out retiroDiarioGuardado);
                            DateTime.TryParse(datosUsuario[2], out ultimaFechaRetiroGuardada);
                        }
                    }
                }

                // Revisa si ya paso el dia para poder resetear el retiro diario
                if (DateTime.Now.Date > ultimaFechaRetiroGuardada.Date)
                {
                    retiroDiarioGuardado = 0;
                    ultimaFechaRetiroGuardada = DateTime.Now;
                }

                // Se verifica si se excedio el retiro diario
                string rutaArchivoRetirosDiarios = "retiros_diarios.csv";
                bool usuarioExcedeLimite = false;

                if (File.Exists(rutaArchivoRetirosDiarios))
                {
                    string[] retirosDiarios = File.ReadAllLines(rutaArchivoRetirosDiarios);

                    foreach (string retiro in retirosDiarios.Skip(1))
                    {
                        string[] datosRetiro = retiro.Split(',');

                        if (datosRetiro[0] == usuario && DateTime.Parse(datosRetiro[2]).Date == DateTime.Now.Date)
                        {
                            int retiroDiarioUsuario = 0;
                            int.TryParse(datosRetiro[4], out retiroDiarioUsuario);

                            if (retiroDiarioUsuario + montoRetiro > 3000)
                            {
                                usuarioExcedeLimite = true;
                                break;
                            }
                        }
                    }
                }

                if (cliente.monto >= montoRetiro && retiroDiarioGuardado + montoRetiro <= 3000 && !usuarioExcedeLimite)
                {
                    cont++;
                    cliente.monto -= montoRetiro;
                    retiroDiarioGuardado += montoRetiro;
                    datos.SubmitChanges();

                    // Se agrega al csv 
                    string fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
                    string horaActual = DateTime.Now.ToString("HH:mm:ss");
                    string registro = $"{usuario},{codigo},{fechaActual},{horaActual},{montoRetiro}";

                    if (!File.Exists(rutaArchivoRetirosDiarios))
                    {
                        File.WriteAllText(rutaArchivoRetirosDiarios, "Usuario,Codigo,Fecha,Hora,Monto\n");
                    }

                    File.AppendAllText(rutaArchivoRetirosDiarios, registro + "\n");

                    //actualiza valores de retiro
                    string[] lineasConfig = File.ReadAllLines(rutaArchivoConfig);
                    lineasConfig[1] = $"{usuario},{retiroDiarioGuardado},{ultimaFechaRetiroGuardada.ToString("yyyy-MM-dd")}";
                    File.WriteAllLines(rutaArchivoConfig, lineasConfig);

                    MessageBox.Show("Retiro realizado", "Retiro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (cliente.monto < montoRetiro) { MessageBox.Show("Fondos Insuficientes", "Error de fondos", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else if (usuarioExcedeLimite){MessageBox.Show("Has excedido el límite diario de retiro", "Error de fondos", MessageBoxButtons.OK, MessageBoxIcon.Error);}
                    else{ MessageBox.Show("Se ha excedido el límite diario de retiro", "Error de fondos", MessageBoxButtons.OK, MessageBoxIcon.Error);}
                }
            }
            catch { }
            /*tabControl1.SelectTab(4);*/
        }

        private void OpcionesCajero_Shown(object sender, EventArgs e)
        {

            string usuarioLower = usuario.ToLower();
            string contraseñaLower = contraseña.ToLower();
            string ruta = Directory.GetCurrentDirectory() + @"\img\";

            var cliente = datos.inforClientesATM.SingleOrDefault(x =>
                x.usuario.ToLower() == usuarioLower &&
                x.codigo == codigo &&
                x.contra.ToLower() == contraseñaLower);
        }

        private void MasTransacciones(object sender, EventArgs e)
        {
            Button aux = (Button)sender;
            switch (aux.Text)
            {
                case "Si, deseo hacer otra operacion":
                    tabControl1.SelectTab(0);
                    break;
                case "No, deseo salir":
                    Form1 principal = new Form1(); 
                    this.Close(); 
                    principal.Show();
                    break;
            }
        }
    }
}
