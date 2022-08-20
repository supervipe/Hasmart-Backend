using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HASmart.Core.Entities;
using HASmart.Core.Repositories;
using HASmart.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using HASmart.Core.Exceptions;


namespace HASmart.Infrastructure.EFDataAccess.Repositories {
    public class FarmaciaRepository : IFarmaciaRepository {
        private AppDBContext Context { get; }

        public FarmaciaRepository(AppDBContext context) { 
            this.Context = context;
        }

          public async Task<Farmacia> Cadastrar(Farmacia c) {
            this.Context.Farmacias.Add(c);
            await this.Context.SaveChangesAsync();
            return c;
        }

         public async Task<Farmacia> BuscarViaId(long id) {
            Farmacia c = await this.Context.Farmacias.FirstOrDefaultAsync(x => x.Id == id);
            return c ?? throw new EntityNotFoundException(typeof(Cidadao));
        }

    }
}
