using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebTreeView.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public List<Item> GetItems()
        {
            string path = @"archivos/items.json";
            var result = new List<Item>();
            using (StreamReader r = new StreamReader(path))
            {
                var json = r.ReadToEnd();
                result = JsonConvert.DeserializeObject<List<Item>>(json);
                
            }
            return result;
        }



        public class Item
        {
            public string Name { get; set; }
            public List<Item> Children { get; set; }

        }

    }



}
