using AutoMapper;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using SM.Application.Common.Mappings;
using SM.Infrastructure.Persistence;
using System;
using Xunit;

namespace SM.Application.UnitTests
{
    public sealed class QueryTestFixture : IDisposable
    {
        public QueryTestFixture()
        {
            Context = ApplicationDbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();

            var op = new SieveOptions { ThrowExceptions = true };
            SieveProcessor = new ApplicationSieveProcessor(new OptionsWrapper<SieveOptions>(op), new ApplicationCustomFilterMethods());
        }
        public ApplicationDbContext Context { get; }

        public IMapper Mapper { get; }

        public ISieveProcessor SieveProcessor { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryTests")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
