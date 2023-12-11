namespace SeniorPomidor.Async;

public sealed class WhenAllOrAny
{
    public static Task WhenAll(params Task[] tasks)
    {
        return Task.WhenAll(tasks);
    }

    public static Task WhenAny(params Task[] tasks)
    {
        return Task.WhenAny(tasks);
    }
}