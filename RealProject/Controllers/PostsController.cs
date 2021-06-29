using RealProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealProject.Controllers
{
    public class PostsController : Controller
    {
        private PostDBContext db = new PostDBContext();

        // GET: Article
        public ActionResult Index()
        {
            var posts = db.Posts.Include("Tag");
            ViewBag.Posts = posts;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }


        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            ViewBag.Post = post;
            ViewBag.Tag = post.Tag;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();

        }
         
        public ActionResult New()
        {
            Post post = new Post(); ;

            // preluam lista de categorii din metoda GetAllCategories()
            post.Tagger = GetAllTags();

            return View(post);
        }

        [HttpPost]
        public ActionResult New(Post post)
        {
            post.Date = DateTime.Now;
            post.Tagger = GetAllTags();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "The post was added.";
                    return RedirectToAction("Index");
                }       
                else
                {
                    return View(post);
                }
            }

            catch (Exception e)
            {
                return View(post);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            TempData["message"] = "The post titled " +
                post.Title + " was deleted.";
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            post.Tagger = GetAllTags();
            return View(post);
        }

        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            requestPost.Tagger = GetAllTags();
            try
            {
                if (ModelState.IsValid)
                {

                    Post post = db.Posts.Find(id);
                    if (TryUpdateModel(post))
                    {
                        //post = requestPost;
                        post.Title = requestPost.Title;
                        post.Content = requestPost.Content;
                        post.Date = requestPost.Date;
                        post.TagId = requestPost.TagId;
                        db.SaveChanges();
                        TempData["message"] = "Post modified.";


                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllTags()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var tags = from tg in db.Tags
                             select tg;

            // iteram prin categorii
            foreach (var tag in tags)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = tag.TagId.ToString(),
                    Text = tag.TagName.ToString()
                });
            }
            /*
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.CategoryId.ToString();
                listItem.Text = category.CategoryName.ToString();

                selectList.Add(listItem);
            }*/

            // returnam lista de categorii
            return selectList;
        }

    }
}