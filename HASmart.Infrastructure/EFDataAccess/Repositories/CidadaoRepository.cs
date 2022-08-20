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


namespace HASmart.Infrastructure.EFDataAccess.Repositories {
    public class CidadaoRepository : ICidadaoRepository {
        private AppDBContext Context  { get; }
        public CidadaoRepository(AppDBContext context) { 
            this.Context = context;
        }


        public async Task<IEnumerable<Cidadao>> BuscarTodos(long de, long para) {
            return await this.Context.Cidadaos.Skip((int)de).Take((int)(para - de)).Include(x => x.Medicoes).ToListAsync();
        }

        //Retirado .Include(x => x.Dispencacoes) dos 3 metodos abaixo
        public async Task<Cidadao> BuscarViaId(long id) {
            Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
            return c ?? throw new EntityNotFoundException(typeof(Cidadao));
        }

        public async Task<Cidadao> BuscarViaCpf(string cpf) {
            Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Cpf == cpf);
            return c ?? throw new EntityNotFoundException(typeof(Cidadao));
        }

        public async Task<Cidadao> BuscarViaRg(string rg) {
            Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Rg == rg);
            return c ?? throw new EntityNotFoundException(typeof(Cidadao));
        }

        public async Task<bool> AlreadyExists(string cpf, string rg) {
            return await this.Context.Cidadaos.AnyAsync(x => x.Cpf == cpf || x.Rg == rg);
        }

        public async Task<Cidadao> Cadastrar(Cidadao c) {
            this.Context.Cidadaos.Add(c);
            await this.Context.SaveChangesAsync();
            return c;
        }

        public async Task<Cidadao> Atualizar(Cidadao c) {
            this.Context.Entry(c).State = EntityState.Modified;
            
            try {
                await this.Context.SaveChangesAsync();
                return c;
            } catch (Exception) {
                if (! await this.Context.Cidadaos.AnyAsync(e => e.Id == c.Id)) {
                    throw new EntityNotFoundException(typeof(Cidadao));
                } else {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }
    }
}
