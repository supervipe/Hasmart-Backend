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
    public class AfericoesTests {
        [Fact]
        public void ValidacaoHappyDay() {
            List<ValidationResult> result = new List<ValidationResult>();
            AfericaoPostDTO obj = new AfericaoPostDTO {
                Sistolica = 12,
                Diastolica = 8,
            };

            bool isValid = Validator.TryValidateObject(obj, new ValidationContext(obj), result);
            isValid.Should().BeTrue();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ValidacaoErros() {
            AfericaoPostDTO obj = new AfericaoPostDTO {
                Sistolica = 12,
                Diastolica = 8,
            };
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> result = new List<ValidationResult>();

            // Todas essas combinações devem falhar
            (string, object)[] cases = {
                (nameof(obj.Sistolica), 0u),
                (nameof(obj.Sistolica), 99u),
                (nameof(obj.Diastolica), 0u),
                (nameof(obj.Diastolica), 99u),
                (nameof(obj.Diastolica), 15u), // maior que a sistólica
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
