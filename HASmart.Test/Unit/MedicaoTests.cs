using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using HASmart.WebApi;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Infrastructure.EFDataAccess;
using HASmart.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HASmart.Test.Extensions;

namespace HASmart.Test.Unit {
    public class MedicaoTests {
        [Fact]
        public void ValidacaoHappyDay() {
            List<ValidationResult> result = new List<ValidationResult>();
            MedicaoPostDTO obj = new MedicaoPostDTO {
                Afericoes = new List<AfericaoPostDTO> {
                    new AfericaoPostDTO {
                        Sistolica = 12,
                        Diastolica = 8,
                    },
                    new AfericaoPostDTO {
                        Sistolica = 12,
                        Diastolica = 8,
                    },
                },
                Peso = 80,
            };

            bool isValid = Validator.TryValidateObject(obj, new ValidationContext(obj), result);
            isValid.Should().BeTrue();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ValidacaoErros() {
            MedicaoPostDTO obj = new MedicaoPostDTO();
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> result = new List<ValidationResult>();

            // Todas essas combinações devem falhar
            (string, object)[] cases = {
                (nameof(obj.Afericoes), null),
                (nameof(obj.Afericoes), new List<AfericaoPostDTO>()),
                (nameof(obj.Afericoes), new List<AfericaoPostDTO> { new AfericaoPostDTO { Sistolica = 12, Diastolica = 8 }}),
                (nameof(obj.Peso), -10f),
                (nameof(obj.Peso), 10f),
                (nameof(obj.Peso), 9999f),
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
