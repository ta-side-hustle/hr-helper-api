using Application.Organization.Dto;
using AutoMapper;
using Domain.Models.Organization;

namespace Application.Organization.Mapper;

public class OrganizationMappingProfile : Profile
{
	public OrganizationMappingProfile()
	{
		CreateMap<OrganizationModel, OrganizationDto>(MemberList.Destination);
		
		CreateMap<OrganizationCreateDto, OrganizationModel>(MemberList.Source);
	}
}