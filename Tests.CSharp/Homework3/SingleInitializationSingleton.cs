namespace Tests.CSharp.Homework3;

public class SingleInitializationSingleton
{
    public const int DefaultDelay = 3_000;
    private static Lazy<SingleInitializationSingleton> _lazySingleton =
        new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(DefaultDelay));
    private static bool _isInitialized = false;

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // имитация сложной инициализационной логики
        Thread.Sleep(delay);
    }

    public int Delay { get; }

    public static SingleInitializationSingleton Instance => _lazySingleton.Value;
    
    internal static void Reset()
    {
        _lazySingleton = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(DefaultDelay));
        _isInitialized = false;
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Cannot be initialized more than once!");
        }
        _lazySingleton = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(delay));
        _isInitialized = true;
    } 
}
