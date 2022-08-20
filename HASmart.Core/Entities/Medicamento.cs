using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities {
    public class Medicamento : Entity {
        public string Nome { get; set; }
        
    }
}