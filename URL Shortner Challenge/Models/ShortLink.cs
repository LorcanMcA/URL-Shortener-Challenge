using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URL_Shortner_Challenge.Models
{
    public class ShortLink
    {
        
        public int Id { get; set; }

        [Required]
        [Display(Name = "Entered URL")]
        public string entered { get; set; }

        [Display(Name = "Generated URL")]
        public string returned { get; set; }

        [Display(Name = "Date created")]
        [DataType(DataType.Date)]
        public DateTime created { get; set; }

        [Display(Name = "Expiration date")]
        [DataType(DataType.Date)]
        public DateTime expired { get; set; }


        public string UserID { get; set; }

        public ShortLink()
        {

        }

        public void generateLink(List<string> links, string passedLink, HttpContext context, string user) // https://www.c-sharpcorner.com/UploadFile/201fc1/what-is-random-urls-and-how-to-creating-them-in-Asp-Net/
        {
            generateLink(links, passedLink, context);

            this.expired = created.AddYears(1);
        }

        public void generateLink(List<string> links, string passedLink, HttpContext context) // https://www.c-sharpcorner.com/UploadFile/201fc1/what-is-random-urls-and-how-to-creating-them-in-Asp-Net/
        {
            string host = $"{context.Request.Scheme}://{context.Request.Host}";
            string newURL = host + "/l/" + randomString();
            if (!links.Contains(newURL))
            {
                this.returned = newURL;
            }

            this.entered = passedLink;
            this.created = DateTime.Now;

            expired = created.AddDays(30);
        }

        //example: ZhIKcCb28K-
        private string randomString()
        {
            string newUrl = "";
            List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            List<char> characters = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_' };

            // Create one instance of the Random  
            Random rand = new Random();
            // run the loop till I get a string of 10 characters  
            for (int i = 0; i < 11; i++)
            {
                // Get random numbers, to get either a character or a number...  
                int random = rand.Next(0, 3);
                if (random == 1)
                {
                    // use a number  
                    random = rand.Next(0, numbers.Count);
                    newUrl += numbers[random].ToString();
                }
                else
                {
                    // Use a character  
                    random = rand.Next(0, characters.Count);
                    newUrl += characters[random].ToString();
                }
            }
            return newUrl;
        }

    }
}
