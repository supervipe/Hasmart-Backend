using System;
using System.Collections.Generic;
using FluentAssertions;
using HASmart.Core.Entities;
using Xbehave;


namespace HASmart.Test.Integracao {
    /// <summary>
    ///     Narrativa - Exibir Autorizações Após Identificação do Cidadão
    ///     Como Atendente e/ou Farmacêutico
    ///     Preciso ser informado das autorizações de medição e dispensação de um certo cidadão após a identificação do mesmo
    ///     Para informar, guiar e recomendar a opção mais adequada para a atual visita/consulta do cidadão
    ///     https://docs.google.com/document/d/1zg0T0QI2n55h2Fvt1b9LkcWk5Qt9vYoA/edit
    /// </summary>
    public class UserStory_05 : BaseStory {
        [Scenario]
        public void ExibirAutorizacoesAposIdentificacaoDoCidadao(Cidadao jorge) {
            "Dado que o Atendente ou Farmacêutico identificou um cidadão a partir do seu RG ou CPF [US04]".x(async () => {
                jorge = await this.cidadaoService.BuscarViaCpf("12312312356");
                jorge.Should().NotBeNull();
            });
            "Então o sistema exibe as respectivas autorizações de medição e dispensação que o cidadão pode exercer na atual visita/consulta [RN05][RN06]".x(() => {
                //jorge.PodeRealizarMedicao.Should().BeTrue();
                //jorge.PodeRealizarDispencacao.Should().BeTrue();
                //(jorge.DataProximaMedicao < DateTime.Now).Should().BeTrue();
               // (jorge.DataProximaDispencacao < DateTime.Now).Should().BeTrue();
            });
        }
    }
}