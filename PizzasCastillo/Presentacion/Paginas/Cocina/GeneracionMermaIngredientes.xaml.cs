﻿using Dominio.Entidades;
using Dominio.Logica;
using Presentacion.Ventanas;
using Presentacion.Ventanas.Usuario;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Presentacion.Paginas.Cocina
{
    /// <summary>
    /// Lógica de interacción para GeneracionMermaIngredientes.xaml
    /// </summary>
    public partial class GeneracionMermaIngredientes : Page
    {
        private Process _teclado;
        ObservableCollection<ArticuloVenta> productos;
        private ProductoPopUp productoPopUP;
        private List<ArticuloVenta> productosList;
        public GeneracionMermaIngredientes()
        {
            InitializeComponent();
            ArticuloVentaController articuloController = new ArticuloVentaController();
            productosList = articuloController.ObtenerProductos();

            productos = new ObservableCollection<ArticuloVenta>();
            productoList.ItemsSource = productos;
            try
            {
                foreach (ArticuloVenta p in productosList)
                {
                    File.WriteAllBytes(Recursos.RecursosGlobales.RUTA_IMAGENES + p.NombreFoto, p.Foto);
                }
            }
            catch (Exception)
            {
                
            }
        }
        private void Eliminar(object sender, RoutedEventArgs e)
        {
            Confirmacion dialogoConfirmacion = new Confirmacion("Regresar",
               "Seguro que desea eliminar el insumo?");

            if (dialogoConfirmacion.ShowDialog() == true)
            {
                ListBoxItem selectedItem = (ListBoxItem)productoList.ItemContainerGenerator.ContainerFromItem(((Button)sender).DataContext);
                selectedItem.IsSelected = true;
                productos.Remove((ArticuloVenta)productoList.SelectedItem);
            }
        }

        private void Agregar(object sender, RoutedEventArgs e)
        {
            productoPopUP = new ProductoPopUp(true, productosList, new ArticuloVenta());
            productoPopUP.RegistroExitoso += new EventHandler(NuevoInsumo);
            productoPopUP.ShowDialog();
        }
        private void NuevoInsumo(object sender, EventArgs e)
        {
            if (productoList.Items.Contains(productoPopUP.productoAgregado))
            {
                InteraccionUsuario error = new InteraccionUsuario("Error de campos", "Este insumo ya esta registrado");
                error.Show();
            }
            else
            {
                productos.Add(productoPopUP.productoAgregado);
                productoPopUP.Close();
            }
        }
        private void ActualizarInsumo(object sender, EventArgs e)
        {
            productos.Add(productoPopUP.productoAgregado);
            productos.Remove((ArticuloVenta)productoList.SelectedItem);
            productoPopUP.Close();
        }


        private void Editar(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)productoList.ItemContainerGenerator.ContainerFromItem(((Button)sender).DataContext);
            selectedItem.IsSelected = true;
            productoPopUP = new ProductoPopUp(false, productosList, (ArticuloVenta)productoList.SelectedItem);
            productoPopUP.RegistroExitoso += new EventHandler(ActualizarInsumo);
            productoPopUP.ShowDialog();
        }


        private void Cancelar(object sender, RoutedEventArgs e)
        {
            Confirmacion dialogoConfirmacion = new Confirmacion("Regresar",
               "Seguro que desea cancelar el registro");

            if (dialogoConfirmacion.ShowDialog() == true)
            {
                NavigationService.GoBack();
            }
        }


        private void Registrar(object sender, RoutedEventArgs e)
        {
            if (productos.Count < 0 || !String.IsNullOrWhiteSpace(justificacionText.Text))
            {
                List<Indica> indicaList = new List<Indica>();
                foreach (ArticuloVenta articulo in productos)
                {
                    indicaList.Add(new Indica
                    {
                        Cantidad = articulo.CantidadLocal,
                        Producto = articulo.Producto
                    });
                }
                Merma nuevaMerma = new Merma
                {
                    Fecha = DateTime.Now,
                    Justificacion = justificacionText.Text,
                    Indica = indicaList
                };

                MermaController mermaController = new MermaController();
                bool guardado = mermaController.GuardarMermaInsumos(nuevaMerma);
                if (guardado)
                {
                    InteraccionUsuario ventana = new InteraccionUsuario("Exito en registro", "Se ha guardado la merma con exito");
                    ventana.Show();
                    NavigationService.GoBack();
                }
                else
                {
                    InteraccionUsuario ventana = new InteraccionUsuario("Error de registro", "A ocurrido un error de registro");
                    ventana.Show();
                }
            }
            else
            {
                InteraccionUsuario interaccionUsuario = new InteraccionUsuario("Error de Campos","Uno o mas campos se encuentran vacios, porfavor ingresar los campos necesarios");
                interaccionUsuario.Show();
            }

        }
        private void AbrirTeclado_Touch(object sender, TouchEventArgs e)
        {
            _teclado = Process.Start("osk.exe");

            if (sender.GetType() == typeof(TextBox))
            {
                ((TextBox)sender).Focus();
            }
            else
            {
                ((PasswordBox)sender).Focus();
            }
        }

        private void CerrarTeclado(object sender, RoutedEventArgs e)
        {
            if (_teclado != null)
            {
                if (!_teclado.HasExited)
                    _teclado.Kill();
            }
        }
    }
}
