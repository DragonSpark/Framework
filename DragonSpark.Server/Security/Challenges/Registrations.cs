using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Security.Challenges;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<ChallengeSettings>()
                 .Start<INewChallenge>()
                 .Forward<NewChallenge>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.Start<IChallengeHasher>()
                 .Forward<ChallengeHasher>()
                 .Singleton()
                 //
                 .Then.Start<IValidateChallenge>()
                 .Forward<ValidateChallenge>()
                 .Singleton()                 
            ;
    }
}