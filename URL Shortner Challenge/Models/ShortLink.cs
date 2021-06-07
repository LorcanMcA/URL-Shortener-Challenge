/****************************************************************************************** 
 *	Object for generating and storing user links  *
 *	
 *	The two constructor are used to generate the new short link.
 *	This could also be achieved by the data base with a stored procedure.
 *	Moving this functionality to the data could assist with the speed of the website when handling large amounts of data.

 ******************************************************************************************/
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace URL_Shortner_Challenge.Models
{
    public class ShortLink
    {
        
        public int Id { get; set; }

        [Required]
        [Display(Name = "Entered URL")]
        public string entered { get; set; } //The URL that is entered by the user.

        [Display(Name = "Generated URL")]
        public string returned { get; set; } //The generated URL that is returned to the user.

        [Display(Name = "Date created")] // The date the URL is added to the database.
        [DataType(DataType.Date)]
        public DateTime created { get; set; }

        [Display(Name = "Expiration date")]
        [DataType(DataType.Date)]
        public DateTime expired { get; set; } //The date the link is to be removed from the data base.

        public string UserID { get; set; }

        public ShortLink()
        {

        }

        // Constructor for permanant links. Calls the temporary link constructor then sets the expired date to the max possible. 
        public ShortLink(List<string> links, string passedLink, HttpContext context, string user) : this(links, passedLink, context)
        {
            this.UserID = user;
            this.expired = DateTime.MaxValue;
        }

        // Constructor for temporary links.
        public ShortLink(List<string> links, string passedLink, HttpContext context)
        {
            string host = $"{context.Request.Scheme}://{context.Request.Host}"; //Gets the current host (currently localhost:{port})

            bool uniqueLink = false;
            while (!uniqueLink)//Loops to ensure there are no duplicate links 
            {
                string newURL = host + "/l/" + randomString(); // Formats the link to use the /l/{passedLink} endpoint

                if (!links.Contains(newURL))
                {
                    this.returned = newURL;
                    uniqueLink = true;
                }
            }


            this.entered = passedLink; 
            this.created = DateTime.Now;

            expired = created.AddYears(1); //Sets the expiration date to 1 year
        }

        //Generates a random string of characters.
        //example: ZhIKcCb28K-
        private string randomString()  // https://www.c-sharpcorner.com/UploadFile/201fc1/what-is-random-urls-and-how-to-creating-them-in-Asp-Net/
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
