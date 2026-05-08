using System;
using System.ComponentModel.DataAnnotations;

namespace ApiUsuario.DTO.Usuario;

public class UsuarioCriacaoDto
{
    [Required(ErrorMessage = "Digite o usuário.")]
    public string Usuario { get; set; } = string.Empty;
    [Required(ErrorMessage = "Digite o nome.")]
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    [Required(ErrorMessage = "Digite o email.")]
    public string Email { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Digite a senha."), MinLength(6, 
    ErrorMessage = "A senha deve conter no mínimo 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;
    [Required(ErrorMessage = "A confirmação da senha é obrigatória."), 
    Compare("Senha", ErrorMessage = "A senha e a confirmação de senha devem ser iguais.")]
    public string ConfirmaSenha { get; set; } = string.Empty;
}
