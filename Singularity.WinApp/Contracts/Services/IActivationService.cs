namespace Singularity.WinApp.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
