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
    ///     Narrativa - Cadastro do cidadão
    ///     Como Atendente e/ou Farmacêutico
    ///     Preciso cadastrar dados pessoais e informações estáticas de hipertensão arterial do cidadão
    ///     Para cadastrar o cidadão no programa HASmart
    ///     https://docs.google.com/document/d/1sToshiSB9CiMvl7zq7F2ZIxGNL8VaNmW/edit
    /// </summary>
    public class UserStory_02 : BaseStory {
        private Operador Operador { get; set; }

        [Background]
        public void PreCondicao() {
            "Usuário do sistema na farmácia deve ter realizado login".x(async () => {
           //     this.Operador = await this.farmaciaService.GetOperador("admin", "12345");
            });
            "E estar devidamente autenticado [USI01]".x(() => {
                this.Operador.Should().NotBeNull();
            });
        }

        [Scenario]
        public void CidadaoNaoCadastrado(string cpf, CidadaoPostDTO dto, bool biometria) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                cpf = null;
            }); // nada para por aqui ainda
            "Quando ele informa o documento de identificação do cidadão (RG ou CPF)".x(() => {
                cpf = "98798798765";
            });
            "E o sistema informa que tal cidadão não está cadastrado no programa".x(async () => {
                await Assert.ThrowsAsync<EntityNotFoundException>(async () => await this.cidadaoService.BuscarViaCpf(cpf));
            });
            "Então o sistema pede dados pessoais: ‘nome’, ‘data de nascimento’, ‘endereço’, ‘RG’, ‘CPF’, e-mail, ‘telefone(s) de contato’, ‘gênero’ [RN2]".x(() => {
                dto = new CidadaoPostDTO {
                    Nome = "Lucas",
                    DataNascimento = DateTime.Now.AddYears(-20),
                    Rg = "1234",
                    Cpf = cpf,
                    DadosPessoais = new DadosPessoaisDTO {
                        Endereco = new EnderecoPostDTO {
                            Rua = "Rua dos Bobos",
                            Numero = "481",
                            Complemento = "Próximo à Praça",
                            Cidade = "Fortaleza",
                            Estado = "Ceará"
                        },
                        Email = "l@l.com",
                        Telefone = "34770000",
                        Genero = "Masculino"
                    },
                    IndicadorRiscoHAS = new IndicadorRiscoHASDTO {
                        Altura = 1.75f,
                        Diabetico = TipoDiabetes.Nao,
                        Fumante = TipoFumante.NaoFumante,
                        //AntiHipertensivos = true,
                        //HistoricoAvc = false
                    }
                };
            });
            "Então o cidadão confirma o cadastro através de leitura biométrica de impressão digital [RN4]".x(() => {
                biometria = true;
            }).Skip("Funcionalidade postergada");
            "Se a leitura biométrica estiver correta".x(() => {
                Assert.True(biometria);
            }).Skip("Funcionalidade postergada");
            "Então o sistema mostra mensagem de sucesso de cadastro".x(async () => {
                Cidadao ret = await this.cidadaoService.CadastrarCidadao(dto);
                ret.Should().NotBeNull();
                ret.Should().BeEquivalentTo(dto);
            });
        }

        [Scenario]
        public void CidadaoJaCadastrado(string cpf, Cidadao jorge) {
            "Dado que o atendente ou farmacêutico recebe um cidadão na farmácia".x(() => {
                cpf = null;
            }); // nada para por aqui ainda
            "Quando ele informa o documento de identificação (RG ou CPF) de um cidadão já cadastrado no programa HASMart".x(() => {
                cpf = "12312312356";
            });
            "Então o sistema informa que o cidadão já está cadastrado".x(async () => {
                jorge = await this.cidadaoService.BuscarViaCpf(cpf);
                jorge.Should().NotBeNull();
            });
            "E exibe as informações do cidadão ".x(() => {
                jorge.Should().NotBeNull();
            }).Skip("Para implementar, é necessário o frontend");
        }

        [Scenario]
        public void CidadaoNaoCadastradoDadosErrados(string cpf, CidadaoPostDTO dto) {
            "Dado que o atendente ou farmacêutico deseja cadastrar um cidadão no programa".x(() => {
                cpf = "45645645623";
            }); // nada para por aqui ainda
            "Quando ele não informa um dos campos Ou informa campo em formato não compatível [RNI02][RNI03]".x(() => {
                dto = new CidadaoPostDTO {
                    Nome = "123123",
                    DataNascimento = DateTime.Now,
                    Rg = "AAAA",
                    Cpf = cpf,
                    DadosPessoais = new DadosPessoaisDTO {
                        Endereco = new EnderecoPostDTO {
                            Rua = "Rua dos Bobos",
                            Numero = "481",
                            Complemento = "Próximo à Praça",
                            Cidade = "Fortaleza",
                            Estado = "Ceará"
                        },
                        Email = "AAAAAAA",
                        Telefone = "AAAAA",
                        Genero = "9999"
                    },
                    IndicadorRiscoHAS = null
                };
            });
            "Então o sistema informa que não pode cadastrar o cidadão e informa qual o problema detectado".x(async () => {
                await Assert.ThrowsAsync<EntityValidationException>(async () => await this.cidadaoService.CadastrarCidadao(dto));
            });
        }
    }
}