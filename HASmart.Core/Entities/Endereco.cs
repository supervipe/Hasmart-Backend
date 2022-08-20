using System;
using HASmart.Core.Architecture;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities  {
    public class Endereco : EntityProperty {
        public string CEP { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade {get; set;}
        public string Estado { get;set; }
    }
}
