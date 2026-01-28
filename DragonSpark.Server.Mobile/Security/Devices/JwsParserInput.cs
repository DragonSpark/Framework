namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct JwsParserInput(int First, int Next, int Second, int All)
{
    public JwsParserInput(int First, int Second) : this(First, First + 1, Second) {}

    public JwsParserInput(int First, int Next, int Second) : this(First, Next, Second, Next + Second) {}
}