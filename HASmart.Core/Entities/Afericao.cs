using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities {
    public class Afericao : Entity {
        public uint Sistolica { get; set; }
        public uint Diastolica { get; set; }
        public long MedicaoId { get; set; }
    }
}