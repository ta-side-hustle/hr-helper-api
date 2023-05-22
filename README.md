# `HR helper API`

## Бэкэнд для приложения HR helper

![Last commit](https://badgen.net/github/last-commit/ta-side-hustle/hr-helper-api/master)
![Latest tag](https://badgen.net/github/tag/ta-side-hustle/hr-helper-api/)
[![Publish Docker image](https://github.com/ta-side-hustle/hr-helper-api/actions/workflows/build-docker-image.yml/badge.svg?branch=master)](https://github.com/ta-side-hustle/hr-helper-api/actions/workflows/build-docker-image.yml)
![Docker version](https://badgen.net/docker/metadata/version/tinymosi/hr-helper-api/latest/arm64/v8)
![Docker size](https://badgen.net/docker/size/tinymosi/hr-helper-api)

## Предварительные требования
- [.NET SDK ^7.0.X](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) - sdk для разработки. Установщик по ссылке или через командную строку:
```shell
winget install Microsoft.DotNet.SDK.7
```
- dotnet CLI (идет вместе с SDK) - инструменты командной строки для сборки приложения
- [dotnet CLI EF tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) - инструменты командной строки для управления миграциями
```shell
dotnet tool install --global dotnet-ef
```
- [SQL Server Express LocalDB](https://learn.microsoft.com/ru-ru/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16)

## Локальный запуск
```shell
git clone git@github.com:ta-side-hustle/hr-helper-api.git
```

```shell
cd hr-helper-api
```

Восстановление NuGet пакетов:
```shell
dotnet restore
```

Для запуска в Rider:
```shell
rider .
```
или для запуска в Visual studio:
```shell
devenv ./hr-helper-api.sln
```

## Устройство проекта

Проект построен на [чистой архитектуре](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture).
Состоит из трех слоев в основе которых принцип инверсии зависимостей и domain-driven design.

- Core layer - корень приложения, описывает что есть в приложении и как оно должно работать
  - Domain
    - бизнес-модели
    - доменные исключения
  - Application
    - правила и спецификации реализации
    - DTO объекты и объекты агрегаций
- Infrastructure layer - опирается на Core layer для реализации контрактов
  - Infrastructure
    - реализация контрактов приложения
    - контекст доступа к данным
    - миграции
    - интеграции с внешними сервисами
- UI layer - опирается на Application и взаимодействует с сервисами посредством публичных контрактов (ссылается на инфраструктуру только для инъекции зависимостей)
  - Api
    - контроллеры
    - фильтры
    - обработчики
  - Vue Nuxt 3 application
    - клиент конечного пользователя

## База данных и миграции

Для подключения к базе данных в проекте используется провайдер для ms sql server.

Доступ к данным и выполнение запросов реализуется средствами Entity Framework Core и LINQ-запросов.

Миграции - это файлы хранящие все изменения вносимые в схему БД.
Они позволяют поддерживать актуальную схему БД среди всех разработчиков и версионировать её через git.

Для первого запуска проекта нужно воссоздать уже существующую схему БД. Это делается посредством применения существующих миграций:

```shell
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project Api/Api.csproj --context Infrastructure.Database.ApplicationDbContext
```

Если вы внесли изменения в схему БД нужно создать новую миграцию:

```shell
dotnet ef migrations add --project Infrastructure/Infrastructure.csproj --startup-project Api/Api.csproj --context Infrastructure.Database.ApplicationDbContext <migration_name> --output-dir Database/Migrations
```

, где `<migration_name>` - название миграции.

После создания миграции в папке `Infrastructure/Database/Migrations` появится новый файл (`<datetime>_<migration_name>.cs`) в котором будут отображены применяемые этой миграцией изменения.

Если все выглядит в порядке, то для применения новой схемы нужно снова выполнить команду:

```shell
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project Api/Api.csproj --context Infrastructure.Database.ApplicationDbContext
```

## Работа с секретами

Для запуска приложению требуется дополнительная информация для доступа к некоторым ресурсам.
Обычно это приватная информация (пароли, ключи доступа, сертификаты) которую неразумно хранить в публичном git репозитории, поэтому используются локальные секреты.

Для локальной разработки можно использовать .NET User secrets или appsettings.Development.json. 

Самый простой вариант это json файл.
В репозитории уже присутствует этот файл с тестовыми данными,
туда нужно будет добавить строку подключения к БД,
которую вы получите после установки локального сервера SQL Express.

В боевой среде приложение считывает сереты из переданного ей окружения.

## Сборка и отладка

Сбоку и отладку проекта можно производить пользуясь инструментами IDE или используя командную строку

Для сборки проекта используется MSBuild устанавливаемый вместе с SDK:
```shell
dotnet build
```

Запуск проекта в режиме отладки:
```shell
dotnet run --project ./Api
```

---

## Docker

Если разбираетесь в докере, то БД можно добавить контейнером в `docker-compose.override.yml`, вместо локальной установки.

- build local image:

```shell
docker build --no-cache --target final -f Api/Dockerfile -t hr-helper-api:dev .
```

- multiplatform build

```shell
docker buildx build --file Api/Dockerfile --platform linux/amd64,linux/arm/v7,linux/arm64/v8 --tag hr-helper-api:dev .
```

### Compose
- dev:

```shell
docker compose -f docker-compose.yml docker-compose.override.yml up -d --build --force-recreate --remove-orphans
```

- stop:

```shell
docker compose down --volumes
```
