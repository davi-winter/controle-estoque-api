<strong>Título:</strong> API para Controle de Estoque

<strong>## Descrição:</strong> 
<p>
Trata-se de uma Web API para gerenciamento de um controle de estoque com funções CRUD desenvolvida com base na arquitetura <i>Clean Architecture</i>, demonstrando a 
segmentação dos projetos envolvidos nas camadas <i>Domain</i>, <i>Application</i>, <i>Infrastructure</i> e <i>Presentation</i>. Na camada de domínio, além das entidades,
contém as abstrações dos repositórios e serviços. Já na camada de aplicação foi utilizado o padrão tradicional de <i>Use Cases</i> em conjunto com DTOs para os 
<i>requests</i> e <i>responses</i>, na camada de infraestrutura contém a implementação dos repositórios e serviços, o mapeamento ORM das entidades, além de recursos de 
paginação, <i>cache</i>, <i>hash</i> para senha do usuário e um <i>seeder</i> (opcional) e por fim, na camada de apresentação (API) utilizou-se a abordagem com <i>Minimal API</i> 
contando com funcionalidades para geração de <i>token</i> e controle por <i>roles</i> para os usuários da API.

<strong>## Funções da API:</strong>
<p>
# Categorias:
<img width="1456" height="389" alt="image" src="https://github.com/user-attachments/assets/dcbe39a5-3488-475d-a7a3-aa85a4b6609f" />
1. <i>CreateCategory</i> (POST: "/api/categories") - Cria uma categoria<p>
2. <i>GetAllCategories</i> (GET: "/api/categories") - Consulta as categorias<p>
3. <i>UpdateCategory</i> (PUT: "/api/categories/{id}") - Edita uma categoria<p>
4. <i>DeleteCategory</i> (DELETE: "/api/categories/{id}") - Exclui uma categoria<p>
5. <i>GetByCategoryId</i> (GET: "/api/categories/{id}") - Consulta uma categoria pelo seu id<p>
6. <i>ChanceStatusCategory</i> (PATCH: "/api/categories/{id}/status") - Ativa ou desativa uma categoria<p>

<p>
# Produtos:
<img width="1459" height="494" alt="image" src="https://github.com/user-attachments/assets/30a3c1bd-9eb2-444b-a055-be2845624f22" />
1. <i>CreateProduct</i> (POST: "/api/products") - Cria um produto<p>
2. <i>UpdateProduct</i> (PUT: "/api/product/{id}") - Edita um produto<p>
3. <i>DeleteProduct</i> (DELETE: "/api/products/{id}") - Exclui uma produto<p>
4. <i>ChanceStatusProduct</i> (PATCH: "/api/product/{id}/status") - Ativa ou desativa um produto<p>
5. <i>GetBySku</i> (GET: "/api/products/{sku}") - Consulta um produto pelo seu código de estoque SKU<p>
6. <i>GetLowStockProducts</i> (GET: "/api/products/low-stock") - Consulta os produtos com estoque baixo<p>
7. <i>GetProductsByCategoryId</i> (GET: "/api/products/products-by-category/{categoryId}") - Consulta os produtos de uma categoria<p>
8. <i>GetProductsByName</i> (GET: "/api/products/products-by-name/{name}") - Consulta os produtos por nome<p>

<p>
# Movimentação de Estoque:
<img width="1451" height="329" alt="image" src="https://github.com/user-attachments/assets/67e0d9c6-fd59-43d5-958a-8362691f9e2d" />
1. <i>CreateStockMovement</i> (POST: "/api/stock-movements") - Cria uma movimentação no estoque<p>
2. <i>GetStockMovements</i> (GET: "/api/stock-movements") - Consulta as movimentações de estoque<p>
3. <i>GetHistoryByProductId</i> (GET: "/api/stock-movements/get-history-by-product-id/{productId}") - Consulta as movimentações de estoque pelo id do produto<p>
4. <i>GetHistoryByUserId</i> (GET: "/api/stock-movements/get-history-by-user-id/{userId}") - Consulta as movimentações de estoque pelo id do usuário<p>
5. <i>GetHistoryByPeriod</i> (GET: "/api/stock-movements/get-history-by-period") - Consulta as movimentações de estoque de um período<p>

<p>
# Usuários:
<img width="1461" height="503" alt="image" src="https://github.com/user-attachments/assets/09eefb68-5a40-4a9d-b318-a17b4c36bac7" />
1. <i>Login</i> (POST: "/api/users/login") - Realiza o login do usuário<p>
2. <i>ChangePasswordUser</i> (POST: "/api/users/change-password") - Realiza a alteração de senha do usuário (primeiro acesso)<p>
3. <i>CreateUser</i> (POST: "/api/users") - Cria um usuário<p>
4. <i>GetAllUsers</i> (GET: "/api/users") - Consulta os usuários<p>
5. <i>UpdateUser</i> (PUT: "/api/users/{id}") - Edita um usuário<p>
6. <i>DeleteUser</i> (DELETE: "/api/users/{id}") - Exclui um usuário<p>
7. <i>GetByUsername</i> (GET: "/api/users/username/{username}") - Consulta o usuário pelo nome de usuário<p>
8. <i>GetByEmail</i> (GET: "/api/users/email/{email}") - Consulta o usuário pelo e-mail<p>

<strong>## Pré-requisitos e instalação:</strong>
<p>
1. Instalar o <i>Visual Studio</i> ou <i>VS Code</i><p>
2. Instalar o <i>SQL Server</i><p>
3. Pacotes:<p>
	3.1. Camada de API: <i>Microsoft.AspNetCore.Authentication.JwtBearer</i> (geração do JWT Token), <i>Microsoft.EntityFrameworkCore.Design, Scrutor</i> (para a aplicação do <i>Decorator Pattern</i> usado para as classes de cacheamento) e <i>Swashbuckle.AspNetCore</i> (para uso das ferramentas <i>Swagger</i> para manipulação da API)<p>
	3.2. Camada de Aplicação: <i>FluentValidation.DependencyInjectionExtensions</i> (para simplificar a DI dos <i>Use Cases</i> no <i>builder</i> da API), <i>SecureIdentity</i> (usado para "hasheamento" da senha do usuário)<p>
	3.3. Camada de Infraestrutura: <i>Bogus</i> (usado para o <i>seeder</i>), <i>Microsoft.AspNetCore.Authentication</i> (para possibilitar o uso de <i>token</i>), <i>Microsoft.EntityFrameworkCore.Design</i>, <i>Microsoft.EntityFrameworkCore.Proxies</i> (para habilitar o uso de <i>Lazy Loading</i> e marcação de algumas propriedades de navegação como "virtual"), <i>Microsoft.EntityFrameworkCore.SqlServer</i>, <i>System.Linq.Dynamic.Core</i> e <i>System.Security.Cryptography.Xml</i> (para contornar vulnerabilidades de segurança)<p>

<p>
4. Como executar:<p>
	4.1. Clone o repositório:<p>
		4.1.1. <i>"git clone https://github.com/davi-winter/controle-estoque-api"</i><p>
	4.2. Entre na pasta do projeto:<p>
		4.2.1. <i>"cd controle-estoque-api"</i><p>
	4.3. Restaure os pacotes e execute o projeto:<p>
		4.3.1. <i>"dotnet restore"</i><p>
		4.3.2. <i>"dotnet run"</i><p>

<p>		
5. Para geração da <i>Migration</i> e do banco <i>SQL Server</i>:<p>
	5.1. Aplicar o caminho do banco na tag <i>DefaultConnection</i> do arquivo <i>appsettings.json</i> na camada da API<p>
	5.2. Executar o comando <i>"dotnet ef migrations add InitialCreate --project InventoryControl.Infrastructure --startup-project InventoryControl.API"</i><p>
	5.3. Executar o comando <i>"dotnet ef database update --project InventoryControl.Infrastructure --startup-project InventoryControl.API"</i> (abordagem <i>Code First</i>)<p>

<p>
6. Demais observações:<p>
	6.1. Por padrão, o banco será populado com dados fictícios através das funcionalidades do pacote <i>Bogus</i>, mas caso deseje inserir dados manualmente basta comentar o bloco de código no arquivo
	<i>Program.cs</i> da figura abaixo antes de rodar a aplicação pela primera vez.
	<img width="593" height="107" alt="image" src="https://github.com/user-attachments/assets/530d502d-04a7-42fa-958f-cf6d976c1b35" />
	<p>
	Neste caso, para iniciar o uso via API é necessário criar obrigatoriamente ao menos um usuário (tabela <i>User</i>) direto no gerenciador do BD com <i>role</i> "admin" e alguma senha previamente "hasheada" com a
	função <i>PasswordHasher.Hash(senha_desejada)</i> do pacote <i>SecureIdentity</i>
	<p>
	6.2. Para fazer uso da API basta fazer o login no endpoint <i>"/api/users/login"</i>, pegar no gerenciador de BD de seu uso algum e-mail num dos registros da tabela <i>User</i> de um dos usuários inseridos 
	no <i>seeder</i> e aplicar a senha padrão "P@ssw0rd".<p>
	6.3. Para saber quais as permissões que os usuários possuem através de seus <i>roles</i>, basta verificar nos métodos HTTP dos endpoints das entidades envolvidas, na extensão <i>.RequireAuthorization(p => p.RequireRole("nome_do_role"))</i>.<p>
	6.4. Na criação de um usuário (permitido apenas para o <i>role</i> "admin") o <i>response</i> retornará uma senha temporária que deverá ser alterada no endpoint de alteração de senha para posteriormente ser possível realizar o login.

<strong>## Tech Stack:</strong>

Linguagem: C# 14<p>
Back-end: .NET 10 com ORM <i>Entity Framework</i>, <i>ASP.NET Core Web API</i> com <i>OpenAPI</i> via <i>Swagger</i>, <i>Use Cases</i> com DTOs de <i>requests</i> e <i>responses</i>, <i>JWT Bearer</i> para geração de <i>token</i> com aplicação de <i>Claims</i> (<i>roles</i> do usuário).<p>
Banco de dados: <i>SQL Server</i><p>
Front-end: <i>Swagger UI</i><p>
Arquitetura: <i>Clean Architecture</i> com abordagem de alguns padrões como <i>Repository Pattern</i> para separação entre as abstrações e implementações em conjunto com recurso de <i>Generics</i> para generalização de funções CRUD para entidades diferentes, <i>Result Pattern</i> para tratamento de erros, <i>Decorator Pattern</i> para interceptação entre consultas originais e cache e <i>Unit Of Work</i> para gerenciamento das transações.

