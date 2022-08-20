using System;
using System.Collections.Generic;
using FluentAssertions;
using HASmart.Core.Entities;
using HASmart.Core.Exceptions;
using Xbehave;
using Xunit;


namespace HASmart.Test.Integracao {
    /// <summary>
    ///     Narrativa - Realizar autenticação de farmácia
    ///     Como atendente e/ou farmacêutico em farmácia já credenciada no programa
    ///     Preciso realizar autenticação no módulo de coleta de dados
    ///     Para ter acesso às demais funcionalidades do módulo de coleta de dados
    ///     https://docs.google.com/document/d/1mydgA7sFEefIBZED149J0bD_0lX0Fpzo/edit
    /// </summary>
    public class UserStory_01 : BaseStory {
        [Scenario]
        public void FarmaciaCredenciadaLoginCorreto(string login, string senha, Operador operador) {
            "Dado que o atendente e/ou farmacêutico acessa a tela de autenticação".x(() => {
                login = null;
                senha = null;
            });
            "Quando o usuário informa login credenciado e senha correta".x(async () => {
                login = "admin";
                senha = "12345";
               // operador = await this.farmaciaService.GetOperador(login, senha);
            });
            "Então o sistema exibe mensagem de sucesso de login [RN1]".x(() => {
                operador.Should().NotBeNull();
            });
        }

        [Scenario]
        public void FarmaciaCredenciadaLoginIncorreto(string login, string senha, Operador operador) {
            "Dado que o atendente e/ou farmacêutico acessa a tela de autenticação".x(() => {
                login = null;
                senha = null;
            });
            "Quando o usuário informa login credenciado e senha incorretos".x(async () => {
                login = "";
                senha = "";
               // await Assert.ThrowsAsync<EntityNotFoundException>(async () => await this.farmaciaService.GetOperador(login, senha));
            });
            "Então o sistema exibe mensagem de detalhes de login incorretos [RN1]".x(() => {
                operador.Should().BeNull();
            });
        }

        [Scenario(Skip = "Ainda não é possível implementar")]
        public void FarmaciaNaoCredenciada() { }
    }
}