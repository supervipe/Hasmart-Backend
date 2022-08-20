using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using Xbehave;
using Xunit;


namespace HASmart.Test.Integracao {
    /// <summary>
    ///     Narrativa - Atualizar Informações do cidadão
    ///     Como Atendente e/ou Farmacêutico
    ///     Preciso atualizar, completa ou parcialmente, os dados pessoais e/ou informações estáticas de hipertensão arterial
    ///     do cidadão
    ///     Para que o programa disponha das informações atualizadas do cidadão
    ///     https://docs.google.com/document/d/1OGa8Al2isYzJJ3xYaqqHF37tFlthpxP5/edit
    /// </summary>
    public class UserStory_03 : BaseStory {
        private Operador Operador { get; set; }
        private string CpfCadastrado { get; set; }

        [Background]
        public void PreCondicao() {
            "Usuário do sistema na farmácia deve ter realizado login".x(async () => {
               // this.Operador = await this.farmaciaService.GetOperador("admin", "12345");
            });
            "E estar devidamente autenticado [USI01]".x(() => {
                this.Operador.Should().NotBeNull();
            });
            "Cidadão já está cadastrado no programa [US02]".x(async () => {
                Cidadao jorge = await this.cidadaoService.BuscarViaCpf("12312312356");
                jorge.Should().NotBeNull();
                this.CpfCadastrado = jorge.Cpf;
            });
        }

        [Scenario]
        public void AtualizacaoDadosCorretos(string cpf, Cidadao jorge, CidadaoPutDTO dto, bool biometria) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                cpf = null;
            });
            "Quando ele informa o documento de identificação do cidadão (RG ou CPF)".x(() => {
                cpf = this.CpfCadastrado;
            });
            "E as informações do cidadão são mostradas".x(async () => {
                jorge = await this.cidadaoService.BuscarViaCpf(cpf);
                jorge.Should().NotBeNull();
            });
            "E o cidadão deseja atualizar alguma informação".x(() => {
                dto = new CidadaoPutDTO {
                    DadosPessoais = new DadosPessoaisDTO {
                        Email = jorge.DadosPessoais.Email,
                        Endereco = new EnderecoPostDTO {
                            Rua = jorge.DadosPessoais.Endereco.Rua,
                            Numero = jorge.DadosPessoais.Endereco.Numero,
                            Complemento = jorge.DadosPessoais.Endereco.Complemento,
                            Cidade = jorge.DadosPessoais.Endereco.Cidade,
                            Estado = jorge.DadosPessoais.Endereco.Estado
                        },
                        Genero = jorge.DadosPessoais.Genero,
                        Telefone = jorge.DadosPessoais.Telefone
                    },
                    IndicadorRiscoHAS = new IndicadorRiscoHASDTO {
                        Altura = jorge.IndicadorRiscoHAS.Altura,
                        //AntiHipertensivos = jorge.IndicadorRiscoHAS.AntiHipertensivos,
                        Diabetico = jorge.IndicadorRiscoHAS.Diabetico,
                        Fumante = jorge.IndicadorRiscoHAS.Fumante,
                        HistoricoAvc = jorge.IndicadorRiscoHAS.HistoricoAvc
                    }
                };

                dto.DadosPessoais.Telefone = "16491649";
                dto.IndicadorRiscoHAS.Fumante = TipoFumante.ExFumante;
            });
            "Então o atendente e/ou farmacêutico atualiza o(s) respectivo(s) campo(s)".x(async () => {
                Cidadao novoJorge = await this.cidadaoService.AtualizarCidadao(jorge.Id.Value, dto);
                novoJorge.Should().NotBeNull();
                novoJorge.Should().BeEquivalentTo(dto);
                novoJorge.DadosPessoais.Telefone.Should().Be("16491649");
                novoJorge.IndicadorRiscoHAS.Fumante.Should().Be(TipoFumante.ExFumante);
            });
            "E o cidadão confirma a atualização do(s) dado(s) através de leitura biométrica de impressão digital [RN4]".x(() => {
                biometria = true;
            }).Skip("Funcionalidade postergada");
            "Se a leitura biométrica estiver correta".x(() => {
                biometria.Should().BeTrue();
            }).Skip("Funcionalidade postergada");
            "Então o sistema mostra mensagem de sucesso de atualização de cadastro".x(() => {
                biometria.Should().BeTrue();
            }).Skip("Requer o front-end / controller");
        }

        [Scenario]
        public void AtualizacaoDadosIncorretos(Cidadao jorge, CidadaoPutDTO dto) {
            "Dado que o atendente ou farmacêutico deseja atualizar as informações de um cidadão".x(async () => {
                jorge = await this.cidadaoService.BuscarViaCpf(this.CpfCadastrado);
                jorge.Should().NotBeNull();
            });
            "Quando ele não informa um dos campos Ou informa em formato não compatível [RN02][RN03]".x(() => {
                dto = new CidadaoPutDTO {
                    DadosPessoais = new DadosPessoaisDTO {
                        Email = jorge.DadosPessoais.Email,
                        Endereco = new EnderecoPostDTO {
                            Rua = jorge.DadosPessoais.Endereco.Rua,
                            Numero = jorge.DadosPessoais.Endereco.Numero,
                            Complemento = jorge.DadosPessoais.Endereco.Complemento,
                            Cidade = jorge.DadosPessoais.Endereco.Cidade,
                            Estado = jorge.DadosPessoais.Endereco.Estado
                        },
                        Genero = jorge.DadosPessoais.Genero,
                        Telefone = jorge.DadosPessoais.Telefone
                    },
                    IndicadorRiscoHAS = new IndicadorRiscoHASDTO {
                        Altura = jorge.IndicadorRiscoHAS.Altura,
                        //AntiHipertensivos = jorge.IndicadorRiscoHAS.AntiHipertensivos,
                        Diabetico = jorge.IndicadorRiscoHAS.Diabetico,
                        Fumante = jorge.IndicadorRiscoHAS.Fumante,
                        HistoricoAvc = jorge.IndicadorRiscoHAS.HistoricoAvc
                    }
                };

                dto.DadosPessoais.Telefone = "ASDASDASD";
                dto.IndicadorRiscoHAS.Altura = +9000;
            });
            "Então o sistema informa que não pode atualizar as informações do cidadão e informa qual o problema detectado".x(async () => {
                EntityValidationException e = await Assert.ThrowsAsync<EntityValidationException>(async () => await this.cidadaoService.AtualizarCidadao(jorge.Id.Value, dto));
                Assert.Equal(2, e.Errors.Count());
            });
        }
    }
}