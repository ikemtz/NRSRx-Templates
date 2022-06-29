using System.Collections.Generic;
using IkeMtz.NRSRx.Core.OData;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OData.Edm;
using V1 = NRSRx_OData.Models.V1;

namespace NRSRx_OData.Configuration
{
    public class ODataModelProvider : BaseODataModelProvider
    {
        public static IEdmModel GetV1EdmModel() =>
          ODataEntityModelFactory(builder =>
          {
              _ = builder.EntitySet<V1.Item>($"{nameof(V1.Item)}s");
          });

        public override IDictionary<ApiVersionDescription, IEdmModel> GetModels() =>
            new Dictionary<ApiVersionDescription, IEdmModel>
            {
                [ApiVersionFactory(1, 0)] = GetV1EdmModel(),
            };
    }
}
