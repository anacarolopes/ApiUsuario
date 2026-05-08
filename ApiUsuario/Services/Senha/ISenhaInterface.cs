using System;
using ApiUsuario.Models;

namespace ApiUsuario.Services.Senha;

public interface ISenhaInterface
{
    void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
    bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
    string GerarToken(UsuarioModel usuario);
}
