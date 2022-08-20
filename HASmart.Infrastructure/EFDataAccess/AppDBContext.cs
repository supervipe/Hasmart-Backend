
using System;
using System.Linq;
using System.Collections.Generic;
using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HASmart.Infrastructure.EFDataAccess {
    public class AppDBContext : DbContext {
        public DbSet<Cidadao> Cidadaos { get; set; }
        public DbSet<Medicao> Medicoes { get; set; }
        public DbSet<Dispencacao> Dispencacoes { get; set; }
        public DbSet<Farmacia> Farmacias { get; set; }
        public DbSet<Operador> Operadores { get; set; }
        public DbSet<Medico> Medicos { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {
            if (this.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory" && 
            this.Database.EnsureCreated()) {
                this.Seed();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }

        public bool IsEmpty => !this.Farmacias.Any();
        public void Seed() {
            Farmacia farmacia = new Farmacia {
                RazaoSocial = "Farmacia Piloto",
                NomeFantasia = "Farmacia Piloto",
                Cnpj = "12342323000123",
                Endereco = new EnderecoFarmacia {
                    CEP ="87654321",
                    Macroregiao = Macroregiao.Grande_Fortaleza, 
                    Cidade = "Fortaleza",
                    Rua = "Rua do Sucesso",
                    Numero = "123",
                    Complemento = "Dos Justos",
                    Estado = "Ceará"
                },
                Email = "Farm@egroj.com",
                Telefone = "32322323"
            };
            Operador operador = new Operador {
                Nome = "Jorge",
                Cpf = "12312312356" 
            };
            Medico medico = new Medico{
                Nome = "Francisco",
                Crm = "937",
                cidadaosAtuais = new List<Cidadao>(),
                cidadaosAtendidos = new List<Cidadao>()
            };
            //aqui cria Jorge
            Cidadao cidadao = new Cidadao {
                Nome = "Jorge",
                Cpf = "12312312356",
                Rg = "1234001002003",
                DataNascimento =new DateTime(2000,10,25),
                DataCadastro = DateTime.Now,
                DadosPessoais = new DadosPessoais {
                    Endereco = new Endereco {
                        Rua = "Rua dos Bobos",
                        CEP = "12345678", 
                        Numero = "481", 
                        Complemento = "Próximo à Praça",
                        Cidade = "Fortaleza", 
                        Estado = "Ceará"
                    },
                    Email = "Jorge@egroj.com",
                    Telefone = "32322323",
                    Genero = "Homem",
                },
                IndicadorRiscoHAS = new IndicadorRiscoHAS {
                    Altura = 1.8f,
                    Diabetico = TipoDiabetes.Nao,
                    Fumante = TipoFumante.Fumante,
                    HistoricoAvc = TipoComorbidade.Nao,
                    DoencaRenalCronica = TipoComorbidade.Nao,
                    InsuficienciaCardiaca = TipoComorbidade.Nao,
                    HistoricoInfarto = TipoComorbidade.Nao,
                    DoencaArterialObstrutivaPeriferica = TipoComorbidade.Nao,
                    RetinopatiaHipertensiva = TipoComorbidade.Sim
                },
                Medicoes = new List<Medicao>(),
                medicoAtual = new Medico(),
                medicosAtendeu = new List<Medico>()
                //Dispencacoes = new List<Dispencacao>()
            };

            this.Medicos.Add(medico);
            this.Farmacias.Add(farmacia);
            this.Operadores.Add(operador);
            this.Cidadaos.Add(cidadao);
            this.SaveChanges();
        }
    }
}
