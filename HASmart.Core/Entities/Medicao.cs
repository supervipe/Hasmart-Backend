using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;
using System.ComponentModel;

namespace HASmart.Core.Entities {
    // TODO RN08 ainda não definida: Processo de medição
    // TODO RN09 ainda não definida: Formato para registro de pressão arterial
    // TODO RN10 ainda não definida: Formato para registro do peso
    public class Medicao : Entity {
        public long CidadaoId { get; set; }
        public List<Afericao> Afericoes { get; set; }
        public float Peso { get; set; }
        public List<Medicamento> Medicamentos{ get; set; }
        
        [DataType(DataType.Date)]
        [DefaultValue("00/00/0000")]
        public DateTime DataHora { get; set; }
        public string CodigoFarm { get; set; }
    }
}
