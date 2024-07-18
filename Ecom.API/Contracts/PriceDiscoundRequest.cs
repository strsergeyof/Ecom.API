namespace Ecom.API.Contracts
{
    public record PriceDiscoundRequest(long NmId, decimal Price, decimal Discount);
}
