using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Biblioteca.Models;


namespace Biblioteca.Controllers
{
    public class UsuarioController:Controller
    {
        public IActionResult ListaDeUsuarios()
        {
            
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View (new UsuarioService().Listar());
        }

        public IActionResult EditarUsuario(int id)
        {
          
            Usuario u = new UsuarioService().Listar(id);

            return View(u);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuarioEditado)
        {
            
            UsuarioService us = new UsuarioService();
            us.editarUsuario(usuarioEditado);

            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult RegistrarUsuario()
        {
            
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(Usuario novoUsuario)
        {
            
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            novoUsuario.Senha = Criptografo.TextoCriptografado(novoUsuario.Senha);

            UsuarioService us = new UsuarioService();
            us.incluirUsuario(novoUsuario);

            return RedirectToAction("cadastroRealizado");
        }

        public IActionResult ExcluirUsuario (int id)
        {
              
            return View(new UsuarioService().Listar(id));
      
        }

        [HttpPost]
        public IActionResult ExcluirUsuario (string decisao, int id)
        {
            
            if(decisao == "EXCLUIR")
            {
             ViewData["Mensagem"] = "Exclusão do usuario " + new UsuarioService().Listar(id).Nome+" realizada com sucesso!";

            new UsuarioService().excluirUsuario(id);

            return View("ListaDeUsuarios", new UsuarioService().Listar());
            }

            else
            {
                ViewData["Mensagem"] = "Exclusão CANCELADA!";
                 return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
            
        }

        public IActionResult CadastroRealizado()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        public IActionResult NeedAdmin()
        {
            
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Sair()
        {
          
          HttpContext.Session.Clear();
          return RedirectToAction("Index","Home");
        }


    }
}