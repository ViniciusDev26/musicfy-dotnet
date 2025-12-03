# Musicfy

API de gerenciamento de músicas e playlists construída com .NET 10.0, seguindo princípios de Clean Architecture e Domain-Driven Design (DDD).

## Sobre o Projeto

Musicfy é uma aplicação backend completa que oferece múltiplos protocolos de comunicação para gerenciar usuários, músicas e playlists. O projeto foi desenvolvido com foco em boas práticas de arquitetura de software, separação de responsabilidades e testabilidade.

## Arquitetura

O projeto segue os princípios de Clean Architecture, organizado em camadas:

```
musicfy/
├── Domain/           # Entidades de negócio e interfaces de repositórios
├── Application/      # Casos de uso e lógica de aplicação
├── Infrastructure/   # Implementação de repositórios e persistência
└── Api/             # Controladores, endpoints e configurações
```

### Camadas

- **Domain**: Contém as entidades de negócio (`User`, `Music`, `Playlist`, `PlaylistMusic`, `PlaylistShare`) com suas validações e regras de domínio
- **Application**: Implementa os casos de uso e orquestra as operações de negócio
- **Infrastructure**: Gerencia a persistência de dados com Entity Framework Core e SQLite
- **Api**: Expõe a aplicação através de múltiplos protocolos (REST, GraphQL, gRPC, SOAP)

## Tecnologias e Ferramentas

### Framework e Linguagem
- **.NET 10.0** - Framework principal
- **C#** - Linguagem de programação

### Protocolos de Comunicação
- **REST API** - Endpoints HTTP tradicionais com controllers
- **GraphQL** - API de consulta flexível usando HotChocolate
- **gRPC** - Comunicação de alta performance com Protocol Buffers
- **SOAP** - Serviços web legados usando SoapCore

### Banco de Dados
- **SQLite** - Banco de dados leve e portável
- **Entity Framework Core 10.0** - ORM para acesso a dados
- **Migrations automáticas** - Banco de dados atualizado automaticamente no startup

### Container
- **Docker** - Containerização da aplicação
- **Multi-stage build** - Otimização do tamanho da imagem

## Funcionalidades

### Gestão de Usuários
- Criar, listar, atualizar e deletar usuários
- Validações: nome mínimo 3 caracteres, email válido, idade mínima 13 anos
- Cálculo automático de idade

### Gestão de Músicas
- Criar, listar, atualizar e deletar músicas
- Validações: nome e artista (1-200 caracteres), URL de áudio válida (HTTP/HTTPS)
- Suporte para metadados (nome, artista, URL do áudio)

### Gestão de Playlists
- Criar playlists de usuário ou playlists do sistema
- Adicionar/remover músicas em playlists
- Compartilhar playlists entre usuários com permissões (visualizar/editar)
- Validações: nome (3-100 caracteres), controle de propriedade
- Sistema de permissões para edição e exclusão

## Instalação e Execução

### Pré-requisitos

- **.NET SDK 10.0** ou superior
- **Docker** (opcional, para execução em container)

### Executando Localmente

1. Clone o repositório:
```bash
git clone <repository-url>
cd musicfy
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Execute a aplicação:
```bash
cd Api
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:8080`
- HTTPS: `https://localhost:8081`

### Executando com Docker

1. Build da imagem:
```bash
docker build -t musicfy .
```

2. Execute o container:
```bash
docker run -p 8080:8080 musicfy
```

## Endpoints

### REST API

#### Users
- `GET /api/users` - Lista todos os usuários
- `GET /api/users/{id}` - Busca usuário por ID
- `POST /api/users` - Cria novo usuário
- `PUT /api/users/{id}` - Atualiza usuário
- `DELETE /api/users/{id}` - Remove usuário

#### Musics
- `GET /api/musics` - Lista todas as músicas
- `GET /api/musics/{id}` - Busca música por ID
- `POST /api/musics` - Cria nova música
- `PUT /api/musics/{id}` - Atualiza música
- `DELETE /api/musics/{id}` - Remove música

#### Playlists
- `GET /api/playlists` - Lista todas as playlists
- `GET /api/playlists/{id}` - Busca playlist por ID
- `POST /api/playlists` - Cria nova playlist
- `PUT /api/playlists/{id}` - Atualiza playlist
- `DELETE /api/playlists/{id}` - Remove playlist

### GraphQL

Endpoint: `/graphql`

Acesse o GraphQL Playground em modo de desenvolvimento para explorar o schema completo.

#### Queries Exemplo
```graphql
query {
  users {
    id
    name
    email
    age
  }

  musics {
    id
    name
    artist
    audioUrl
  }

  playlists {
    id
    name
    user {
      name
    }
  }
}
```

#### Mutations Exemplo
```graphql
mutation {
  createUser(input: {
    name: "John Doe"
    email: "john@example.com"
    birthDate: "1990-01-01"
  }) {
    id
    name
  }

  createMusic(input: {
    name: "Song Name"
    artist: "Artist Name"
    audioUrl: "https://example.com/audio.mp3"
  }) {
    id
    name
  }
}
```

### gRPC

Services disponíveis:
- `UserService` - Gerenciamento de usuários
- `MusicService` - Gerenciamento de músicas
- `PlaylistService` - Gerenciamento de playlists

Proto files localizados em `Api/Protos/`:
- `users.proto`
- `musics.proto`
- `playlists.proto`

### SOAP

Endpoints SOAP:
- `/soap/UserService.asmx` - Serviço de usuários
- `/soap/MusicService.asmx` - Serviço de músicas
- `/soap/PlaylistService.asmx` - Serviço de playlists

## Estrutura do Banco de Dados

### Entidades Principais

#### User
```csharp
- Id: int
- Name: string
- Email: string
- BirthDate: DateTime
```

#### Music
```csharp
- Id: int
- Name: string
- Artist: string
- AudioUrl: string
```

#### Playlist
```csharp
- Id: int
- Name: string
- UserId: int? (nullable - null para playlists do sistema)
- CreatedAt: DateTime
```

#### PlaylistMusic (relação N:N)
```csharp
- PlaylistId: int
- MusicId: int
- Position: int
- AddedAt: DateTime
```

#### PlaylistShare
```csharp
- Id: int
- PlaylistId: int
- SharedWithUserId: int
- Permission: SharePermission (View/Edit)
- SharedAt: DateTime
```

## Validações de Domínio

### User
- Nome: mínimo 3 caracteres
- Email: formato válido (contém @)
- Data de nascimento: no passado, idade mínima 13 anos

### Music
- Nome: 1-200 caracteres
- Artista: 1-200 caracteres
- URL do áudio: formato HTTP/HTTPS válido

### Playlist
- Nome: 3-100 caracteres
- Controle de propriedade para edição/exclusão
- Playlists do sistema não podem ser editadas/deletadas

## Performance e Benchmarks

Os benchmarks de performance e testes de carga estão disponíveis no repositório separado:

**[Musicfy K6 Benchmarks](https://github.com/ViniciusDev26/musicfy-k6)**

O repositório contém:
- Testes de carga para todos os endpoints REST
- Cenários de stress testing
- Métricas de throughput e latência
- Comparações de performance entre protocolos

## Desenvolvimento

### Migrations

O projeto está configurado para aplicar migrations automaticamente no startup. Para criar novas migrations manualmente:

```bash
cd Infrastructure
dotnet ef migrations add NomeDaMigration --startup-project ../Api
```

### Estrutura de Pastas da API

```
Api/
├── Controllers/       # REST API controllers
├── GraphQL/          # Queries e Mutations GraphQL
├── Grpc/             # Serviços gRPC
├── Soap/             # Serviços e contratos SOAP
├── Protos/           # Protocol Buffer definitions
└── Program.cs        # Configuração da aplicação
```

## Princípios de Design

- **Clean Architecture**: Separação clara entre camadas, com dependências apontando para o domínio
- **Domain-Driven Design**: Entidades ricas com validações e regras de negócio encapsuladas
- **SOLID**: Princípios aplicados em toda a solução
- **Dependency Injection**: Injeção de dependências nativa do .NET
- **Repository Pattern**: Abstração da camada de dados

## Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença

Este projeto é parte de um trabalho acadêmico da Universidade de Fortaleza (UNIFOR).

## Autor

Desenvolvido por Vinicius

## Links Relacionados

- [Benchmarks com K6](https://github.com/ViniciusDev26/musicfy-k6)
- [Documentação .NET 10.0](https://learn.microsoft.com/pt-br/dotnet/)
- [HotChocolate GraphQL](https://chillicream.com/docs/hotchocolate)
- [gRPC for .NET](https://grpc.io/docs/languages/csharp/)
