using Hermes.Data;
using Hermes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChamadoController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public ChamadoController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("get_all")]
        public IEnumerable<ChamadoModel> GetChamados()
        {
            IEnumerable<ChamadoModel> chamados = _dapper.LoadData<ChamadoModel>("select * from chamados");
            return chamados;
        }

        [HttpGet("get_one/{id}")]
        public ChamadoModel GetChamado(int id)
        {
            return _dapper.LoadDataSingle<ChamadoModel>($"select * from chamados where id = {id}");
        }

        [HttpGet("get_all_by_loggedin_user")]
        public IEnumerable<ChamadoModel> GetChamadoByUser()
        {
            IEnumerable<ChamadoModel> chamadoByUser = _dapper.LoadData<ChamadoModel>($"select * from chamados where user_id = {User.FindFirst("userId")?.Value}");
            return chamadoByUser;
        }
    }
}
