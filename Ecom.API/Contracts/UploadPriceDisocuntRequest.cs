namespace Ecom.API.Contracts
{
    public record UploadPriceDisocuntRequest(string Token, PriceDiscoundRequest[] PriceDiscounds);
}
