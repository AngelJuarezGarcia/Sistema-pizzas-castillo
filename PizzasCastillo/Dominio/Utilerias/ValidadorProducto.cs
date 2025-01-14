﻿using System.Collections.Generic;
using Dominio.Entidades;
using FluentValidation;
using FluentValidation.Results;

namespace Dominio.Utilerias
{
    public class ValidadorProducto : AbstractValidator<Producto>
    {
        private ValidationResult resultado;
        public ValidadorProducto()
        {
            RuleFor(x => x.CodigoBarra).MaximumLength(10);
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Cantidad).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(180);
            RuleFor(x => x.PrecioCompra).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(x => x.Restricciones).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Tipo).NotEmpty().NotNull();
            RuleFor(x => x.UnidadDeMedida).NotEmpty().MaximumLength(15);
        }

        public bool Validar(Producto producto)
        {
            var validador = new ValidadorProducto();
            resultado = validador.Validate(producto);

            if (!resultado.IsValid)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<string> ObtenerPropiedadesIncorrectas()
        {
            List<string> propiedadesIncorrectas = new List<string>();

            if (resultado != null)
            {
                foreach (var error in resultado.Errors)
                {
                    propiedadesIncorrectas.Add(error.PropertyName);
                }
            }

            return propiedadesIncorrectas;
        }
    }
}
