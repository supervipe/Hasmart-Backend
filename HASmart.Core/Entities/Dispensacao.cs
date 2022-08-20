using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;
using System.ComponentModel;

namespace HASmart.Core.Entities {
    public class Dispencacao : Entity {
        public long CidadaoId { get; set; }
        public List<Medicamento> Medicamentos { get; set; }
        
        [DataType(DataType.Date)]
        [DefaultValue("00/00/0000")]
        public DateTime DataHora { get; set; }
    }
}