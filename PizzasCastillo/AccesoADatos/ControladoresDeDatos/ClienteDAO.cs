﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos.ControladoresDeDatos
{
    public class ClienteDAO
    {

        private PizzasBDEntities _connection;
        private List<Persona> _clientes;
        private const int ACTIVO = 1;
        private const int SIN_CAMBIOS = 0;
        private int _resultado;

        public ClienteDAO()
        {
            _connection = new PizzasBDEntities();
            _resultado = 0;
        }

        public List<Persona> ObtenerClientes()
        {
            _clientes = _connection.Persona
                .Where(persona => persona.Estatus == ACTIVO)
                .Include("Cliente")
                .ToList();

            return _clientes;
        }

        public bool RegistrarCliente(Persona cliente)
        {
            try
            {
                TipoUsuario tipoUsuario = _connection.TipoUsuario.FirstOrDefault(tipo => tipo.Nombre == "Cliente");
                cliente.IdTipoUsuario = tipoUsuario.Id;
                _connection.Entry(cliente).State = EntityState.Added;
                _resultado = _connection.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }

            if (_resultado == SIN_CAMBIOS)
            {
                return false;
            }

            return true;
        }

    }
}