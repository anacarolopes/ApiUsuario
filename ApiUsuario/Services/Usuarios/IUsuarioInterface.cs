using System;
using ApiUsuario.DTO.Login;
using ApiUsuario.DTO.Usuario;
using ApiUsuario.Models;

namespace ApiUsuario.Services;

public interface IUsuarioInterface
{
    Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto);
    Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios();
    Task<ResponseModel<UsuarioModel>> BuscarUsuarioPorId(int id);
    Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto);
    Task<ResponseModel<UsuarioModel>> RemoverUsuario(int id);
    Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto);
}
