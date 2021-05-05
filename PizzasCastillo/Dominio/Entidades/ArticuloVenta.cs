﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class ArticuloVenta
    {
        public string CodigoBarra { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public byte[] Foto { get; set; }
        public int Estatus { get; set; }
        public bool EsPlatillo { get; set; }

        public static ArticuloVenta Clone(AccesoADatos.ArticuloVenta articuloVenta)
        {
            return new ArticuloVenta 
            {
                CodigoBarra = articuloVenta.CodigoBarra,
                Nombre = articuloVenta.Nombre,
                Precio = articuloVenta.Precio,
                Foto = articuloVenta.Foto,
                Estatus = articuloVenta.Estatus,
                EsPlatillo = articuloVenta.EsPlatillo
            };
        }
    }
}
