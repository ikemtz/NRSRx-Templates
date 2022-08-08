using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using IkeMtz.NRSRx.Core.Web;

namespace NRSRx_WebApi
{

  [ExcludeFromCodeCoverage]
  public class VersionDefinitions : IApiVersionDefinitions
  {
    public const string v1_0 = "1.0";

    public IEnumerable<string> Versions => new[] { v1_0 };
  }
}
