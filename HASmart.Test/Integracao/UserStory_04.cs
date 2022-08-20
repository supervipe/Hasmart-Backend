using System;
using System.Collections.Generic;
using FluentAssertions;
using HASmart.Core.Entities;
using HASmart.Core.Exceptions;
using Xbehave;
using Xunit;


namespace HASmart.Test.Integracao {
    /// <summary>
    ///     Narrativa - Pesquisar Cidadão Cadastrado
    ///     Como Atendente e/ou Farmacêutico
    ///     Preciso consultar um cidadão cadastrado no programa
    ///     Para checar suas informações, checar suas autorizações, registrar medições e registrar dispensações
    ///     https://docs.google.com/document/d/1OGa8Al2isYzJJ3xYaqqHF37tFlthpxP5/edit
    /// </summary>
    public class UserStory_04 : BaseStory {
        private Operador Operador { get; set; }
        private string CpfCadastrado { get; set; }
        private string RgCadastrado { get; set; }

        [Background]
        public void PreCondicao() {
            "Usuário do sistema na farmácia deve ter realizado login".x(async () => {
              //  this.Operador = await this.farmaciaService.GetOperador("admin", "12345");
            });
            "E estar devidamente autenticado [USI01]".x(() => {
                this.Operador.Should().NotBeNull();
            });
            "Cidadão já está cadastrado no programa [US02]".x(async () => {
                Cidadao jorge = await this.cidadaoService.BuscarViaCpf("12312312356");
                jorge.Should().NotBeNull();
                this.CpfCadastrado = jorge.Cpf;
                this.RgCadastrado = jorge.Rg;
            });
        }

        [Scenario]
        public void BuscaPorRg(string rg, Cidadao c) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                rg = this.RgCadastrado;
            });
            "Quando ele informa o RG do cidadão [RN02] E o cidadão está cadastrado no programa".x(async () => {
                c = await this.cidadaoService.BuscarViaRg(rg);
                c.Should().NotBeNull();
            });
            "Então o sistema exibe seus dados pessoais e as informações estáticas de hipertensão arterial".x(() => {
                c.DadosPessoais.Should().NotBeNull();
                c.IndicadorRiscoHAS.Should().NotBeNull();
            });
        }

        [Scenario]
        public void BuscaPorCpf(string cpf, Cidadao c) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                cpf = this.CpfCadastrado;
            });
            "Quando ele informa o CPF do cidadão [RN02] E o cidadão está cadastrado no programa".x(async () => {
                c = await this.cidadaoService.BuscarViaCpf(cpf);
                c.Should().NotBeNull();
            });
            "Então o sistema exibe seus dados pessoais e as informações estáticas de hipertensão arterial".x(() => {
                c.DadosPessoais.Should().NotBeNull();
                c.IndicadorRiscoHAS.Should().NotBeNull();
            });
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example("ASDASD")]
        [Example("123123123AB")]
        [Example("12312312356999999999")]
        public void BuscaPorCpfInvalido(string cpf) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                true.Should().BeTrue();
            });
            "Quando ele informa o RG ou CPF do cidadão [RN02] e o cidadão não está cadastrado no programa HASmart".x(() => {
                true.Should().BeTrue();
            });
            "Então o sistema informa que o cidadão não foi encontrado e sugere o cadastro do cidadão".x(async () => {
                await Assert.ThrowsAsync<EntityValidationException>(async () => await this.cidadaoService.BuscarViaCpf(cpf));
            });
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example("ASDASD")]
        [Example("0001002003004 ASDASDASDASD")]
        public void BuscaPorRgInvalido(string rg) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                true.Should().BeTrue();
            });
            "Quando ele informa o RG ou CPF do cidadão [RN02] e o cidadão não está cadastrado no programa HASmart".x(() => {
                true.Should().BeTrue();
            });
            "Então o sistema informa que o cidadão não foi encontrado e sugere o cadastro do cidadão".x(async () => {
                await Assert.ThrowsAsync<EntityValidationException>(async () => await this.cidadaoService.BuscarViaRg(rg));
            });
        }

        [Scenario]
        public void BuscaPorRgCidadaoInexistente(string rg) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                rg = null;
            });
            "Quando ele informa o RG ou CPF do cidadão [RN02] e o cidadão não está cadastrado no programa HASmart".x(() => {
                rg = "19519519573";
            });
            "Então o sistema informa que o cidadão não foi encontrado e sugere o cadastro do cidadão".x(async () => {
                await Assert.ThrowsAsync<EntityNotFoundException>(async () => await this.cidadaoService.BuscarViaRg(rg));
            });
        }

        [Scenario]
        public void BuscaPorCpfCidadaoInexistente(string cpf) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                cpf = null;
            });
            "Quando ele informa o RG ou CPF do cidadão [RN02] e o cidadão não está cadastrado no programa HASmart".x(() => {
                cpf = "19519519573";
            });
            "Então o sistema informa que o cidadão não foi encontrado e sugere o cadastro do cidadão".x(async () => {
                await Assert.ThrowsAsync<EntityNotFoundException>(async () => await this.cidadaoService.BuscarViaCpf(cpf));
            });
        }
    }
}