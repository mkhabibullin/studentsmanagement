using AutoMapper;
using SM.Application.Common.Mappings;
using SM.Domain.Entities;
using SM.Domain.Enums;

namespace SM.Application.Groups.Queries
{
    public class GroupStudentsListDto : IMapFrom<StudentGroup>
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PublicId { get; set; }

        public Genders Gender { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StudentGroup, GroupStudentsListDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Student.Id))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Student.FirstName))
                .ForMember(d => d.MiddleName, opt => opt.MapFrom(s => s.Student.MiddleName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Student.LastName))
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Student.Gender))
                .ForMember(d => d.PublicId, opt => opt.MapFrom(s => s.Student.PublicId));
        }
    }
}
