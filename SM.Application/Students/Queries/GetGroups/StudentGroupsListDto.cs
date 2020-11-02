using AutoMapper;
using SM.Application.Common.Mappings;
using SM.Domain.Entities;

namespace SM.Application.Groups.Queries
{
    public class StudentGroupsListDto : IMapFrom<StudentGroup>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StudentGroup, StudentGroupsListDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.GroupId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Group.Name));
        }
    }
}
