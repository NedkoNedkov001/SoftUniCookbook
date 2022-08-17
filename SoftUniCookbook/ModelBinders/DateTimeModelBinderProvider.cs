using Cookbook.Core.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cookbook.ModelBinders
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        private readonly string customDateFormatter;

        public DateTimeModelBinderProvider(string _customDateFormatter)
        {
            customDateFormatter = _customDateFormatter;
        }
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
            {
                return new DateTimeModelBinder(customDateFormatter);
            }

            return null;
        }
    }
}
