using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApi.Domain.Enums;

namespace TodoApi.Api.ModelBinding;

public sealed class StatusTarefaModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var tipo = context.Metadata.ModelType;
        if (tipo == typeof(StatusTarefa) || tipo == typeof(StatusTarefa?))
        {
            return new StatusTarefaModelBinder();
        }

        return null;
    }
}
