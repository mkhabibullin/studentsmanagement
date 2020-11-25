using MediatR;
using Shared.Contract.Events;
using SM.Application.Common.Interfaces;
using SM.Application.Common.Models;
using SM.Domain.Entities;
using SM.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Application.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : IRequest<Response<long>>
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public Genders? Gender { get; set; }

        public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Response<long>>
        {
            private readonly IApplicationDbContext _context;
            private readonly MassTransit.IPublishEndpoint _publishEndpoint;

            public CreateStudentCommandHandler(IApplicationDbContext context, MassTransit.IPublishEndpoint publishEndpoint)
            {
                _context = context;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<Response<long>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
            {
                var entity = new Student
                {
                    FirstName = request.FirstName.Trim(),
                    MiddleName = request.MiddleName.Trim(),
                    LastName = request.LastName?.Trim(),
                    Gender = request.Gender.Value
                };

                _context.Students.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                await _publishEndpoint.Publish<StudentCreatedEvent>(new
                {
                    StudentId = entity.Id,
                    FullName = $"{entity.FirstName} {entity.MiddleName} {entity.LastName ?? string.Empty}".Trim(),
                    CorrelationId = Guid.NewGuid()
                });

                // TODO: Wait until Saga has finished

                return new Response<long>(entity.Id);
            }
        }
    }
}
