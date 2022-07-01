using IkeMtz.NRSRx.Core.Unigration;
using NRSRx_ServiceName.Models.V1;
using static IkeMtz.NRSRx.Core.Unigration.TestDataFactory;

namespace NRSRx_ServiceName.Tests
{
  public static partial class Factories
  {
    public static ItemModel ItemModelFactory()
    {
      var itemModel = CreateIdentifiable(new ItemModel());
      itemModel.Name = StringGenerator(100, true, CharacterSets.AlphaNumericChars);
      return itemModel;
    }
  }
}
