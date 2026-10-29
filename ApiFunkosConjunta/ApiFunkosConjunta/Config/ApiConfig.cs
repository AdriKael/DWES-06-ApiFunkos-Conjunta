namespace ApiFunkosConjunta.Config;

/// <summary>
/// Configuración tipada de la API (leída desde la sección "Api" de appsettings).
/// </summary>
public class ApiConfig
{
    /// <summary>Nombre de la API.</summary>
    public string Nombre { get; set; } = "ApiFunkosConjunta";

    /// <summary>Mensaje de bienvenida.</summary>
    public string Mensaje { get; set; } = "API REST de Funkos";
}
