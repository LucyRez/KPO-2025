# Занятие 16. Inbox и Outbox

## Теория

Паттерны Transactional Outbox и Inbox используются для повышения надежности асинхронных взаимодействий между сервисами.

### Transactional Outbox

Суть паттерна в том, чтобы при получении запроса в рамках единой транзакции:

- Выполнить бизнес-логику;
- Поставить в специальную очередь уведомления о внесенных изменениях, если они необходимы для других сервисов;
- Если удалось положить сообщение в очередь, то сообщить отправителю об успешной обработке запроса.

После этого, в фоновом режиме:

- Уведомления из очереди считываются и отправляются в другие сервисы;
- Если удалось отправить уведомление, то оно удаляется из очереди.

Таким образом мы можем гарантировать, что если транзакция завершится успешно, то и все уведомления будут доставлены рано или поздно.

### Transactional Inbox

Суть паттерна:

- При получении сообщения от друго сервиса складываем сообщение в специальную очередь;
- Если получилось сложить сообщение в очередь, то сообщаем отправителю об успешной обработке сообщения;
- Далее в фоновом режиме сообщение из очереди считывается и обрабатывается;
- Получилось обработать - убираем сообщение из очереди.

Таким образом мы гарантируем, что если сообщение попало в очередь, то оно будет обработано рано или поздно.

## Практика

На данном занятии мы рассмотрим пример реализации платежного сервиса с использованием паттернов Transactional Outbox и Transactional Inbox.

### Сценарии

Мы будем поддерживать следующие сценарии:

1. Заморозка средств на счету;
2. Перевод замороженных средств на другой счет;
3. Отмена заморозки средств на счету.

#### Заморозка средств на счету

Сценарий заморозки средств на счету будет состоять из следующих шагов:

1. Сервис платежей получает запрос на заморозку средств;
2. Сервис платежей проверяет, есть ли нужное количество средств на счету;
3. Если средств достаточно, то сервис платежей замораживает средства на счету;
4. Если удалось заморозить средства, то сервис создает запись о транзакции;
5. Если удалось создать запись о транзакции, то сервис создает задание на отправку уведомления пользователю;
6. Если задание успешно создано, то сервис платежей отправляет ответ на запрос.

При этом шаги 3-6 выполняются в рамках единой транзакции. Если хотя бы один из шагов не выполнился, то транзакция отменяется.

Далее в фоновом режиме:

1. Сервис платежей вычитывает задание на отправку уведомления пользователю;
2. Отправляет уведомление в сервис уведомлений;
3. Если удалось отправить уведомление, то задание удаляется из очереди.

#### Перевод замороженных средств на другой счет

Сценарий перевода замороженных средств на другой счет будет состоять из следующих шагов:

1. Сервис платежей получает запрос на перевод средств;
2. Сервис платежей проверяет, есть ли соответствующая запись о транзакции;
3. Если запись о транзакции есть, то сервис добавляет средства на целевой счет;
4. Если удалось добавить средства на целевой счет, то сервис помечает транзакцию как выполненную;
5. Если удалось пометить транзакцию как выполненную, то сервис создает задание на отправку уведомления пользователю;
6. Если задание успешно создано, то сервис платежей отправляет ответ на запрос.

При этом шаги 3-6 выполняются в рамках единой транзакции. Если хотя бы один из шагов не выполнился, то транзакция отменяется.

Далее в фоновом режиме:

1. Сервис платежей вычитывает задание на отправку уведомления пользователю;
2. Отправляет уведомление в сервис уведомлений;
3. Если удалось отправить уведомление, то задание удаляется из очереди.

#### Отмена заморозки средств на счету

1. Сервис платежей получает запрос на отмену заморозки средств;
2. Сервис платежей проверяет, есть ли соответствующая запись о транзакции;
3. Если запись о транзакции есть, то сервис платежей добавляет средства на исходный счет;
4. Если удалось добавить средства на исходный счет, то сервис платежей помечает транзакцию как отмененную;
5. Если удалось пометить транзакцию как отмененную, то сервис платежей создает задание на отправку уведомления пользователю;
6. Если задание успешно создано, то сервис платежей отправляет ответ на запрос.

При этом шаги 3-6 выполняются в рамках единой транзакции. Если хотя бы один из шагов не выполнился, то транзакция отменяется.

Далее в фоновом режиме:

1. Сервис платежей вычитывает задание на отправку уведомления пользователю;
2. Отправляет уведомление в сервис уведомлений;
3. Если удалось отправить уведомление, то задание удаляется из очереди.

#### Сервис уведомлений

Сервис уведомлений будет работать следующим образом:

1. Сервис уведомлений получает запрос на отправку уведомления;
2. Если такого уведомления еще нет в очереди, то добавляем его в очередь;
3. Если удалось добавить уведомление в очередь, то сообщаем сервису платежей об успешной обработке запроса.

Далее в фоновом режиме:

1. Сервис уведомлений вычитывает уведомление из очереди;
2. Отправляет уведомление пользователю;
3. Если удалось отправить уведомление, то уведомление удаляется из очереди.

### Архитектура

Как видно из сценариев, нам понадобится два сервиса:

- Сервис платежей;
- Сервис уведомлений.

Внутри сервиса платежей мы реализуем паттерн Transactional Outbox для гарантированной отправки уведомлений после выполнения операций.
Внутри сервиса уведомлений мы реализуем паттерн Transactional Inbox для гарантированной доставки уведомлений при получении запроса на их отправку.

### Реализация

#### Сервис платежей

В данном занятии мы сосредоточимся на самих паттернах, а не на архитектуре приложений - поэтому каждый из наших сервисов будет состоять всего из одного слоя. Начнем с сервиса платежей.

Для начала создадим решение для нашего приложения при помощи команд:

```bash
dotnet new sln -n PaymentGateway -o PaymentGateway
cd PaymentGateway
```

Теперь создадим проекты для сервиса платежей при помощи команд:

```bash
dotnet new webapi -n PaymentService -o PaymentService
dotnet new classlib -n PaymentService.Contracts -o PaymentService.Contracts
dotnet sln add PaymentService/
dotnet sln add PaymentService.Contracts/
dotnet add PaymentService/PaymentService.csproj reference PaymentService.Contracts/PaymentService.Contracts.csproj
```

Как можно заметить, мы сразу завели и проект самого сервиса, и проект контрактов. Проект контрактов будет использоваться для обеспечения взаимодействия между сервисами.

Сразу добавим в проект контрактов каталог `DTOs` и в нем классы для получения информации об изменениях на счету.

**TransactionStatus - статус транзакции:**

```csharp
namespace PaymentService.Contracts.DTOs;

/// <summary>
/// Статус транзакции
/// </summary>
public enum TransactionStatusType
{
    /// <summary>
    /// Заморозка средств на исходном счету
    /// </summary>
    Hold,
    /// <summary>
    /// Перевод средств на целевой счет
    /// </summary>
    Charge,
    /// <summary>
    /// Отмена транзакции
    /// </summary>
    Cancel,
}
```

**TransactionChangeDto - информация о транзакции:**

```csharp
namespace PaymentService.Contracts.DTOs;

/// <summary>
/// Информация о транзакции
/// </summary>
/// <param name="TransactionId">Идентификатор транзакции</param>
/// <param name="SubjectId">Идентификатор счета, над которым происходит транзакция</param>
/// <param name="PeerId">Идентификатор счета, с которым происходит транзакция</param>
/// <param name="Amount">Сумма транзакции</param>
/// <param name="Status">Статус транзакции</param>
public sealed record TransactionChangeDto(
    Guid TransactionId,
    Guid SubjectId,
    Guid PeerId,
    decimal Amount,
    TransactionStatusType Status,
    DateTimeOffset UpdatedAt);
```

С проектом контрактов закончили - теперь можно приступать к реализации сервиса платежей.

#### Сервис платежей

##### Модели

Начнем с определения моделей, с которыми будем работать - добавим каталог `Models` и в нем следующие классы.

**Account - счет:**

```csharp
namespace PaymentService.Models;

internal sealed class Account
{
    public Guid Id { get; }
    public decimal Balance { get; }
    public decimal Hold { get; }

    public Account(Guid id, decimal balance, decimal hold)
    {
        Id = id;
        Balance = balance;
        Hold = hold;
    }
}
```

**TransactionStatusType - тип статуса транзакции:**

```csharp
namespace PaymentService.Models;

/// <summary>
/// Тип статуса транзакции
/// </summary>
public enum TransactionStatusType
{
    /// <summary>
    /// Заморозка средств на исходном счету
    /// </summary>
    Hold,
    /// <summary>
    /// Перевод средств на целевой счет
    /// </summary>
    Charge,
    /// <summary>
    /// Отмена транзакции
    /// </summary>
    Cancel,
}
```

**TransactionStatus - статус транзакции:**

```csharp
namespace PaymentService.Models;

internal sealed class TransactionStatus
{
    public Guid Id { get; }
    public Guid TransactionId { get; }
    public TransactionStatusType Status { get; }
    public DateTimeOffset CreatedAt { get; }

    public TransactionStatus(Guid id, Guid transactionId, TransactionStatusType status, DateTimeOffset createdAt)
    {
        Id = id;
        TransactionId = transactionId;
        Status = status;
        CreatedAt = createdAt;
    }
}
```

**Transaction - транзакция:**

```csharp
namespace PaymentService.Models;

internal sealed class Transaction
{
    public Guid Id { get; }
    public Guid SubjectId { get; }
    public Guid PeerId { get; }
    public decimal Amount { get; }
    public DateTimeOffset CreatedAt { get; }

    public Transaction(Guid id, Guid subjectId, Guid peerId, decimal amount, DateTimeOffset createdAt)
    {
        Id = id;
        SubjectId = subjectId;
        PeerId = peerId;
        Amount = amount;
        CreatedAt = createdAt;
    }
}
```

**TransactionChangeNotification - уведомление о транзакции:**

```csharp
namespace PaymentService.Models;

internal sealed class TransactionChangeNotification
{
    public Guid Id { get; }
    public string Payload { get; }
    public bool IsSent { get; }
    public DateTimeOffset CreatedAt { get; }

    public TransactionChangeNotification(Guid id, string payload, bool isSent, DateTimeOffset createdAt)
    {
        Id = id;
        Payload = payload;
        IsSent = isSent;
        CreatedAt = createdAt;
    }
}
```

##### Хранение данных

Теперь настроим хранение данных в базе данных. Начнем с добавления зависимостей:

```bash
dotnet add PaymentService/PaymentService.csproj package Microsoft.EntityFrameworkCore.Design --version 8
dotnet add PaymentService/PaymentService.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 8
```

Теперь создадим каталог `Database` и в нем класс контекста базы данных:

```csharp
namespace PaymentService.Database;

internal sealed class PaymentContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }
    public DbSet<TransactionChangeNotification> TransactionChangeNotifications { get; set; }

    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.Balance)
                .HasColumnName("balance")
                .IsRequired();

            entity.Property(e => e.Hold)
                .HasColumnName("hold")
                .IsRequired();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.SubjectId)
                .HasColumnName("subject_id")
                .IsRequired();

            entity.Property(e => e.PeerId)
                .HasColumnName("peer_id")
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(e => e.SubjectId);

            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(e => e.PeerId);
            
            entity.HasMany<TransactionStatus>()
                .WithOne()
                .HasForeignKey(e => e.TransactionId);
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.TransactionId)
                .HasColumnName("transaction_id")
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>(
                    v => v.ToString(),
                    v => (TransactionStatusType)Enum.Parse(typeof(TransactionStatusType), v)
                )
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });

        modelBuilder.Entity<TransactionChangeNotification>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();
            
            entity.Property(e => e.Payload)
                .HasColumnName("payload")
                .IsRequired();
            
            entity.Property(e => e.IsSent)
                .HasColumnName("is_sent")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });
    }
}
```

В коде выше мы настроили маппинг всех трех моделей, а также настроили отношения между ними:

- У счета может быть много транзакций;
- У транзакции может быть много статусов.

Добавим фабрику для создания контекста, чтобы можно было сгенерировать миграции. Для этого в каталоге `Database` добавим файл `PaymentContextFactory.cs`:

```csharp
namespace PaymentService.Database;

internal sealed class PaymentContextFactory : IDesignTimeDbContextFactory<PaymentContext>
{
    public PaymentContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=PaymentGateway;Username=postgres;Password=postgres");

        return new PaymentContext(optionsBuilder.Options);
    }
}
```

Теперь сгенерируем миграции:

```bash
dotnet ef migrations add InitialCreate --project PaymentService/PaymentService.csproj
```

Далее добавим код для применения миграций. В каталоге `Database` добавим класс `MigrationRunner`:

```csharp
namespace PaymentService.Database;

internal sealed class MigrationRunner : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationRunner(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        context.Database.Migrate();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```

##### Отправка уведомлений

Далее создадим каталог `Common/SendNotification` и в нем интерфейс для отправки уведомлений - он понадобится нам во всех наших сценариях.

**ISendNotificationService:**

```csharp
namespace PaymentService.Common.SendNotification;

internal interface ISendNotificationService
{
    Task SendTransactionChangeNotificationAsync(Transaction transaction, TransactionStatusType status, CancellationToken cancellationToken);
}
```

Далее добавим реализацию интерфейса в классе `SendNotificationService` в том же каталоге.

**SendNotificationService:**

```csharp
namespace PaymentService.Common.SendNotification;

internal sealed class SendNotificationService : ISendNotificationService
{
    private readonly PaymentContext _context;

    public SendNotificationService(PaymentContext context)
    {
        _context = context;
    }
    
    public async Task SendTransactionChangeNotificationAsync(Transaction transaction, TransactionStatusType status, CancellationToken cancellationToken)
    {
        var payload = new TransactionChangeDto(
            transaction.Id,
            transaction.SubjectId,
            transaction.PeerId,
            transaction.Amount,
            ToContractStatus(status),
            transaction.CreatedAt
        );

        var payloadJson = JsonSerializer.Serialize(payload);

        var notification = new TransactionChangeNotification(
            Guid.NewGuid(),
            payloadJson,
            isSent: false,
            DateTimeOffset.UtcNow
        );

        await _context.TransactionChangeNotifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static ContractsStatus ToContractStatus(ModelsStatus status)
    {
        return status switch
        {
            ModelsStatus.Hold => ContractsStatus.Hold,
            ModelsStatus.Charge => ContractsStatus.Charge,
            ModelsStatus.Cancel => ContractsStatus.Cancel,
            _ => throw new ArgumentException($"Invalid transaction status: {status}", nameof(status))
        };
    }
}
```

##### Создание счетов

Теперь добавим в проект каталог `UseCases/CreateAccount` и в нем классы `CreateAccountRequest` и `CreateAccountResponse`.

**CreateAccountRequest:**

```csharp
namespace PaymentService.UseCases.CreateAccount;

public sealed record CreateAccountRequest(decimal Balance);
```

**CreateAccountResponse:**

```csharp
namespace PaymentService.UseCases.CreateAccount;

public sealed record CreateAccountResponse(Guid AccountId);
```

После этого в том же каталоге добавим интерфейс `ICreateAccountService`:

```csharp
namespace PaymentService.UseCases.CreateAccount;

public interface ICreateAccountService
{
    Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken);
}
```

После этого добавим реализацию интерфейса в классе `CreateAccountService`:

```csharp
namespace PaymentService.UseCases.CreateAccount;

internal sealed class CreateAccountService : ICreateAccountService
{
    private readonly PaymentContext _context;

    public CreateAccountService(PaymentContext context)
    {
        _context = context;
    }

    public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = new Models.Account(Guid.NewGuid(), hold: 0, balance: request.Balance);

        await _context.Accounts.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateAccountResponse(account.Id);
    }
}
```

##### Создание транзакций

Теперь добавим в проект каталог `UseCases/CreateTransaction` и в нем классы `CreateTransactionRequest` и `CreateTransactionResponse`.

**CreateTransactionRequest:**

```csharp
namespace PaymentService.UseCases.CreateTransaction;

public sealed record CreateTransactionRequest(Guid SubjectId, Guid PeerId, decimal Amount);
```

**CreateTransactionResponse:**

```csharp
namespace PaymentService.UseCases.CreateTransaction;

public sealed record CreateTransactionResponse(Guid TransactionId);
```

После этого добавим интерфейс `ICreateTransactionService`:

```csharp
namespace PaymentService.UseCases.CreateTransaction;

public interface ICreateTransactionService
{
    Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken);
}
```

Далее добавим реализацию интерфейса в классе `CreateTransactionService`:

```csharp
using TransactionModel = PaymentService.Models.Transaction;
using TransactionStatusModel = PaymentService.Models.TransactionStatus;

namespace PaymentService.UseCases.CreateTransaction;

internal sealed class CreateTransactionService : ICreateTransactionService
{
    private readonly PaymentContext _context;
    private readonly ISendNotificationService _sendNotificationService;

    public CreateTransactionService(PaymentContext context, ISendNotificationService sendNotificationService)
    {
        _context = context;
        _sendNotificationService = sendNotificationService;
    }

    public async Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            if (!await TryHoldAsync(request.SubjectId, request.Amount, cancellationToken))
            {
                await tx.RollbackAsync(cancellationToken);
                continue;
            }
            
            var (transaction, transactionStatus) = await CreateTransactionAsync(request.SubjectId, request.PeerId, request.Amount, cancellationToken);

            await _sendNotificationService.SendTransactionChangeNotificationAsync(transaction, transactionStatus.Status, cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return new CreateTransactionResponse(transaction.Id);
        } while (true);
    }

    private async Task<bool> TryHoldAsync(Guid accountId, decimal amount, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FindAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException("Account not found");
        }

        if (account.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient balance");
        }

        var oldBalance = account.Balance;
        var newBalance = oldBalance - amount;

        if (newBalance < 0)
        {
            throw new InvalidOperationException("Insufficient balance");
        }

        var oldHold = account.Hold;
        var newHold = oldHold + amount;
        
        var updatedRows = await _context.Accounts
            .Where(a => a.Id == account.Id)
            .Where(a => a.Balance == oldBalance)
            .Where(a => a.Hold == oldHold)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Balance, a => newBalance)
                    .SetProperty(a => a.Hold, a => newHold),
                cancellationToken
            );

        if (updatedRows == 1)
        {
            return true;
        }

        return false;
    }

    private async Task<(TransactionModel, TransactionStatusModel)> CreateTransactionAsync(Guid subjectId, Guid peerId, decimal amount, CancellationToken cancellationToken)
    {
        var transaction = new TransactionModel(
            Guid.NewGuid(),
            subjectId,
            peerId,
            amount,
            DateTimeOffset.UtcNow
        );

        var transactionStatus = new TransactionStatusModel(
            Guid.NewGuid(),
            transaction.Id,
            TransactionStatusType.Hold,
            DateTimeOffset.UtcNow
        );

        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.TransactionStatuses.AddAsync(transactionStatus, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return (transaction, transactionStatus);
    }
}
```

В данном коде мы делаем следующее:

- Создаем в базе данных транзакцию;
- Пытаемся вычесть с баланса нужное количество средств;
- Если нам это удается - создаем запись об этом в базе данных;
- Если смогли создать запись - отправляем уведомление;
- Если смогли сделать все необходимые действия - делаем коммит транзакции в базе данных.

##### Отмена транзакций

Теперь добавим в проект каталог `UseCases/CancelTransaction` и в нем классы `CancelTransactionRequest` и `CancelTransactionResponse`.

**CancelTransactionRequest:**

```csharp
namespace PaymentService.UseCases.CancelTransaction;

public sealed record CancelTransactionRequest(Guid TransactionId);
```

**CancelTransactionResponse:**

```csharp
namespace PaymentService.UseCases.CancelTransaction;

public sealed record CancelTransactionResponse;
```

После этого добавим интерфейс `ICancelTransactionService`:

```csharp
namespace PaymentService.UseCases.CancelTransaction;

public interface ICancelTransactionService
{
    Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request, CancellationToken cancellationToken);
}
```

Далее добавим реализацию интерфейса в классе `CancelTransactionService`:

```csharp
using TransactionModel = PaymentService.Models.Transaction;
using TransactionStatusModel = PaymentService.Models.TransactionStatus;

namespace PaymentService.UseCases.CancelTransaction;

internal sealed class CancelTransactionService : ICancelTransactionService
{
    private readonly PaymentContext _context;
    private readonly ISendNotificationService _sendNotificationService;

    public CancelTransactionService(PaymentContext context, ISendNotificationService sendNotificationService)
    {
        _context = context;
        _sendNotificationService = sendNotificationService;
    }

    public async Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            var transaction = await _context.Transactions.FindAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
            {
                throw new InvalidOperationException("Transaction not found");
            }

            if (!await TryCancelHoldAsync(transaction, cancellationToken))
            {
                await tx.RollbackAsync(cancellationToken);
                continue;
            }

            var transactionStatus = new TransactionStatusModel(
                Guid.NewGuid(),
                request.TransactionId,
                TransactionStatusType.Cancel,
                DateTimeOffset.UtcNow
            );

            await _context.TransactionStatuses.AddAsync(transactionStatus, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _sendNotificationService.SendTransactionChangeNotificationAsync(transaction, transactionStatus.Status, cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return new CancelTransactionResponse();
        } while (true);
    }

    private async Task<bool> TryCancelHoldAsync(TransactionModel transaction, CancellationToken cancellationToken)
    {
        var lastStatus = await _context.TransactionStatuses
            .Where(s => s.TransactionId == transaction.Id)
            .OrderByDescending(s => s.CreatedAt)
            .FirstAsync(cancellationToken);

        if (lastStatus.Status != TransactionStatusType.Hold)
        {
            throw new InvalidOperationException("Transaction cannot be cancelled");
        }

        var account = await _context.Accounts.FindAsync(transaction.SubjectId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException("Account not found");
        }

        var oldBalance = account.Balance;
        var oldHold = account.Hold;
        var newBalance = oldBalance + transaction.Amount;
        var newHold = oldHold - transaction.Amount;

        var updatedRows = await _context.Accounts
            .Where(a => a.Id == account.Id)
            .Where(a => a.Balance == oldBalance)
            .Where(a => a.Hold == oldHold)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Balance, a => newBalance)
                    .SetProperty(a => a.Hold, a => newHold),
                cancellationToken
            );

        if (updatedRows != 1)
        {
            return false;
        }

        return true;
    }
}
```

В данном коде мы делаем следующее:

- Начинаем транзакцию в базе данных;
- Находим транзакцию по идентификатору;
- Проверяем, что последний статус транзакции - Hold;
- Находим счет, с которого были заморожены средства;
- Возвращаем замороженные средства на счет;
- Создаем новый статус транзакции - Cancel;
- Отправляем уведомление об отмене транзакции;
- Если все действия выполнены успешно - делаем коммит транзакции в базе данных.

##### Подтверждение транзакций

Теперь добавим в проект каталог `UseCases/ConfirmTransaction` и в нем классы `ConfirmTransactionRequest` и `ConfirmTransactionResponse`.

**ConfirmTransactionRequest:**

```csharp
namespace PaymentService.UseCases.ConfirmTransaction;

public sealed record ConfirmTransactionRequest(Guid TransactionId);
```

**ConfirmTransactionResponse:**

```csharp
namespace PaymentService.UseCases.ConfirmTransaction;

public sealed record ConfirmTransactionResponse;
```

После этого добавим интерфейс `IConfirmTransactionService`:

```csharp
namespace PaymentService.UseCases.ConfirmTransaction;

public interface IConfirmTransactionService
{
    Task<ConfirmTransactionResponse> ConfirmTransactionAsync(ConfirmTransactionRequest request, CancellationToken cancellationToken);
}
```

Далее добавим реализацию интерфейса в классе `ConfirmTransactionService`:

```csharp
using TransactionModel = PaymentService.Models.Transaction;
using TransactionStatusModel = PaymentService.Models.TransactionStatus;

namespace PaymentService.UseCases.ConfirmTransaction;

internal sealed class ConfirmTransactionService : IConfirmTransactionService
{
    private readonly PaymentContext _context;
    private readonly ISendNotificationService _sendNotificationService;

    public ConfirmTransactionService(PaymentContext context, ISendNotificationService sendNotificationService)
    {
        _context = context;
        _sendNotificationService = sendNotificationService;
    }

    public async Task<ConfirmTransactionResponse> ConfirmTransactionAsync(ConfirmTransactionRequest request, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            var transaction = await _context.Transactions.FindAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
            {
                throw new InvalidOperationException("Transaction not found");
            }

            if (!await TryConfirmTransactionAsync(transaction, cancellationToken))
            {
                await tx.RollbackAsync(cancellationToken);
                continue;
            }

            var transactionStatus = new TransactionStatusModel(
                Guid.NewGuid(),
                request.TransactionId,
                TransactionStatusType.Charge,
                DateTimeOffset.UtcNow
            );

            await _context.TransactionStatuses.AddAsync(transactionStatus, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _sendNotificationService.SendTransactionChangeNotificationAsync(transaction, transactionStatus.Status, cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return new ConfirmTransactionResponse();
        } while (true);
    }

    private async Task<bool> TryConfirmTransactionAsync(TransactionModel transaction, CancellationToken cancellationToken)
    {
        var lastStatus = await _context.TransactionStatuses
            .Where(s => s.TransactionId == transaction.Id)
            .OrderByDescending(s => s.CreatedAt)
            .FirstAsync(cancellationToken);

        if (lastStatus.Status != TransactionStatusType.Hold)
        {
            throw new InvalidOperationException("Transaction cannot be confirmed");
        }

        if (!await TryUpdatePeerAccountAsync(transaction.PeerId, transaction.Amount, cancellationToken))
        {
            return false;
        }

        if (!await TryUpdateSubjectAccountAsync(transaction.SubjectId, transaction.Amount, cancellationToken))
        {
            return false;
        }

        return true;
    }

    private async Task<bool> TryUpdatePeerAccountAsync(Guid peerId, decimal amount, CancellationToken cancellationToken)
    {
        var peerAccount = await _context.Accounts.FindAsync(peerId, cancellationToken);

        if (peerAccount is null)
        {
            throw new InvalidOperationException("Peer account not found");
        }

        var oldBalance = peerAccount.Balance;
        var newBalance = oldBalance + amount;

        var updatedRows = await _context.Accounts
            .Where(a => a.Id == peerAccount.Id)
            .Where(a => a.Balance == oldBalance)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Balance, a => newBalance),
                cancellationToken
            );

        if (updatedRows != 1)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> TryUpdateSubjectAccountAsync(Guid subjectId, decimal amount, CancellationToken cancellationToken)
    {
        var subjectAccount = await _context.Accounts.FindAsync(subjectId, cancellationToken);

        if (subjectAccount is null)
        {
            throw new InvalidOperationException("Subject account not found");
        }

        var oldHold = subjectAccount.Hold;
        var newHold = oldHold - amount;

        var updatedRows = await _context.Accounts
            .Where(a => a.Id == subjectAccount.Id)
            .Where(a => a.Hold == oldHold)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Hold, a => newHold),
                cancellationToken
            );

        if (updatedRows != 1)
        {
            return false;
        }

        return true;
    }
}
```

В данном коде мы делаем следующее:

- Начинаем транзакцию в базе данных;
- Находим транзакцию по идентификатору;
- Проверяем, что последний статус транзакции - Hold;
- Находим целевой счет, на который нужно перевести средства;
- Добавляем средства на целевой счет;
- Создаем новый статус транзакции - Charge;
- Отправляем уведомление о подтверждении транзакции;
- Если все действия выполнены успешно - делаем коммит транзакции в базе данных.

##### Отправка уведомлений в Kafka

Теперь, когда у нас есть код бизнес-логики, добавим фоновый сервис для отправки уведомлений в Kafka.

Начнем с добавления зависимостей:

```bash
dotnet add PaymentService/PaymentService.csproj package Confluent.Kafka
```

Далее добавим класс `NotificationSender` в каталоге `Common/SendNotification`:

```csharp
namespace PaymentService.Common.SendNotification;

internal sealed class NotificationSender : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationSender> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public NotificationSender(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NotificationSender> logger,
        IProducer<string, string> producer,
        string topic)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _producer = producer;
        _topic = topic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await SendNotificationAsync(stoppingToken);

                if (result == SendResult.AllSent)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending notifications");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task<SendResult> SendNotificationAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var notifications = await context.TransactionChangeNotifications
            .Where(n => !n.IsSent)
            .OrderBy(n => n.CreatedAt)
            .Take(2)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
        {
            return SendResult.AllSent;
        }

        var notification = notifications.First();

        var result = await _producer.ProduceAsync(_topic, new Message<string, string>
        {
            Key = notification.Id.ToString(),
            Value = notification.Payload
        }, cancellationToken);

        if (result.Status == PersistenceStatus.Persisted)
        {
            await context.TransactionChangeNotifications
                .Where(n => n.Id == notification.Id)
                .ExecuteUpdateAsync(
                    n => n.SetProperty(n => n.IsSent, true),
                    cancellationToken
                );
        }

        return notifications.Count == 1
            ? SendResult.AllSent
            : SendResult.HasMore;
    }

    private enum SendResult
    {
        AllSent,
        HasMore
    }
}
```

##### Эндпоинты

Теперь добавим эндпоинты для нашего сервиса платежей. Для этого добавим в класс `Program.cs` перед `app.Run();` следующий код:

```csharp
app.MapPost("/api/v1/accounts", async (
        CreateAccountRequest request,
        ICreateAccountService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.CreateAccountAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("CreateAccount")
    .WithOpenApi();

app.MapPost("/api/v1/transactions/hold", async (
        CreateTransactionRequest request,
        ICreateTransactionService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.CreateTransactionAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("CreateTransaction")
    .WithOpenApi();

app.MapPost("/api/v1/transactions/commit", async (
        ConfirmTransactionRequest request,
        IConfirmTransactionService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.ConfirmTransactionAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("ConfirmTransaction")
    .WithOpenApi();

app.MapPost("/api/v1/transactions/cancel", async (
        CancelTransactionRequest request,
        ICancelTransactionService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.CancelTransactionAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("CancelTransaction")
    .WithOpenApi();
```

##### Регистрация сервисов

Далее перед строкой `var app = builder.Build();` добавим код регистрации сервисов:

```csharp
builder.Services.AddDbContext<PaymentContext>((serviceProvider, options) =>
{
    options.UseNpgsql(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Default"));
});

builder.Services.AddHostedService<MigrationRunner>();

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var producerConfig = new ProducerConfig
    {
        BootstrapServers = config.GetSection("Kafka:BootstrapServers").Value
    };
    return new ProducerBuilder<string, string>(producerConfig).Build();
});

builder.Services.AddHostedService(sp =>
{
    var producer = sp.GetRequiredService<IProducer<string, string>>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new NotificationSender(
        sp.GetRequiredService<IServiceScopeFactory>(),
        sp.GetRequiredService<ILogger<NotificationSender>>(),
        producer,
        configuration.GetSection("Kafka:Topic").Value
            ?? throw new InvalidOperationException("Kafka topic is not configured")
    );
});

builder.Services.AddScoped<ISendNotificationService, SendNotificationService>();
builder.Services.AddScoped<ICreateAccountService, CreateAccountService>();
builder.Services.AddScoped<ICreateTransactionService, CreateTransactionService>();
builder.Services.AddScoped<IConfirmTransactionService, ConfirmTransactionService>();
builder.Services.AddScoped<ICancelTransactionService, CancelTransactionService>();
```

##### Запуск сервиса

Для запуска сервиса создадим в каталоге решения файл `Dockerfile.PaymentService` и добавим в него следующий код:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PaymentService/PaymentService.csproj", "PaymentService/"]
COPY ["PaymentService.Contracts/PaymentService.Contracts.csproj", "PaymentService.Contracts/"]

RUN dotnet restore "PaymentService/PaymentService.csproj"

COPY . .

WORKDIR "/src/PaymentService"

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "PaymentService.dll"]
```

Теперь создадим в каталоге решения файл `docker-compose.yml` и добавим в него следующий код:

```yaml
services:
  payment-service-db:
    image: postgres:13
    ports:
      - "5432:5432"
    environment:
      POSTGRES_PASSWORD: payment-service-db-pass
      POSTGRES_DB: payment-service-db
      POSTGRES_USER: payment-service-db
  payment-service:
    build:
      context: .
      dockerfile: PaymentService.Dockerfile
    ports:
      - "8080:8080"
    depends_on:
      - payment-service-db
      - kafka
    environment:
      CONNECTIONSTRINGS__Default: Host=payment-service-db;Database=payment-service-db;Username=payment-service-db;Password=payment-service-db-pass
      KAFKA__TOPIC: transaction-changes
      KAFKA__BOOTSTRAPSERVERS: kafka:9093
      ASPNETCORE_ENVIRONMENT: Development
  kafka:
    image: confluentinc/cp-kafka:latest
    ports:
      - "9092:9092"
    expose:
      - "29092"
      - "9093"
    environment:
      ALLOW_PLAINTEXT_LISTENER: yes
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: CLIENT:PLAINTEXT, LOCALHOST:PLAINTEXT, SERVICE:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: CLIENT
      KAFKA_ADVERTISED_LISTENERS: CLIENT://kafka:29092, LOCALHOST://localhost:9092, SERVICE://kafka:9093
      KAFKA_LISTENERS: CLIENT://kafka:29092,LOCALHOST://0.0.0.0:9092,SERVICE://0.0.0.0:9093
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
  zookeeper:
    image: confluentinc/cp-zookeeper:latest
    ports:
      - "2181:2181"
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
  kafka-init:
    image: confluentinc/cp-kafka:latest
    depends_on:
      - kafka
    entrypoint: ["sh", "-c"]
    command: |
      "
      kafka-topics --bootstrap-server kafka:29092 --create --if-not-exists --topic transaction-changes --partitions 1 --replication-factor 1
      "
```

#### Сервис уведомлений

Теперь реализуем сервис уведомлений. Для этого добавим в решение новый проект:

```bash
dotnet new console -n NotificationService -o NotificationService
dotnet sln add NotificationService/
```

И добавим зависимости:

```bash
dotnet add NotificationService/NotificationService.csproj package Microsoft.Extensions.Hosting --version 8
dotnet add NotificationService/NotificationService.csproj reference PaymentService.Contracts/PaymentService.Contracts.csproj
```

##### Модели

Начнем с определения моделей. Добавим каталог `Models` и в нем класс `Notification`:

```csharp
namespace NotificationService.Models;

internal sealed class Notification
{
    public Guid Id { get; }
    public string NotificationKey { get; }
    public string Payload { get; }
    public bool IsProcessed { get; }
    public DateTimeOffset CreatedAt { get; }

    public Notification(Guid id, string notificationKey, string payload, bool isProcessed, DateTimeOffset createdAt)
    {
        Id = id;
        NotificationKey = notificationKey;
        Payload = payload;
        IsProcessed = isProcessed;
        CreatedAt = createdAt;
    }
}
```

##### Хранение данных

Добавим зависимости для работы с базой данных:

```bash
dotnet add NotificationService/NotificationService.csproj package Microsoft.EntityFrameworkCore.Design --version 8
dotnet add NotificationService/NotificationService.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 8
```

Создадим каталог `Database` и в нем класс контекста базы данных:

```csharp
namespace NotificationService.Database;

internal sealed class NotificationContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }

    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.NotificationKey)
                .HasColumnName("notification_key")
                .IsRequired();
            
            entity.Property(e => e.Payload)
                .HasColumnName("payload")
                .IsRequired();
            
            entity.Property(e => e.IsProcessed)
                .HasColumnName("is_processed")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.NotificationKey)
                .IsUnique();
        });
    }
}
```

Добавим фабрику для создания контекста:

```csharp
namespace NotificationService.Database;

internal sealed class NotificationContextFactory : IDesignTimeDbContextFactory<NotificationContext>
{
    public NotificationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=NotificationService;Username=postgres;Password=postgres");

        return new NotificationContext(optionsBuilder.Options);
    }
}
```

Сгенерируем миграции:

```bash
dotnet ef migrations add InitialCreate --project NotificationService/NotificationService.csproj
```

Добавим код для применения миграций:

```csharp
namespace NotificationService.Database;

internal sealed class MigrationRunner : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationRunner(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<NotificationContext>();

        context.Database.Migrate();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```

##### Обработка уведомлений

Создадим каталог `UseCases/ProcessNotification` и в нем класс `NotificationProcessor`:

```csharp
namespace NotificationService.UseCases.ProcessNotification;

internal sealed class NotificationProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationProcessor> _logger;

    public NotificationProcessor(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NotificationProcessor> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await ProcessNotificationAsync(stoppingToken);

                if (result == ProcessResult.AllProcessed)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing notifications");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task<ProcessResult> ProcessNotificationAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationContext>();

        var notifications = await context.Notifications
            .Where(n => !n.IsProcessed)
            .OrderBy(n => n.CreatedAt)
            .Take(2)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
        {
            return ProcessResult.AllProcessed;
        }

        var notification = notifications.First();

        _logger.LogInformation("Processing notification {NotificationId}: {Payload}", notification.Id, notification.Payload);

        await context.Notifications
            .Where(n => n.Id == notification.Id)
            .ExecuteUpdateAsync(
                n => n.SetProperty(n => n.IsProcessed, true),
                cancellationToken
            );

        return notifications.Count == 1
            ? ProcessResult.AllProcessed
            : ProcessResult.HasMore;
    }

    private enum ProcessResult
    {
        AllProcessed,
        HasMore
    }
}
```

##### Получение уведомлений из Kafka

Добавим зависимости для работы с Kafka:

```bash
dotnet add NotificationService/NotificationService.csproj package Confluent.Kafka
```

Создадим каталог `UseCases/ConsumeNotification` и в нем класс `NotificationConsumer`:

```csharp
namespace NotificationService.UseCases.ConsumeNotification;

internal sealed class NotificationConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;

    public NotificationConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NotificationConsumer> logger,
        IConsumer<string, string> consumer,
        string topic)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _consumer = consumer;
        _topic = topic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);

                if (result is null)
                {
                    continue;
                }

                var transactionChange = JsonSerializer.Deserialize<TransactionChangeDto>(result.Message.Value);

                if (transactionChange is null)
                {
                    _logger.LogWarning("Received invalid transaction change: {Message}", result.Message.Value);
                    continue;
                }

                await ProcessMessageAsync(transactionChange, stoppingToken);
                
                // Ручной коммит оффсета после успешной обработки сообщения
                _consumer.Commit(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while consuming messages");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task ProcessMessageAsync(TransactionChangeDto transactionChange, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationContext>();

        var notification = new Models.Notification(
            Guid.NewGuid(),
            GetNotificationKey(transactionChange),
            payload: JsonSerializer.Serialize(transactionChange),
            isProcessed: false,
            DateTimeOffset.UtcNow
        );

        await context.Notifications.AddAsync(notification, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Received notification {NotificationId}: {Payload}", notification.Id, notification.Payload);
    }

    private string GetNotificationKey(TransactionChangeDto transactionChange)
    {
        var notificationKey = transactionChange.TransactionId.ToString();
                
        notificationKey += transactionChange.Status switch
        {
            TransactionStatusType.Hold => "-hold",
            TransactionStatusType.Charge => "-charged",
            TransactionStatusType.Cancel => "-canceled",
            _ => throw new InvalidOperationException($"Unknown transaction status: {transactionChange.Status}")
        };

        return notificationKey;
    }
}
```

##### Регистрация сервисов

В файле `Program.cs` заменяем код на следующий:

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<NotificationProcessor>();
        services.AddHostedService<NotificationConsumer>(sp => {
            var consumer = sp.GetRequiredService<IConsumer<string, string>>();
            var topic = context.Configuration["Kafka:Topic"];
            var logger = sp.GetRequiredService<ILogger<NotificationConsumer>>();
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

            return new NotificationConsumer(
                scopeFactory,
                logger,
                consumer,
                topic ?? throw new InvalidOperationException("Kafka topic is not configured")
            );
        });
        services.AddDbContext<NotificationContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("Default")));
        
        services.AddSingleton<IConsumer<string, string>>(sp =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = context.Configuration["Kafka:BootstrapServers"],
                GroupId = context.Configuration["Kafka:GroupId"],
                EnableAutoCommit = false,
            };

            return new ConsumerBuilder<string, string>(config).Build();
        });

        services.AddHostedService<MigrationRunner>();
    })
    .Build();

await host.RunAsync();
```

##### Запуск сервиса

Создадим в каталоге решения файл `NotificationService.Dockerfile` и добавим в него следующий код:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["NotificationService/NotificationService.csproj", "NotificationService/"]
COPY ["PaymentService.Contracts/PaymentService.Contracts.csproj", "PaymentService.Contracts/"]

RUN dotnet restore "NotificationService/NotificationService.csproj"

COPY . .

WORKDIR "/src/NotificationService"

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "NotificationService.dll"]
```

Теперь обновим файл `docker-compose.yml`, добавив в него сервис уведомлений и его базу данных:

```yaml
services:
  # ... другие сервисы ...
  notification-service-db:
    image: postgres:13
    ports:
      - "5433:5432"
    environment:
      POSTGRES_PASSWORD: notification-service-db-pass
      POSTGRES_DB: notification-service-db
      POSTGRES_USER: notification-service-db
  notification-service:
    build:
      context: .
      dockerfile: NotificationService.Dockerfile
    depends_on:
      - notification-service-db
      - kafka
    environment:
      CONNECTIONSTRINGS__Default: Host=notification-service-db;Database=notification-service-db;Username=notification-service-db;Password=notification-service-db-pass
      KAFKA__TOPIC: transaction-changes
      KAFKA__BOOTSTRAPSERVERS: kafka:9093
      KAFKA__GROUPID: notification-service-group
      ASPNETCORE_ENVIRONMENT: Development
```

#### Тестирование

Теперь запустим сервисы и проверим, что все работает:

```bash
docker-compose up -d
```

Также запустим просмотр логов сервиса уведомлений:

```bash
docker compose logs -f notification-service
```

Создадим пару аккаунтов, после чего переведем средства с одного аккаунта на другой. При этом в логах сервиса уведомлений должны появиться сообщения об обработанных уведомлениях.
