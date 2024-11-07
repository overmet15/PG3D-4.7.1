namespace Rilisoft.PixlGun3D
{
	internal static class MarketProductFactory
	{
		internal static GoogleMarketProduct CreateGoogleMarketProduct(GoogleSkuInfo googleSkuInfo)
		{
			return new GoogleMarketProduct(googleSkuInfo);
		}

		internal static AmazonMarketProduct CreateAmazonMarketProduct(AmazonItem amazonItem)
		{
			return new AmazonMarketProduct(amazonItem);
		}
	}
}
