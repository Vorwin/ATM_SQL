using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cajero
{
    public partial class CuentaNueva : Form
    {

        //Crear el data context para guardar los datos luego
        ClientesDatosDataContext datos = new ClientesDatosDataContext();

        //Variables que guardan la info de los txt
        String nombre, apellido, usuario, correo, contra, fecha, banco;
        int numero, monto, codigo;
        long dpi;
        bool bandera = false;

        int cont = 0;
        Random r = new Random();

        //Regex para verificar los txt
        Regex NombresYApellidos = new Regex(@"\A[A-z]+\Z");
        Regex DPI = new Regex(@"\A(\d{10}|\d{15})\Z");
        Regex Fecha = new Regex(@"\A(\d{2})/(01|02|03|04|05|06|07|08|09|10|11|12)/(19\d{2}|20\d{2})\Z");
        Regex Correo = new Regex(@"\A[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.(com|edu|org|net)\Z");
        Regex Numero = new Regex(@"\A[0-9]{4}(\s{1})?[0-9]{4}\Z");

        public CuentaNueva()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txt_nombreCuentaNueva.Text) || string.IsNullOrWhiteSpace(txt_ApellidoCuentaNueva.Text) ||
               string.IsNullOrWhiteSpace(txt_FechaCuentaNueva.Text) || string.IsNullOrWhiteSpace(txt_DpiCuentaNueva.Text) ||
               string.IsNullOrWhiteSpace(txt_CorreoCuentaNueva.Text) || string.IsNullOrWhiteSpace(txt_NumeroTelCuentaNueva.Text))
            {
                MessageBox.Show("Debe ingresar todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!NombresYApellidos.IsMatch(txt_nombreCuentaNueva.Text))
            {
                MessageBox.Show("Ingresa un nombre válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cont++;
                nombre = txt_nombreCuentaNueva.Text;
            }

            if (!NombresYApellidos.IsMatch(txt_ApellidoCuentaNueva.Text))
            {
                MessageBox.Show("Ingresa un apellido válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cont++;
                apellido = txt_ApellidoCuentaNueva.Text;
            }

            int year = 0;
            try
            {
                String substring = txt_FechaCuentaNueva.Text.Substring(6);
                year = Convert.ToInt32(substring);

            }
            catch { MessageBox.Show("Ingrese la fecha de nacimiento como dd/mm/aaaa ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }


            if (!(Fecha.IsMatch(txt_FechaCuentaNueva.Text) && year <= 2005))
            {
                MessageBox.Show("Ingresa la fecha con el formato correspondiente y debe ser mayor de edad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cont++;
                fecha = txt_FechaCuentaNueva.Text;
            }

            if (!DPI.IsMatch(txt_DpiCuentaNueva.Text))
            {
                MessageBox.Show("Ingresa un DPI válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cont++;
                dpi = Convert.ToInt64(txt_DpiCuentaNueva.Text);
            }

            if (!Correo.IsMatch(txt_CorreoCuentaNueva.Text))
            {
                MessageBox.Show("Ingresa un correo válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cont++;
                correo = txt_CorreoCuentaNueva.Text;
            }

            if (!Numero.IsMatch(txt_NumeroTelCuentaNueva.Text))
            {
                MessageBox.Show("Ingresa un número de 8 dígitos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cont++;
                numero = Convert.ToInt32(txt_NumeroTelCuentaNueva.Text);
            }

            if (cont == 6)
            {
                tabControl1.SelectTab(1);
                txt_codigo.Text = r.Next(1000, 99999).ToString();
            }

            cont = 0;
        }


        private void btn_salir_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 principal = new Form1();
            principal.Show();

        }

        //regex para contraseñas, para SixChar acepta de [a-zA-Z][0-9] y creo que guion bajo, los otros carateres creo que no, asi que procura poner los 6 caracteres 
        Regex SixChar = new Regex(@"\w{6}");
        Regex Mayus = new Regex(@"[A-Z]");
        Regex mayusInicio = new Regex(@"\A[A-Z]");
        Regex unNum = new Regex(@"\d");
        Regex CharUnico = new Regex(@"[!#$%&/()=?¡¿@\+\*\-,.;:]");
        int punto = 0;

        private void button2_Click(object sender, EventArgs e)
        {

            //reseteo el control del flujo
            punto = 0;

            //valido que los campos esten llenos
            if (string.IsNullOrWhiteSpace(txt_usuario.Text) || string.IsNullOrWhiteSpace(txt_monto.Text) || string.IsNullOrWhiteSpace(txt_contra.Text))
            {
                MessageBox.Show("Debe ingresar todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Regex NameUsuario = new Regex(txt_usuario.Text);

            inforClientesATM nuevoCliente = new inforClientesATM();

            codigo = Convert.ToInt32(txt_codigo.Text);
            banco = cb_banco.Text;
            nuevoCliente.cuentaActiva = true;

            // Verifica Nombre
            if (!NombresYApellidos.IsMatch(txt_usuario.Text))
            {
                MessageBox.Show("Ingrese al menos una letra", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_usuario.Text = "";
            }
            else
            {
                usuario = txt_usuario.Text;
                punto++;
            }

            // Verfica monto
            if (Convert.ToInt32(txt_monto.Text) < 100)
            {
                MessageBox.Show("El monto inicial debe ser mayor a 100", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_monto.Text = "";
            }
            else
            {
                monto = Convert.ToInt32(txt_monto.Text);
                punto++;
            }


            //Verificar la contraseña
            if (SixChar.IsMatch(txt_contra.Text))
            {
                if (Mayus.IsMatch(txt_contra.Text))
                { 
                    if (!mayusInicio.IsMatch(txt_contra.Text))
                    {
                        if (unNum.IsMatch(txt_contra.Text))
                        {
                            if (CharUnico.IsMatch(txt_contra.Text))
                            {
                                if (!NameUsuario.IsMatch(txt_contra.Text))
                                {
                                    //Se guardan aqui para que no haya calvo a la hora de que ponga mal la contraseña y cuando sea correcta los guarda
                                    nuevoCliente.nombre = nombre;
                                    nuevoCliente.apellido = apellido;
                                    nuevoCliente.fechar = fecha;
                                    nuevoCliente.banco = banco;
                                    nuevoCliente.codigo = codigo;
                                    nuevoCliente.DPI = dpi;
                                    nuevoCliente.correo = correo;
                                    nuevoCliente.numero = numero;
                                    nuevoCliente.contra = txt_contra.Text;
                                    nuevoCliente.monto = monto;
                                    nuevoCliente.usuario = usuario;

                                    // Verificar duplicados en la base de datos
                                    bool duplicado = true;

                                    if (datos.inforClientesATM.Any(cl => cl.DPI == dpi))
                                    {
                                        MessageBox.Show($"El DPI {dpi} ya está registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txt_DpiCuentaNueva.Text = "";
                                        duplicado = false;
                                    }
                                    if (datos.inforClientesATM.Any(cl => cl.correo == correo))
                                    {
                                        MessageBox.Show($"El correo {correo} ya está registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txt_CorreoCuentaNueva.Text = "";
                                        duplicado = false;
                                    }
                                    if (datos.inforClientesATM.Any(cl => cl.numero == numero))
                                    {
                                        MessageBox.Show($"El número telefónico {numero} ya está registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txt_NumeroTelCuentaNueva.Text = "";
                                        duplicado = false;
                                    }
                                    if (datos.inforClientesATM.Any(cl => cl.usuario == usuario))
                                    {
                                        MessageBox.Show($"El usuario {usuario} ya está registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txt_usuario.Text = "";
                                        duplicado = false;
                                    }

                                    if (duplicado)
                                    {
                                        try
                                        {
                                            datos.inforClientesATM.InsertOnSubmit(nuevoCliente);
                                            datos.SubmitChanges();
                                            vaciarTxt();
                                            MessageBox.Show("Se han ingresado los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            this.Close();
                                            Form1 principal = new Form1();
                                            principal.Show();
                                        }
                                        catch{ MessageBox.Show("Ha ocurrido un error al guardar los datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);}
                                    }
                                }
                                else { MessageBox.Show("La contraseña no debe tener el nombre de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                            }
                            else { MessageBox.Show("La contraseña debe tener al menos un caracter especial", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                        }
                        else { MessageBox.Show("La contraseña debe tener al menos un número", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                    }
                    else { MessageBox.Show("La contraseña no debe empezar con mayúscula", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                }
                else { MessageBox.Show("La contraseña debe tener al menos una mayúscula", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }
            else
            { MessageBox.Show("La contraseña debe tener al menos 6 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }

        }

        private void regresar(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
            cont = 0;
        }

        private void vaciarTxt()
        {
            txt_ApellidoCuentaNueva.Text = "";
            txt_codigo.Text = "";
            txt_contra.Text = "";
            txt_CorreoCuentaNueva.Text = "";
            txt_DpiCuentaNueva.Text = "";
            txt_FechaCuentaNueva.Text = "";
            txt_monto.Text = "";
            txt_nombreCuentaNueva.Text = "";
            txt_NumeroTelCuentaNueva.Text = "";
            txt_usuario.Text = "";
        }
    }
}
