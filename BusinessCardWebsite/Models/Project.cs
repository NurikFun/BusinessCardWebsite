using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessCardWebsite.Models
{
    public class Project : QueryResult
    {
        public int id { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Описание")]
        public string description { get; set; }

        [AllowHtml]
        [Display(Name = "Контент")]
        public string content { get; set; }

        public string img_path { get; set; }

        public string doc_path { get; set; }

        public List<Images> images { get; set; }


        [Display(Name = "Направление")]
        public int direction_id { get; set; }
        public Direction direction { get; set; }

        public Project()
        {
            images = new List<Images>();
        }
    }

    public class Images
    {
        public int id { get; set; }

        public int project_id { get; set; }

        public string img_path { get; set; }
    }

    public class Direction : QueryResult
    {
        public int id { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Описание")]
        public string description { get; set; }

        [AllowHtml]
        [Display(Name = "Контент")]
        public string content { get; set; }

        public string img_path { get; set; }
    }



    public class Request : QueryResult
    {
        public int id { get; set; }

        [Display(Name = "Имя")]
        public string name { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Телефон")]
        public string phone { get; set; }

        [Display(Name = "Название компании")]
        public string company_name { get; set; }

        [Display(Name = "Ориентировочный бюджет")]
        public string estimated_budget { get; set; }

        [Display(Name = "Сфера деятельности")]
        public string field { get; set; }

        [Display(Name = "Описание")]
        public string description { get; set; }

        public DateTime date_request { get; set; }

        public int status { get; set; }

        public DateTime date_of_reading { get; set; }
    }
}