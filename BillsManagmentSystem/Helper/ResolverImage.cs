using AutoMapper;
using BillsEntity;
using BillsManagmentSystem.ViewModels;

namespace BillsManagmentSystem.Helper
{
	public class ResolverImage : IValueResolver<BillHeader, BillHeaderViewModel, string>
	{
		private readonly IConfiguration configuration;

		public ResolverImage(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        public string Resolve(BillHeader source, BillHeaderViewModel destination, string destMember, ResolutionContext context)
		{
			if(!string.IsNullOrEmpty(source.BILIMG))
			{
				return $"{configuration["BaseUrl"]}{source.BILIMG}";
			}
			return string.Empty;
		}
	}
}
