using AdminModule.Dal;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class TagsController : Controller
    {
        private readonly TagService tagService;

        public TagsController(TagService tagService)
        {
            this.tagService = tagService;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await tagService.GetAllTagsAsync());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        //Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagRequest tagReq)
        {
            try
            {
                await tagService.CreateTagAsync(tagReq);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var tag = await tagService.GetTagAsync(id);
                return View(tag);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TagRequest tagReq)
        {
            try
            {
                await tagService.UpdateTagAsync(id, tagReq);
                return RedirectToAction(nameof(Index));
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
                await tagService.DeleteTagAsync(id);
                return RedirectToAction(nameof(Index));

            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
