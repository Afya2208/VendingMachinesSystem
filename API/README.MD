# VendingMachinesSystem API
RESTful Web-API ASP.NET Core 9 для системы франчайзинга вендинговых торговых автоматов.

Функционал для франчайзера или франчайзи доступен только после авторизации с токеном JWT.

Настройка подключения к базе данных и JWT производится через файл appsettings.json

СУБД PostgreSQL
ORM Entity Framework Core 9

## Использованные библиотеки
| Название                                      | Версия |
|-----------------------------------------------|--------|
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.9  |
| Microsoft.AspNetCore.OpenApi                  | 9.0.9  |
| Swashbuckle.AspNetCore                        | 9.0.6  |

### Библиотеки у проекта Models (Entities, DTO)
| Название                              | Версия |
|---------------------------------------|--------|
| Microsoft.EntityFrameworkCore.Design  | 9.0.9  |
| Microsoft.EntityFrameworkCore.Tools   | 9.0.9  |
| Npgsql.EntityFrameworkCore.Postgresql | 9.0.4  |
