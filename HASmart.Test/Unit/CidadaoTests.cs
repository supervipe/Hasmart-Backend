using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;
using HASmart.WebApi;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Infrastructure.EFDataAccess;
using HASmart.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HASmart.Test.Extensions;
using FluentAssertions;

namespace HASmart.Test.Unit {
    public class CidadaoTests {
        [Fact]
        public void ValidacaoHappyDay() {
            List<ValidationResult> result = new List<ValidationResult>();
            CidadaoPostDTO obj = new CidadaoPostDTO {
                Nome = "Jorge",
                Cpf = "12312312356",
                Rg = "1234123123123",
                DataNascimento = DateTime.Parse("01/01/1990"),
                DadosPessoais = new DadosPessoaisDTO {
                    Email = "Jorge@egroj.com",
                    Telefone = "32322323",
                    Genero = "Homem",
                },
                IndicadorRiscoHAS = new IndicadorRiscoHASDTO {
                    Altura = 1.8f,
                    Diabetico = TipoDiabetes.Nao,
                    Fumante = TipoFumante.NaoFumante,
                },
            };

            bool isValid = Validator.TryValidateObject(obj, new ValidationContext(obj), result);
            isValid.Should().BeTrue();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ValidacaoErros() {
            CidadaoPostDTO obj = new CidadaoPostDTO();
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> result = new List<ValidationResult>();

            // Todas essas combinações devem falhar
            (string, object)[] cases = {
                (nameof(obj.Nome), null),
                (nameof(obj.Nome), ""),
                (nameof(obj.Cpf), null),
                (nameof(obj.Cpf), ""),
                (nameof(obj.Cpf), "asda"),
                (nameof(obj.Cpf), "123"),
                (nameof(obj.Cpf), "1231231235699999999"),
                (nameof(obj.Rg), null),
                (nameof(obj.Rg), ""),
                (nameof(obj.Rg), "fqfq"),
                (nameof(obj.DataNascimento), null),
                (nameof(obj.DataNascimento), DateTime.Now.AddYears(-17)),
                (nameof(obj.DataNascimento), DateTime.Now.AddYears(999)),
                (nameof(obj.DadosPessoais), null),
                (nameof(obj.IndicadorRiscoHAS), null),
            };

            foreach ((string, object) pair in cases) {
                context.MemberName = pair.Item1;
                bool isValid = Validator.TryValidateProperty(pair.Item2, context, result);
                isValid.Should().BeFalse($"{pair.Item1} aceitou {pair.Item2}");
                result.Should().NotBeEmpty();
                result.Clear();
            }
        }

        [Fact]
        public void ValidacaoErros_DadoPessoal() {
            DadosPessoaisDTO obj = new DadosPessoaisDTO();
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> result = new List<ValidationResult>();

            // Todas essas combinações devem falhar
            (string, object)[] cases = {
                (nameof(obj.Email), null),
                (nameof(obj.Email), ""),
                (nameof(obj.Email), "asdasdasd"),
                (nameof(obj.Email), "114141"),
                (nameof(obj.Endereco), null),
                (nameof(obj.Telefone), null),
                (nameof(obj.Telefone), "asdasd"),
                (nameof(obj.Genero), null),
                (nameof(obj.Genero), ""),
                (nameof(obj.Genero), "123"),
            };

            foreach ((string, object) pair in cases) {
                context.MemberName = pair.Item1;
                bool isValid = Validator.TryValidateProperty(pair.Item2, context, result);
                isValid.Should().BeFalse($"{pair.Item1} aceitou {pair.Item2}");
                result.Should().NotBeEmpty();
                result.Clear();
            }
        }

        [Fact]
        public void ValidacaoErros_Endereco() {
            EnderecoPostDTO obj = new EnderecoPostDTO();
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> result = new List<ValidationResult>();

            // Todas essas combinações devem falhar
            (string, object)[] cases = {
                (nameof(obj.CEP), null),
                (nameof(obj.Rua), null),
                (nameof(obj.Numero), null),
                (nameof(obj.Complemento), null),
                (nameof(obj.Cidade), null),
                (nameof(obj.Estado), null),
            };

            foreach ((string, object) pair in cases) {
                context.MemberName = pair.Item1;
                bool isValid = Validator.TryValidateProperty(pair.Item2, context, result);
                isValid.Should().BeFalse($"{pair.Item1} aceitou {pair.Item2}");
                result.Should().NotBeEmpty();
                result.Clear();
            }
        }

        [Fact]
        public void ValidacaoErros_IndicadorRiscoHAS() {
            IndicadorRiscoHASDTO obj = new IndicadorRiscoHASDTO();
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> result = new List<ValidationResult>();

            // Todas essas combinações devem falhar
            (string, object)[] cases = {
                (nameof(obj.Altura), 0f),
                (nameof(obj.Altura), 4f),
                (nameof(obj.Diabetico), (TipoDiabetes)0),
                (nameof(obj.Diabetico), TipoDiabetes.Invalido),
                (nameof(obj.Diabetico), (TipoDiabetes)999),
                (nameof(obj.Fumante), (TipoFumante)0),
                (nameof(obj.Fumante), TipoFumante.Invalido),
                (nameof(obj.Fumante), (TipoFumante)999),
                //(nameof(obj.AntiHipertensivos), null),
                (nameof(obj.HistoricoAvc), null),
            };

            foreach ((string, object) pair in cases) {
                context.MemberName = pair.Item1;
                bool isValid = Validator.TryValidateProperty(pair.Item2, context, result);
                isValid.Should().BeFalse($"{pair.Item1} aceitou {pair.Item2}");
                result.Should().NotBeEmpty();
                result.Clear();
            }
        }
    }
}
