using ESH.BuildingBlocks.SampleResult;

namespace ViewModels;

public class CategoryResponseViewModel
	: ESH.SeedworkSystem.ViewModel.Response.BaseResponseViewModel<CategoryRequestViewModel>
{
	public override CategoryRequestViewModel ToRequest()
	{
		throw new NotImplementedException();
	}
}

public class CategoryRequestViewModel
	: ESH.SeedworkSystem.ViewModel.Request.BaseRequestViewModel
{
	public override Result Validate()
	{
		throw new NotImplementedException();
	}
}