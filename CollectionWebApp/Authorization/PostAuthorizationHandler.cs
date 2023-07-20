using CollectionWebApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace CollectionWebApp.Authorization
{
    public class PostAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    { 
        private readonly UserManager<ApplicationUser> userManager;

        public PostAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Post resource)
        {
            var applicationUser = await userManager.GetUserAsync(context.User);

            if ((requirement.Name == Operations.Update.Name || requirement.Name == Operations.Delete.Name) && applicationUser == resource.Creator)
            {
                context.Succeed(requirement);
            }

            if (requirement.Name == Operations.Read.Name && !resource.Published && applicationUser == resource.Creator)
                context.Succeed(requirement);
        }
    }
}



