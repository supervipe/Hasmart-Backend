using HASmart.Core.Architecture;
using System.Collections.Generic;

namespace HASmart.Core.Entities
{
    public class Medico : AggregateRoot
    {
        public string Nome { get; set; }
        public string Crm { get; set; } 
        public List<Cidadao> cidadaosAtuais { get; set; } 
        public List<Cidadao> cidadaosAtendidos { get; set; }  
    }
}