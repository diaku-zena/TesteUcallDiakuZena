# Sistema de Gestão de Pedidos com RabbitMQ

Este é um sistema simples de gestão de pedidos com processamento assíncrono utilizando RabbitMQ para enfileiramento e processamento de mensagens. O sistema permite a criação de pedidos, consulta de status e é escalável para lidar com grandes quantidades de dados.

## Requisitos

Antes de executar o sistema, certifique-se de que você tem os seguintes requisitos instalados:

- **.NET 6.0 ou superior**: [Instalar .NET](https://dotnet.microsoft.com/download)
- **RabbitMQ**: [Instalar RabbitMQ](https://www.rabbitmq.com/download.html) ou use um serviço RabbitMQ hospedado (exemplo: [CloudAMQP](https://www.cloudamqp.com/)).
- **SQL Server ou outro banco de dados**: O sistema utiliza um banco de dados relacional (exemplo: SQL Server) para armazenar os pedidos e seus status.

## Configuração do Ambiente

1. Clone o repositório:

   ```bash
   git clone https://github.com/seu-usuario/sistema-de-pedidos.git
   cd sistema-de-pedidos
