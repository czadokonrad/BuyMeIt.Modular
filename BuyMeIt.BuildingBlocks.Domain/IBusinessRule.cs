namespace BuyMeIt.BuildingBlocks.Domain
{
    public interface IBusinessRule
    {
        bool IsBroken();

        //move to IAsyncBusinessRule
        //Task<bool> IsBrokenAsync();
        string Message { get; }
    }
}
