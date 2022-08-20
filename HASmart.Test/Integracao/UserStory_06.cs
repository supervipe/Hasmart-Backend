using System;
using System.Collections.Generic;
using FluentAssertions;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using Xbehave;
using Xunit;


namespace HASmart.Test.Integracao {
    /// <summary>
    ///     Narrativa - Registro de Medição
    ///     Como Farmacêutico
    ///     Preciso registrar uma medição realizada em um cidadão
    ///     Para dar sequência, ou finalizar, o atendimento a um cidadão
    ///     https://docs.google.com/document/d/1PM4By2Ixu1yO0GSDwX9E8xbZUKCGDTpj/edit
    /// </summary>
    public class UserStory_06 : BaseStory {
        private Operador Operador { get; set; }
        private Cidadao Jorge { get; set; }


        [Background]
        public void PreCondicao() {
            "Usuário do sistema na farmácia deve ter realizado login e estar devidamente autenticado [US01]".x(async () => {
                // necessário pq um dos cenários modifica o banco de forma que afeta o outro cenário
                this.ResetTestingContext(); 
                this.Operador = null;
                this.Jorge = null;

               // this.Operador = await this.farmaciaService.GetOperador("admin", "12345");
                this.Operador.Should().NotBeNull();
            });
            "Cidadão já está cadastrado no programa [US02] Cidadão já foi identificado via RG ou CPF [US04]".x(async () => {
                this.Jorge = await this.cidadaoService.BuscarViaCpf("12312312356");
                this.Jorge.Should().NotBeNull();
            });
            "Cidadão está autorizado a realizar uma medição".x(() => {
               // this.Jorge.PodeRealizarMedicao.Should().BeTrue();
               // this.Jorge.PodeRealizarDispencacao.Should().BeTrue();
                //(this.Jorge.DataProximaMedicao < DateTime.Now).Should().BeTrue();
                //(this.Jorge.DataProximaDispencacao < DateTime.Now).Should().BeTrue();
            });
        }

        [Scenario]
        public void RegistroDeMedicaoDadosCorretos(MedicaoPostDTO dto, bool biometria) {
            "Dado que o cidadão está autorizado a fazer uma medição [US05]".x(() => {
               // this.Jorge.PodeRealizarMedicao.Should().BeTrue();
              //  this.Jorge.PodeRealizarDispencacao.Should().BeTrue();
               // (this.Jorge.DataProximaMedicao < DateTime.Now).Should().BeTrue();
               // (this.Jorge.DataProximaDispencacao < DateTime.Now).Should().BeTrue();
            });
            "E o farmacêutico inicia o processo de medição [RN08]".x(() => {
                dto = new MedicaoPostDTO {
                    Peso = 80,
                    Afericoes = new List<AfericaoPostDTO>()
                };
            });
            "Quando o farmacêutico finaliza o processo de medição, então o sistema pede os dados da medição".x(() => {
                dto.Afericoes.Add(new AfericaoPostDTO {
                    Sistolica = 12,
                    Diastolica = 8
                });
                dto.Afericoes.Add(new AfericaoPostDTO {
                    Sistolica = 13,
                    Diastolica = 8
                });
            });
            "Quando o farmacêutico insere os dados em formato especificado [RN09][RN10]".x(async () => {
                Cidadao novoJorge = await this.farmaciaService.RegistrarMedicao(this.Jorge.Id.Value, dto);
                novoJorge.Should().NotBeNull();
                novoJorge.Medicoes.Should().NotBeEmpty();
                novoJorge.Medicoes[^1].Should().BeEquivalentTo(dto);
                //novoJorge.PodeRealizarMedicao.Should().BeFalse();
               // novoJorge.PodeRealizarDispencacao.Should().BeTrue();
                //(novoJorge.DataProximaMedicao > DateTime.Now).Should().BeTrue();
            });
            "Então o cidadão confirma a medição através de leitura biométrica de impressão digital [RN4]".x(() => {
                biometria = true;
            }).Skip("Funcionalidade postergada");
            "Se a leitura biométrica estiver correta".x(() => {
                biometria.Should().BeTrue();
            }).Skip("Funcionalidade postergada");
            "Então o sistema mostra mensagem de sucesso de registro de medição".x(() => {
                biometria.Should().BeTrue();
            }).Skip("Requer frontend");
        }

        [Scenario]
        public void RegistroDeMedicaoDadosIncorretos(MedicaoPostDTO dto, EntityValidationException e) {
            "Dado que o cidadão está autorizado a fazer uma medição [US05]".x(() => {
                //this.Jorge.PodeRealizarMedicao.Should().BeTrue();
                //this.Jorge.PodeRealizarDispencacao.Should().BeTrue();
               // (this.Jorge.DataProximaMedicao < DateTime.Now).Should().BeTrue();
                //(this.Jorge.DataProximaDispencacao < DateTime.Now).Should().BeTrue();
            });
            "E o farmacêutico inicia o processo de medição [RN08]".x(() => {
                dto = new MedicaoPostDTO {
                    Peso = -999999,
                    Afericoes = new List<AfericaoPostDTO>()
                };
            });
            "Quando o farmacêutico finaliza o processo de medição".x(() => {
                dto.Afericoes.Add(new AfericaoPostDTO {
                    Sistolica = 12,
                    Diastolica = 8
                });
            });
            "Então o sistema pede os dados da medição. Quando o farmacêutico insere dados incompletos e/ou fora do formato especificado [RN09][RN10]".x(async () => {
                e = await Assert.ThrowsAsync<EntityValidationException>(async () => await this.farmaciaService.RegistrarMedicao(this.Jorge.Id.Value, dto));
                e.Errors.Should().NotBeEmpty();
            });
            "Então o sistema exibe mensagem indicando os problemas encontrados e indica como os problemas podem ser solucionados".x(() => {
                Assert.True(true);
            }).Skip("Requer frontend");
        }
    }
}