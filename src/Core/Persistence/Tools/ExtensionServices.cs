using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Settings;

public static class ExtensionServices
{
	// migrations settings
	public static void AddDatabaseContextEntityFrameworkCore
		(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<DatabaseContext>(opts =>
		{
			opts.UseSqlServer(connectionString);
		});
	}

	// identity microsoft settings
	// public static void AddIdentityMicrosoft(this IServiceCollection services)
	// {
	//     services.AddAuthorizationCore(options =>
	//     {
	//         // ******************************************
	//         // options.AddPolicy(ESH.Resources.Policies.AdminPolicy,
	//         // 	policy =>
	//         // 	{
	//         // 		policy.RequireRole(
	//         // 			// nameof(ESH.Resources.InitialData.Roles.Admin),
	//         // 			nameof(ESH.Resources.InitialData.Roles.Admin));
	//         // 	});
	//         // ******************************************
	//     
	//         // ******************************************
	//         //options.AddPolicy(ESH.Resources.Policies.AdminsPolicy,
	//         //	policy =>
	//         //	{
	//         //		policy.RequireRole(
	//         //			nameof(ESH.Resources.InitialData.Roles.Admin),
	//         //			nameof(ESH.Resources.InitialData.Roles.SuperAdmin));
	//         //	});
	//         // ******************************************
	//     });
	//     
	//     
	//     IdentityBuilder builder =
	//     	services.AddIdentity<Domain.User,Domain.Role>(option =>
	//     	{
	//     		option.Password.RequireDigit = false;
	//     		option.Password.RequireLowercase = false;
	//     		option.Password.RequireUppercase = false;
	//     		option.Password.RequireNonAlphanumeric = false;
	//     		option.Password.RequiredLength = 5;
	//     		option.User.RequireUniqueEmail = false;
	//     
	//     		option.SignIn.RequireConfirmedAccount = false;
	//     		option.Lockout.MaxFailedAccessAttempts = 3;
	//     		option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
	//     
	//     	}).AddRoles<Domain.Role>()
	//     	.AddEntityFrameworkStores<Persistence.DatabaseContext>();
	//     
	//     builder.PollingServices.ConfigureApplicationCookie(options =>
	//     {
	//     	// Cookie settings
	//     	options.Cookie.HttpOnly = true;
	//     	options.ExpireTimeSpan = TimeSpan.FromHours(1);
	//     
	//     	options.LoginPath = "/Panel/Accounting/Login";
	//     	options.AccessDeniedPath = "/Panel/Accounting/Login";
	//     	options.SlidingExpiration = true;
	//     });
	//     
	//     builder =
	//     	new IdentityBuilder
	//     		(builder.UserType, typeof(Domain.Role), builder.PollingServices);
	//     
	//     builder.AddEntityFrameworkStores<DatabaseContext>()
	//     	.AddDefaultTokenProviders();
	// }
}
