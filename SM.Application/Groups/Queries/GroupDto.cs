using AutoMapper;
using Sieve.Attributes;
using SM.Application.Common.Mappings;
using SM.Domain.Entities;

namespace SM.Application.Groups.Queries
{
    public class GroupDto : IMapFrom<Group>
    {
        public int Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }

        [Sieve(CanSort = true)]
        public int StudentsCount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupDto>()
                .ForMember(d => d.StudentsCount, opt => opt.MapFrom(s => s.StudentGroups.Count));
        }
    }
}
