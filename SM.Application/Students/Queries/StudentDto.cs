using AutoMapper;
using Sieve.Attributes;
using SM.Application.Common.Mappings;
using SM.Domain.Entities;
using SM.Domain.Enums;
using System.Collections.Generic;

namespace SM.Application.Students.Queries
{
    public class StudentDto : IMapFrom<Student>
    {
        public long Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string FullName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PublicId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public Genders Gender { get; set; }

        public IEnumerable<StudentGroupDto> Groups { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Student, StudentDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName + " " + s.MiddleName + (s.LastName != null ? (" " + s.LastName) : "")))
                .ForMember(d => d.Groups, opt => opt.MapFrom(s => s.StudentGroups));
        }
    }

    public class StudentGroupDto : IMapFrom<StudentGroup>
    {
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StudentGroup, StudentGroupDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Group.Name));
        }
    }
}
