
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace catalog
{


    class Categorie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Product> Products { get; set; }
    }

    class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }



    class Program
    {
        static void Main(string[] args)
        {
            List<Categorie> listCategories = GetExcelCategorie();
            List<Product> listProducts = GetExcelProducts();

            foreach (var cat in listCategories)
            {
                cat.Products = new List<Product>();
                foreach (var pro in listProducts)
                {
                    if (cat.Id == pro.CategoryId)
                    {
                        cat.Products.Add(pro);
                    }
                }
            }
            //generar json
            string cJson = JsonConvert.SerializeObject(listCategories.ToArray(), Newtonsoft.Json.Formatting.Indented);
            string patharchiveJson = "../../../archivos/Catalog.json";
            File.WriteAllText(patharchiveJson, cJson);
            
            //generar xml
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration dec =  xmlDoc.CreateXmlDeclaration("1.0","utf-8","");
            XmlNode anode = xmlDoc.CreateElement("ArrayOfCategory");
            XmlAttribute xmlnsi = xmlDoc.CreateAttribute("xmlns:i");
            xmlnsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
            anode.Attributes.Append(xmlnsi);
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns");
            xmlns.Value = "http://schemas.datacontract.org/2004/07/Catalog";
            anode.Attributes.Append(xmlns);

            foreach (var cat in listCategories)
            {
                XmlNode rnode = xmlDoc.CreateElement("Category");
                XmlNode description = xmlDoc.CreateElement("Description");
                description.InnerText = cat.Description;
                rnode.AppendChild(description);
                XmlNode id = xmlDoc.CreateElement("Id");
                id.InnerText = cat.Id.ToString();
                rnode.AppendChild(id);
                XmlNode name = xmlDoc.CreateElement("Name");
                name.InnerText = cat.Name;
                rnode.AppendChild(name);
                foreach (var pro in cat.Products)
                {
                    XmlNode pnode = xmlDoc.CreateElement("Products");

                    XmlNode idcategory = xmlDoc.CreateElement("CategoryId");
                    idcategory.InnerText = pro.CategoryId.ToString();
                    pnode.AppendChild(idcategory);
                    XmlNode idp = xmlDoc.CreateElement("Id");
                    idp.InnerText = pro.Id.ToString();
                    pnode.AppendChild(idp);
                    XmlNode namep = xmlDoc.CreateElement("Name");
                    namep.InnerText = pro.Name;
                    pnode.AppendChild(namep);
                    XmlNode price = xmlDoc.CreateElement("Price");
                    price.InnerText = pro.Price.ToString();
                    pnode.AppendChild(price);
                    rnode.AppendChild(pnode);
                }
                anode.AppendChild(rnode);
            }
            xmlDoc.AppendChild(dec);
            xmlDoc.AppendChild(anode);
            xmlDoc.Save("../../../archivos/Catalog.xml");

        }

        //funciones recuperar de excel
        public static List<Categorie> GetExcelCategorie()
        {
            string path = @"C:\proyectosNet\catalog\catalog\archivos\Categories.csv";
            var reader = new StreamReader(File.OpenRead(path));
            List<Categorie> list = new List<Categorie>();
            int cont = 1;
            while (!reader.EndOfStream)
            {
                var linea = reader.ReadLine();
                if (cont != 1)
                {
                    var valores = linea.Split(';');
                    Categorie cat = new Categorie();
                    cat.Id = Convert.ToInt32(valores[0] ?? "0");
                    cat.Name = valores[1] ?? "";
                    cat.Description = valores[2] ?? "";
                    list.Add(cat);
                }
                cont++;
            }
            return list;
        }

        public static List<Product> GetExcelProducts()
        {
            string path = @"C:\proyectosNet\catalog\catalog\archivos\Products.csv";
            var reader = new StreamReader(File.OpenRead(path));
            List<Product> list = new List<Product>();
            int cont = 1;
            while (!reader.EndOfStream)
            {
                var linea = reader.ReadLine();
                if (cont != 1)
                {
                    var valores = linea.Split(';');
                    Product pro = new Product();
                    pro.Id = Convert.ToInt32(valores[0] ?? "0");
                    pro.CategoryId = Convert.ToInt32(valores[1] ?? "0");
                    pro.Name = valores[2] ?? "";
                    pro.Price = Convert.ToDouble(valores[3] ?? "0");
                    list.Add(pro);
                }
                cont++;
            }
            return list;
        }


    }
}
