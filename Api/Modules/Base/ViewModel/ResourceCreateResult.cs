namespace Api.Modules.Base.ViewModel;

public class ResourceCreateResult<T>: SuccessResult<ResourceCreateDto<T>>
{
	public ResourceCreateResult(T id) : base(new ResourceCreateDto<T> { Id = id})
	{
	}
}