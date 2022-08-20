using System;
using System.Collections.Generic;
using AutoMapper;
using HASmart.Core;
using HASmart.Core.Services;
using HASmart.Infrastructure.EFDataAccess;
using HASmart.Infrastructure.EFDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;


namespace HASmart.Test.Integracao {
    public class BaseStory {
        public CidadaoService cidadaoService { get; private set; }
        public FarmaciaService farmaciaService { get; private set; }

        public BaseStory() {
            this.ResetTestingContext();
        }

        public void ResetTestingContext() {
            DbContextOptions<AppDBContext> options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(DateTime.Now.GetHashCode().ToString()).Options;
            AppDBContext context = new AppDBContext(options);
            IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();

            this.farmaciaService = new FarmaciaService(new FarmaciaRepository(context), new CidadaoRepository(context), mapper);
            this.cidadaoService = new CidadaoService(new FarmaciaRepository(context),new CidadaoRepository(context),this.farmaciaService, mapper);
        }
    }
}