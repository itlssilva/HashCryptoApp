# HashCryptoApp

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 21.1.4.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

---

## Sobre o Projeto

Este é um projeto **Angular 17+ com SSR (Server-Side Rendering)**, focado em operações de hash e criptografia.

### Estrutura dos Arquivos Principais

| Arquivo                        | Descrição                                                                                                                |
| ------------------------------ | ------------------------------------------------------------------------------------------------------------------------ |
| `src/main.ts`                  | Entry point do **cliente**. Inicializa a aplicação no navegador usando `bootstrapApplication`.                           |
| `src/main.server.ts`           | Entry point do **servidor**. Exporta a função `bootstrap` para renderização no lado do servidor (SSR).                   |
| `src/server.ts`                | Servidor **Node.js/Express**. Responsável por servir a aplicação e arquivos estáticos, escutando na porta 4000 (padrão). |
| `src/app/app.ts`               | Componente **raiz** da aplicação Angular.                                                                                |
| `src/app/app.routes.ts`        | Definição das **rotas** da aplicação.                                                                                    |
| `src/app/app.config.ts`        | Configuração principal da aplicação (providers, router, client hydration).                                               |
| `src/app/app.config.server.ts` | Configuração específica para **SSR**, mesclada com a config do cliente.                                                  |

### Fluxo da Estrutura

```
┌─────────────────────────────────────────────────────────────┐
│                     Entry Points                             │
├─────────────────────┬───────────────────────────────────────┤
│     main.ts         │         main.server.ts                │
│   (Browser)         │          (SSR)                         │
└─────────┬───────────┴───────────┬───────────────────────────┘
          │                       │
          ▼                       ▼
┌─────────────────────────────────────────────────────────────┐
│                    app/app.ts                                │
│              (Componente Raiz)                              │
└─────────────────────────────────────────────────────────────┘
          │                       │
          ▼                       ▼
┌─────────────────────────────────────────────────────────────┐
│                 app/app.config.ts                           │
│            (Configuração Principal)                         │
└─────────────────────────────────────────────────────────────┘
          │                       │
          ▼                       ▼
┌─────────────────────────────────────────────────────────────┐
│                    server.ts                                │
│              (Express Server - Porta 4000)                 │
└─────────────────────────────────────────────────────────────┘
          │
          ▼
┌─────────────────────────────────────────────────────────────┐
│              /browser (Arquivos Estáticos)                 │
└─────────────────────────────────────────────────────────────┘
```

### Entendendo Cada Arquivo

- **main.ts**: Inicia a aplicação no navegador. Sem ele, a app não carrega no client.
- **main.server.ts**: Permite que o Angular renderize a página no servidor antes de enviá-la ao cliente (melhor SEO e performance inicial).
- **server.ts**: Criado automaticamente pelo Angular SSR. Gerencia requisições, serve arquivos estáticos e integra o Angular no Node.js.
- **app.ts**: Contém a lógica do componente raiz e o template principal.
- **app.config.ts**: Define providers globais (router, hydration, etc.).
- **app.config.server.ts**: Configura o ambiente de renderização no servidor.
