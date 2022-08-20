using System;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;


namespace HASmart.Core.Entities {
    public class DadosPessoais : EntityProperty {
        public Endereco Endereco { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Genero { get; set; }
    }
}
