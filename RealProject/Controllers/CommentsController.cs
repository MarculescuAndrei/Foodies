using RealProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealProject.Controllers
{

    public class CommentsController : Controller
    {
        private PostDBContext db = new PostDBContext();
        
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            TempData["message"] = "Comment " + "was deleted.";
            db.Comments.Remove(comm);
            db.SaveChanges();
            return Redirect("/Posts/Show/" + comm.PostId);
        }
        
        [HttpPost]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    TempData["message"] = "The comment was added.";
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                else
                {
                    return View(comm);
                }
            }

            catch (Exception e)
            {
                return Redirect("/Post/Show/" + comm.PostId);
            }

        }
        
            public ActionResult Edit(int id)
            {
                Comment comm = db.Comments.Find(id);
                TempData["message"] = "Comment " + "was edited.";
                ViewBag.Comment = comm;
                return View();
            }

            [HttpPut]
            public ActionResult Edit(int id, Comment requestComment)
            {
                try
                {
                    Comment comm = db.Comments.Find(id);
                    if (TryUpdateModel(comm))
                    {
                        comm.Content = requestComment.Content;
                        db.SaveChanges();
                    }
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                catch (Exception e)
                {
                    return View();
                }

            }


        }
        
    }
