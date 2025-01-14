﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos.ControladoresDeDatos
{
    public class ArticuloVentaDAO
    {
        private readonly PizzasBDEntities connection;
        private List<ArticuloVenta> articulosVenta;
        private ArticuloVenta articuloVenta;
        private const int ACTIVO = 1;
        private const int SIN_CAMBIOS = 0;
        private int resultado;


        public ArticuloVentaDAO()
        {
            connection = new PizzasBDEntities();
            resultado = 0;
        }

        public List<ArticuloVenta> ObtenerPlatillos()
        {
            try
            {
                articulosVenta = connection.ArticuloVenta
                .Where(articulo => articulo.EsPlatillo == true)
                .ToList();
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articulosVenta;
        }

        public ArticuloVenta ObtenerPlatillo(string codigoBarra)
        {
            try
            {
                articuloVenta = connection.ArticuloVenta.Find(codigoBarra);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articuloVenta;
        }

        public ArticuloVenta ObtenerPlatilloNombre(string nombrePlatillo)
        {
            try
            {
                articuloVenta = connection.ArticuloVenta.Where(id => id.Nombre == nombrePlatillo).FirstOrDefault();
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articuloVenta;
        }

        public List<ArticuloVenta> ObtenerProductos()
        {
            try
            {
                articulosVenta = connection.ArticuloVenta
                .Where(articulo => articulo.EsPlatillo == false && articulo.Estatus == 1 && articulo.Producto.TipoProducto.Nombre.Equals("Final"))
                .ToList();
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articulosVenta;
        }


        public List<ArticuloVenta> ObtenerProductosInsumos()
        {
            try
            {
                articulosVenta = connection.ArticuloVenta
                .Where(articulo => articulo.EsPlatillo == false)
                .ToList();
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articulosVenta;
        }



        public ArticuloVenta ObtenerProducto(string id)
        {
            try
            {
                articuloVenta = connection.ArticuloVenta.Find(id);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articuloVenta;
        }

        public bool RegistrarArticuloVenta(ArticuloVenta articuloVenta)
        {
            try
            {
                connection.Entry(articuloVenta).State = EntityState.Added;
                resultado = connection.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }

            if (resultado == SIN_CAMBIOS)
            {
                return false;
            }

            return true;
        }

        public bool ActualizarArticuloVenta(ArticuloVenta articuloVenta)
        {
            try
            {
                var articuloVentaBD = connection.ArticuloVenta.Where(p => p.CodigoBarra == articuloVenta.CodigoBarra).Include("Platillo").First();
                if (articuloVentaBD != null)
                {
                    articuloVentaBD.Nombre = articuloVenta.Nombre;
                    articuloVentaBD.NombreFoto = articuloVenta.NombreFoto;
                    articuloVentaBD.Precio = articuloVenta.Precio;
                    articuloVentaBD.Foto = articuloVenta.Foto;
                    articuloVentaBD.Estatus = articuloVenta.Estatus;
                    articuloVentaBD.EsPlatillo = articuloVenta.EsPlatillo;
                    while(articuloVentaBD.Platillo.Consume.Count < 0)
                    {
                        connection.Entry(articuloVentaBD.Platillo.Consume.First()).State = EntityState.Deleted;
                    }

                    articuloVentaBD.Platillo = articuloVenta.Platillo;
                    connection.Entry(articuloVentaBD).State = EntityState.Modified;
                    resultado = connection.SaveChanges();
                }
            }
            catch(DbUpdateException)
            {
                throw;
            }
            if (resultado == SIN_CAMBIOS)
            {

            }
            return true;
        }


        public List<ArticuloVenta> ObtenerArticuloNombre(string nombre)
        {
            try {
                articulosVenta = connection.ArticuloVenta
                       .Where(lista => lista.Nombre.Contains(nombre)).ToList();

            }
            catch (ArgumentNullException) {
                throw;
            }
            return articulosVenta;
        }

        public ArticuloVenta ChecarArticuloNombre(string nombre)
        {
            try
            {
                return connection.ArticuloVenta.Where(lista => lista.Nombre.Equals(nombre)).FirstOrDefault();

            }
            catch (ArgumentNullException)
            {
                throw;
            }
            return articuloVenta;
        }

    }
}
