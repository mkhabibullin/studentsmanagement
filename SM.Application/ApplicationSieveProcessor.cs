using FluentValidation;
using Microsoft.Extensions.Options;
using Sieve.Exceptions;
using Sieve.Models;
using Sieve.Services;
using SM.Application.Students.Queries;
using System.Linq;

namespace SM.Application
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, customFilterMethods)
        {

        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            base.MapProperties(mapper);

            return mapper;
        }
    }

    public class ApplicationCustomFilterMethods : ISieveCustomFilterMethods
    {
        public IQueryable<StudentDto> Groups(IQueryable<StudentDto> source, string op, string[] values)
        {
            var result = source;
            switch (op)
            {
                case "==":
                    foreach (var v in values)
                    {
                        result = result.Where(p => p.Groups.Any(g => g.Name == v));
                    }
                    break;
                case "@=":
                    foreach (var v in values)
                    {
                        result = result.Where(p => p.Groups.Any(g => g.Name.Contains(v)));
                    }
                    break;
                default:
                    throw new SieveException("The only available filters for Groups are '==' and '@='");
            }

            return result;
        }
    }
}
