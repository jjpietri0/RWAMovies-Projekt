using AdminModule.Dal;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class TagAdminController : Controller
    {
        private readonly TagService _tagService;

        public TagAdminController(TagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _tagService.GetAllTagsAsync());
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagReq tag)
        {
            try
            {
                await _tagService.CreateTagAsync(tag);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var tag = await _tagService.GetTagByIdAsync(id);
                return View(tag);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TagReq tag)
        {
            try
            {
                await _tagService.UpdateTagAsync(id, tag);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tagService.DeleteTagAsync(id);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
