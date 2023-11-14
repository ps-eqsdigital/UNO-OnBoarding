using Business.Interfaces;
using Data.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;

namespace api.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Create a scope to access scoped services
            using (var scope = context.RequestServices.CreateScope())
            {
                IServiceProvider serviceProvider = scope.ServiceProvider;
                IUserDataAccessObject userDataAccessObject = serviceProvider.GetRequiredService<IUserDataAccessObject>();

                string token = context.Request.Headers["Authorization"].FirstOrDefault()!;
                Endpoint endpoint = context.GetEndpoint()!;

                if (endpoint != null)
                {
                    AuthorizeAttribute authorizeAttribute = endpoint.Metadata.GetMetadata<AuthorizeAttribute>()!;

                    if (authorizeAttribute == null)
                    {
                        await _next(context); 
                    }
                    else if (!string.IsNullOrWhiteSpace(token))
                    {
                        UserTokenAuthentication userToken = await userDataAccessObject.GetTokenUuidByToken(token.Substring("Bearer ".Length));

                        if (userToken != null && userToken.IsValid == true)
                        {
                            await _next(context);
                        }
                        else
                        {
                            context.Response.StatusCode = 401; 
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                    }
                }
            }
        }
    }

}
