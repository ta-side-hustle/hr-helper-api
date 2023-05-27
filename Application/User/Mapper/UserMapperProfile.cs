using Application.User.Dto;
using AutoMapper;
using Domain.Models.Auth;

namespace Application.User.Mapper;

public class UserMapperProfile : Profile
{
	public UserMapperProfile()
	{
		CreateMap<RoleModel, UserRoleDto>(MemberList.Destination)
			.ForMember(dest => dest.LocalizedDescription, opt => opt.MapFrom(src => src.Description))
			;
	}
}