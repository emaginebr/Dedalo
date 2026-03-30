# Dedalo API Documentation

> Base URL: `https://localhost:44374`

## Authentication

A API utiliza autenticação via **Bearer Token (JWT)** com segredos por tenant. O header `X-Tenant-Id` é obrigatório para identificar o tenant. A maioria dos endpoints requer autenticação, exceto os endpoints públicos marcados com `[AllowAnonymous]`.

---

## Objects

### WebsiteInfo

Representa um website completo com todas as informações.

```json
{
  "websiteId": 1,
  "userId": 42,
  "websiteSlug": "meu-portfolio",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio",
  "domainType": 1,
  "customDomain": "portfolio.example.com",
  "logoUrl": "https://cdn.example.com/logos/portfolio-logo.png",
  "css": "body { font-family: 'Inter', sans-serif; } .header { background-color: #1a1a2e; }",
  "status": 1,
  "createdAt": "2026-01-15T10:30:00",
  "updatedAt": "2026-03-20T14:45:00"
}
```

| Property | Type | Description |
|----------|------|-------------|
| websiteId | long | Identificador único do website |
| userId | long | ID do usuário proprietário |
| websiteSlug | string | Slug único do website |
| templateSlug | string | Slug do template utilizado |
| name | string | Nome do website |
| domainType | DomainTypeEnum | Tipo de domínio configurado |
| customDomain | string | Domínio customizado (quando aplicável) |
| logoUrl | string | URL do logotipo do website |
| css | string? | Personalizações de CSS do website (opcional) |
| status | WebsiteStatusEnum | Status atual do website |
| createdAt | DateTime | Data de criação |
| updatedAt | DateTime | Data da última atualização |

### WebsiteInsertInfo

Payload para criação de um novo website.

```json
{
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio",
  "domainType": 1,
  "customDomain": "portfolio.example.com",
  "css": null
}
```

| Property | Type | Description |
|----------|------|-------------|
| templateSlug | string | Slug do template a ser utilizado |
| name | string | Nome do website |
| domainType | DomainTypeEnum | Tipo de domínio |
| customDomain | string | Domínio customizado (opcional, depende do domainType) |
| css | string? | Personalizações de CSS do website (opcional) |

### WebsiteUpdateInfo

Payload para atualização de um website existente.

```json
{
  "websiteId": 1,
  "websiteSlug": "meu-portfolio-atualizado",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio Atualizado",
  "domainType": 3,
  "customDomain": "www.meuportfolio.com.br",
  "css": "body { font-family: 'Inter', sans-serif; } .header { background-color: #1a1a2e; }",
  "status": 1
}
```

| Property | Type | Description |
|----------|------|-------------|
| websiteId | long | ID do website (definido pela rota) |
| websiteSlug | string | Novo slug do website |
| templateSlug | string | Slug do template |
| name | string | Nome atualizado |
| domainType | DomainTypeEnum | Tipo de domínio |
| customDomain | string | Domínio customizado |
| css | string? | Personalizações de CSS do website (opcional) |
| status | WebsiteStatusEnum | Status do website |

### PageInfo

Representa uma página do website.

```json
{
  "pageId": 10,
  "websiteId": 1,
  "pageSlug": "sobre",
  "templatePageSlug": "about-page",
  "name": "Sobre Nós",
  "createdAt": "2026-02-01T09:00:00",
  "updatedAt": "2026-03-10T16:20:00"
}
```

| Property | Type | Description |
|----------|------|-------------|
| pageId | long | Identificador único da página |
| websiteId | long | ID do website ao qual pertence |
| pageSlug | string | Slug único da página |
| templatePageSlug | string | Slug do template da página |
| name | string | Nome da página |
| createdAt | DateTime | Data de criação |
| updatedAt | DateTime | Data da última atualização |

### PageInsertInfo

Payload para criação de uma nova página.

```json
{
  "websiteId": 1,
  "pageSlug": "contato",
  "templatePageSlug": "contact-page",
  "name": "Contato"
}
```

| Property | Type | Description |
|----------|------|-------------|
| websiteId | long | ID do website (definido pela rota) |
| pageSlug | string | Slug da página |
| templatePageSlug | string | Slug do template da página |
| name | string | Nome da página |

### PageUpdateInfo

Payload para atualização de uma página.

```json
{
  "pageId": 10,
  "pageSlug": "sobre-nos",
  "templatePageSlug": "about-page",
  "name": "Sobre Nós - Atualizado"
}
```

| Property | Type | Description |
|----------|------|-------------|
| pageId | long | ID da página (definido pela rota) |
| pageSlug | string | Slug atualizado |
| templatePageSlug | string | Slug do template |
| name | string | Nome atualizado |

### PagePublicInfo

Representa uma página pública com seus conteúdos agrupados por contentSlug.

```json
{
  "pageId": 10,
  "websiteId": 1,
  "pageSlug": "sobre",
  "templatePageSlug": "about-page",
  "name": "Sobre Nós",
  "contents": {
    "hero-section": [
      {
        "contentId": 100,
        "websiteId": 1,
        "pageId": 10,
        "contentType": "text",
        "index": 0,
        "contentSlug": "hero-section",
        "contentValue": "Bem-vindo ao nosso site!",
        "createdAt": "2026-02-01T09:00:00",
        "updatedAt": "2026-03-10T16:20:00"
      }
    ],
    "gallery": [
      {
        "contentId": 101,
        "websiteId": 1,
        "pageId": 10,
        "contentType": "image",
        "index": 0,
        "contentSlug": "gallery",
        "contentValue": "https://cdn.example.com/images/foto1.jpg",
        "createdAt": "2026-02-05T11:00:00",
        "updatedAt": "2026-03-12T08:30:00"
      }
    ]
  },
  "createdAt": "2026-02-01T09:00:00",
  "updatedAt": "2026-03-10T16:20:00"
}
```

| Property | Type | Description |
|----------|------|-------------|
| pageId | long | Identificador único da página |
| websiteId | long | ID do website |
| pageSlug | string | Slug da página |
| templatePageSlug | string | Slug do template da página |
| name | string | Nome da página |
| contents | Dictionary\<string, List\<ContentInfo\>\> | Conteúdos agrupados por contentSlug |
| createdAt | DateTime | Data de criação |
| updatedAt | DateTime | Data da última atualização |

### MenuInfo

Representa um item de menu.

```json
{
  "menuId": 5,
  "websiteId": 1,
  "parentId": null,
  "name": "Início",
  "linkType": 3,
  "externalLink": null,
  "pageId": 10,
  "createdAt": "2026-01-20T08:00:00",
  "updatedAt": "2026-03-15T12:00:00"
}
```

| Property | Type | Description |
|----------|------|-------------|
| menuId | long | Identificador único do menu |
| websiteId | long | ID do website |
| parentId | long? | ID do menu pai (null se for raiz) |
| name | string | Nome exibido no menu |
| linkType | LinkTypeEnum | Tipo de link do menu |
| externalLink | string | URL externa (quando linkType = External) |
| pageId | long? | ID da página interna (quando linkType = InternalPage) |
| createdAt | DateTime | Data de criação |
| updatedAt | DateTime | Data da última atualização |

### MenuInsertInfo

Payload para criação de um item de menu.

```json
{
  "websiteId": 1,
  "parentId": null,
  "name": "Blog",
  "linkType": 2,
  "externalLink": "https://blog.example.com",
  "pageId": null
}
```

| Property | Type | Description |
|----------|------|-------------|
| websiteId | long | ID do website (definido pela rota) |
| parentId | long? | ID do menu pai (null para raiz) |
| name | string | Nome do menu |
| linkType | LinkTypeEnum | Tipo de link |
| externalLink | string | URL externa |
| pageId | long? | ID da página interna |

### MenuUpdateInfo

Payload para atualização de um item de menu.

```json
{
  "menuId": 5,
  "parentId": null,
  "name": "Blog Atualizado",
  "linkType": 3,
  "externalLink": null,
  "pageId": 15
}
```

| Property | Type | Description |
|----------|------|-------------|
| menuId | long | ID do menu (definido pela rota) |
| parentId | long? | ID do menu pai |
| name | string | Nome atualizado |
| linkType | LinkTypeEnum | Tipo de link |
| externalLink | string | URL externa |
| pageId | long? | ID da página interna |

### ContentInfo

Representa um bloco de conteúdo.

```json
{
  "contentId": 100,
  "websiteId": 1,
  "pageId": 10,
  "contentType": "text",
  "index": 0,
  "contentSlug": "hero-title",
  "contentValue": "Bem-vindo ao nosso site!",
  "createdAt": "2026-02-01T09:00:00",
  "updatedAt": "2026-03-10T16:20:00"
}
```

| Property | Type | Description |
|----------|------|-------------|
| contentId | long | Identificador único do conteúdo |
| websiteId | long | ID do website |
| pageId | long | ID da página |
| contentType | string | Tipo do conteúdo (text, image, html, etc.) |
| index | int | Ordem de exibição dentro da área |
| contentSlug | string | Slug que identifica a área de conteúdo |
| contentValue | string | Valor do conteúdo |
| createdAt | DateTime | Data de criação |
| updatedAt | DateTime | Data da última atualização |

### ContentInsertInfo

Payload para criação de um bloco de conteúdo.

```json
{
  "websiteId": 1,
  "pageId": 10,
  "contentType": "text",
  "index": 0,
  "contentSlug": "hero-title",
  "contentValue": "Título Principal"
}
```

| Property | Type | Description |
|----------|------|-------------|
| websiteId | long | ID do website (definido pela rota) |
| pageId | long | ID da página (definido pela rota) |
| contentType | string | Tipo do conteúdo |
| index | int | Ordem de exibição |
| contentSlug | string | Slug da área de conteúdo |
| contentValue | string | Valor do conteúdo |

### ContentUpdateInfo

Payload para atualização de um bloco de conteúdo.

```json
{
  "contentId": 100,
  "contentType": "text",
  "index": 1,
  "contentSlug": "hero-title",
  "contentValue": "Título Principal Atualizado"
}
```

| Property | Type | Description |
|----------|------|-------------|
| contentId | long | ID do conteúdo (definido pela rota) |
| contentType | string | Tipo do conteúdo |
| index | int | Ordem de exibição |
| contentSlug | string | Slug da área de conteúdo |
| contentValue | string | Valor atualizado |

### ContentAreaInfo

Payload para salvar um lote de conteúdos de uma área (content area). O serviço faz diff: insere novos (contentId=0), atualiza existentes e remove os que não estão na lista.

```json
{
  "websiteId": 1,
  "pageId": 10,
  "contentSlug": "gallery",
  "items": [
    {
      "contentId": 0,
      "contentType": "image",
      "index": 0,
      "contentValue": "https://cdn.example.com/images/nova-foto.jpg"
    },
    {
      "contentId": 101,
      "contentType": "image",
      "index": 1,
      "contentValue": "https://cdn.example.com/images/foto-existente.jpg"
    }
  ]
}
```

| Property | Type | Description |
|----------|------|-------------|
| websiteId | long | ID do website (definido pela rota) |
| pageId | long | ID da página (definido pela rota) |
| contentSlug | string | Slug da área de conteúdo |
| items | List\<ContentAreaItemInfo\> | Lista de itens da área |

### ContentAreaItemInfo

Item individual dentro de uma ContentAreaInfo.

```json
{
  "contentId": 101,
  "contentType": "image",
  "index": 0,
  "contentValue": "https://cdn.example.com/images/foto.jpg"
}
```

| Property | Type | Description |
|----------|------|-------------|
| contentId | long | ID do conteúdo (0 para novo) |
| contentType | string | Tipo do conteúdo |
| index | int | Ordem de exibição |
| contentValue | string | Valor do conteúdo |

---

### Enums

#### WebsiteStatusEnum

```json
{
  "1": "Active",
  "2": "Blocked",
  "3": "Inactive"
}
```

#### DomainTypeEnum

```json
{
  "1": "Subdomain",
  "2": "Folder",
  "3": "CustomDomain"
}
```

#### LinkTypeEnum

```json
{
  "1": "None",
  "2": "External",
  "3": "InternalPage"
}
```

---

## Endpoints

---

## Website

### 1. Listar Websites do Usuário

Retorna todos os websites do usuário autenticado.

**Endpoint:** `GET /website`

**Authentication:** Required

**Request Example:**
```http
GET /website
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
[
  {
    "websiteId": 1,
    "userId": 42,
    "websiteSlug": "meu-portfolio",
    "templateSlug": "starter-blog",
    "name": "Meu Portfolio",
    "domainType": 1,
    "customDomain": null,
    "logoUrl": "https://cdn.example.com/logos/portfolio-logo.png",
    "css": "body { font-family: 'Inter', sans-serif; }",
    "status": 1,
    "createdAt": "2026-01-15T10:30:00",
    "updatedAt": "2026-03-20T14:45:00"
  },
  {
    "websiteId": 2,
    "userId": 42,
    "websiteSlug": "loja-virtual",
    "templateSlug": "ecommerce-starter",
    "name": "Minha Loja",
    "domainType": 3,
    "customDomain": "www.minhaloja.com.br",
    "logoUrl": null,
    "css": null,
    "status": 1,
    "createdAt": "2026-02-10T08:00:00",
    "updatedAt": "2026-03-25T11:30:00"
  }
]
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 2. Obter Website por ID

Retorna um website específico pelo seu ID, validando a propriedade do usuário.

**Endpoint:** `GET /website/{websiteId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Example:**
```http
GET /website/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
{
  "websiteId": 1,
  "userId": 42,
  "websiteSlug": "meu-portfolio",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio",
  "domainType": 1,
  "customDomain": null,
  "logoUrl": "https://cdn.example.com/logos/portfolio-logo.png",
  "css": "body { font-family: 'Inter', sans-serif; }",
  "status": 1,
  "createdAt": "2026-01-15T10:30:00",
  "updatedAt": "2026-03-20T14:45:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

**Response Error (404):**
```json
null
```

---

### 3. Obter Website por Slug (Público)

Retorna um website pelo slug. Endpoint público, sem autenticação.

**Endpoint:** `GET /website/slug/{slug}`

**Authentication:** Not Required

**Path Parameters:**
- `slug` (string, required) - Slug do website

**Request Example:**
```http
GET /website/slug/meu-portfolio
```

**Response Success (200):**
```json
{
  "websiteId": 1,
  "userId": 42,
  "websiteSlug": "meu-portfolio",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio",
  "domainType": 1,
  "customDomain": null,
  "logoUrl": "https://cdn.example.com/logos/portfolio-logo.png",
  "css": "body { font-family: 'Inter', sans-serif; }",
  "status": 1,
  "createdAt": "2026-01-15T10:30:00",
  "updatedAt": "2026-03-20T14:45:00"
}
```

---

### 4. Obter Website por Domínio (Público)

Retorna um website pelo domínio customizado. Endpoint público, sem autenticação.

**Endpoint:** `GET /website/domain/{domain}`

**Authentication:** Not Required

**Path Parameters:**
- `domain` (string, required) - Domínio customizado do website

**Request Example:**
```http
GET /website/domain/www.minhaloja.com.br
```

**Response Success (200):**
```json
{
  "websiteId": 2,
  "userId": 42,
  "websiteSlug": "loja-virtual",
  "templateSlug": "ecommerce-starter",
  "name": "Minha Loja",
  "domainType": 3,
  "customDomain": "www.minhaloja.com.br",
  "logoUrl": null,
  "css": null,
  "status": 1,
  "createdAt": "2026-02-10T08:00:00",
  "updatedAt": "2026-03-25T11:30:00"
}
```

---

### 5. Criar Website

Cria um novo website para o usuário autenticado.

**Endpoint:** `POST /website`

**Authentication:** Required

**Request Body:**
```json
{
  "templateSlug": "starter-blog",
  "name": "Meu Novo Site",
  "domainType": 1,
  "customDomain": null,
  "css": null
}
```

**Request Example:**
```http
POST /website
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "templateSlug": "starter-blog",
  "name": "Meu Novo Site",
  "domainType": 1,
  "customDomain": null,
  "css": null
}
```

**Response Success (201):**
```json
{
  "websiteId": 3,
  "userId": 42,
  "websiteSlug": "meu-novo-site",
  "templateSlug": "starter-blog",
  "name": "Meu Novo Site",
  "domainType": 1,
  "customDomain": null,
  "logoUrl": null,
  "css": null,
  "status": 1,
  "createdAt": "2026-03-30T10:00:00",
  "updatedAt": "2026-03-30T10:00:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 6. Atualizar Website

Atualiza um website existente. Valida propriedade do usuário.

**Endpoint:** `PUT /website/{websiteId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Body:**
```json
{
  "websiteSlug": "meu-portfolio-v2",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio V2",
  "domainType": 3,
  "customDomain": "www.meuportfolio.com.br",
  "css": ".header { background-color: #1a1a2e; color: #fff; }",
  "status": 1
}
```

**Request Example:**
```http
PUT /website/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "websiteSlug": "meu-portfolio-v2",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio V2",
  "domainType": 3,
  "customDomain": "www.meuportfolio.com.br",
  "css": ".header { background-color: #1a1a2e; color: #fff; }",
  "status": 1
}
```

**Response Success (200):**
```json
{
  "websiteId": 1,
  "userId": 42,
  "websiteSlug": "meu-portfolio-v2",
  "templateSlug": "starter-blog",
  "name": "Meu Portfolio V2",
  "domainType": 3,
  "customDomain": "www.meuportfolio.com.br",
  "logoUrl": "https://cdn.example.com/logos/portfolio-logo.png",
  "css": ".header { background-color: #1a1a2e; color: #fff; }",
  "status": 1,
  "createdAt": "2026-01-15T10:30:00",
  "updatedAt": "2026-03-30T10:15:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

## Page

### 7. Listar Páginas Públicas

Retorna todas as páginas de um website via slug ou domínio. Endpoint público.

**Endpoint:** `GET /page`

**Authentication:** Not Required

**Query Parameters:**
- `websiteSlug` (string, optional) - Slug do website
- `domain` (string, optional) - Domínio customizado do website

**Request Example:**
```http
GET /page?websiteSlug=meu-portfolio
```

**Response Success (200):**
```json
[
  {
    "pageId": 10,
    "websiteId": 1,
    "pageSlug": "sobre",
    "templatePageSlug": "about-page",
    "name": "Sobre Nós",
    "createdAt": "2026-02-01T09:00:00",
    "updatedAt": "2026-03-10T16:20:00"
  },
  {
    "pageId": 11,
    "websiteId": 1,
    "pageSlug": "contato",
    "templatePageSlug": "contact-page",
    "name": "Contato",
    "createdAt": "2026-02-05T14:00:00",
    "updatedAt": "2026-03-12T09:45:00"
  }
]
```

---

### 8. Obter Página por Slug (Público)

Retorna uma página com seus conteúdos agrupados por contentSlug. Endpoint público.

**Endpoint:** `GET /page/{pageSlug}`

**Authentication:** Not Required

**Path Parameters:**
- `pageSlug` (string, required) - Slug da página

**Query Parameters:**
- `websiteSlug` (string, optional) - Slug do website
- `domain` (string, optional) - Domínio customizado

**Request Example:**
```http
GET /page/sobre?websiteSlug=meu-portfolio
```

**Response Success (200):**
```json
{
  "pageId": 10,
  "websiteId": 1,
  "pageSlug": "sobre",
  "templatePageSlug": "about-page",
  "name": "Sobre Nós",
  "contents": {
    "hero-section": [
      {
        "contentId": 100,
        "websiteId": 1,
        "pageId": 10,
        "contentType": "text",
        "index": 0,
        "contentSlug": "hero-section",
        "contentValue": "Bem-vindo ao nosso site!",
        "createdAt": "2026-02-01T09:00:00",
        "updatedAt": "2026-03-10T16:20:00"
      }
    ],
    "gallery": [
      {
        "contentId": 101,
        "websiteId": 1,
        "pageId": 10,
        "contentType": "image",
        "index": 0,
        "contentSlug": "gallery",
        "contentValue": "https://cdn.example.com/images/foto1.jpg",
        "createdAt": "2026-02-05T11:00:00",
        "updatedAt": "2026-03-12T08:30:00"
      },
      {
        "contentId": 102,
        "websiteId": 1,
        "pageId": 10,
        "contentType": "image",
        "index": 1,
        "contentSlug": "gallery",
        "contentValue": "https://cdn.example.com/images/foto2.jpg",
        "createdAt": "2026-02-05T11:05:00",
        "updatedAt": "2026-03-12T08:30:00"
      }
    ]
  },
  "createdAt": "2026-02-01T09:00:00",
  "updatedAt": "2026-03-10T16:20:00"
}
```

---

### 9. Listar Páginas por Website

Retorna todas as páginas de um website específico.

**Endpoint:** `GET /website/{websiteId}/page`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Example:**
```http
GET /website/1/page
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
[
  {
    "pageId": 10,
    "websiteId": 1,
    "pageSlug": "sobre",
    "templatePageSlug": "about-page",
    "name": "Sobre Nós",
    "createdAt": "2026-02-01T09:00:00",
    "updatedAt": "2026-03-10T16:20:00"
  }
]
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 10. Obter Página por ID

Retorna uma página específica pelo ID.

**Endpoint:** `GET /website/{websiteId}/page/{pageId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página

**Request Example:**
```http
GET /website/1/page/10
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
{
  "pageId": 10,
  "websiteId": 1,
  "pageSlug": "sobre",
  "templatePageSlug": "about-page",
  "name": "Sobre Nós",
  "createdAt": "2026-02-01T09:00:00",
  "updatedAt": "2026-03-10T16:20:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

**Response Error (404):**
```json
null
```

---

### 11. Criar Página

Cria uma nova página em um website. Valida propriedade do website.

**Endpoint:** `POST /website/{websiteId}/page`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Body:**
```json
{
  "pageSlug": "servicos",
  "templatePageSlug": "services-page",
  "name": "Serviços"
}
```

**Request Example:**
```http
POST /website/1/page
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "pageSlug": "servicos",
  "templatePageSlug": "services-page",
  "name": "Serviços"
}
```

**Response Success (201):**
```json
{
  "pageId": 12,
  "websiteId": 1,
  "pageSlug": "servicos",
  "templatePageSlug": "services-page",
  "name": "Serviços",
  "createdAt": "2026-03-30T10:30:00",
  "updatedAt": "2026-03-30T10:30:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 12. Atualizar Página

Atualiza uma página existente. Valida propriedade do website.

**Endpoint:** `PUT /website/{websiteId}/page/{pageId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página

**Request Body:**
```json
{
  "pageSlug": "servicos-atualizado",
  "templatePageSlug": "services-page",
  "name": "Nossos Serviços"
}
```

**Request Example:**
```http
PUT /website/1/page/12
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "pageSlug": "servicos-atualizado",
  "templatePageSlug": "services-page",
  "name": "Nossos Serviços"
}
```

**Response Success (200):**
```json
{
  "pageId": 12,
  "websiteId": 1,
  "pageSlug": "servicos-atualizado",
  "templatePageSlug": "services-page",
  "name": "Nossos Serviços",
  "createdAt": "2026-03-30T10:30:00",
  "updatedAt": "2026-03-30T11:00:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 13. Deletar Página

Remove uma página. Valida propriedade do website.

**Endpoint:** `DELETE /website/{websiteId}/page/{pageId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página

**Request Example:**
```http
DELETE /website/1/page/12
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (204):**
```
No Content
```

**Response Error (401):**
```json
"Not Authorized"
```

---

## Menu

### 14. Listar Menus Públicos

Retorna todos os menus de um website via slug ou domínio. Endpoint público.

**Endpoint:** `GET /menu`

**Authentication:** Not Required

**Query Parameters:**
- `websiteSlug` (string, optional) - Slug do website
- `domain` (string, optional) - Domínio customizado do website

**Request Example:**
```http
GET /menu?websiteSlug=meu-portfolio
```

**Response Success (200):**
```json
[
  {
    "menuId": 5,
    "websiteId": 1,
    "parentId": null,
    "name": "Início",
    "linkType": 3,
    "externalLink": null,
    "pageId": 10,
    "createdAt": "2026-01-20T08:00:00",
    "updatedAt": "2026-03-15T12:00:00"
  },
  {
    "menuId": 6,
    "websiteId": 1,
    "parentId": null,
    "name": "GitHub",
    "linkType": 2,
    "externalLink": "https://github.com/exemplo",
    "pageId": null,
    "createdAt": "2026-01-20T08:05:00",
    "updatedAt": "2026-03-15T12:00:00"
  }
]
```

---

### 15. Listar Menus por Website

Retorna todos os menus de um website específico.

**Endpoint:** `GET /website/{websiteId}/menu`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Example:**
```http
GET /website/1/menu
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
[
  {
    "menuId": 5,
    "websiteId": 1,
    "parentId": null,
    "name": "Início",
    "linkType": 3,
    "externalLink": null,
    "pageId": 10,
    "createdAt": "2026-01-20T08:00:00",
    "updatedAt": "2026-03-15T12:00:00"
  }
]
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 16. Obter Menu por ID

Retorna um item de menu específico.

**Endpoint:** `GET /website/{websiteId}/menu/{menuId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `menuId` (long, required) - ID do menu

**Request Example:**
```http
GET /website/1/menu/5
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
{
  "menuId": 5,
  "websiteId": 1,
  "parentId": null,
  "name": "Início",
  "linkType": 3,
  "externalLink": null,
  "pageId": 10,
  "createdAt": "2026-01-20T08:00:00",
  "updatedAt": "2026-03-15T12:00:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

**Response Error (404):**
```json
null
```

---

### 17. Criar Menu

Cria um novo item de menu. Valida propriedade do website.

**Endpoint:** `POST /website/{websiteId}/menu`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Body:**
```json
{
  "parentId": null,
  "name": "Contato",
  "linkType": 3,
  "externalLink": null,
  "pageId": 11
}
```

**Request Example:**
```http
POST /website/1/menu
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "parentId": null,
  "name": "Contato",
  "linkType": 3,
  "externalLink": null,
  "pageId": 11
}
```

**Response Success (201):**
```json
{
  "menuId": 7,
  "websiteId": 1,
  "parentId": null,
  "name": "Contato",
  "linkType": 3,
  "externalLink": null,
  "pageId": 11,
  "createdAt": "2026-03-30T10:45:00",
  "updatedAt": "2026-03-30T10:45:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 18. Atualizar Menu

Atualiza um item de menu existente. Valida propriedade do website.

**Endpoint:** `PUT /website/{websiteId}/menu/{menuId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `menuId` (long, required) - ID do menu

**Request Body:**
```json
{
  "parentId": 5,
  "name": "Fale Conosco",
  "linkType": 3,
  "externalLink": null,
  "pageId": 11
}
```

**Request Example:**
```http
PUT /website/1/menu/7
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "parentId": 5,
  "name": "Fale Conosco",
  "linkType": 3,
  "externalLink": null,
  "pageId": 11
}
```

**Response Success (200):**
```json
{
  "menuId": 7,
  "websiteId": 1,
  "parentId": 5,
  "name": "Fale Conosco",
  "linkType": 3,
  "externalLink": null,
  "pageId": 11,
  "createdAt": "2026-03-30T10:45:00",
  "updatedAt": "2026-03-30T11:15:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 19. Deletar Menu

Remove um item de menu. Valida propriedade do website.

**Endpoint:** `DELETE /website/{websiteId}/menu/{menuId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `menuId` (long, required) - ID do menu

**Request Example:**
```http
DELETE /website/1/menu/7
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (204):**
```
No Content
```

**Response Error (401):**
```json
"Not Authorized"
```

---

## Content

### 20. Listar Conteúdos Públicos por Página

Retorna todos os conteúdos de uma página via slug. Endpoint público.

**Endpoint:** `GET /content/{pageSlug}`

**Authentication:** Not Required

**Path Parameters:**
- `pageSlug` (string, required) - Slug da página

**Query Parameters:**
- `websiteSlug` (string, optional) - Slug do website
- `domain` (string, optional) - Domínio customizado

**Request Example:**
```http
GET /content/sobre?websiteSlug=meu-portfolio
```

**Response Success (200):**
```json
[
  {
    "contentId": 100,
    "websiteId": 1,
    "pageId": 10,
    "contentType": "text",
    "index": 0,
    "contentSlug": "hero-title",
    "contentValue": "Bem-vindo ao nosso site!",
    "createdAt": "2026-02-01T09:00:00",
    "updatedAt": "2026-03-10T16:20:00"
  },
  {
    "contentId": 101,
    "websiteId": 1,
    "pageId": 10,
    "contentType": "image",
    "index": 0,
    "contentSlug": "gallery",
    "contentValue": "https://cdn.example.com/images/foto1.jpg",
    "createdAt": "2026-02-05T11:00:00",
    "updatedAt": "2026-03-12T08:30:00"
  }
]
```

---

### 21. Listar Conteúdos por Página

Retorna todos os conteúdos de uma página específica.

**Endpoint:** `GET /website/{websiteId}/page/{pageId}/content`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página

**Request Example:**
```http
GET /website/1/page/10/content
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
[
  {
    "contentId": 100,
    "websiteId": 1,
    "pageId": 10,
    "contentType": "text",
    "index": 0,
    "contentSlug": "hero-title",
    "contentValue": "Bem-vindo ao nosso site!",
    "createdAt": "2026-02-01T09:00:00",
    "updatedAt": "2026-03-10T16:20:00"
  }
]
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 22. Obter Conteúdo por ID

Retorna um bloco de conteúdo específico.

**Endpoint:** `GET /website/{websiteId}/page/{pageId}/content/{contentId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página
- `contentId` (long, required) - ID do conteúdo

**Request Example:**
```http
GET /website/1/page/10/content/100
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (200):**
```json
{
  "contentId": 100,
  "websiteId": 1,
  "pageId": 10,
  "contentType": "text",
  "index": 0,
  "contentSlug": "hero-title",
  "contentValue": "Bem-vindo ao nosso site!",
  "createdAt": "2026-02-01T09:00:00",
  "updatedAt": "2026-03-10T16:20:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

**Response Error (404):**
```json
null
```

---

### 23. Salvar Área de Conteúdo (Batch)

Salva um lote de conteúdos de uma mesma área (contentSlug). O serviço faz diff automático: insere itens com contentId=0, atualiza existentes e remove os que não estão mais na lista.

**Endpoint:** `PUT /website/{websiteId}/page/{pageId}/content/area`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página

**Request Body:**
```json
{
  "contentSlug": "gallery",
  "items": [
    {
      "contentId": 0,
      "contentType": "image",
      "index": 0,
      "contentValue": "https://cdn.example.com/images/nova-foto.jpg"
    },
    {
      "contentId": 101,
      "contentType": "image",
      "index": 1,
      "contentValue": "https://cdn.example.com/images/foto-existente-atualizada.jpg"
    }
  ]
}
```

**Request Example:**
```http
PUT /website/1/page/10/content/area
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "contentSlug": "gallery",
  "items": [
    {
      "contentId": 0,
      "contentType": "image",
      "index": 0,
      "contentValue": "https://cdn.example.com/images/nova-foto.jpg"
    },
    {
      "contentId": 101,
      "contentType": "image",
      "index": 1,
      "contentValue": "https://cdn.example.com/images/foto-existente-atualizada.jpg"
    }
  ]
}
```

**Response Success (200):**
```json
[
  {
    "contentId": 103,
    "websiteId": 1,
    "pageId": 10,
    "contentType": "image",
    "index": 0,
    "contentSlug": "gallery",
    "contentValue": "https://cdn.example.com/images/nova-foto.jpg",
    "createdAt": "2026-03-30T11:00:00",
    "updatedAt": "2026-03-30T11:00:00"
  },
  {
    "contentId": 101,
    "websiteId": 1,
    "pageId": 10,
    "contentType": "image",
    "index": 1,
    "contentSlug": "gallery",
    "contentValue": "https://cdn.example.com/images/foto-existente-atualizada.jpg",
    "createdAt": "2026-02-05T11:00:00",
    "updatedAt": "2026-03-30T11:00:00"
  }
]
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 24. Criar Conteúdo

Cria um novo bloco de conteúdo. Valida propriedade do website.

**Endpoint:** `POST /website/{websiteId}/page/{pageId}/content`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página

**Request Body:**
```json
{
  "contentType": "text",
  "index": 0,
  "contentSlug": "footer-text",
  "contentValue": "© 2026 Meu Portfolio. Todos os direitos reservados."
}
```

**Request Example:**
```http
POST /website/1/page/10/content
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "contentType": "text",
  "index": 0,
  "contentSlug": "footer-text",
  "contentValue": "© 2026 Meu Portfolio. Todos os direitos reservados."
}
```

**Response Success (201):**
```json
{
  "contentId": 104,
  "websiteId": 1,
  "pageId": 10,
  "contentType": "text",
  "index": 0,
  "contentSlug": "footer-text",
  "contentValue": "© 2026 Meu Portfolio. Todos os direitos reservados.",
  "createdAt": "2026-03-30T11:30:00",
  "updatedAt": "2026-03-30T11:30:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 25. Atualizar Conteúdo

Atualiza um bloco de conteúdo existente. Valida propriedade do website.

**Endpoint:** `PUT /website/{websiteId}/page/{pageId}/content/{contentId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página
- `contentId` (long, required) - ID do conteúdo

**Request Body:**
```json
{
  "contentType": "text",
  "index": 0,
  "contentSlug": "footer-text",
  "contentValue": "© 2026 Meu Portfolio - Atualizado"
}
```

**Request Example:**
```http
PUT /website/1/page/10/content/104
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: application/json

{
  "contentType": "text",
  "index": 0,
  "contentSlug": "footer-text",
  "contentValue": "© 2026 Meu Portfolio - Atualizado"
}
```

**Response Success (200):**
```json
{
  "contentId": 104,
  "websiteId": 1,
  "pageId": 10,
  "contentType": "text",
  "index": 0,
  "contentSlug": "footer-text",
  "contentValue": "© 2026 Meu Portfolio - Atualizado",
  "createdAt": "2026-03-30T11:30:00",
  "updatedAt": "2026-03-30T12:00:00"
}
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 26. Deletar Conteúdo

Remove um bloco de conteúdo. Valida propriedade do website.

**Endpoint:** `DELETE /website/{websiteId}/page/{pageId}/content/{contentId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website
- `pageId` (long, required) - ID da página
- `contentId` (long, required) - ID do conteúdo

**Request Example:**
```http
DELETE /website/1/page/10/content/104
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
```

**Response Success (204):**
```
No Content
```

**Response Error (401):**
```json
"Not Authorized"
```

---

## Image

### 27. Upload de Imagem

Faz upload de uma imagem para o bucket S3 do tenant.

**Endpoint:** `POST /image/upload`

**Authentication:** Required

**Request Headers:**
- `X-Tenant-Id` (string, required) - Identificador do tenant (define o bucket S3)

**Request Example:**
```http
POST /image/upload
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: multipart/form-data; boundary=----FormBoundary

------FormBoundary
Content-Disposition: form-data; name="file"; filename="banner.jpg"
Content-Type: image/jpeg

<binary file data>
------FormBoundary--
```

**Response Success (200):**
```json
{
  "url": "https://s3.amazonaws.com/tenant-abc-bucket/uploads/banner-a1b2c3d4.jpg"
}
```

**Response Error (400):**
```json
"File is required"
```

**Response Error (401):**
```json
"Not Authorized"
```

---

### 28. Upload de Logo do Website

Faz upload de um logotipo e o associa ao website. Valida propriedade do website.

**Endpoint:** `POST /image/upload/logo/{websiteId}`

**Authentication:** Required

**Path Parameters:**
- `websiteId` (long, required) - ID do website

**Request Headers:**
- `X-Tenant-Id` (string, required) - Identificador do tenant

**Request Example:**
```http
POST /image/upload/logo/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: tenant-abc
Content-Type: multipart/form-data; boundary=----FormBoundary

------FormBoundary
Content-Disposition: form-data; name="file"; filename="logo.png"
Content-Type: image/png

<binary file data>
------FormBoundary--
```

**Response Success (200):**
```json
{
  "url": "https://s3.amazonaws.com/tenant-abc-bucket/uploads/logo-e5f6g7h8.png"
}
```

**Response Error (400):**
```json
"File is required"
```

**Response Error (401):**
```json
"Not Authorized"
```

**Response Error (404):**
```json
null
```
