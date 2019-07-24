using System.Collections.Generic;
using System.Linq;
using ProjectX.Api.Abstractions;

namespace ProjectX.Api
{
    public class VersionService : IVersionService
    {
        public ApiVersion Get()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            return new ApiVersion
            {
                Major = version.Major,
                Minor = version.Minor,
                Build = version.Build,
                Revision = version.Revision
            };
        }
    }
}