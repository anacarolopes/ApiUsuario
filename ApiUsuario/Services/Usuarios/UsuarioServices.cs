using System;
using ApiUsuario.DTO.Usuario;
using ApiUsuario.Models;
using ApiUsuario.Data;
using Azure;
using ApiUsuario.Services.Senha;
using Microsoft.EntityFrameworkCore;
using ApiUsuario.DTO.Login;

namespace ApiUsuario.Services.Usuarios;

public class UsuarioServices : IUsuarioInterface
{
    private readonly AppDbContext _context;
    private readonly ISenhaInterface _senhaInterface;
    ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
    ResponseModel<List<UsuarioModel>> responseList = new ResponseModel<List<UsuarioModel>>();

    public UsuarioServices(AppDbContext context, ISenhaInterface senhaInterface)
    {
        _context = context;
        _senhaInterface = senhaInterface;
    }    

    public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto)
    {
        try
        {
            if (!VerificaSeExisteEmailUsuarioRepetidos(usuarioCriacaoDto))
            {
                response.Mensagem = "Email ou usuário já cadastrado!";
                response.Status = false;
                return response;
            }

            _senhaInterface.CriarSenhaHash(usuarioCriacaoDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            UsuarioModel usuario = new UsuarioModel
            {
                Nome = usuarioCriacaoDto.Nome,
                Sobrenome = usuarioCriacaoDto.Sobrenome,
                Email = usuarioCriacaoDto.Email,
                Usuario = usuarioCriacaoDto.Usuario,
                SenhaHash = senhaHash,
                SenhaSalt = senhaSalt
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            response.Mensagem = "Usuário cadastrado com sucesso!";
            response.Dados = usuario;
            return response;
        }
        catch (Exception ex)
        {
            response.Mensagem = ex.Message;
            response.Status = false;
            return response;
        }

    }

    public async Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios()
    {
        try
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            responseList.Dados = usuarios;
            responseList.Mensagem = "Usuários listados com sucesso!";
            return responseList;
        }
        catch (Exception ex)
        {
            responseList.Mensagem = ex.Message;
            responseList.Status = false;
            return responseList;
        }
    }

    public async Task<ResponseModel<UsuarioModel>> BuscarUsuarioPorId(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                response.Mensagem = "Usuário não encontrado!";
                return response;
            }
            response.Dados = usuario;
            response.Mensagem = "Usuário localizado com sucesso!";
            return response;
        }
        catch (Exception ex)
        {
            response.Mensagem = ex.Message;
            response.Status = false;
            return response;
        }
    }

     public async Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
    {
        try
        {
            var usuarioBanco = await _context.Usuarios.FindAsync(usuarioEdicaoDto.Id);
            if (usuarioBanco == null)            
            {
                response.Mensagem = "Usuário não encontrado!";
                return response;
            }

            usuarioBanco.Nome = usuarioEdicaoDto.Nome;
            usuarioBanco.Sobrenome = usuarioEdicaoDto.Sobrenome;
            usuarioBanco.Email = usuarioEdicaoDto.Email;
            usuarioBanco.Usuario = usuarioEdicaoDto.Usuario;
            usuarioBanco.DataAlteracao = DateTime.Now;

            _context.Usuarios.Update(usuarioBanco);
            await _context.SaveChangesAsync();

            response.Dados = usuarioBanco;
            response.Mensagem = "Usuário editado com sucesso!";
            return response;
        }
        catch (Exception ex)
        {
            response.Mensagem = ex.Message;
            response.Status = false;
            return response;
        }
    }

    public async Task<ResponseModel<UsuarioModel>> RemoverUsuario(int id)
    {
        try
        {
           var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                response.Mensagem = "Usuário não localizado!";
                return response;
            }

            response.Dados = usuario;
            response.Mensagem = "Usuário deletado com sucesso!";
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return response;
        }
        catch (Exception ex)
        {
            response.Mensagem = ex.Message;
            response.Status = false;
            return response;
        }
    }

    public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
    {
        try
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuarioLoginDto.Email);
            if (usuario == null)
            {
                response.Mensagem = "Usuário não encontrado!";
                response.Status = false;
                return response;
            }

            if(!_senhaInterface.VerificarSenhaHash(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
            {
                response.Mensagem = "Credenciais inválidas!";
                response.Status = false;
                return response;
            }

            var token = _senhaInterface.GerarToken(usuario);
            usuario.Token = token;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            response.Dados = usuario;
            response.Mensagem = "Login realizado com sucesso!";
            
            return response;

        }
        catch (Exception ex)
        {
            response.Mensagem = ex.Message;
            response.Status = false;
            return response;
        }
    }

    private bool VerificaSeExisteEmailUsuarioRepetidos(UsuarioCriacaoDto usuarioCriacaoDto)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == usuarioCriacaoDto.Email || u.Usuario == usuarioCriacaoDto.Usuario);
        if (usuario != null)
        {
            return false;
        }
        return true;
    }
}
