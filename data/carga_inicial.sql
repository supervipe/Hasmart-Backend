IF DB_ID('HASmartDB') IS NOT NULL
BEGIN
    DROP DATABASE HASmartDB;
END;

GO

IF DB_ID('HASmartDB') IS NULL
BEGIN
    CREATE DATABASE HASmartDB;
END;
GO

USE HASmartDB;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Cidadao] (
    [Id] bigint NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Cpf] nvarchar(max) NOT NULL,
    [Rg] nvarchar(max) NOT NULL,
    [DataNascimento] datetime2 NOT NULL,
    [DataCadastro] datetime2 NOT NULL,
    [DadosPessoais_Id] bigint NULL,
    [DadosPessoais_Endereco_Rua] nvarchar(max) NULL,
    [DadosPessoais_Endereco_CEP] nvarchar(max) NULL,
    [DadosPessoais_Endereco_Numero] nvarchar(max) NULL,
    [DadosPessoais_Endereco_Complemento] nvarchar(max) NULL,
    [DadosPessoais_Endereco_Cidade] nvarchar(max) NULL,
    [DadosPessoais_Endereco_Estado] nvarchar(max) NULL,
    [DadosPessoais_Email] nvarchar(max) NULL,
    [DadosPessoais_Telefone] nvarchar(max) NULL,
    [DadosPessoais_Genero] nvarchar(max) NULL,
    [indicadorRiscoHAS_Altura] real NULL,
    [indicadorRiscoHAS_Diabetico] int NULL,
    [indicadorRiscoHAS_Fumante] int NULL,
    [IndicadorRiscoHAS_RetinopatiaHipertensiva] int NULL,
    [IndicadorRiscoHAS_InsuficienciaCardiaca] int NULL,
    [IndicadorRiscoHAS_DoencaArterialObstrutivaPeriferica] int NULL,
    [IndicadorRiscoHAS_DoencaRenalCronica] int NULL,
    [IndicadorRiscoHAS_HistoricoInfarto] int NULL,
    [indicadorRiscoHAS_HistoricoAvc] int NULL,
    CONSTRAINT [PK_Cidadao] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Farmacia] (
    [Id] bigint NOT NULL IDENTITY,
    [NomeFantasia] nvarchar(max) NOT NULL,
    [RazaoSocial] nvarchar(max) NOT NULL,
    [Cnpj] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NULL,
    [Telefone] nvarchar(max) NULL,
    [Endereco_Rua] nvarchar(max) NULL,
    [Endereco_Macroregiao] int NULL,
    [Endereco_CEP] nvarchar(max) NULL,
    [Endereco_Numero] nvarchar(max) NULL,
    [Endereco_Complemento] nvarchar(max) NULL,
    [Endereco_Cidade] nvarchar(max) NULL,
    [Endereco_Estado] nvarchar(max) NULL,
    CONSTRAINT [PK_Farmacia] PRIMARY KEY ([Id])
);


GO

CREATE TABLE [Medicao] (
    [Id] bigint NOT NULL IDENTITY,
    [CidadaoId] bigint NULL,
    [Peso] real NOT NULL,
    [DataHora] datetime2 NOT NULL,
    [CodigoFarm] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Medicao] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Medicao_Cidadao_CidadaoId] FOREIGN KEY ([CidadaoId]) REFERENCES [Cidadao] ([Id]) ON DELETE NO ACTION
);

GO

/*CREATE TABLE [Operador] (
    [Id] bigint NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Cpf] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Operador] PRIMARY KEY ([Id])
);*/

GO

CREATE TABLE [Afericao] (
    [Id] bigint NOT NULL IDENTITY,
    [Sistolica] bigint NOT NULL,
    [Diastolica] bigint NOT NULL,
    [MedicaoId] bigint NOT NULL,
    CONSTRAINT [PK_Afericao] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Afericao_Medicao_MedicaoId] FOREIGN KEY ([MedicaoId]) REFERENCES [Medicao] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Dispencacao] (
    [Id] bigint NOT NULL IDENTITY,
    [CidadaoId] bigint NOT NULL,
    [MedicaoId] bigint NULL,
    [DataHora] datetime2 NOT NULL,
    CONSTRAINT [PK_Dispencacao] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Dispencacao_Cidadao_CidadaoId] FOREIGN KEY ([CidadaoId]) REFERENCES [Cidadao] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Dispencacao_Medicao_MedicaoId] FOREIGN KEY ([MedicaoId]) REFERENCES [Medicao] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Medicamento] (
    [Id] bigint NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    /*[Finalidade] nvarchar(max) NOT NULL,
    [Dosagem] nvarchar(max) NOT NULL,
    [Quantidade] nvarchar(max) NOT NULL,*/
    [DispencacaoId] bigint NULL,
    [MedicaoId] bigint NULL,
    CONSTRAINT [PK_Medicamento] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Medicamento_Dispencacao_DispencacaoId] FOREIGN KEY ([DispencacaoId]) REFERENCES [Dispencacao] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Medicamento_Medicao_MedicaoId] FOREIGN KEY ([MedicaoId]) REFERENCES [Medicao] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Afericao_MedicaoId] ON [Afericao] ([MedicaoId]);

GO

CREATE INDEX [IX_Dispencacao_CidadaoId] ON [Dispencacao] ([CidadaoId]);

GO

/*CREATE INDEX [IX_Dispencacao_EstabelecimentoId] ON [Dispencacao] ([EstabelecimentoId]);

GO*/

CREATE INDEX [IX_Dispencacao_MedicaoId] ON [Dispencacao] ([MedicaoId]);

GO

/*CREATE INDEX [IX_Estabelecimento_FarmaciaId] ON [Estabelecimento] ([FarmaciaId]);

GO*/

CREATE INDEX [IX_Medicamento_DispencacaoId] ON [Medicamento] ([DispencacaoId]);

GO

CREATE INDEX [IX_Medicao_CidadaoId] ON [Medicao] ([CidadaoId]);

GO

/*CREATE INDEX [IX_Medicao_EstabelecimentoId] ON [Medicao] ([EstabelecimentoId]);

GO*/

/*CREATE INDEX [IX_Operador_PontoDeTrabalhoId] ON [Operador] ([PontoDeTrabalhoId]);

GO*/

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200206190013_Teste', N'3.1.1');

GO
