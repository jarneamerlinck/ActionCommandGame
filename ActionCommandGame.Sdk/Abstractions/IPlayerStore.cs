namespace ActionCommandGame.Sdk.Abstractions
{
    public interface IPlayerStore
    {
        Task<int> GetTokenAsync();
        Task SaveTokenAsync(int playerId);
    }
}
