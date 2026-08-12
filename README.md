# tracking-service

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-336791)](https://www.postgresql.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

REST API для учёта времени, потраченного на разные области подготовки: теория, алгоритмы, практика. Сессию можно записать задним числом или засечь таймером, а затем получить сводку по дням и темам.

## Стек

- .NET 8, ASP.NET Core Minimal API
- Entity Framework Core 8, Npgsql
- PostgreSQL 17
- FluentValidation
- Docker Compose

## Архитектура

Проект разделён на четыре слоя по принципам чистой архитектуры:

```
src/
├── Tracking.Domain          — сущности и бизнес-правила, без внешних зависимостей
├── Tracking.Application     — сценарии использования, интерфейсы репозиториев
├── Tracking.Infrastructure  — EF Core, репозитории, миграции
└── Tracking.Api             — HTTP-эндпоинты, композиция зависимостей
tests/
├── Tracking.UnitTests
└── Tracking.IntegrationTests
```

Зависимости направлены внутрь: `Domain` не знает ни о ком, `Application` знает только `Domain` и объявляет интерфейсы, `Infrastructure` их реализует. `Api` ссылается на `Infrastructure` только для регистрации зависимостей в composition root.

### Принятые решения

**Репозитории вместо `DbSet` за интерфейсом.** `Application` не ссылается на EF Core вообще — вместо этого объявляет `ISessionRepository` и `IUnitOfWork` в терминах домена. Это делает слой независимым от способа хранения и упрощает подмену в юнит-тестах, ценой необходимости добавлять метод под каждый новый запрос.

**Один класс — один эндпоинт.** Вместо контроллеров используется собственная абстракция `IEndpoint`: реализации находятся сканированием сборки и регистрируются автоматически. Каждый эндпоинт держит свои `Request`, `Response` и `Validator` в одном файле, поэтому фича читается целиком, без переходов между папками.

**Хранение времени.** Момент начала (`StartedAt`) хранится в UTC как `timestamptz`, локальный день занятия (`Date`) — отдельным полем типа `date`. Разделение нужно потому, что занятие поздним вечером по локальному времени попадает в следующие сутки UTC и иначе искажает суточную статистику. Длительность хранится явно в минутах: это позволяет записывать сессии задним числом, у которых нет времени начала и конца.

## Запуск

Требуется .NET 8 SDK и Docker.

```bash
# поднять базу
docker compose up -d

# применить миграции
dotnet ef database update \
  --project src/Tracking.Infrastructure \
  --startup-project src/Tracking.Api

# запустить API
dotnet run --project src/Tracking.Api
```

Swagger будет доступен на `https://localhost:5001/swagger`.

Строка подключения задаётся в `appsettings.Development.json` в секции `ConnectionStrings:Postgres`.

## API

| Метод | Маршрут | Описание |
|---|---|---|
| POST | `/api/v1/sessions` | Записать сессию занятия |

## Тесты

```bash
dotnet test
```

## Планы

- [ ] Таймер: старт и остановка сессии
- [ ] Сводка по дням и темам
- [ ] Пагинация и фильтрация списка сессий
- [ ] Кэширование сводок
- [ ] Аутентификация по JWT
- [ ] Фоновая обработка через очередь

## Лицензия

MIT