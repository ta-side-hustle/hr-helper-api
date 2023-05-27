using System;
using System.Collections.Generic;
using System.Linq;
using Application.Organization.Dto;
using Application.User.Dto;
using AutoMapper;
using Domain.Models.Organization;

namespace Application.Organization.Mapper;

public class OrganizationMappingProfile : Profile
{
	public OrganizationMappingProfile()
	{
		CreateMap<OrganizationModel, OrganizationDto>(MemberList.Destination);

		CreateMap<OrganizationCreateDto, OrganizationModel>(MemberList.Source);

		CreateMap<IEnumerable<UserOrganizationModel>, IEnumerable<UserOrganizationDto>>()
			.ConstructUsing((models, context) => MapUserOrganizationModelToUserOrganizationDto(models, context.Mapper))
			;
	}

	private static IEnumerable<UserOrganizationDto> MapUserOrganizationModelToUserOrganizationDto(
		IEnumerable<UserOrganizationModel> models, IMapperBase mapper)
	{
		return models
			.Select(x => new { x.Organization, x.Role, x.OrganizationId })
			.GroupBy(
				x => x.Organization,
				x => x.Role,
				(key, group) => new UserOrganizationDto
				{
					Organization = mapper.Map<OrganizationDto>(key),
					Roles = mapper.Map<IEnumerable<UserRoleDto>>(group)
				}
			);
	}
}