using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Services;
using TicketSystem.Models;
using TicketSystem.ViewModels;
using TicketSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using TicketSystem.FIlters;

namespace TicketSystem.Controllers
{
    [Authorize(Roles ="Problem")]
    public class ProblemController : Controller
    {
       // private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly ProblemService _problemService;
        private readonly SeverityService _severityService;
        private readonly PriorityService _priorityService;
        private readonly ProblemCatrgoryService _problemCatrgoryService;
        private readonly LoginService _loginService;
        
        public ProblemController(IMapper mapper,ProblemService problemService,SeverityService severityService,
            PriorityService priorityService,ProblemCatrgoryService problemCatrgoryService,LoginService loginService)
        {
            //_accessor = accessor;
            _mapper = mapper;
            _problemService = problemService;
            _severityService = severityService;
            _priorityService = priorityService;
            _problemCatrgoryService = problemCatrgoryService;
            _loginService = loginService;
        }
        [Authorize(Roles ="P_Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ProblemCategoryId"] = new 
                SelectList(_problemCatrgoryService.GetAllProblemCategories(), "Id", "Name");
            ViewData["SeverityId"] = new SelectList(_severityService.GetAllSeverities(), "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_priorityService.GetAllPriorities(), "Id", "Name");
            return View();
        }
        [Authorize(Policy= "P_Create")]
        [HttpPost]
        public async Task<IActionResult> Create(ProblemCreateVM problemCreateVM)
        {
            if (ModelState.IsValid)
            {
                Problem problem = _mapper.Map<ProblemCreateVM, Problem>(problemCreateVM);
                await _problemService.AddProblemAsync(problem);
                // todo...
                return RedirectToAction("Index");
            }
            ViewData["ProblemCategoryId"] = new
                SelectList(_problemCatrgoryService.GetAllProblemCategories(), "Id", "Name");
            ViewData["SeverityId"] = new SelectList(_severityService.GetAllSeverities(), "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_priorityService.GetAllPriorities(), "Id", "Name");
            return View(problemCreateVM);
        }
        [Authorize(Roles = "P_Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Problem problem = await _problemService.GetProblemByIdAsync(id);
            ProblemEditVM problemEditVM = _mapper.Map<Problem, ProblemEditVM>(problem);
           // ViewData["ProblemCategoryId"] = new
           //     SelectList(_problemCatrgoryService.GetAllProblemCategories(), "Id", "Name");
            ViewData["SeverityId"] = new SelectList(_severityService.GetAllSeverities(), "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_priorityService.GetAllPriorities(), "Id", "Name");
            return View(problemEditVM);
        }
        [Authorize(Roles = "P_Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProblemEditVM problemEditVM)
        {
            if (ModelState.IsValid)
            {
                Problem problem = _mapper.Map<ProblemEditVM, Problem>(problemEditVM);
                await _problemService.EditProblemAsync(problem);
                return RedirectToAction("Details", new { id = problemEditVM.Id });
            }
          //  ViewData["ProblemCategoryId"] = new
          //      SelectList(_problemCatrgoryService.GetAllProblemCategories(), "Id", "Name");
            ViewData["SeverityId"] = new SelectList(_severityService.GetAllSeverities(), "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_priorityService.GetAllPriorities(), "Id", "Name");
            return View(problemEditVM);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            Problem problem= await _problemService.GetProblemByIdAsync(id);
            ProblemShowVM problemShowVM = _mapper.Map<Problem, ProblemShowVM>(problem);
            return View(problemShowVM);
        }
        [Authorize(Roles = "P_Solve")]
        [HttpGet]
        public async Task<IActionResult> Resolve(int id)
        {
            Problem problem = await _problemService.GetProblemByIdAsync(id);
            ProblemSolvedVM problemResolvedVM = _mapper.Map<Problem, ProblemSolvedVM>(problem);
            return View(problemResolvedVM);
        }
        //[Authorize(Roles = "P_Resolve")]
        [Authorize(Policy="P_Solve")]
        [HttpPost]
        public async Task<IActionResult> Resolve(ProblemSolvedVM problemResolvedVM)
        {
            if (ModelState.IsValid)
            {
                Problem problem= _mapper.Map<ProblemSolvedVM, Problem>(problemResolvedVM);
                await _problemService.EditProblemAsync(problem);
                return RedirectToAction("Details", new { id = problemResolvedVM.Id });
            }
            Problem problemDb = await _problemService.GetProblemByIdAsync(problemResolvedVM.Id);
            problemResolvedVM.Priority = problemDb.Priority;
            problemResolvedVM.Severity = problemDb.Severity;
            problemResolvedVM.ProblemCategory = problemDb.ProblemCategory;
            return View(problemResolvedVM);
        }
        [Authorize(Roles = "P_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var problem = await _problemService.GetProblemByIdAsync(id);
            if (problem == null)
            {
                return NotFound();
            }
            ProblemShowVM problemShowVM = _mapper.Map<Problem, ProblemShowVM>(problem);
            return View(problemShowVM);
        }
        [Authorize(Roles = "P_Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Problem problem = await _problemService.GetProblemByIdAsync(id);
            await _problemService.RemoveProblemAsync(problem);
            return RedirectToAction("Index");
        }

        public IActionResult Index(int page=1,bool status=false,string categoryname="All")
        {
            IEnumerable<Problem> problems = _problemService.
                GetAllProblemsByStatusAndCategoryAsync(status, categoryname);

            int page_count = 3;            
            int pages = problems.GetPages(page_count);
            problems = problems.GetPages(page_count, page);

            IEnumerable<ProblemShowVM> problemShowVMs = _mapper.
                Map<IEnumerable<Problem>,IEnumerable<ProblemShowVM>>(problems);   

            ViewData["pages"] = pages;
            ViewData["status"] = status;
            ViewData["nowpage"] = page;
            ViewData["categoryname"] = categoryname;
            ViewData["allcategorys"]= _problemCatrgoryService.GetAllNamewithAll();
            return View(problemShowVMs);
        }


        // remote check -----------------
        public async Task<IActionResult> IsSummaryExisted(string Summary)
        {
            if (await _problemService.IsSummaryExistedAsync(Summary))
                return Json($"Summary {Summary} 已經存在");
            return Json("true");
        }

        //// 新增檢查 ProblemCreateVM 欄位 ProblemCategoryId
        //public async Task<IActionResult> CheckProblemCreateVM(int ProblemCategoryId)
        //{
        //    string categoryname = (await _problemCatrgoryService.GetProblemCategorybyId(ProblemCategoryId)).Name.ToUpper();
        //    string rolename = _loginService.GetUserInfo<LoginStateVM>().RoleName.ToUpper();
        //    if(categoryname== "Feature Request".ToUpper())
        //    {
        //        if (rolename == "PM")
        //            return Json(true);
        //        else
        //            return Json($"只有PM能新增{categoryname}的問題");
        //    }
        //    if (rolename == "QA")
        //        return Json(true);
        //    return Json($"只有QA能新增{categoryname}的問題");
        //}

        //// Solve檢查 ProblemResolvedVM 欄位 isSolved 為true時
        //public async Task<IActionResult> CheckProblemSolvedVM(int ProblemCategoryId, bool isSolved)
        //{
        //    string categoryname = (await _problemCatrgoryService.GetProblemCategorybyId(ProblemCategoryId)).Name.ToUpper();
        //    string rolename = _loginService.GetUserInfo<LoginStateVM>().RoleName.ToUpper();
        //    if(categoryname== "Test Case".ToUpper())
        //    {
        //        if (rolename == "QA")
        //            return Json(true);
        //        else
        //            return Json($"{categoryname}類型的錯誤只有QA可以關閉");
        //    }
        //    if (rolename == "RD")
        //        return Json(true);
        //    return Json($"只有RD可以解決{categoryname}類型的問題");
        //} 
    }
}
