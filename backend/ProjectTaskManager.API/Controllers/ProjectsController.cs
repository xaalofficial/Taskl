namespace ProjectTaskManager.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManager.Application.DTOs.Projects;
using ProjectTaskManager.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userId = GetUserId();
        var projects = await _projectService.GetUserProjectsAsync(userId);
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        var userId = GetUserId();
        var project = await _projectService.GetProjectByIdAsync(id, userId);

        if (project == null)
            return NotFound(new { message = "Project not found" });

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var project = await _projectService.CreateProjectAsync(request, userId);

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var project = await _projectService.UpdateProjectAsync(id, request, userId);

        if (project == null)
            return NotFound(new { message = "Project not found" });

        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var userId = GetUserId();
        var result = await _projectService.DeleteProjectAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Project not found" });

        return NoContent();
    }
}