using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cikkek.Models;
using CsvHelper;
using System.IO;

namespace Cikkek.Controllers
{
    public class HomeController : Controller
    {
        private static List<Product> _allProducts = new List<Product>();
        private static List<Product> _currentProducts = new List<Product>();
        private static int rendezes = 0;
        private static int tol = 0;
        private static int meret = 20;

        static HomeController()
        {
            /*
           string[] lines = System.IO.File.ReadAllLines(@"C:\Users\blghktt\Downloads\cikkek.csv");

           foreach (string st in lines)
           {
               string[] s = st.Replace("\"", "").Split(';');
               _products.Add(new Product(s[0], s[1], s[2], s[3]));
           }
           */
            //@"C:\Users\blghktt\Downloads\cikkek.csv"
            ;
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"App_Data\cikkek.csv"))
            {
                var csv = new CsvReader(sr);
                csv.Configuration.Delimiter = ";";
                IEnumerable<Product> record = csv.GetRecords<Product>();

                foreach (var rec in record)
                {
                    _allProducts.Add(new Product(rec.Cikknev, rec.Cikkszam, rec.Vonalkod, rec.MennyisegiEgyseg));
                }
            }
            _currentProducts = _allProducts;

        }

        [HttpGet]
        public ActionResult Index(string search)
        {
            if (search == string.Empty)
            {
                tol = 0;
                _currentProducts = _allProducts;
            }


            else if (!string.IsNullOrEmpty(search))
            {
                _currentProducts = _allProducts.Where(p => p.Cikknev.Contains(search) || p.Cikkszam.Contains(search)).ToList();
                tol = 0;
            }
               

            return View(_currentProducts.GetRange(tol*meret, tol*meret+meret<=_currentProducts.Count ? meret : _currentProducts.Count-tol*meret));
        }

        public ActionResult Letoltes()
        {
            using (StreamWriter sr = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"App_Data\kimenet.csv"))
            {
                var csv = new CsvWriter(sr);
                csv.Configuration.Encoding = System.Text.Encoding.UTF8;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.QuoteAllFields = true;
                csv.WriteHeader<Product>();
                foreach (var rec in _allProducts)
                {
                    csv.WriteRecord(rec);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UjElem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UjElem(Product p)
        {
            _allProducts.Add(p);
            return RedirectToAction("Index");
        }

        public ActionResult Torol(Product prod)
        {
            var b = _allProducts.Remove(_allProducts.Single(p => p.Cikkszam == prod.Cikkszam));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Szerkeszt(string cikkszam)
        {
            return View("UjElem", _allProducts.Single(p => p.Cikkszam == cikkszam));
        }

        [HttpPost]
        public ActionResult Szerkeszt(Product prod)
        {
            var act = _allProducts.Single(p => p.Cikkszam == prod.Cikkszam);
            act.Cikknev = prod.Cikknev;

            act.Vonalkod = prod.Vonalkod;
            act.MennyisegiEgyseg = prod.MennyisegiEgyseg;
            return RedirectToAction("Index");
            //return View("UjElem", prod);
        }

        public ActionResult CikknevRendez()
        {
            _currentProducts = rendezes == 0 ? _currentProducts.OrderBy(x => x.Cikknev.Trim()).ToList() : _currentProducts.OrderByDescending(x => x.Cikknev.Trim()).ToList();
            rendezes = 1 - rendezes;

            return RedirectToAction("Index");
        }

        public ActionResult CikkszamRendez()
        {
            _currentProducts = rendezes == 0 ? _currentProducts.OrderBy(x => x.Cikkszam.Trim()).ToList() : _currentProducts.OrderByDescending(x => x.Cikkszam.Trim()).ToList();
            rendezes = 1 - rendezes;
            return RedirectToAction("Index");
        }

        public ActionResult Elozo()
        {
            if (tol > 0)
                tol--;

            return RedirectToAction("Index");
            
        }

        public ActionResult Kovetkezo()
        {
            if (tol * meret + meret <= _currentProducts.Count)
                tol++;

            return RedirectToAction("Index");
        }
    }
}