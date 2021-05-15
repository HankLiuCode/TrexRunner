namespace TrexRunner.Entities
{
    public interface IDayNightCycle
    {
        int NightCount { get; }
        bool isNight { get; }
    }
}
