using Application.Organization.Dto;
using AutoMapper;
using Domain.Models.Organization;

namespace Application.Organization.Mapper;

public class OrganizationMappingProfile : Profile
{
	public OrganizationMappingProfile()
	{
		CreateMap<OrganizationModel, OrganizationDto>(MemberList.Destination)
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForAllMembers(opt => opt.Ignore());
		
		CreateMap<OrganizationCreateDto, OrganizationModel>(MemberList.Destination)
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OrganizationName))
			.ForAllMembers(opt => opt.Ignore());
	}
}