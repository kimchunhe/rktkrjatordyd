using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace lyrics_api {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);

      //开启认证需要设置该值
      IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
      var signingKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (Configuration.GetSection ("JWT:SecretKey").Value));

      var tokenValidationParameters = new TokenValidationParameters {
        RequireExpirationTime = false,
        RequireSignedTokens = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = false,
        // ValidIssuer = Configuration.GetSection ("TokenProviderOptions:Issuer").Value,
        ValidateAudience = false,
        // ValidAudience = Configuration.GetSection ("TokenProviderOptions:Audience").Value,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      services.AddAuthentication (options => {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer (options => {
        // options.Audience = Configuration.GetSection ("TokenProviderOptions:Audience").Value;
        // options.ClaimsIssuer = Configuration.GetSection ("TokenProviderOptions:Issuer").Value;
        options.TokenValidationParameters = tokenValidationParameters;
        options.SaveToken = true;
        // options.
      });

      services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2)
        .AddJsonOptions (options =>
          //api返回结果中的日期格式化
          options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"
        );

      // Register the Swagger generator, defining 1 or more Swagger documents
      // services.AddSwaggerGen (c => {
      //   c.SwaggerDoc ("v1", new Info { Title = "retail shoplink out api", Version = "v1" });
      //   c.OperationFilter<HttpHeaderFilter> ();
      // });

      services.AddCors (
        options => options.AddPolicy ("ALLOW_ANY", p => p.AllowAnyOrigin ()
          .AllowAnyHeader ().AllowAnyMethod ())
      );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      } else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts ();
      }
      app.UseAuthentication ();
      app.UseHttpsRedirection ();
      app.UseMvc ();

      app.UseCors ("ALLOW_ANY");
    }
  }
}