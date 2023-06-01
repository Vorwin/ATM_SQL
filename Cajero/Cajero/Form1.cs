using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cajero
{

    public partial class Form1 : Form
    {
        private int intentosFallidos = 0;
        private bool cuentaBloqueada = false;

        public Form1()
        {
            InitializeComponent();
            txt_codigo.Text = "";
            txt_contraseña.Text = "";
            txt_usuario.Text = "";
        }

        //Crear el data context para guardar los datos luego
        ClientesDatosDataContext datos = new ClientesDatosDataContext();
        String rutaPDf = Directory.GetCurrentDirectory() + @"\Proyecto (ATM).pdf";
        Regex code = new Regex(@"\A\d*\Z");

        private void BotonesEventos(object sender, EventArgs e)
        {
            Button aux = (Button)sender;

            switch (aux.Text)
            {
                case "X":
                    this.Hide();
                    Application.Exit();
                    break;
                case "Sign in":
                    this.Hide();
                    CuentaNueva FormCuentaNueva = new CuentaNueva();
                    FormCuentaNueva.ShowDialog();
                    break;
                case "Login":
                    Login();
                    break;
            }
        }

        // Función para verificar los datos en la BD y pasar a la siguiente pantalla
        private void Login()
        {
            // Validar campos vacíos
            if (string.IsNullOrEmpty(txt_usuario.Text) || string.IsNullOrEmpty(txt_contraseña.Text) || string.IsNullOrEmpty(txt_codigo.Text))
            {
                MessageBox.Show("Por favor, llene todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Obtener los valores de los campos
            String usuario = txt_usuario.Text;
            String contraseña = txt_contraseña.Text;

            int codigo = 0; 
            if (code.IsMatch(txt_codigo.Text)){codigo = int.Parse(txt_codigo.Text);}
            else { MessageBox.Show("Por favor, solo ingrese números en el código", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            // Obtener la cuenta del cliente por el usuario
            var cliente = datos.inforClientesATM.SingleOrDefault(x => x.usuario == usuario && x.codigo == codigo);

            // Validar la existencia del cliente
            if (cliente == null)
            {
                MessageBox.Show($"Error, El usuario {usuario} con codigo {codigo} no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_usuario.Text = "";
                txt_codigo.Text = "";
                txt_contraseña.Text = "";
                return;
            }

            // Validar si la cuenta está bloqueada
            if (!cliente.cuentaActiva)
            {
                MessageBox.Show($"La cuenta del usuario '{usuario}' está bloqueada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar la contraseña
            if (!string.Equals(cliente.contra, contraseña, StringComparison.OrdinalIgnoreCase))
            {
                intentosFallidos++;
                if (intentosFallidos >= 3)
                {
                    BloquearCuenta(cliente);
                    return;
                }
                MessageBox.Show("La contraseña es incorrecta, tiene " + (3 - intentosFallidos) + " intentos más", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_contraseña.Text = "";
                return;
            }

            // Si todas las validaciones pasan, se muestra el formulario deseado
            this.Hide();
            OpcionesCajero FormLogin = new OpcionesCajero(usuario, codigo, contraseña);
            FormLogin.ShowDialog();
        }

        private void BloquearCuenta(inforClientesATM cliente)
        {
            cuentaBloqueada = true;
            try
            {
                cliente.cuentaActiva = false; // Establecer cuenta inactiva
                datos.SubmitChanges();
                txt_codigo.Text = "";
                txt_contraseña.Text = "";
                txt_usuario.Text = "";
                MessageBox.Show("Se ha bloqueado su cuenta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Algo salió mal", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_información_Click(object sender, EventArgs e){MessageBox.Show("©-Copyright by: LosttSky & Vorwin\n\tVersion: 1.00", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);}

        private void descargarPdf(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Quieres abrir el informe de la aplicación?", "Abriendo...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes){Process.Start(rutaPDf);}
        }

        private void mostrarMensaje(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenido a nuestro proyecto! :)", "Hola uwu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mensaje mensajeForm = new mensaje();
            mensajeForm.Show();
            mensajeForm.ShowDialog();
        }
    }
}
