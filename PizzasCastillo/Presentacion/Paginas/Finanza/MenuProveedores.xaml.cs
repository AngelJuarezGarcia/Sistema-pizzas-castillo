﻿using System;
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

namespace Presentacion.Paginas.Finanza
{
    /// <summary>
    /// Lógica de interacción para MenuProveedores.xaml
    /// </summary>
    public partial class MenuProveedores : Page
    {
        public MenuProveedores()
        {
            InitializeComponent();
        }

        private void RegistroProveedor(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrarProveedor());
        }

        private void ListaProveedor(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListaProveedores());
        }
    }
}
