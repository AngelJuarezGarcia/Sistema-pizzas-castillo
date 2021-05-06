﻿namespace Dominio.Entidades
{
    public class Repartidor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public int Estatus { get; set; }
        public string EmpresaRepartidora { get; set; }

        public static Repartidor Clone(AccesoADatos.Repartidor repartidor)
        {
            return new Repartidor
            {
                Id = repartidor.Id,
                Nombre = repartidor.Nombre,
                Telefono = repartidor.Telefono,
                Estatus = repartidor.Estatus,
                EmpresaRepartidora = repartidor.EmpresaRepartidora
            };
        }
    }
}