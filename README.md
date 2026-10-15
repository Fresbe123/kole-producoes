# Kole Produções

Sistema web completo para gestão de eventos, shows e locação de equipamentos, desenvolvido em ASP.NET Core 8 com arquitetura limpa.

> **Status do projeto:** Em desenvolvimento ativo.
> O painel administrativo está concluído. O site público (área do cliente) está em construção.

---

## Tecnologias

- ASP.NET Core 8 MVC (Razor Pages)
- Entity Framework Core 8
- SQL Server / SQL Server LocalDB
- ASP.NET Core Identity (autenticação e autorização)

---

## Arquitetura

O projeto segue os princípios de Clean Architecture, separado em quatro camadas:

```
KoleProducoes/
├── KoleProducoes.Domain/         # Entidades, enums e interfaces
├── KoleProducoes.Infrastructure/ # EF Core, DbContext, repositórios e seed
├── KoleProducoes.Application/    # Regras de negócio (Services)
└── KoleProducoes.Web/            # ASP.NET Core MVC
    ├── Areas/Admin/              # Painel administrativo (concluído)
    └── Controllers/              # Site público (em construção)
```

---

## O que está pronto

### Painel Administrativo (`/admin`)

- **Dashboard** com KPIs em tempo real (shows, produtos, DJs, agendamentos pendentes), tabela de agendamentos recentes e próximos shows
- **Shows** — CRUD completo com status (Rascunho, Publicado, Encerrado, Destacado), slug automático e categorias dinâmicas
- **Produtos** — CRUD completo com controle de estoque e disponibilidade
- **DJs** — CRUD completo com links para redes sociais (Instagram, YouTube, SoundCloud)
- **Categorias** — CRUD dinâmico com tipos (Produto, Serviço, Contratação) e ordenação
- **Agendamentos** — Gerenciamento de horários disponíveis (slots) e solicitações de clientes, com confirmação/cancelamento rápido
- **Usuários** — Gerenciamento com dois perfis de acesso: Administrador (acesso total) e Editor (shows e produtos)
- **Histórico** — Soft Delete com restauração de registros excluídos (Shows, Produtos, DJs e Categorias)

### Segurança implementada

- SQL Injection: prevenido pelo EF Core (queries parametrizadas via LINQ)
- CSRF: AntiForgeryToken em todos os formulários POST
- Brute Force: lockout automático após 5 tentativas incorretas (bloqueio de 15 minutos)
- Rate Limiting: 100 requisições/minuto geral, 10 requisições/15 minutos no login
- Soft Delete: registros excluídos são preservados no banco para auditoria e restauração
- Cookie de autenticação: HttpOnly, SecurePolicy e SameSite configurados

---

## O que está em construção

### Site Público

- Página inicial com hero, próximos shows e formulário de agendamento
- Listagem de shows com filtro por gênero musical
- Histórico de shows realizados agrupado por ano
- Catálogo de produtos para locação
- Perfis de DJs para contratação
- Página dedicada de agendamento de reunião comercial

---

## Como rodar localmente

**Pré-requisitos**
- .NET 8 SDK
- SQL Server ou SQL Server LocalDB (incluído no Visual Studio)

**Passos**

1. Clone o repositório:
```bash
git clone https://github.com/Fresbe123/kole-producoes.git
cd kole-producoes
```

2. Restaure os pacotes:
```bash
dotnet restore
```

3. Configure a connection string em `KoleProducoes.Web/appsettings.json` se necessário (o padrão usa LocalDB)

4. Execute as migrations para criar o banco:
```bash
dotnet ef database update -p KoleProducoes.Infrastructure -s KoleProducoes.Web
```

5. Rode o projeto:
```bash
dotnet run --project KoleProducoes.Web
```

6. Acesse o painel administrativo em:
```
https://localhost:{porta}/admin/login
```

**Credenciais padrão (criadas automaticamente pelo seed)**
- E-mail: `admin@koleproducoes.com.br`
- Senha: `Kole@2025!`

> Altere a senha imediatamente após o primeiro acesso em produção.

---

## Próximos passos

- Concluir o site público
- Upload de imagens (AWS S3 ou Azure Blob Storage)
- Envio de e-mails automáticos (SendGrid)
- Deploy na Hostinger
- Configuração de domínio e HTTPS
- Cloudflare como camada de proteção

---

## Licença

Projeto desenvolvido para fins de estudo e uso comercial pela Kole Produções.
