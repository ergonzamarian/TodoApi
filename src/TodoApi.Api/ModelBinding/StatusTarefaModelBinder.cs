using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApi.Domain.Enums;

namespace TodoApi.Api.ModelBinding;

public sealed class StatusTarefaModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var valor = context.ValueProvider.GetValue(context.ModelName).FirstValue;
        if (string.IsNullOrWhiteSpace(valor))
        {
            context.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        if (TentarConverter(valor, out var status))
        {
            context.Result = ModelBindingResult.Success(status);
            return Task.CompletedTask;
        }

        context.ModelState.TryAddModelError(
            context.ModelName,
            "Status inválido. Use: Pendente, Em andamento ou Concluído.");
        context.Result = ModelBindingResult.Failed();
        return Task.CompletedTask;
    }

    public static bool TentarConverter(string valor, out StatusTarefa status)
    {
        var normalizado = valor.Trim().ToLowerInvariant();

        switch (normalizado)
        {
            case "pendente":
                status = StatusTarefa.Pendente;
                return true;
            case "em andamento":
            case "emandamento":
                status = StatusTarefa.EmAndamento;
                return true;
            case "concluído":
            case "concluido":
                status = StatusTarefa.Concluido;
                return true;
            default:
                return Enum.TryParse(valor, ignoreCase: true, out status);
        }
    }
}
