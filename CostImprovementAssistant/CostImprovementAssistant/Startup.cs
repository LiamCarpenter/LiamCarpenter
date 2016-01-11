using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CostImprovementAssistant.Startup))]
namespace CostImprovementAssistant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
