using Microsoft.EntityFrameworkCore;
using MediatR;

public class NumberEntity
{
    public int Id { get; set; }
    public int Value { get; set; }
}

public class NumberGenerated : IRequest<int> { }

// NumberHandler.cs
public class NumberHandler : IRequestHandler<NumberGenerated, int>
{
    private readonly IRepository _repository;

    public NumberHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(NumberGenerated request, CancellationToken cancellationToken)
    {
        return await _repository.SaveNumberAsync(cancellationToken);
    }
}

public interface IRepository
{
    Task<int> SaveNumberAsync(CancellationToken cancellationToken);
}

public class NumberRepository : IRepository
{
    private readonly LocalDbContext _dbContext;

    public NumberRepository(LocalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveNumberAsync(CancellationToken cancellationToken)
    {
        var randomNumber = new Random().Next(1, 100);
        var numberEntity = new NumberEntity { Value = randomNumber };
        _dbContext.Numbers.Add(numberEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return numberEntity.Value;
    }
}

public class LocalDbContext : DbContext
{
    public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

    public DbSet<NumberEntity> Numbers { get; set; }
}