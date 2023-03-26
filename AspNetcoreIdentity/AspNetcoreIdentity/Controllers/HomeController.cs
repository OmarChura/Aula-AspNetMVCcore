using AspNetcoreIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using AspNetcoreIdentity.Extensions;
using KissLog;
using Microsoft.Extensions.Logging;

namespace AspNetcoreIdentity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IKLogger _logger;

        public HomeController(IKLogger logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            _logger.Trace("Usuario acessou a home!");
            return View();
        }

        //[Authorize]
        public IActionResult Privacy()
        {
            throw new Exception("Erro");
            return View();
        }
        [Authorize(Roles = "Admin, Gestor")]
        public IActionResult Secret()
        {
            try
            {
                throw new Exception("al deu errado");
            }
            catch(Exception e)
            {
                _logger.Error(e);
                throw;
            }

            return View();
        }

        [Authorize(Policy = "PodeExcluir")]
        public IActionResult SecretClaim()
        {
            return View("Secret");
        }

        [Authorize(Policy = "PodeEscrever")]
        public IActionResult SecretClaimGravar()
        {
            return View("Secret");
        }

        [ClaimsAuthorize("Produtos","Ler")]
        public IActionResult ClaimsCustom()
        {
            return View("Secret");
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int  id)
        {
            var modelErro = new ErrorViewModel();

            if(id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro ! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }else if(id == 404)
            {
                modelErro.Mensagem = "A pagina que esta procurando nao existe! <br> />Em caso de duvida entre em contato com nosso suporte.";
                modelErro.Titulo = "Ops! Pagina nao encontrada";
                modelErro.ErroCode = id;
            }else if(id == 403)
            {
                modelErro.Mensagem = "Voce nao tem permissao de fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}