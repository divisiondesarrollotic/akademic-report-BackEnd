using AkademicReport.Data;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Collections.Generic;

namespace AkademicReport.Service.UsuarioServices
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        public UsuarioService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServicesResponseMessage<string>> Create(UsuarioAddDto usuario)
        {
            try
            {
         

                var usuarios = await CargarUsuarios(usuario.IdPrograma.Value);
                var usuariodb = usuarios.Where(c => c.Correo == usuario.Correo).FirstOrDefault();
                if (usuariodb != null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjUsuarioExiste };
                var userReady = _mapper.Map<Usuario>(usuario);
                 userReady.Contra = "Issu1234";
                _dataContext.Usuarios.Add(userReady);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUsuarioInsertado };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjUsuarioExiste +  ex.ToString()};
            }
            
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var usuariodb = await _dataContext.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
                if (usuariodb == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                usuariodb.SoftDelete = 1;
                _dataContext.Entry(usuariodb).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseData<List<UsuarioGetDto>>> GetAll(int idPrograma)
        {
            try
            {
                var usuariosdb = await CargarUsuarios(idPrograma);
                if (usuariosdb.Count<1)
                    return new ServiceResponseData<List<UsuarioGetDto>>() { Data = _mapper.Map<List<UsuarioGetDto>>(usuariosdb), Message = Msj.MsjNoData,  Status = 204};
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 200, Data=  _mapper.Map<List<UsuarioGetDto>>(usuariosdb) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 500};
            }
        }

        public async Task<ServiceResponseData<UsuarioGetDto>> GetById(int id)
        {
            try
            {
                var usuarios = await _dataContext.Usuarios.Where(c => c.SoftDelete == 0).Include(c => c.IdRecintoNavigation).Include(c => c.NivelNavigation).Include(c => c.IdProgramaNavigation).ToListAsync();

                var usuariodb = _mapper.Map<UsuarioGetDto>(usuarios.Where(c => c.Id == id).FirstOrDefault());
                if (usuariodb==null)
                    return new ServiceResponseData<UsuarioGetDto>() { Status = 204 };
                return new ServiceResponseData<UsuarioGetDto>() { Status = 200, Data =  usuariodb };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<UsuarioGetDto>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<UsuarioGetDto>>> GetByIdRecinto(int id, int idPrograma)
        {
            try
            {
                var usuarios = await CargarUsuarios(idPrograma);
                var usuariodb = usuarios.Where(c => c.IdRecinto == id);
                if (usuariodb == null)
                    return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 204 };
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 200, Data = _mapper.Map<List<UsuarioGetDto>>(usuariodb) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServisResponseLogin<List<UsuarioGetDto>, string >> Login(UsuarioCredentialsDto credentials)
        {
            try
            {

                var usuariodb = await _dataContext.Usuarios.Where(c => c.Correo == credentials.correo && c.Contra == credentials.contra && c.SoftDelete==0).Include(c => c.IdRecintoNavigation).Include(c => c.NivelNavigation).Include(c=>c.IdProgramaNavigation).ToListAsync();
                if (usuariodb == null)
                    return new ServisResponseLogin<List<UsuarioGetDto>, string>() { Status = 204, Message = (_mapper.Map<List<UsuarioGetDto>>(usuariodb), Msj.MsjCredencialesIncorrectas) };


                return new ServisResponseLogin<List<UsuarioGetDto>, string>() { Status = 200, Message = (_mapper.Map<List<UsuarioGetDto>>(usuariodb), "")};
            
            }
            catch (Exception ex)
            {
                return new ServisResponseLogin<List<UsuarioGetDto>, string>() { Status = 500, Message = (new List<UsuarioGetDto>(), Msj.MsjError + ex.ToString())};
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(UsuarioUpdateDto usuario)
        {
            try
            {
                var usuariodb = await _dataContext.Usuarios.AsNoTracking().Where(c => c.Correo == usuario.Correo && c.Id==usuario.Id).ToListAsync();
                if (usuariodb.Count>1)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjUsuarioExiste };
                var usuariodbForUpdate = await _dataContext.Usuarios.AsNoTracking().Where(c=>c.Id == usuario.Id).FirstOrDefaultAsync();
                string beforePasswor = usuariodbForUpdate.Contra;
                int idRecinto = usuariodbForUpdate.IdRecinto;
                usuariodbForUpdate = _mapper.Map<Usuario>(usuario);
                usuariodbForUpdate.Contra = beforePasswor;
                //usuariodbForUpdate.IdRecinto = idRecinto;

                // _dataContext.Usuarios.Add(_mapper.Map<Usuario>(usuariodbForUpdate));
                _dataContext.Entry(usuariodbForUpdate).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();

                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUsuarioInsertado };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjUsuarioExiste + ex.ToString() };
            }
        }

        public async Task<List<Usuario>>CargarUsuarios(int idPrograma)
        {
           
            return await _dataContext.Usuarios.Where(c=>c.SoftDelete==0 && c.IdPrograma==idPrograma).Include(c => c.IdRecintoNavigation).Include(c => c.NivelNavigation).Include(c=>c.IdProgramaNavigation) .ToListAsync();
        }

        public async Task<ServicesResponseMessage<string>> UpdatePassword(UpdatePasswordDto password)
        {
            try
            {
                var usuariodb = await _dataContext.Usuarios.AsNoTracking().Where(c => c.Id == password.IdUsuario && c.Contra == password.CurrentPassword).FirstOrDefaultAsync();
                if (usuariodb==null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = "la contraseña que inserto es incorrecta" };
                if(password.NewPassword!=password.ConfirmPassword)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = "Las contraseñas no coinciden" };
                usuariodb.Contra = password.NewPassword;
                // _dataContext.Usuarios.Add(_mapper.Map<Usuario>(usuariodbForUpdate));
                _dataContext.Entry(usuariodb).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();

                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjUsuarioExiste + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> ResetPassword(int idUsuario)
        {
            try
            {
                var usuariodb = await _dataContext.Usuarios.AsNoTracking().Where(c => c.Id == idUsuario).FirstOrDefaultAsync();
                usuariodb.Contra = "Issu1234";
                _dataContext.Entry(usuariodb).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();

                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjResetPassword };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<NivelUsuario>>> GetNiveles()
        {
              try
            {
                var niveles = await _dataContext.NivelUsuarios.ToListAsync();
                return new ServiceResponseData<List<NivelUsuario>>() { Status = 200, Data = niveles };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<NivelUsuario>>() { Status = 500 };
            }
        }
    }
}
