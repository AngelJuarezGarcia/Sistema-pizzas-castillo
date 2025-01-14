﻿using AccesoADatos.ControladoresDeDatos;
using Dominio.Entidades;
using Dominio.Enumeraciones;
using Dominio.Utilerias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Logica
{
    public class ArticuloVentaController
    {
        public const string CLAVE_PLATILLO = "PLAT-";
        public ResultadoRegistro GuardarPlatilloVenta(ArticuloVenta articuloVenta)
        {
            ArticuloVentaDAO articuloVentaDAO = new ArticuloVentaDAO();
            if (articuloVenta == null)
            {
                return ResultadoRegistro.InformacionIncorrecta;
            }

            if (articuloVenta.Platillo.Consume == null || articuloVenta.Platillo.Consume.Count == 0)
            {
                return ResultadoRegistro.ProductosNoEspecificados;
            }

            if (!EstaInformacionCorrecta(articuloVenta))
            {
                return ResultadoRegistro.InformacionIncorrecta;
            }

            if (articuloVentaDAO.ChecarArticuloNombre(articuloVenta.Nombre) != null)
            {
                return ResultadoRegistro.YaExiste;
            }


            bool seRegistro;
            try
            {
                seRegistro =articuloVentaDAO.RegistrarArticuloVenta(CloneArticuloPlatillo(articuloVenta));
            }
            catch (Exception)
            {
                throw;
            }

            if (!seRegistro)
            {
                return ResultadoRegistro.RegistroFallido;

            }
            else
            {
                return ResultadoRegistro.RegistroExitoso;
            }
        }

        private bool EstaInformacionCorrecta(ArticuloVenta articuloVenta)
        {
            ValidadorArticuloVenta validadorArticulo = new ValidadorArticuloVenta();

            return validadorArticulo.Validar(articuloVenta);
        }

        public ResultadoRegistro ActualizarPlatilloVenta(ArticuloVenta articuloVenta,bool nuevoNombre)
        {
            ArticuloVentaDAO articuloVentaDAO = new ArticuloVentaDAO();
            if (articuloVenta == null)
            {
                return ResultadoRegistro.InformacionIncorrecta;
            }

            if (articuloVenta.Platillo.Consume == null || articuloVenta.Platillo.Consume.Count == 0)
            {
                return ResultadoRegistro.ProductosNoEspecificados;
            }

            if (!EstaInformacionCorrecta(articuloVenta))
            {
                return ResultadoRegistro.InformacionIncorrecta;
            }

            if (nuevoNombre == true)
            {
                if (articuloVentaDAO.ChecarArticuloNombre(articuloVenta.Nombre) != null)
                {
                    return ResultadoRegistro.YaExiste;
                }
            }
            bool seRegistro;
            try 
            {
                seRegistro = articuloVentaDAO.ActualizarArticuloVenta(CloneActualizarArticuloPlatillo(articuloVenta));
            }
            catch (Exception)
            {
                throw;
            }

            if (!seRegistro)
            {
                return ResultadoRegistro.RegistroFallido;

            }
            else
            {
                return ResultadoRegistro.RegistroExitoso;
            }
        }

        private AccesoADatos.ArticuloVenta CloneActualizarArticuloPlatillo(ArticuloVenta articuloVenta)
        {
            List<AccesoADatos.Consume> consumesCollection = new List<AccesoADatos.Consume>();

            foreach (Consume c in articuloVenta.Platillo.Consume)
            {
                consumesCollection.Add(new AccesoADatos.Consume
                {
                    Cantidad = c.Cantidad,
                    PlatilloCodigoBarra = articuloVenta.CodigoBarra,
                    ProductoCodigoBarra = c.Producto.CodigoBarra
                });
            }

            return new AccesoADatos.ArticuloVenta
            {
                CodigoBarra = articuloVenta.CodigoBarra,
                Nombre = articuloVenta.Nombre,
                Precio = articuloVenta.Precio,
                Foto = articuloVenta.Foto,
                Estatus = articuloVenta.Estatus,
                EsPlatillo = articuloVenta.EsPlatillo,
                NombreFoto = articuloVenta.NombreFoto,
                Platillo = new AccesoADatos.Platillo
                {
                    CodigoBarra = articuloVenta.CodigoBarra,
                    FechaRegisto = DateTime.Now,
                    Receta = articuloVenta.Platillo.Receta,
                    Consume = consumesCollection
                },
            };
        }


        private AccesoADatos.ArticuloVenta CloneArticuloPlatillo(ArticuloVenta articuloVenta)
        {
            List<AccesoADatos.Consume> consumesCollection = new List<AccesoADatos.Consume>();
            ArticuloVentaDAO articuloVentaDAO = new ArticuloVentaDAO();
            int numeroPlatillos = articuloVentaDAO.ObtenerPlatillos().Count;
            string clavePlatillo = CLAVE_PLATILLO;
            if (numeroPlatillos < 9)
            {
                numeroPlatillos += 1;
                clavePlatillo = CLAVE_PLATILLO + "0000" + numeroPlatillos;
            }
            else
            {
                if (numeroPlatillos < 99)
                {
                    numeroPlatillos += 1;
                    clavePlatillo = CLAVE_PLATILLO + "000" + numeroPlatillos;
                }
                else
                {
                    if (numeroPlatillos < 999)
                    {
                        numeroPlatillos += 1;
                        clavePlatillo = CLAVE_PLATILLO + "00" + numeroPlatillos;
                    }
                    else
                    {
                        if (numeroPlatillos < 9999)
                        {
                            numeroPlatillos += 1;
                            clavePlatillo = CLAVE_PLATILLO + "0" + numeroPlatillos;
                        }
                        else
                        {
                            numeroPlatillos += 1;
                            clavePlatillo = CLAVE_PLATILLO + numeroPlatillos;
                        }
                    }
                }
            }

            foreach (Consume c in articuloVenta.Platillo.Consume)
            {
                consumesCollection.Add(new AccesoADatos.Consume
                {
                    Cantidad = c.Cantidad,
                    PlatilloCodigoBarra = clavePlatillo,
                    ProductoCodigoBarra = c.Producto.CodigoBarra
                });
            }

            return new AccesoADatos.ArticuloVenta
            {
                CodigoBarra = clavePlatillo,
                Nombre = articuloVenta.Nombre,
                Precio = articuloVenta.Precio,
                Foto = articuloVenta.Foto,
                Estatus = articuloVenta.Estatus,
                EsPlatillo = articuloVenta.EsPlatillo,
                NombreFoto = articuloVenta.NombreFoto,
                Platillo = new AccesoADatos.Platillo
                {
                    CodigoBarra = clavePlatillo,
                    FechaRegisto = DateTime.Now,
                    Receta = articuloVenta.Platillo.Receta,
                    Consume = consumesCollection
                },
            };
        }

        public List<ArticuloVenta> ObtenerProductos()
        {
            ArticuloVentaDAO articuloDAO = new ArticuloVentaDAO();
           
            List<AccesoADatos.ArticuloVenta> productosBD = articuloDAO.ObtenerProductos();
            List<AccesoADatos.ArticuloVenta> platillosbd = articuloDAO.ObtenerPlatillos();

            List<ArticuloVenta> articulos = ArticuloVenta.CloneListProducto(productosBD);
            List<ArticuloVenta> articulosPlatillos = ArticuloVenta.CloneListPlatillo(platillosbd);

            foreach (ArticuloVenta p in articulosPlatillos) {

                articulos.Add(p);
            
            }

            return articulos;
        }

        public ArticuloVenta ObtenerProducto(string id)
        {
            ArticuloVentaDAO articuloDAO = new ArticuloVentaDAO();
            AccesoADatos.ArticuloVenta productoBD = articuloDAO.ObtenerProducto(id);

            ArticuloVenta articulo = ArticuloVenta.CloneProducto(productoBD);

            return articulo;
        }

        public List<ArticuloVenta> ObtenerProductosPlatilo()
        {
            ArticuloVentaDAO articuloDAO = new ArticuloVentaDAO();
            List<AccesoADatos.ArticuloVenta> productosBD = articuloDAO.ObtenerProductos();
            List<ArticuloVenta> articulos = ArticuloVenta.CloneListProducto(productosBD);

            return articulos;
        }
        public List<ArticuloVenta> ObtenerProductosNombre(string nombre)
        {
            ArticuloVentaDAO articuloDAO = new ArticuloVentaDAO();
            List<AccesoADatos.ArticuloVenta> productosBD = articuloDAO.ObtenerArticuloNombre(nombre);

            List<ArticuloVenta> articulos = ArticuloVenta.CloneListProducto(productosBD);

            return articulos;
        }

        public List<ArticuloVenta> ObtenerPlatillos()
        {
            ArticuloVentaDAO articuloDAO = new ArticuloVentaDAO();
            List<AccesoADatos.ArticuloVenta> platillosBD = articuloDAO.ObtenerPlatillos();

            List<ArticuloVenta> platillos = ArticuloVenta.CloneListPlatillo(platillosBD);

            return platillos;

        }

    }
}
