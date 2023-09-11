using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XFrame.DomainContainers;
using XFrame.Rules.Validations;

namespace XFrame.Rules.Extensions
{
    public static class DomainContainerRulesExtensions
    {
        public static IDomainContainer AddRules(
            this IDomainContainer domainContainer,
            Assembly fromAssembly,
            Predicate<Type> predicate = null)
        {
            predicate = predicate ?? (t => true);
            var ruleTypes = fromAssembly
                .GetTypes()
                .Where(t => t.IsRule())
                .Where(t => predicate(t));

            foreach (var ruleType in ruleTypes)
            {
                domainContainer
                    .ServiceCollection
                    .AddTransient(ruleType);
            }

            return domainContainer
            .AddTypes(ruleTypes);
        }

        private static bool IsRule(this Type type)
        {
            return typeof(IValidation).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }
    }
}
