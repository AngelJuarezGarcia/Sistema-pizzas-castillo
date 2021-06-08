﻿using Dominio.Entidades;
using Dominio.Enumeraciones;
using Dominio.Excepciones;
using Dominio.Logica;
using Dominio.Utilerias;
using Presentacion.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentacion.Paginas.Usuario
{
    /// <summary>
    /// Lógica de interacción para EditarUsuario.xaml
    /// </summary>
    public partial class EditarUsuario : Page
    {
        private readonly List<string> estadosLista;
        public List<Tipo> TiposUsuario { get; set; }
        private bool _seActualizoUsername = false;
        private bool _seActualizoPassword = false;

        public EditarUsuario(Empleado empleado)
        {
            InitializeComponent();

            estadosLista = new List<string>
            {
                "Aguascalientes",
                "Baja California",
                "Baja California Sur",
                "Campeche",
                "Chiapas",
                "Chihuahua",
                "Coahuila",
                "Colima",
                "Ciudad de México / Distrito Federal",
                "Durango",
                "Estado de México",
                "Guanajuato",
                "Guerrero",
                "Hidalgo",
                "Jalisco",
                "Michoacán",
                "Morelos",
                "Nayarit",
                "Nuevo León",
                "Oaxaca",
                "Puebla",
                "Querétaro",
                "Quintana Roo",
                "San Luis Potosí",
                "Sinaloa",
                "Sonora",
                "Tabasco",
                "Tamaulipas",
                "Tlaxcala",
                "Veracruz",
                "Yucatán",
                "Zacatecas"
            };
            ListaEstidadesFederativas.ItemsSource = estadosLista;

            TipoUsuarioController tipoUsuarioController = new TipoUsuarioController();
            TiposUsuario = tipoUsuarioController.ObtenerTiposUsuario();

            ListaTiposUsuario.ItemsSource = TiposUsuario;
            DataContext = empleado;

            foreach (var tipoUsuario in ListaTiposUsuario.ItemsSource as List<Tipo>)
            {
                if (tipoUsuario.Nombre.Equals(empleado.TipoUsuario.Nombre))
                {
                    ListaTiposUsuario.SelectedItem = tipoUsuario;
                    break;
                }
            }
        }

        private void Cancelar_Clic(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListaDeUsuarios());
        }

        private void PasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _seActualizoPassword = true;
            ActualizarBoton.IsEnabled = true;
        }

        private void Actualizar_Clic(object sender, RoutedEventArgs e)
        {
            Empleado empleadoAActualizar = DataContext as Empleado;

            empleadoAActualizar.Nombres = NombresText.Text;
            empleadoAActualizar.Apellidos = ApellidosText.Text;
            empleadoAActualizar.Telefono = TelefonoText.Text;
            empleadoAActualizar.Email = EmailText.Text;
            empleadoAActualizar.TipoUsuario = ListaTiposUsuario.SelectedItem as Tipo;
            empleadoAActualizar.Username = UsernameText.Text;
            if(_seActualizoPassword)
                empleadoAActualizar.Contrasenia = PasswordText.Password;

            decimal.TryParse(SalarioText.Text, out decimal salario);
            empleadoAActualizar.SalarioQuincenal = salario;

            if(empleadoAActualizar.Direcciones != null && empleadoAActualizar.Direcciones.Count > 0)
            {
                empleadoAActualizar.Direcciones[0].Calle = CalleText.Text;
                empleadoAActualizar.Direcciones[0].Colonia = ColoniaText.Text;
                empleadoAActualizar.Direcciones[0].Ciudad = CiudadText.Text;
                empleadoAActualizar.Direcciones[0].CodigoPostal = CodigoPostalText.Text;
                empleadoAActualizar.Direcciones[0].NumeroInterior = NoInteriorText.Text;
                empleadoAActualizar.Direcciones[0].Referencias = ReferenciasText.Text;
                empleadoAActualizar.Direcciones[0].NumeroExterior = NoExteriorText.Text;
                empleadoAActualizar.Direcciones[0].EntidadFederativa = ListaEstidadesFederativas.Text;
            }
            else
            {
                Direccion direccion = new Direccion
                {
                    Calle = CalleText.Text,
                    Colonia = ColoniaText.Text,
                    Ciudad = CiudadText.Text,
                    CodigoPostal = CodigoPostalText.Text,
                    NumeroInterior = NoInteriorText.Text,
                    Referencias = ReferenciasText.Text,
                    NumeroExterior = NoExteriorText.Text,
                    EntidadFederativa = ListaEstidadesFederativas.Text
                };

                empleadoAActualizar.Direcciones?.Add(direccion);
            }

            Dialog ventanaDialog = new Dialog();

            if (EstaInformacionCorrecta(empleadoAActualizar))
            {
                EmpleadoController empleadoController = new EmpleadoController();
                ResultadoRegistro resultado;

                try
                {
                    resultado = empleadoController.ActualizarEmpleado(empleadoAActualizar, _seActualizoUsername, _seActualizoPassword);
                }
                catch (Exception)
                {
                    ventanaDialog.Titulo = "Error de actualización";
                    ventanaDialog.Mensaje = "Ocurrió un error al intentar actualizar el empleado. Intente más tarde.";
                    ventanaDialog.ShowDialog();
                    return;
                }

                switch (resultado)
                {
                    case ResultadoRegistro.RegistroExitoso:
                        ventanaDialog.Titulo = "Actualización exitosa";
                        ventanaDialog.Mensaje = "El empleado fue actualizado correctamente.";
                        ventanaDialog.ShowDialog();

                        NavigationService.Navigate(new ListaDeUsuarios());
                        break;
                    case ResultadoRegistro.RegistroFallido:
                        ventanaDialog.Titulo = "Error de actualización";
                        ventanaDialog.Mensaje = "Ocurrió un error al intentar actualizar el empleado. Intente más tarde.";
                        ventanaDialog.ShowDialog();
                        break;
                    case ResultadoRegistro.UsuarioYaExiste:
                        ventanaDialog.Titulo = "El usuario ya existe";
                        ventanaDialog.Mensaje = $"El usuario {empleadoAActualizar.Username} ya pertenece a otro empleado. Por favor ingrese uno diferente.";
                        ventanaDialog.ShowDialog();
                        break;
                    case ResultadoRegistro.DireccionNoEspecificada:
                        ventanaDialog.Titulo = "Error de actualización";
                        ventanaDialog.Mensaje = "La dirección es requerida, por favor especifiquela.";
                        ventanaDialog.ShowDialog();
                        break;
                    case ResultadoRegistro.InformacionIncorrecta:
                        ventanaDialog.Titulo = "Error de actualización";
                        ventanaDialog.Mensaje = "La información ingresada es incorrecta, por favor verifiquela.";
                        ventanaDialog.ShowDialog();
                        break;
                }
            }
            else
            {
                ventanaDialog.Titulo = "Información incorrecta";
                ventanaDialog.Mensaje = "La información ingresada en uno o varios campos es incorrecta. Por favor verifiquela e intente de nuevo.";
                ventanaDialog.ShowDialog();
            }
        }

        private void CamposText_TextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox campo = (TextBox)sender;

            if (campo.Name.Equals(UsernameText.Name))
                _seActualizoUsername = true;
        }

        private bool EstaInformacionCorrecta(Empleado empleado)
        {
            ValidadorPersonas validadorPersona = new ValidadorPersonas();
            ValidadorDireccion validadorDireccion = new ValidadorDireccion();
            ValidadorEmpleado validadorEmpleado = new ValidadorEmpleado();

            return validadorDireccion.Validar(empleado.Direcciones[0]) && validadorPersona.Validar(empleado) &&
                validadorEmpleado.Validar(empleado);
        }
    }
}
