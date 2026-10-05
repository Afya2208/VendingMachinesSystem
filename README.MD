# VendingMachinesSystem

Это информационная система франшизы вендинговых торговых автоматов.

В систему входят:
- База данных PostgreSQL - [/Models (Entities, DTO)](https://github.com/Afya2208/VendingMachinesSystem/tree/main/Models)
- Web-API ASP.NET Core - [/API](https://github.com/Afya2208/VendingMachinesSystem/tree/main/API)
- Настольное приложение на Avalonia личного кабинета франчайзера для учета вендинговых автоматов и компаний-партнеров - [/DesktopApp](https://github.com/Afya2208/VendingMachinesSystem/tree/main/DesktopApp)
- Мобильное приложение на Kotlin для обслужающего персонала торговых автоматов - [/MobileApp](https://github.com/Afya2208/VendingMachinesSystem/tree/main/MobileApp)
- Веб-сайт на Next.js личного кабинета франчайзи для аренды торговых автоматов - [/website](https://github.com/Afya2208/VendingMachinesSystem/tree/main/website)

Тесты на XUnit (Selenium, Headless Avalonia) - [/Tests](https://github.com/Afya2208/VendingMachinesSystem/tree/main/Tests)

UML диаграммы, бизнес-анализ - [/Business analysis, diagrams](https://github.com/Afya2208/VendingMachinesSystem/tree/main/Business%20analysis,%20diagrams)

Настольное приложение личного кабинета франчайзера позволяет управлять информацией о вендинговых торговых автоматах и компаниях-партнерах. На главной странице отображаются диаграммы и аналитика по состоянию ТА, продажам, сводка статистики и новостей. Есть разделы Торговые автоматы и Компании.

## Технологии
- **Фреймворки**: Avalonia, ASP.NET Core 9 (Web-API), Next.js, Jetpack Compose (Kotlin)
- **СУБД**: PostgreSQL 17
- **Работа с API**: Postman, Swagger
- **Работа с базой данных, ORM**: Entity Framework Core 9

## Скриншоты работы

#### Страница авторизации

![img.png](Apps%20Screenshots/d1.png)

#### Главная страница - личный кабинет с показателями

![img.png](Apps%20Screenshots/d2.png)

#### Главная страница - вторая половина

![img.png](Apps%20Screenshots/d3.png)

#### Торговые автоматы

![img.png](Apps%20Screenshots/d4.png)

#### Экспорт ТА в .pdf

![img.png](Apps%20Screenshots/d5.png)

#### Экспорт ТА в .html

![img.png](Apps%20Screenshots/d6.png)

#### Форма добавления и редактирования ТА

![img.png](Apps%20Screenshots/d7.png)

#### Компании

![img.png](Apps%20Screenshots/d8.png)

#### Форма добавления и редактирования компаний

![img.png](Apps%20Screenshots/d9.png)

#### Экспорт компаний в .csv

![img.png](Apps%20Screenshots/d10.png)

#### Экспорт компаний в .pdf

![img.png](Apps%20Screenshots/d11.png)

## Запуск приложений

Сначала необходимо скачать репозиторий:
```shell
git clone https://github.com/...
```

В API нужно настроить строку подключения в appsettings.json, указать имя для будущей базы данных

Восстановление базы данных внутри Models:
```shell
dotnet ef migrations add Init -s ../API
dotnet ef database update
```

Запуск Web-API (выполнить внутри папки API):
```shell
dotnet run
```

В appsettings.json внутри папки DesktopApp нужно указать адрес запущенного приложения Web API, обязательно в конце добавить символ `/`

Запуск настольного приложения (выполнить внутри папки DesktopApp):
```shell
dotnet run
```