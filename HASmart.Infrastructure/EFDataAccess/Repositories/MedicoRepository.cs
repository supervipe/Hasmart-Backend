using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HASmart.Core.Architecture;
using HASmart.Core.Entities;
using HASmart.Core.Exceptions;
using HASmart.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HASmart.Infrastructure.EFDataAccess.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private AppDBContext Context  { get; }
        public MedicoRepository(AppDBContext context) { 
            this.Context = context;
        }

        public async Task<Medico> BuscarViaId(long id) {
            Medico m = await this.Context.Medicos.FirstOrDefaultAsync(x => x.Id == id);
            return m ?? throw new EntityNotFoundException(typeof(Medico));
        }   
        public async Task<Medico> BuscarViaCrm(string crm) {
            Medico m = await this.Context.Medicos.FirstOrDefaultAsync(x => x.Crm == crm);
            return m ?? throw new EntityNotFoundException(typeof(Medico));
        }

        public async Task<bool> AlreadyExists(string crm) {
            return await this.Context.Medicos.AnyAsync(x => x.Crm == crm);
        }

        public async Task<Medico> Cadastrar(Medico m) {
            this.Context.Medicos.Add(m);
            await this.Context.SaveChangesAsync();
            return m;
        }

        public async Task<Medico> Atualizar(Medico m) {
            this.Context.Entry(m).State = EntityState.Modified;
            
            try {
                await this.Context.SaveChangesAsync();
                return m;
            } catch (Exception) {
                if (! await this.Context.Medicos.AnyAsync(e => e.Id == m.Id)) {
                    throw new EntityNotFoundException(typeof(Medico));
                } else {
                    throw new EntityConcurrencyException(typeof(Medico));
                }
            }
        }

    }
}