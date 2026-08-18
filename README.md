<strong>Título:</strong> API para Controle de Estoque

<strong>Descrição:</strong> 
<p>
Trata-se de uma Web API para gerenciamento de um controle de estoque com funções CRUD desenvolvida com base na arquitetura 
Clean Architecture, demonstrando a segmentação dos projetos envolvidos nas camadas de Domínio, Application, Infrastructure e Api. Na camada de domínio 
contém as abstrações dos repositórios e serviços. Já na camada de aplicação foi utilizado o padrão tradicional de Use Cases em conjunto com DTOs para 
os requests e responses, na camada de infraestrutura contém a implementação dos repositórios e serviços, o mapeamento ORM das entidades, Unit Of Work 
para gerenciamento de transações, além de recursos de paginação, cache e seeder (opcional) e por fim na camada de API utilizou-se a abordagem com Minimal 
API contando com funcionalidades para geração de token e controle por roles para os usuários da API.

<strong>Funções da API:</strong>
<p>
# Categorias:
<img width="1456" height="389" alt="image" src="https://github.com/user-attachments/assets/dcbe39a5-3488-475d-a7a3-aa85a4b6609f" />
1. CreateCategory (POST: "/api/categories") - Cria uma categoria<p>
2. GetAllCategories (GET: "/api/categories") - Consulta as categorias<p>
3. UpdateCategory (PUT: "/api/categories/{id}") - Edita uma categoria<p>
4. DeleteCategory (DELETE: "/api/categories/{id}") - Exclui uma categoria<p>
5. GetByCategoryId (GET: "/api/categories/{id}") - Consulta uma categoria pelo seu id<p>
6. ChanceStatusCategory (PATCH: "/api/categories/{id}/status") - Ativa ou desativa uma categoria<p>

<p>
# Produtos:
<img width="1459" height="494" alt="image" src="https://github.com/user-attachments/assets/30a3c1bd-9eb2-444b-a055-be2845624f22" />
1. CreateProduct (POST: "/api/products") - Cria um produto<p>
2. UpdateProduct (PUT: "/api/product/{id}") - Edita um produto<p>
3. DeleteProduct (DELETE: "/api/products/{id}") - Exclui uma produto<p>
4. ChanceStatusProduct (PATCH: "/api/product/{id}/status") - Ativa ou desativa um produto<p>
5. GetBySku (GET: "/api/products/{sku}") - Consulta um produto pelo seu código de estoque SKU<p>
6. GetLowStockProducts (GET: "/api/products/low-stock") - Consulta os produtos com estoque baixo<p>
7. GetProductsByCategoryId (GET: "/api/products/products-by-category/{categoryId}") - Consulta os produtos de uma categoria<p>
8. GetProductsByName (GET: "/api/products/products-by-name/{name}") - Consulta os produtos por nome<p>

<p>
# Movimentação de Estoque:
<img width="1451" height="329" alt="image" src="https://github.com/user-attachments/assets/67e0d9c6-fd59-43d5-958a-8362691f9e2d" />
1. CreateStockMovement (POST: "/api/stock-movements") - Cria uma movimentação no estoque<p>
2. GetStockMovements (GET: "/api/stock-movements") - Consulta as movimentações de estoque<p>
3. GetHistoryByProductId (GET: "/api/stock-movements/get-history-by-product-id/{productId}") - Consulta as movimentações de estoque pelo id do produto<p>
4. GetHistoryByUserId (GET: "/api/stock-movements/get-history-by-user-id/{userId}") - Consulta as movimentações de estoque pelo id do usuário<p>
5. GetHistoryByPeriod (GET: "/api/stock-movements/get-history-by-period") - Consulta as movimentações de estoque de um período<p>

<p>
# Usuários:
<img width="1461" height="503" alt="image" src="https://github.com/user-attachments/assets/09eefb68-5a40-4a9d-b318-a17b4c36bac7" />
1. Login (POST: "/api/users/login") - Realiza o login do usuário<p>
2. ChangePasswordUser (POST: "/api/users/change-password") - Realiza a alteração de senha do usuário (primeiro acesso)<p>
3. CreateUser (POST: "/api/users") - Cria um usuário<p>
4. GetAllUsers (GET: "/api/users") - Consulta os usuários<p>
5. UpdateUser (PUT: "/api/users/{id}") - Edita um usuário<p>
6. DeleteUser (DELETE: "/api/users/{id}") - Exclui um usuário<p>
7. GetByUsername (GET: "/api/users/username/{username}") - Consulta o usuário pelo nome de usuário<p>
8. GetByEmail (GET: "/api/users/email/{email}") - Consulta o usuário pelo e-mail<p>

<strong>Pré-requisitos e instalação:</strong>

1. Instalar o Visual Studio<p>
2. Instalar o SQL Server<p>
3. Pacotes:<p>
	3.1. Camada Api: Microsoft.AspNetCore.Authentication.JwtBearer (geração do JWT Token), Microsoft.EntityFrameworkCore.Design, Scrutor (para a aplicação do Decorator Pattern usado no cacheamento) e Swashbuckle.AspNetCore (para uso das ferramentas Swagger para manipulação da API)<p>
	3.2. Camada Application: FluentValidation.DependencyInjectionExtensions, SecureIdentity (usado para hashamento da senha do usuário)<p>
	3.3. Camada Infrastructure: Bogus (usado para o seeder), Microsoft.AspNetCore.Authentication, Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.Proxies, Microsoft.EntityFrameworkCore.SqlServer,
   System.Linq.Dynamic.Core e System.Security.Cryptography.Xml<p>
4. Para geração da Migration e do banco SQL Server:<p>
  4.1. Aplicar o caminho do banco na tag "DefaultConnection" do assembly <i>appsettings.json</i> na camada da API
  4.2. Executar o comando "dotnet ef migrations add InitialCreate --project InventoryControl.Infrastructure --startup-project InventoryControl.API"<p>
	4.3. Executar o comando "dotnet ef database update --project InventoryControl.Infrastructure --startup-project InventoryControl.API" (abordagem Code First)<p>
5. Demais observações:<p>
  5.1. Por padrão, o banco será populado com dados fictícios através das funcionalidades do pacote Bogus, mas caso deseje inserir dados manualmente basta comentar o bloco de código no assembly
   <i>Program.cs</i> da figura abaixo.
   <img width="593" height="107" alt="image" src="https://github.com/user-attachments/assets/530d502d-04a7-42fa-958f-cf6d976c1b35" />
   <p>
   Neste caso, para iniciar o uso via API é necessário criar obrigatoriamente ao menos um usuário (tabela User) direto no gerenciador do BD com role "admin" e alguma senha previamente "hashada" com a
   função PasswordHasher.Hash(senha_desejada)
  5.2. Para fazer uso da API basta fazer o login no endpoint "/api/users/login", pegar no gerenciador de BD de seu uso algum e-mail num dos registros da tabela User de um dos usuários inseridos no seeder e
  aplicar a senha padrão "P@ssw0rd".


<strong>Tech Stack:</strong>

Linguagem: C#<p>
Back-end: .NET com ORM Entity Framework, WebAPI com OpenAPI via Swagger, Use Cases com DTOs de requests e responses, JWT Bearer para geração de token com aplicação de Claims (roles do usuário)<p>
Banco de dados: SQL Server<p>
Front-end: Swagger UI<p>
Arquitetura: Clean Architecture com uso de padrões Use Cases e Unit Of Work<p>

