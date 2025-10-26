namespace mvc_practica.Models;

public class AutenticacionDTO
{
    public required string? Usuario { get; set; }
    public required string? Contrasena { get; set; }
    public required string? Rol { get; set; }
}