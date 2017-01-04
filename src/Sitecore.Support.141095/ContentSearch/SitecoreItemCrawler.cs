using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace Sitecore.Support.ContentSearch
{
  public class SitecoreItemCrawler : Sitecore.ContentSearch.SitecoreItemCrawler
  {
    public SitecoreItemCrawler()
    {
    }

    public SitecoreItemCrawler(IIndexOperations indexOperations) : base(indexOperations)
    {
    }

    protected override IEnumerable<IIndexableUniqueId> GetIndexablesToUpdateOnDelete(IIndexableUniqueId indexableUniqueId)
    {
      ItemUri iteratorVariable = indexableUniqueId.Value as ItemUri;
      using (new SecurityDisabler())
      {
        Item item;
        ItemUri uri = new ItemUri(iteratorVariable.ItemID, iteratorVariable.Language, Version.Latest, iteratorVariable.DatabaseName);
        using (new WriteCachesDisabler())
        {
          item = Sitecore.Data.Database.GetItem(uri);
        }
        if ((item != null) && (item.Version.Number < iteratorVariable.Version.Number) && (item.Versions.Count > 0)) // Sitecore.Support.141095
        {
          yield return new SitecoreItemUniqueId(item.Uri);
        }
      }
    }
  }
}