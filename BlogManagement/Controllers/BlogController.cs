using System.Reflection.Metadata;
using System.Security.Claims;
using BlogManagement.Helper;
using Data.Repository;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dto;
using Services.Dto.Admin;

namespace BlogManagement.Controllers
{
    public class BlogController : Controller
    {
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly BlogService blogService;

        public BlogController(CookieUserDetailsHandler cookieUserDetailsHandler, BlogService blogService)
        {
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.blogService = blogService;
        }

        public IActionResult AddBlog()
        {
            return View();
        }

        public async Task<IActionResult> BlogDetail(Blog blog)
        {
            var blogDetail = await blogService.GetById(blog.Id - 169999);
            return View(blogDetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddBlog(AddBlogRequest dto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            dto.Requestor = user;

            if(!(string.IsNullOrWhiteSpace(dto.Title) && string.IsNullOrWhiteSpace(dto.Category) && string.IsNullOrWhiteSpace(dto.Content)))
            {
                var (blog, message) = await blogService.Add(dto);
                TempData["BlogAdded"] = message;
                if(blog is not null)
                {
                    return RedirectToAction("BlogList");
                }
            }

            return View(dto);
        }

        public async Task<IActionResult> BlogList(GetAllBlogCreatedByUserRequest dto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            dto.Requestor = user;

            if(user.Role.Name == Constants.Keys.ADMIN)
            {
                var (allBlogs, blogTotalCount) = await blogService.GetAllBlogs(dto);

                var modelForAdmin = new PagedList<Blog>
                {
                    Items = allBlogs,
                    PageNumber = dto.PageNo,
                    PageSize = dto.PageSize,
                    TotalItems = blogTotalCount,
                    Requestor = user
                };

                return View(modelForAdmin);
            }
            
            var (blogs, totalCount) = await blogService.GetBlogs(dto);

            var model = new PagedList<Blog>
            {
                Items = blogs,
                PageNumber = dto.PageNo,
                PageSize = dto.PageSize,
                TotalItems = totalCount,
                Requestor = user
            };

            return View(model);
        }

        public async Task<IActionResult> Approve(ApproveBlogRequest dto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            if(user.Role.Name != Constants.Keys.ADMIN)
            {
                return RedirectToAction("BlogList");
            }

            dto.Requestor = user;

            var (blog, message) = await blogService.Approve(dto);

            if(blog is null)
            {
                ViewBag.message = message;
                return View("BlogList");
            }

            return View("ApproveBlog", blog);
        }

        public async Task<IActionResult> ActiveStatus(UpdateBlogActiveStatusRequest dto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            dto.Requestor = user;

            var (blog, message) = await blogService.UpdateActiveStatus(dto);
            
            ViewBag.message = message;
            
            return RedirectToAction("BlogList");
        }

        public async Task<IActionResult> Explore(GetAllBlogCreatedByUserRequest dto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            dto.Requestor = user;

            var (blogs, totalCount) = await blogService.GetAllBlogCretedByOther(dto);

            var model = new PagedList<Blog>
            {
                Items = blogs,
                PageNumber = dto.PageNo,
                PageSize = dto.PageSize,
                TotalItems = totalCount,
                Requestor = user
            };
            
            return View(model);
        }

        public async Task<IActionResult> AddLike(AddLikeToBlogRequest dto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            dto.Requestor = user;

            var (blog, message) = await blogService.AddLike(dto);

            if(blog is null)
            {
                return RedirectToAction("BlogList");
            }

            return RedirectToAction("Explore");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
            if(user is null)
            {
                return RedirectToAction("Registration", "User");
            }

            var blog = await blogService.GetById(id);
            if(blog.CreatedById == user.Id)
            {
                return View(blog);
            }
            
            return RedirectToAction("Explore");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Blog blog)
        {
            if (blog is not null)
            {
                var (updatedBlog,message) = await blogService.Update(blog);
                return RedirectToAction("BlogDetail", updatedBlog);
            }

            return View(blog);
        }

        public async Task<IActionResult> AddComment(AddCommentRequest dto)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
            
                var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
                if(user is null)
                {
                    return RedirectToAction("Registration", "User");
                }

                dto.Requestor = user;

                var (blog, message) = await blogService.AddComment(dto);

                TempData["Message"] =  message;
                
                if(blog is null)
                {
                    return RedirectToAction("Explore");
                }

                return RedirectToAction("Explore");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }
    }
}