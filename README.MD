# VendingMachinesSystem

Это информационная система франшизы вендинговых торговых автоматов.

В систему входят:
- База данных PostgreSQL - [/Models (Entities, DTO)](https://github.com/Afya2208/VendingMachinesSystem/tree/master/Models)
- Web-API ASP.NET Core - [/API](https://github.com/Afya2208/VendingMachinesSystem/tree/master/API)
- Настольное приложение Avalonia франчайзера для учета вендинговых автоматов и компаний-партнеров - [/DesktopApp](https://github.com/Afya2208/VendingMachinesSystem/tree/master/DesktopApp)
- Мобильное приложение на Kotlin для обслужающего персонала торговых автоматов - [/MobileApp](https://github.com/Afya2208/VendingMachinesSystem/tree/master/MobileApp)
- Веб-сайт на Next.js личного кабинета франчайзи для аренды торговых автоматов - [/website](https://github.com/Afya2208/VendingMachinesSystem/tree/master/website)

Тесты на XUnit (Selenium, Headless Avalonia) - [/Tests](https://github.com/Afya2208/VendingMachinesSystem/tree/master/Tests)

UML диаграммы, бизнес-анализ - [/Business analysis, diagrams](https://github.com/Afya2208/VendingMachinesSystem/tree/master/Business%20analysis,%20diagrams)

Настольное приложение личного кабинета франчайзера позволяет управлять информацией о вендинговых торговых автоматах и компаниях-партнерах. На главной странице отображаются диаграммы и аналитика по состоянию ТА, продажам, сводка статистики и новостей. Есть разделы Торговые автоматы и Компании.

Пока система в процессе разработки, переписывается старый код. Планируется добавить:
- Настольное приложение:
  - Мониторинг ТА
- Веб-сайт (личный кабинет франчайзи):
  - Авторизация и регистрация с математической каптчей
  - Боковое меню с разделами
  - Поддержка многоязычности (английский)
  - Центр уведомлений
  - Раздел Торговые автоматы
  - Модальное окно выбора доступных ТА для аренды
  - Процесс бронирования выбранных ТА с календарем
  - Договоры с франчайзером - просмотр с помощью встроенного PDF-ридера
- Мобильное приложение (обслуживание ТА):
  - Авторизация инженера
  - Раздел заданий/заказов на обслуживание
  - Фото- и видеосъемка работы до и после, сохранение данных
  - Редактор фото - обрезка, поворот, рисование маркером, добавление аннотации к фото
  - Создание заметок - текстовый редактор, возможность прикрепить фото из галереи, голосовой ввод

## Технологии
- **Фреймворки**: Avalonia, ASP.NET Core 9 (Web-API), Next.js, Jetpack Compose (Kotlin)
- **СУБД**: PostgreSQL 17
- **Работа с API**: Postman, Swagger
- **Работа с базой данных, ORM**: Entity Framework Core 9

## Скриншоты работы

### Настольное приложение

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

### Веб-сайт

Пока в процессе разработки, переписывается старый код - [текущее состояние проекта website](https://github.com/Afya2208/VendingMachinesSystem/tree/master/website).

### Мобильное приложение

Пока в процессе разработки, переписывается старый код - [текущее состояние проекта MobileApp](https://github.com/Afya2208/VendingMachinesSystem/tree/master/MobileApp).




