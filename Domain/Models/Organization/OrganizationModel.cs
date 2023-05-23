namespace Domain.Models.Organization;

public class OrganizationModel
{
	/// <summary>
	/// Id that uniquely identifies organization
	/// </summary>
	public int Id { get; set; }
	
	/// <summary>
	/// Public name of the organization
	/// </summary>
	public string Name { get; set; }
}