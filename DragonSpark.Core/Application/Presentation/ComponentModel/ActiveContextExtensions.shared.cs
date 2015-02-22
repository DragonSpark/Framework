namespace DragonSpark.Application.Presentation.ComponentModel
{
    public static class ActiveContextExtensions
    {
        public static bool IsActive( this IActiveContext target )
        {
            return target.IsActive;
        }
    }
}