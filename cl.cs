//------------------------------------------------if -------------shared/_Layout.cshtml

body>
    <nav class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a href="/Dashboard" class="navbar-brand">Survey NET</a>
            </div>
            @if (Context.Request.Cookies["idUsuario"] != null && Context.Request.Cookies["idUsuario"] != "") //<-----
            {
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li><a href="/Dashboard">Dashboard</a></li>
                    <li><a href="/Questionario">Questionários</a></li>
                    <li><a href="/Pergunta">Perguntas</a></li>
                    <li><a href="#">Relatórios</a></li>
                </ul>
                <ul id="navbar2" class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="" class="dropdown-toggle" data-toggle="dropdown">
                            <span class="glyphicon glyphicon-user"></span> @Context.Request.Cookies["nomeUsuario"].ToString()<span class="caret"></span>//<---
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="#">Alterar dados</a></li>
                            <li><a href="/Home/Logout">Sair</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            }
        </div>
    </nav>
    <div class="container body-content" style="padding-top:20px">
        <div class="loading" id="divLoading">
            <img src="~/images/loading.gif" style="width:32px" alt="loading..." /> Loading...
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title"><b>@ViewBag.Title</b></h3>
            </div>
            <div class="panel-body">
                @RenderBody()
            </div>
        </div>
        <footer>
            <p>&copy; @DateTime.Now.Year - Survey NET</p>
        </footer>
    </div>

    <environment include="Development">
        <script src="~/lib/jquery/dist/jquery.js"></script>
        <script src="~/lib/bootstrap/dist/js/bootstrap.js"></script>
        <script src="~/js/site.js" asp-append-version="true"></script>
    </environment>
    <environment exclude="Development">
        <script src="https://ajax.aspnetcdn.com/ajax/jquery/jquery-2.2.0.min.js"
                asp-fallback-src="~/lib/jquery/dist/jquery.min.js"
                asp-fallback-test="window.jQuery"
                crossorigin="anonymous"
                integrity="sha384-K+ctZQ+LL8q6tP7I94W+qzQsfRV2a+AfHIi9k8z8l9ggpc8X+Ytst4yBo/hH+8Fk">
        </script>
        <script src="https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/bootstrap.min.js"
                asp-fallback-src="~/lib/bootstrap/dist/js/bootstrap.min.js"
                asp-fallback-test="window.jQuery && window.jQuery.fn && window.jQuery.fn.modal"
                crossorigin="anonymous"
                integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa">
        </script>
        <script src="~/js/site.min.js" asp-append-version="true"></script>
    </environment>
    <script src="~/lib/fancybox/jquery.fancybox.min.js" type="text/javascript"></script>
    <script src="~/lib/bootbox/bootbox.min.js" type="text/javascript"></script>

    @RenderSection("Scripts", required: false)
</body>



//------------------------------------------------Filter    criar nova pasta Filters---------------


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Filters
{
    public class ValidarUsuario : Attribute, IActionFilter, IOrderedFilter
    {
        public int Order { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var nomeController = (string)context.RouteData.Values["controller"];
            //var nomeAction = (string)context.RouteData.Values["action"];
            //var ipCliente = context.HttpContext.Connection.RemoteIpAddress;
            //var browser = context.HttpContext.Request.Headers["User-Agent"].ToString();
            //var urlReferrer = context.HttpContext.Request.Headers["Referer"].ToString();

            if (context.HttpContext.Request.Cookies["idUsuario"] == null || 
                context.HttpContext.Request.Cookies["idUsuario"] == "")
            {
                context.Result = new RedirectResult("/Home/Logout");
            }
        }
    }
}







//------------------------------------------------Model Usuario---------------------------------


using Survey.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Survey.Models
{
    internal class Usuario
    {
        #region Definição das propriedades
        private int _id;
        private string _nome;
        private string _email;
        private string _senha;
        private DateTime _dataCadastro;
        private DateTime? _dataFim;
        //private List<Questionario> _questionarios;

        internal int Id { get => _id; set => _id = value; }
        internal string Nome { get => _nome; set => _nome = value; }
        internal string Email { get => _email; set => _email = value; }
        internal string Senha { get => _senha; set => _senha = value; }
        internal DateTime DataCadastro { get => _dataCadastro; set => _dataCadastro = value; }
        internal DateTime? DataFim { get => _dataFim; set => _dataFim = value; }
        #endregion

        internal Usuario Autenticar(string email, string senha)
        {
            if (email.Contains("@") && senha.Length > 0)
                return new UsuarioDAO().Autenticar(email, senha);
            else
                return null;
        }

        internal int Gravar()
        {
            if (this.Id == 0 && this.Nome.Length >= 3 &&
                this.Senha != "")
                return new UsuarioDAO().Gravar(this);
            else
                return -10;
        }
    }
}

//------------------------------------------------Model Questionario--------------------------------

using Survey.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Survey.Models
{
    internal class Questionario
    {
        private int _id;
        private string _nome;
        private DateTime _inicio;
        private DateTime _fim;
        private string _msgFeedback;
        private string _guid;
        private int _usuarioId;
        private Usuario _usuario;
        //private List<Pergunta> _perguntas;

        internal int Id { get => _id; set => _id = value; }
        internal string Nome { get => _nome; set => _nome = value; }
        internal DateTime Inicio { get => _inicio; set => _inicio = value; }
        internal DateTime Fim { get => _fim; set => _fim = value; }
        internal string MsgFeedback { get => _msgFeedback; set => _msgFeedback = value; }
        internal string Guid { get => _guid; set => _guid = value; }
        internal int UsuarioId { get => _usuarioId; set => _usuarioId = value; }
        internal Usuario Usuario { get => _usuario; set => _usuario = value; }

        internal List<Questionario> ObterPorUsuario(int id)
        {
            if (id > 0)
                return new QuestionarioDAO().ObterPorUsuario(id);
            else
                return null;
        }

        internal List<Questionario> ObterPorPalavraChave(string palavra, int idUsuario)
        {
            if (palavra.Length > 0)
                return new QuestionarioDAO().ObterPorPalavraChave(palavra, idUsuario);
            else
                return null;
        }

        internal Questionario Obter(int id)
        {
            if (id > 0)
                return new QuestionarioDAO().Obter(id);
            else
                return null;
        }

        internal int Gravar()
        {
            if (_id >= 0 && _nome.Length > 0 && _inicio > DateTime.MinValue && _fim > DateTime.MinValue &&
                _guid.Length > 0 && _usuarioId > 0)
                return new QuestionarioDAO().Gravar(this);
            else
                return -10;
        }

        internal int Excluir(int id)
        {
            if (id > 0)
                return new QuestionarioDAO().Excluir(id);
            else
                return -10;
        }



    }
}


//------------------------------------------------DAO Usuario-----------------------------------


using Survey.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace Survey.DAL
{
    internal class UsuarioDAO : Banco
    {
        private List<Usuario> TableToList(DataTable dt)
        {
            List<Usuario> dados = null;
            if (dt != null && dt.Rows.Count > 0)
                dados = (from DataRow linha in dt.Rows
                         select new Usuario()
                         {
                             Id = Convert.ToInt32(linha["Id"]),
                             Nome = linha["Nome"].ToString(),
                             Email = linha["Email"].ToString(),
                             Senha = linha["Senha"].ToString(),
                             DataCadastro = Convert.ToDateTime(linha["DataCadastro"]),
                             DataFim = linha["DataFim"] is DBNull ? (DateTime?)null : Convert.ToDateTime(linha["DataFim"])
                         }).ToList();
            return dados;
        }

        internal Usuario Autenticar(string email, string senha)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select * from USUARIO 
                where Email = @email and
                      Senha = @senha and
                      DataFim is null";
            ComandoSQL.Parameters.AddWithValue("@email", email);
            ComandoSQL.Parameters.AddWithValue("@senha", senha);

            DataTable dt = ExecutaSelect();
            var dados = TableToList(dt);
            return dados == null ? null : dados.FirstOrDefault();
        }

        internal int Gravar(Usuario u)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"insert into USUARIO 
                (Nome, Email, Senha, DataCadastro) 
                values (@nome, @email, @senha, @dataCadastro)";
            ComandoSQL.Parameters.AddWithValue("@nome", u.Nome);
            ComandoSQL.Parameters.AddWithValue("@email", u.Email);
            ComandoSQL.Parameters.AddWithValue("@senha", u.Senha);
            ComandoSQL.Parameters.AddWithValue("@dataCadastro", u.DataCadastro);

            return ExecutaComando();
        }
    }
}


//------------------------------------------------DAO QUESTIONARIO-------------------------------


using Survey.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace Survey.DAL
{
    internal class QuestionarioDAO : Banco
    {
        private List<Questionario> TableToList(DataTable dt)
        {
            List<Questionario> dados = null;
            if (dt != null && dt.Rows.Count > 0)
                dados = (from DataRow linha in dt.Rows
                         select new Questionario()
                         {
                             Id = Convert.ToInt32(linha["Id"]),
                             Nome = linha["Nome"].ToString(),
                             Inicio = Convert.ToDateTime(linha["Inicio"]),
                             Fim = Convert.ToDateTime(linha["Fim"]),
                             MsgFeedback = linha["MsgFeedback"].ToString(),
                             Guid = linha["Guid"].ToString(),
                             UsuarioId = Convert.ToInt32(linha["UsuarioId"]),
                             Usuario = null
                         }).ToList();
            return dados;
        }

        internal List<Questionario> ObterPorUsuario(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select * 
                from QUESTIONARIO 
                where UsuarioId = @id
                order by Inicio";
            ComandoSQL.Parameters.AddWithValue("@id", id);

            DataTable dt = ExecutaSelect();
            return TableToList(dt);
        }

        internal List<Questionario> ObterPorPalavraChave(string palavra, int idUsuario)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select Id, Nome, Inicio, Fim, MsgFeedback, Guid, Imagem, UsuarioId 
                                        from Questionario 
                                        where Nome like @palavra and UsuarioId = @idUsuario
                                        order by Nome, Inicio";
            ComandoSQL.Parameters.AddWithValue("@palavra", "%" + palavra + "%");
            ComandoSQL.Parameters.AddWithValue("@idUsuario", idUsuario);
            DataTable dt = ExecutaSelect();
            return TableToList(dt);
        }

        internal Questionario Obter(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select Id, Nome, Inicio, Fim, MsgFeedback, Guid, Imagem, UsuarioId 
                                        from Questionario 
                                        where Id = @id";
            ComandoSQL.Parameters.AddWithValue("@id", id);
            DataTable dt = ExecutaSelect();
            var dados = TableToList(dt);
            return dados == null ? null : dados.FirstOrDefault();
        }


        internal int Gravar(Questionario q)
        {
            ComandoSQL.Parameters.Clear();
            if (q.Id == 0)
                ComandoSQL.CommandText = @"insert into Questionario (Nome, Inicio, Fim, MsgFeedback, Guid, UsuarioId) 
                                    values (@nome, @inicio, @fim, @msgFeedback, @guid, @usuarioId)";
            else
            {
                ComandoSQL.CommandText = @"update Questionario set Nome = @nome, Inicio = @inicio, Fim = @fim, 
                                    MsgFeedback = @msgFeedback, Guid = @guid, UsuarioId = @usuarioId 
                                    where Id = @id";
                ComandoSQL.Parameters.AddWithValue("@id", q.Id);
            }
            ComandoSQL.Parameters.AddWithValue("@nome", q.Nome);
            ComandoSQL.Parameters.AddWithValue("@inicio", q.Inicio);
            ComandoSQL.Parameters.AddWithValue("@fim", q.Fim);
            ComandoSQL.Parameters.AddWithValue("@msgFeedback", q.MsgFeedback);
            ComandoSQL.Parameters.AddWithValue("@guid", q.Guid);
            ComandoSQL.Parameters.AddWithValue("@usuarioId", q.UsuarioId);
            return ExecutaComando();
        }

        internal int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"delete from Questionario where id = @id";
            ComandoSQL.Parameters.AddWithValue("@id", id);
            return ExecutaComando();
        }


    }
}


//------------------------------------------------CONTROLLER USUARIO-------------------------------------

using Survey.Models;
using Survey.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Survey.Controllers
{
    public class UsuarioController
    {
        /// <summary>
        /// Autenticar um usuário para acesso ao sistema
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns></returns>
        public UsuarioViewModel Autenticar(string email, string senha)
        {
            Usuario u = new Usuario().Autenticar(email, senha);
            if (u != null)
                return new UsuarioViewModel()
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Senha = u.Senha,
                    DataCadastro = u.DataCadastro,
                    DataFim = u.DataFim
                };
            else
                return null;
        }

        public int Gravar(UsuarioViewModel u)
        {
            Usuario usuario = new Usuario()
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Senha = u.Senha,
                DataCadastro = u.DataCadastro,
                DataFim = u.DataFim
            };

            return usuario.Gravar();
        }
    }
}



//------------------------------------------------CONTROLLER QUESTIONARIO-----------------------------


using Survey.Models;
using Survey.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Survey.Controllers
{
    public class QuestionarioController
    {
        public List<QuestionarioViewModel> ObterPorUsuario(int id)
        {
            var dados = new Questionario().ObterPorUsuario(id);
            if (dados != null && dados.Count > 0)
                return (from d in dados
                        select new QuestionarioViewModel()
                        {
                            Id = d.Id,
                            Nome = d.Nome,
                            Inicio = d.Inicio,
                            Fim = d.Fim,
                            Guid = d.Guid,
                            MsgFeedback = d.MsgFeedback,
                            UsuarioId = d.UsuarioId,
                            Usuario = null
                        }).ToList();
            else
                return null;
        }

        public QuestionarioViewModel Obter(int id)
        {
            var dados = new Questionario().Obter(id);
            if (dados != null)
                return new QuestionarioViewModel()
                {
                    Id = dados.Id,
                    Nome = dados.Nome,
                    Inicio = dados.Inicio,
                    Fim = dados.Fim,
                    MsgFeedback = dados.MsgFeedback,
                    Guid = dados.Guid,
                    UsuarioId = dados.UsuarioId,
                    Usuario = null
                };
            else
                return null;
        }

        public List<QuestionarioViewModel> ObterPorPalavraChave(string palavra, int idUsuario)
        {
            var dados = new Questionario().ObterPorPalavraChave(palavra, idUsuario);
            if (dados != null && dados.Count > 0)
                return (from d in dados
                        select new QuestionarioViewModel()
                        {
                            Id = d.Id,
                            Nome = d.Nome,
                            Inicio = d.Inicio,
                            Fim = d.Fim,
                            MsgFeedback = d.MsgFeedback,
                            Guid = d.Guid,
                            UsuarioId = d.UsuarioId,
                            Usuario = null
                        }).ToList();
            else
                return null;
        }

        public int Gravar(QuestionarioViewModel questionario)
        {
            Questionario q = new Questionario();
            q.Id = questionario.Id;
            q.Nome = questionario.Nome;
            q.Inicio = questionario.Inicio;
            q.Fim = questionario.Fim;
            q.MsgFeedback = questionario.MsgFeedback;
            q.Guid = questionario.Guid;
            q.UsuarioId = questionario.UsuarioId;

            return q.Gravar();
        }

        public int Excluir(int id)
        {
            Questionario q = new Questionario();
            return q.Excluir(id);
        }


    }
}

//-------------------------------------------------------ViewModels--------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Survey.ViewModels
{
    public class QuestionarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public string MsgFeedback { get; set; }
        public string Guid { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        //public List<PerguntaViewModel> Perguntas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Survey.ViewModels
{
    /// <summary>
    /// Representa um usuário do sistema
    /// </summary>
    public class UsuarioViewModel
    {
        /// <summary>
        /// Identificação única do usuário
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Senha de acesso (não criptografada)
        /// </summary>
        public string Senha { get; set; }
        /// <summary>
        /// Data do cadastro
        /// </summary>
        public DateTime DataCadastro { get; set; }
        /// <summary>
        /// Preencher essa data quando quiser "excluir" o usuário
        /// </summary>
        public DateTime? DataFim { get; set; }
        //public List<QuestionarioViewModel> Questionarios { get; set; }
    }
}


//-------------------------------------------------------Controller Home Web------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cl = Survey.Controllers;
using Survey.ViewModels;

namespace SurveyWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //TipoPerguntaController ctlTipoPergunta = 
            //    new TipoPerguntaController();

            //ctlTipoPergunta.Excluir(10);

            return View();
        }

        [HttpPost]
        public IActionResult Validar(string Email, string Senha)
        {
            if (Email != "" && Senha != "")
            {
                cl.UsuarioController ctlUsuario = new cl.UsuarioController();
                var usuario = ctlUsuario.Autenticar(Email, Senha);
                if (usuario != null)
                {
                    //CookieOptions ck = new CookieOptions();
                    //ck.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Append("idUsuario", usuario.Id.ToString());
                    Response.Cookies.Append("nomeUsuario", usuario.Nome);

                    return Json("");
                }
                else
                {
                    return Json("O usuário e/ou a senha informados não conferem.");
                }
            }
            else
            {
                return Json("Por favor, informe um usuário e uma senha para acesso.");
            }
        }

        [HttpPost]
        public IActionResult Gravar(IFormCollection form)
        {
            if (form["Email"] != "" && form["Nome"].ToString().Length > 2
                && form["Senha"].ToString().Length > 0)
            {
                cl.UsuarioController ctlUsuario = new cl.UsuarioController();
                UsuarioViewModel usuario = new UsuarioViewModel()
                {
                    Id = 0,
                    Nome = form["Nome"],
                    Email = form["Email"],
                    Senha = form["Senha"],
                    DataCadastro = DateTime.Now,
                    DataFim = null
                };
                if (ctlUsuario.Gravar(usuario) > 0)
                    return Json("");
                else
                    return Json("Erro ao gravar o novo usuário");
            }
            else
                return Json("Por favor, informe todos os dados do formulário.");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("idUsuario");
            Response.Cookies.Delete("nomeUsuario");

            return RedirectToAction("Index", "Home");
        }

    }
}



//-------------------------------------------------------Controller Questionario Web------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyWeb.Filters;
using cl = Survey.Controllers;
using Survey.ViewModels;

namespace SurveyWeb.Controllers
{
    [ValidarUsuario]
    public class QuestionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult ObterPorUsuario()
        {
            int idUsuario = int.Parse(Request.Cookies["idUsuario"].ToString());
            var dados = new cl.QuestionarioController().ObterPorUsuario(idUsuario);
            return Json(dados);
        }
        [HttpPost]
        public JsonResult ObterPorPalavraChave(string palavra)
        {
            var dados = new cl.QuestionarioController().ObterPorPalavraChave(palavra, Convert.ToInt32(Request.Cookies["idUsuario"]));
            return dados == null ? Json("") : Json(dados);
        }

        [HttpPost]
        public JsonResult Gravar(IFormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                int id = 0;
                int.TryParse(form["Id"], out id);
                string nome = form["Nome"];
                DateTime inicio = DateTime.MinValue;
                DateTime.TryParse(form["Inicio"], out inicio);
                DateTime fim = DateTime.MinValue;
                DateTime.TryParse(form["Fim"], out fim);
                string msgFeedback = form["MsgFeedBack"];
                string guid = form["Guid"];
                int idUsuario = int.Parse(Request.Cookies["idUsuario"].ToString());

                QuestionarioViewModel q = new QuestionarioViewModel();
                q.Id = id;
                q.Nome = nome;
                q.Inicio = inicio;
                q.Fim = fim;
                q.MsgFeedback = msgFeedback;
                q.Guid = guid;
                q.UsuarioId = idUsuario;

                cl.QuestionarioController ctlQuestionario = new cl.QuestionarioController();
                if (ctlQuestionario.Gravar(q) > 0)
                    return Json("");
                else
                    return Json("Erro ao gravar o questionário: " + q.Nome.ToUpper());
            }
            else
            {
                return Json("O formulário submetido não contem valores válidos.");
            }
        }

        [HttpPost]
        public JsonResult Obter(int id)
        {
            var dados = new cl.QuestionarioController().Obter(id);
            return dados == null ? Json("") : Json(dados);
        }

        [HttpPost]
        public JsonResult Excluir(int id)
        {
            cl.QuestionarioController ctlQuestionario = new cl.QuestionarioController();
            if (ctlQuestionario.Excluir(id) > 0)
                return Json("");
            else
                return Json("Não foi possível excluir o registro selecionado.");
        }



    }
}



//-------------------------------------------------------View Home Web--------------------------------

@{
    ViewBag.Title = "Autenticação do Usuário";
}
@section Scripts {
    <script type="text/javascript" src="~/js/Util.js"></script>
    <script type="text/javascript" src="~/js/Views/Home.js"></script>
}

<form name="formValidar" id="formValidar">
    <div class="form-group">
        <label for="txtEmail">Informe seu e-mail:</label>
        <input type="email" name="txtEmail" id="txtEmail"
               required class="form-control" />
    </div>
    <div class="form-group">
        <label for="txtSenha">Senha de acesso:</label>
        <input type="password" name="txtSenha" id="txtSenha"
               class="form-control" />
    </div>
    <div class="form-inline">
        <button type="button" name="btnValidarAcesso" id="btnValidarAcesso" class="btn btn-primary">Validar acesso</button>
        <a role="button" class="btn btn-success" data-fancybox data-src="#novoUsuario" href="javascript:;">Criar novo usuário</a>
    </div>
    <div class="form-group">
        <label>
            <a href="javascript:;" data-fancybox data-src="#esqueciSenha"> Esqueci minha senha</a>
        </label>
    </div>
    <br />
    <div id="divAlerta" class="alert alert-danger" role="alert" style="display:none"></div>
</form>

@*Fancybox NOVO USUÁRIO*@
<div style="display: none; min-width: 300px; width: 600px" id="novoUsuario">
    <h3>Novo Usuário</h3>
    <form>
        <div class="form-group">
            <label for="txtEmail">Informe seu e-mail:</label>
            <input type="email" required name="txtEmailNovo" id="txtEmailNovo" class="form-control" />
        </div>
        <div class="form-group">
            <label for="txtNome">Nome:</label>
            <input type="text" required name="txtNomeNovo" id="txtNomeNovo" class="form-control" />
        </div>
        <div class="form-group">
            <label for="txtSenha">Defina sua senha:</label>
            <input type="password" required name="txtSenhaNovo" id="txtSenhaNovo" class="form-control" />
        </div>
        <div class="form-group">
            <label for="txtSenha2">Confirme sua senha:</label>
            <input type="password" required name="txtSenhaNovo2" id="txtSenhaNovo2" class="form-control" />
        </div>
        <div class="form-inline">
            <button type="button" name="btnConfirmar" id="btnConfirmar" class="btn btn-success">Confirmar</button>
            <button type="button" name="btnCancelar" id="btnCancelar" class="btn btn-danger"
                    onclick="javascript: $.fancybox.close();">
                Cancelar
            </button>
        </div>
    </form>
    <div id="divAlertaNovoUsuario" class="alert alert-danger" role="alert" style="display:none"></div>
</div>

@*FancyBox ESQUECI MINHA SENHA*@
<div style="display: none; min-width: 300px; width: 600px" id="esqueciSenha">
    <h3>Recuperar minha senha</h3>
    <form>
        <div class="form-group">
            <label for="txtEmailRecuperar">Informe seu e-mail:</label>
            <input type="email" name="txtEmailRecuperar" id="txtEmailRecuperar" class="form-control" />
        </div>
        <div class="form-inline">
            <button type="button" name="btnEnviarRecuperar" id="btnEnviarRecuperar" class="btn btn-success">Recuperar senha</button>
            <button type="button" name="btnCancelarRecuperar" id="btnCancelarRecuperar" class="btn btn-danger"
                    onclick="javascript: $.fancybox.close();">
                Cancelar
            </button>
        </div>
    </form>
</div>


//-------------------------------------------------------View Questionario Web-----------------------


@{
    ViewBag.Title = "Meus Questionários";
}
@section Scripts {
    <script src="~/js/Util.js"></script>
    <script src="~/js/Views/Questionario.js"></script>
}

<form>
    <div class="row">
        <div class="col-sm-9">
            <input type="text" name="txtPalavraChave" id="txtPalavraChave" value=""
                   class="form-control" placeholder="Procurar..." />
        </div>
        <div class="col-sm-1">
            <button type="button" id="btnPesquisar" class="btn btn-primary">Pesquisar</button>
        </div>
        <div class="col-sm-2">
            <a role="button" class="btn btn-success"
               data-fancybox data-src="#formQuestionario"
               href="javascript:;">Novo questionário</a>
        </div>
    </div>
</form>
<hr />
<h3>Questionários Cadastrados</h3>
<table id="tableQuestionarios" class="table table-striped table-responsive table-condensed table-hover"></table>
@*<div id="divExportarPDF">
    <a role="button" id="linkPDF" class="btn btn-primary" href="#">Exportar para PDF</a>
</div>*@

@*Fancybox FORM QUESTIONÁRIO*@
<div style="display: none; min-width: 300px; width: 800px" id="formQuestionario">
    <h3>Dados do Questionário</h3>
    <form>
        <input type="hidden" name="txtId" id="txtId" value="0" />
        <div class="form-group">
            <label for="txtTitulo">Título:</label>
            <input type="text" required name="txtTitulo" id="txtTitulo" class="form-control" />
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label for="txtDataInicio">Início:</label>
                    <input type="date" name="txtDataInicio" id="txtDataInicio" class="form-control" />
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label for="txtDataFim">Fim:</label>
                    <input type="date" name="txtDataFim" id="txtDataFim" class="form-control" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="txtFeedback">Feedback ao final do questionário:</label>
            <textarea class="form-control" name="txtFeedback" id="txtFeedback" rows="4"></textarea>
        </div>
        <div class="form-group">
            <label for="txtSenha">Guid para acesso ao questionário:</label>
            <div class="row">
                <div class="col-sm-4">
                    <label>http://www.seudominio.com/survey/</label>
                </div>
                <div class="col-sm-8">
                    <input type="text" required name="txtGuid" id="txtGuid" class="form-control" />
                </div>
            </div>
        </div>
        <div class="form-inline">
            <button type="button" name="btnConfirmar" id="btnConfirmar" class="btn btn-success">Confirmar</button>
            <button type="button" name="btnCancelar" id="btnCancelar" class="btn btn-danger"
                    onclick="javascript: LimparFormulario(); $.fancybox.close();">
                Cancelar
            </button>
        </div>
    </form>
    <div id="divAlertaNovoQuestionario" class="alert alert-danger" role="alert" style="display:none"></div>
</div>



//-------------------------------------------------------JS home Web ---- wwwroot/js/Views/Home.js


$("#btnValidarAcesso").click(function () {
    var msg = "";
    var email = $("#txtEmail").val();
    var senha = $("#txtSenha").val();
    if (email == "") {
        msg = "Por favor, informe o e-mail para autenticação.<br />";
    }
    if (senha == "") {
        msg += "Por favor, informe uma senha para o processo de autenticação.";
    }
    if (msg.length > 0) {
        Mensagem("divAlerta", msg);
    }
    else {
        $("#divLoading").show(300);
        $.ajax({
            type: 'POST',
            url: '/Home/Validar',
            data: { Email: email, Senha: senha },
            success: function (result) {
                $("#divLoading").hide(300);
                if (result.length > 0) {
                    Mensagem("divAlerta", result)
                }
                else {
                    window.location.href = "/Dashboard";
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                $("#divLoading").hide(300);
            }
        });
    }
});

$("#btnConfirmar").click(function () {
    var msg = "";
    var email = $("#txtEmailNovo").val();
    var nome = $("#txtNomeNovo").val();
    var senha = $("#txtSenhaNovo").val();
    var senha2 = $("#txtSenhaNovo2").val();
    if (email == "") {
        msg += "Por favor, informe um e-mail para o usuário.<br />";
    }
    if (nome.length < 3) {
        msg += "Por favor, informe um nome válido para o usuário.<br />";
    }
    if (senha.length < 6) {
        msg += "Por favor, informe uma senha com pelo menos 6 caracteres.<br />";
    }
    if (senha2 != senha) {
        msg += "A senha e a confirmação da senha não conferem.<br />"
    }
    if (msg.length > 0) {
        Mensagem("divAlertaNovoUsuario", msg);
    }
    else {
        $("#divLoading").show(300);
        $.ajax({
            type: 'POST',
            url: '/Home/Gravar',
            data: { Email: email, Nome: nome, Senha: senha },
            success: function (result) {
                $("#divLoading").hide(300);
                if (result.length > 0) {
                    Mensagem("divAlertaNovoUsuario", result)
                }
                else {
                    //window.location.href = "/Home";
                    $.fancybox.close();
                    $("#txtEmail").val(email);
                    $("#txtSenha").val(senha);
                    Mensagem("divAlerta", "Autentique-se com o seu novo usuário e senha");
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                $("#divLoading").hide(300);
            }
        });
    }
});




//-------------------------------------------------------JS Questionarios Web---- wwwroot/js/Views/Questionario.js

function PreencherTabela(dados) {
    var txt = '<thead>\
            <tr>\
                <th>#ID</th>\
                <th>Título</th>\
                <th>Início</th>\
                <th>Fim</th>\
                <th>Guid</th>\
                <th>...</th>\
            </tr>\
        </thead >\
        <tbody>';
    $.each(dados, function () {
        txt += '<tr><td>' + this.id + '</td><td>' + this.nome + '</td><td>' + FormatarData(this.inicio) +
            '</td><td>' + FormatarData(this.fim) + '</td><td>' + this.guid + '</td><td>\
                <a role="button" class="btn btn-warning" href="javascript:Alterar(' + this.id + ')">Alterar</a>\
                <a role="button" class="btn btn-danger" href="javascript:Excluir(' + this.id + ')">Excluir</a>\
                </td></tr>';
    });
    txt += '</tbody>';
    $("#tableQuestionarios").html(txt);
};

function ObterQuestionarios() {
    $("#divLoading").show(300);
    $.getJSON("/Questionario/ObterPorUsuario/", function (data) {
        PreencherTabela(data);
    });
    $("#divLoading").hide(300);
};

$(document).ready(function () {
    ObterQuestionarios();
});

$("#btnPesquisar").click(function () {
    if ($("#txtPalavraChave").val() == "") {
        ObterQuestionarios();
    }
    else {
        $("#divLoading").show(300);
        $.ajax({
            type: 'POST',
            url: '/Questionario/ObterPorPalavraChave',
            data: { Palavra: $("#txtPalavraChave").val() },
            success: function (result) {
                if (result != null && result.length > 0) {
                    PreencherTabela(result);
                }
                else {
                    bootbox.alert("Nenhum questionário encontrado.");
                }
                $("#divLoading").hide(300);
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                $("#divLoading").hide(300);
            }
        });
    }
});

$("#btnConfirmar").click(function () {
    var msg = "";
    var id = $("#txtId").val();
    var titulo = $("#txtTitulo").val();
    var inicio = $("#txtDataInicio").val();
    var fim = $("#txtDataFim").val();
    var feedback = $("#txtFeedback").val();
    var guid = $("#txtGuid").val();

    if (titulo == "") {
        msg += "Por favor, informe um título para o questionário.<br />";
    }
    if (inicio == "") {
        msg += "Por favor, informe a data para início do questionário.<br />";
    }
    if (fim == "") {
        msg += "Por favor, informe a data para fechamento do questionário.<br />";
    }
    if (guid == "") {
        msg += "Por favor, informe a Guid (URL) para o questionário.<br />";
    }
    if (msg.length > 0) {
        Mensagem("divAlertaNovoQuestionario", msg);
    }
    else {
        $("#divLoading").show(300);
        $.ajax({
            type: 'POST',
            url: '/Questionario/Gravar',
            data: { Id: id, Nome: titulo, Inicio: inicio, Fim: fim, MsgFeedBack: feedback, Guid: guid },
            success: function (result) {
                $("#divLoading").hide(300);
                if (result.length > 0) {
                    Mensagem("divAlertaNovoQuestionario", result);
                }
                else {
                    LimparFormulario();
                    $.fancybox.close();
                    ObterQuestionarios();
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                $("#divLoading").hide(300);
            }
        });
    }
});

function Alterar(id) {
    $("#divLoading").show(300);
    $.ajax({
        type: 'POST',
        url: '/Questionario/Obter',
        data: { Id: id },
        success: function (result) {
            if (Object.keys(result).length > 0) {
                $("#divLoading").hide(300);
                $.fancybox.open({
                    src: '#formQuestionario',
                    type: 'inline'
                });
                $("#txtId").val(result.id);
                $("#txtTitulo").val(result.nome);
                $("#txtDataInicio").val(FormatarDataIso(result.inicio));
                $("#txtDataFim").val(FormatarDataIso(result.fim));
                $("#txtFeedback").val(result.msgFeedback);
                $("#txtGuid").val(result.guid);
            }
            else {
                $("#divLoading").hide(300);
            }
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            alert("Status: " + txtStatus); alert("Error: " + errorThrown);
            $("#divLoading").hide(300);
        }
    });
};

function Excluir(id) {
    bootbox.confirm({
        message: "Confirma a exclusão deste registro?",
        buttons: {
            confirm: {
                label: 'Sim',
                className: 'btn-success'
            },
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: '/Questionario/Excluir',
                    data: { Id: id },
                    success: function (result) {
                        if (result == "") {
                            ObterQuestionarios();
                        }
                        else {
                            Mensagem("divAlerta", result);
                        }
                    },
                    error: function (XMLHttpRequest, txtStatus, errorThrown) {
                        alert("Status: " + txtStatus); alert("Error: " + errorThrown);
                        $("#divLoading").hide(300);
                    }
                });
            }
        }
    });
};

function LimparFormulario() {
    $("#txtId").val("0");
    $("input[type='text']").val("");
    $("input[type='date']").val("0000-00-00");
    $("textarea").val("");
}