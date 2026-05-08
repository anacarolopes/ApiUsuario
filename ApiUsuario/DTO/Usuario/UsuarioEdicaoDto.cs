using System;
using System.ComponentModel.DataAnnotations;

namespace ApiUsuario.DTO.Usuario;

public class UsuarioEdicaoDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Digite o usuário.")]
    public string Usuario { get; set; } = string.Empty;
    [Required(ErrorMessage = "Digite o nome.")]
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    [Required(ErrorMessage = "Digite o email.")]
    public string Email { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
    
}
