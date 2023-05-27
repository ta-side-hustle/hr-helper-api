using Application.Organization.Dto;
using Application.Organization.Mapper;
using AutoMapper;
using Domain.Models.Organization;

namespace Application.UnitTests.Organization.Mapper;

[TestFixture]
public class OrganizationMappingProfileTests
{
	private IMapper _mapper = null!;
	
	[OneTimeSetUp]
	public void OneTimeSetup()
	{
		var mapperConfiguration = new MapperConfiguration(config =>
		{
			config.AddProfile<OrganizationMappingProfile>();
		});
		
		var executionPlan = mapperConfiguration.BuildExecutionPlan(typeof(OrganizationCreateDto), typeof(OrganizationModel));
		
		mapperConfiguration.CompileMappings();
		
		_mapper = new AutoMapper.Mapper(mapperConfiguration);
	}

	[Test]
	public void Mapper_is_not_null()
	{
		_mapper.Should().NotBeNull();
	}

	[Test]
	public void Mapper_configuration_must_be_valid()
	{
		var assertMapperConfiguration = () => _mapper.ConfigurationProvider.AssertConfigurationIsValid();

		assertMapperConfiguration.Should().NotThrow();
	}

	[Test]
	public void MapFor_OrganizationCreateDtoToOrganizationModel_IsValid()
	{
		var data = new OrganizationCreateDto
		{
			Name = "sequi-vel-voluptatem"
		};

		var expected = new OrganizationModel
		{
			Name = data.Name
		};

		var actual = _mapper.Map<OrganizationModel>(data);

		actual.Should().BeEquivalentTo(expected);
	}
}