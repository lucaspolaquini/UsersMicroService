# UsersMicroService

Microserviço de Usuários com reconhecimento facial para a aplicação GeekBurger
O serviço pode ser encontrado na url: http://geekburgerusers.azurewebservices.net/
Os métodos disponíveis estão em http://geekburgerusers.azurewebservices.net/swagger/v1/swagger.json

## Decisões

- Será considerado que o serviço estará "AlwaysOn" e portanto os dados ficarão sempre na memória. 
- Isso foi decidido por ser um serviço só de exemplo feito com o objetivo didático de entender o conceito de "Microserviços", não tendo o foco no acesso a dados.
- O contrato foi publicado no Nuget para consumo do serviço e recebimento de mensagens.
- Um exemplo de como chamar o método do serviço pode ser encontrado nos testes integrados.