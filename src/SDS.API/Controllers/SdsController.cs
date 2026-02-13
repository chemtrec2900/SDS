using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SDS.Application.DTOs;
using SDS.Application.Interfaces;
using SDS.Domain.Entities;

namespace SDS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SdsController : ControllerBase
{
    private readonly ISdsService _sdsService;
    private readonly ILogger<SdsController> _logger;

    public SdsController(ISdsService sdsService, ILogger<SdsController> logger)
    {
        _sdsService = sdsService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new SDS document
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Author")]
    public async Task<ActionResult<SdsDto>> CreateSds([FromBody] CreateSdsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tenantId = GetTenantId();
            var userId = GetUserId();
            
            var sds = await _sdsService.CreateSdsAsync(tenantId, request.DocumentNumber, userId, cancellationToken);
            
            if (!string.IsNullOrEmpty(request.ProductName))
                sds.ProductName = request.ProductName;
            if (!string.IsNullOrEmpty(request.CasNumber))
                sds.CasNumber = request.CasNumber;
            if (!string.IsNullOrEmpty(request.SupplierName))
                sds.SupplierName = request.SupplierName;
            
            await _sdsService.UpdateSdsAsync(sds, cancellationToken);
            
            var dto = MapToDto(sds);
            return CreatedAtAction(nameof(GetSds), new { id = sds.Id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SDS");
            return StatusCode(500, "An error occurred while creating the SDS");
        }
    }

    /// <summary>
    /// Get SDS by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "Viewer")]
    public async Task<ActionResult<SdsDto>> GetSds(Guid id, CancellationToken cancellationToken)
    {
        var sds = await _sdsService.GetSdsByIdAsync(id, cancellationToken);
        if (sds == null)
            return NotFound();

        return Ok(MapToDto(sds));
    }

    /// <summary>
    /// Search SDS documents
    /// </summary>
    [HttpGet("search")]
    [Authorize(Policy = "Viewer")]
    public async Task<ActionResult<IEnumerable<SdsDto>>> SearchSds(
        [FromQuery] string? searchTerm,
        [FromQuery] string? casNumber,
        [FromQuery] string? supplier,
        CancellationToken cancellationToken)
    {
        var tenantId = GetTenantId();
        var results = await _sdsService.SearchSdsAsync(tenantId, searchTerm, casNumber, supplier, cancellationToken);
        return Ok(results.Select(MapToDto));
    }

    /// <summary>
    /// Update an SDS section
    /// </summary>
    [HttpPut("{id}/sections")]
    [Authorize(Policy = "Author")]
    public async Task<ActionResult> UpdateSection(
        Guid id,
        [FromBody] UpdateSdsSectionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            await _sdsService.UpdateSectionAsync(id, request.SectionNumber, request.Content, userId, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating section");
            return StatusCode(500, "An error occurred while updating the section");
        }
    }

    /// <summary>
    /// Submit SDS for review
    /// </summary>
    [HttpPost("{id}/submit-review")]
    [Authorize(Policy = "Author")]
    public async Task<ActionResult> SubmitForReview(Guid id, [FromQuery] Guid reviewerId, CancellationToken cancellationToken)
    {
        try
        {
            await _sdsService.SubmitForReviewAsync(id, reviewerId, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting for review");
            return StatusCode(500, "An error occurred while submitting for review");
        }
    }

    /// <summary>
    /// Review SDS (approve/reject)
    /// </summary>
    [HttpPost("{id}/review")]
    [Authorize(Policy = "Reviewer")]
    public async Task<ActionResult> ReviewSds(Guid id, [FromBody] ReviewSdsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var reviewerIdStr = GetUserId();
            if (!Guid.TryParse(reviewerIdStr, out var reviewerId))
            {
                return BadRequest("Invalid user ID");
            }

            // Get the pending review for this SDS
            var sds = await _sdsService.GetSdsByIdAsync(id, cancellationToken);
            if (sds == null)
                return NotFound();

            // In a real implementation, fetch the actual review record
            // For now, create a new review ID
            var reviewId = Guid.NewGuid();
            var review = await _sdsService.ReviewSdsAsync(reviewId, request.Status, request.Comments, reviewerId, cancellationToken);
            return Ok(review);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reviewing SDS");
            return StatusCode(500, "An error occurred while reviewing the SDS");
        }
    }

    /// <summary>
    /// Get version history
    /// </summary>
    [HttpGet("{id}/versions")]
    [Authorize(Policy = "Viewer")]
    public async Task<ActionResult<IEnumerable<SdsDto>>> GetVersionHistory(Guid id, CancellationToken cancellationToken)
    {
        var versions = await _sdsService.GetVersionHistoryAsync(id, cancellationToken);
        return Ok(versions.Select(MapToDto));
    }

    private Guid GetTenantId()
    {
        // Extract tenant ID from claims or context
        var tenantClaim = User.FindFirst("tenant_id")?.Value;
        return Guid.TryParse(tenantClaim, out var tenantId) ? tenantId : Guid.Empty;
    }

    private string GetUserId()
    {
        return User.FindFirst("sub")?.Value ?? User.FindFirst("oid")?.Value ?? "system";
    }

    private SdsDto MapToDto(SdsDocument sds)
    {
        return new SdsDto
        {
            Id = sds.Id,
            DocumentNumber = sds.DocumentNumber,
            RevisionNumber = sds.RevisionNumber,
            Version = sds.Version,
            Status = sds.Status,
            Title = sds.Title,
            ProductName = sds.ProductName,
            CasNumber = sds.CasNumber,
            SupplierName = sds.SupplierName,
            EffectiveDate = sds.EffectiveDate,
            ReviewDate = sds.ReviewDate,
            NextReviewDate = sds.NextReviewDate,
            IsRestricted = sds.IsRestricted,
            IsShared = sds.IsShared,
            CreatedBy = sds.CreatedBy,
            CreatedAt = sds.CreatedAt,
            ApprovedBy = sds.ApprovedBy,
            ApprovedAt = sds.ApprovedAt,
            Sections = sds.Sections.Select(s => new SdsSectionDto
            {
                Id = s.Id,
                SectionNumber = s.SectionNumber,
                Title = s.Title,
                Content = s.Content,
                HtmlContent = s.HtmlContent,
                Version = s.Version,
                HasChanges = s.HasChanges
            }).ToList()
        };
    }
}
