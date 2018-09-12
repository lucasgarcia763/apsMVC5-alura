using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaelumEstoque.DAO;
using CaelumEstoque.Models;
using CaelumEstoque.Filtros;

namespace CaelumEstoque.Controllers
{
    [AutorizacaoFilter]
    public class ProdutoController : Controller
    {
        [Route("produtos", Name="ListaProdutos")]
        public ActionResult Index()
        {
            ProdutosDAO dao = new ProdutosDAO();
            IList<Produto> produtos = dao.Lista();
            return View(produtos);
        }

        public ActionResult Form()
        {
            CategoriasDAO dao = new CategoriasDAO();
            IList<CategoriaDoProduto> categorias = dao.Lista();
            ViewBag.Categorias = categorias;
            ViewBag.Produto = new Produto();
            return View();
        }

        [HttpPost]
        public ActionResult Adiciona(Produto produto)
        {
            int idDaInformatica = 1;
            if(produto.CategoriaId.Equals(idDaInformatica) && produto.Preco < 100)
            {
                ModelState.AddModelError("produto.Inavalido", "Informática com preço menor que R$ 100,00");
            }

            if(ModelState.IsValid)
            {
                ProdutosDAO dao = new ProdutosDAO();
                dao.Adiciona(produto);
                return RedirectToAction("Index", "Produto");
            }
            else
            {
                ViewBag.Produto = produto;
                CategoriasDAO dao = new CategoriasDAO();
                ViewBag.Categorias = dao.Lista();
                return View("Form");
            }
        }

        [Route("produtos/{id}", Name="VisualizaProduto")]
        public ActionResult Visualiza(int id)
        {
            ProdutosDAO dao = new ProdutosDAO();
            Produto produto = dao.BuscaPorId(id);
            ViewBag.Produto = produto;

            return View();
        }

        public ActionResult DecrementaQtd(int id)
        {
            ProdutosDAO dao = new ProdutosDAO();
            Produto produto = dao.BuscaPorId(id);
            produto.Quantidade--;
            dao.Atualiza(produto);

            return Json(produto);
        }
    }
}